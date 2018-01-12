using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnitToXUnit.Extensions;

namespace NUnitToXUnit.Core
{
    public class SetupConverter
    {
        private readonly MethodDeclarationSyntax _node;

        public SetupConverter(MethodDeclarationSyntax node)
        {
            _node = node;
        }

        public bool IsSetup => _node.AttributeLists.Contains("setup");

        public SyntaxNode Replace()
        {
            var className = ((ClassDeclarationSyntax)_node.Parent).Identifier.ToString().Trim();
            var x =
                SyntaxFactory.ConstructorDeclaration(className)
                    .WithModifiers(SyntaxTokenList.Create(SyntaxFactory.Token(SyntaxKind.PublicKeyword)))
                    .WithBody(_node.Body);

            return x;
        }
    }
}
