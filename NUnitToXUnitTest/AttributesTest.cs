using System;
using System.IO;
using Microsoft.CodeAnalysis.CSharp;
using NUnitToXUnit;
using NUnitToXUnit.Converters;
using NUnitToXUnit.Core;
using Xunit;

namespace NUnitToXUnitTest
{
    public class AttributesTest
    {
        [Fact]
        public void Test()
        {
            var text = File.ReadAllText("From.txt");
            var syntaxTree = CSharpSyntaxTree.ParseText(text);
            var root = syntaxTree.GetRoot();
            var rewriter = new Rewritter();
            root = rewriter.Visit(root);
            Assert.Equal(File.ReadAllText("To.txt"), FileConverter.Prettify(root));
        }
    }
}
