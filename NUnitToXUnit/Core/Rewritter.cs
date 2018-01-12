using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnitToXUnit.Extensions;

namespace NUnitToXUnit.Core
{
    public partial class Rewritter : CSharpSyntaxRewriter
    {
        public override SyntaxNode VisitUsingDirective(UsingDirectiveSyntax node)
        {
            if (node.Name.ToString().Equals("NUnit.Framework"))
                return node.WithName(SyntaxFactory.IdentifierName("Xunit"));

            if (node.Name.ToString().Contains("NUnit"))
                return null;

            return base.VisitUsingDirective(node);
        }

        public bool IsValidFile(CompilationUnitSyntax root)
        {
            return root != null && root.Usings.ToList().Any(u => u.Name.ToString().Contains("NUnit.Framework"));
        }

        public override SyntaxNode VisitMethodDeclaration(MethodDeclarationSyntax node)
        {
            var setup = new SetupConverter(node);
            if (setup.IsSetup)
            {
                return setup.Replace();
            }

            var testToFact = new TestToFact(node);
            if (testToFact.IsTest)
            {
                node = node.WithAttributeLists(testToFact.Convert(node.AttributeLists));
            }

            var attributeRemover = new AttributeRemover(node.AttributeLists, "TearDown");
            if (attributeRemover.IsHavingAttribute)
            {
                node = node.WithAttributeLists(attributeRemover.Remove());
                node = node.WithIdentifier(SyntaxFactory.Identifier("Dispose"));
            }

            return base.VisitMethodDeclaration(node);
        }

        public override SyntaxNode VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            var attributeRemover = new AttributeRemover(node.AttributeLists, "TestFixture");
            if (attributeRemover.IsHavingAttribute)
            {
                if (!node.Modifiers.Any(m => m.IsKind(SyntaxKind.PublicKeyword)))
                {
                    var publicKeyword = SyntaxFactory.Token(SyntaxKind.PublicKeyword)
                        .WithTrailingTrivia(SyntaxTriviaList.Create(SyntaxFactory.Space));
                    node = node.WithModifiers(node.Modifiers.Add(publicKeyword));
                }

                node = node.WithAttributeLists(attributeRemover.Remove());
            }

            if (node.DescendantNodes().Any(n => n is MethodDeclarationSyntax m
                                                && m.AttributeLists.Contains("TearDown")))
            {
                node = node.AddBaseListTypes(
                    SyntaxFactory.SimpleBaseType(
                        SyntaxFactory.IdentifierName("IDisposable")).WithoutTrivia());
                node = node.WithIdentifier(node.Identifier.WithoutTrivia());
            }

            return base.VisitClassDeclaration(node);
        }
    }
}
