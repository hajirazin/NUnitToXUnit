using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace NUnitToXUnit.Extensions
{
    public static class AttributeListsExtensions
    {
        public static bool Contains(this SyntaxList<AttributeListSyntax> attributeList, 
            string attributeText)
        {
            return attributeList.Any(a =>
                a.Attributes.Any(at =>
                    at.Name.ToString().Equals(attributeText, StringComparison.OrdinalIgnoreCase)));
        }
    }
}
