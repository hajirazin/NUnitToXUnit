using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnitToXUnit.Extensions;

namespace NUnitToXUnit.Core
{
    public class TestToFact
    {
        private readonly MethodDeclarationSyntax _node;

        public TestToFact(MethodDeclarationSyntax node)
        {
            _node = node;
        }

        public bool IsTest => _node.AttributeLists.Contains("Test");

        public SyntaxList<AttributeListSyntax> Convert(
            SyntaxList<AttributeListSyntax> attributeList)
        {
            var attributeRemover = new AttributeRemover(attributeList, "Ignore");
            var isIgnore = attributeRemover.IsHavingAttribute;

            var ignoreText = "\"\"";
            if (isIgnore)
            {
                var ignoreAttributes = attributeList.First(a =>
                    a.Attributes.Any(at =>
                        at.Name.ToString().Equals("Ignore", StringComparison.OrdinalIgnoreCase)));

                var ignoreAttribute = ignoreAttributes.Attributes.First(at =>
                    at.Name.ToString().Equals("Ignore", StringComparison.OrdinalIgnoreCase));

                if (ignoreAttribute.ArgumentList?.Arguments.Any() ?? false)
                {
                    ignoreText = ignoreAttribute.ArgumentList.Arguments.First().Expression.ToString();
                }

                attributeList = attributeRemover.Remove();
            }

            attributeList = Convert(attributeList, isIgnore, ignoreText);
            return attributeList;
        }

        public SyntaxList<AttributeListSyntax> Convert(
            SyntaxList<AttributeListSyntax> attributeList,
            bool isSkip,
            string skipText)
        {
            var attributes = attributeList.First(a =>
                a.Attributes.Any(at => at.Name.ToString().Equals("Test", StringComparison.OrdinalIgnoreCase)));

            var attribute = attributes.Attributes.First(at =>
                at.Name.ToString().Equals("Test", StringComparison.OrdinalIgnoreCase));

            var newAttribute = attribute.WithName(SyntaxFactory.IdentifierName("Fact"));

            if (isSkip)
            {
                newAttribute = newAttribute.WithArgumentList(
                    SyntaxFactory.AttributeArgumentList(
                        SyntaxFactory.SeparatedList(new[]
                        {
                            SyntaxFactory.AttributeArgument(
                                SyntaxFactory.NameEquals("Skip"),
                                null,
                                SyntaxFactory.IdentifierName(skipText))
                        })));
            }

            return attributeList.Replace(attributes, attributes.WithAttributes(
                attributes.Attributes.Replace(attribute, newAttribute)));
        }
    }
}
