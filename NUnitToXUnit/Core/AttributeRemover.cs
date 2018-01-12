using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnitToXUnit.Extensions;

namespace NUnitToXUnit.Core
{
    public class AttributeRemover
    {
        private readonly SyntaxList<AttributeListSyntax> _attributeList;
        private readonly string _attributeText;

        public AttributeRemover(SyntaxList<AttributeListSyntax> attributeList, string attributeText)
        {
            _attributeList = attributeList;
            _attributeText = attributeText;
        }
        public bool IsHavingAttribute => _attributeList.Contains(_attributeText);

        public SyntaxList<AttributeListSyntax> Remove()
        {
            var attributes = _attributeList.First(a =>
                a.Attributes.Any(at =>
                    at.Name.ToString().Equals(_attributeText, StringComparison.OrdinalIgnoreCase)));

            if (attributes.Attributes.Count == 1 && _attributeList.Count == 1)
            {
                return new SyntaxList<AttributeListSyntax>();
            }

            if (attributes.Attributes.Count == 1)
            {
                return _attributeList.Remove(attributes);
            }

            var attribute = attributes.Attributes.First(at =>
                at.Name.ToString().Equals(_attributeText, StringComparison.OrdinalIgnoreCase));

            var newAttributes = attributes.WithAttributes(attributes.Attributes.Remove(attribute));

            return _attributeList.Replace(attributes, newAttributes);
        }
    }
}
