using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnitToXUnit.Extensions;

namespace NUnitToXUnit.Core
{
    public partial class Rewritter
    {
        public override SyntaxNode VisitExpressionStatement(ExpressionStatementSyntax node)
        {
            var nodeString = node.ToString();
            if (!nodeString.StartsWith("Assert.That"))
                return base.VisitExpressionStatement(node);

            if (node.Expression is InvocationExpressionSyntax assertInvocation
                && assertInvocation.ArgumentList.Arguments.Count == 2
                && assertInvocation.ArgumentList.Arguments[1].Expression is InvocationExpressionSyntax isInvocation
                && isInvocation.Expression is MemberAccessExpressionSyntax isMember)
            {
                var i = SyntaxFactory.InvocationExpression(
                    SyntaxFactory.MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        SyntaxFactory.IdentifierName("Assert"),
                        Convert(isMember.Name)),
                    SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(new[]
                    {
                        isInvocation.ArgumentList.Arguments.First(),
                        assertInvocation.ArgumentList.Arguments.First()
                    })));

                node = node.WithExpression(i);
            }
            else if (node.Expression is InvocationExpressionSyntax assertInvocation1
                && assertInvocation1.ArgumentList.Arguments.Count == 2
                && assertInvocation1.ArgumentList.Arguments[1].Expression
                         is MemberAccessExpressionSyntax isMember1)
            {
                var i = SyntaxFactory.InvocationExpression(
                    SyntaxFactory.MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        SyntaxFactory.IdentifierName("Assert"),
                        Convert(isMember1.Name)),
                    SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(new[]
                    {
                        assertInvocation1.ArgumentList.Arguments.First()
                    })));

                node = node.WithExpression(i);
            }

            else if (node.Expression is InvocationExpressionSyntax assertInvocation2
                && assertInvocation2.ArgumentList.Arguments.Count == 3
                && assertInvocation2.ArgumentList.Arguments[1].Expression is InvocationExpressionSyntax isInvocation2
                && isInvocation2.Expression is MemberAccessExpressionSyntax isMember2)
            {
                var i = SyntaxFactory.InvocationExpression(
                    SyntaxFactory.MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        SyntaxFactory.IdentifierName("Assert"),
                        Convert(isMember2.Name)),
                    SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(new[]
                    {
                        isInvocation2.ArgumentList.Arguments.First(),
                        assertInvocation2.ArgumentList.Arguments.First(),
                        assertInvocation2.ArgumentList.Arguments.Third()
                    })));

                node = node.WithExpression(i);
            }
            else if (node.Expression is InvocationExpressionSyntax assertInvocation3
                     && assertInvocation3.ArgumentList.Arguments.Count == 3
                     && assertInvocation3.ArgumentList.Arguments[1].Expression
                         is MemberAccessExpressionSyntax isMember3)
            {
                var i = SyntaxFactory.InvocationExpression(
                    SyntaxFactory.MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        SyntaxFactory.IdentifierName("Assert"),
                        Convert(isMember3.Name)),
                    SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(new[]
                    {
                        assertInvocation3.ArgumentList.Arguments.First(),
                        assertInvocation3.ArgumentList.Arguments.Third()
                    })));

                node = node.WithExpression(i);
            }
            else if (node.Expression is InvocationExpressionSyntax assertInvocation4
                     && assertInvocation4.ArgumentList.Arguments.Count == 1)
            {
                var i = SyntaxFactory.InvocationExpression(
                    SyntaxFactory.MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        SyntaxFactory.IdentifierName("Assert"),
                        SyntaxFactory.IdentifierName("True")),
                    SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(new[]
                    {
                        assertInvocation4.ArgumentList.Arguments.First()
                    })));
                node = node.WithExpression(i);
            }
            else if (node.Expression is InvocationExpressionSyntax assertInvocation5)
            {
                ArgumentSyntax secondArgument;
                if (assertInvocation5.ArgumentList.Arguments.Count == 2)
                {
                    secondArgument = assertInvocation5.ArgumentList.Arguments.Second();
                }
                else
                {
                    var stringFormat = SyntaxFactory.InvocationExpression(
                        SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.IdentifierName("string"),
                            SyntaxFactory.IdentifierName("Format")),
                        assertInvocation5.ArgumentList.WithArguments(assertInvocation5.ArgumentList.Arguments
                            .RemoveAt(0)));

                    secondArgument = SyntaxFactory.Argument(stringFormat);
                }

                var i = SyntaxFactory.InvocationExpression(
                    SyntaxFactory.MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        SyntaxFactory.IdentifierName("Assert"),
                        SyntaxFactory.IdentifierName("True")),
                    SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(new[]
                    {
                        assertInvocation5.ArgumentList.Arguments.First(),
                        secondArgument
                    })));

                node = node.WithExpression(i);
            }

            return base.VisitExpressionStatement(node);
        }

        private static SimpleNameSyntax Convert(SimpleNameSyntax name)
        {
            switch (name.Identifier.ValueText)
            {
                case "EqualTo":
                    return name.WithIdentifier(SyntaxFactory.Identifier("Equal"));
            }

            return name;
        }
    }
}
