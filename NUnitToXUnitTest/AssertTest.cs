using System.IO;
using Microsoft.CodeAnalysis.CSharp;
using NUnitToXUnit.Converters;
using NUnitToXUnit.Core;
using Xunit;

namespace NUnitToXUnitTest
{
    public class AssertTest
    {
        [Fact]
        public void TestAssertStatements()
        {
            var text = File.ReadAllText("From_Assert.txt");
            var syntaxTree = CSharpSyntaxTree.ParseText(text);
            var root = syntaxTree.GetRoot();
            var rewriter = new Rewritter();
            root = rewriter.Visit(root);
            Assert.Equal(File.ReadAllText("To_Assert.txt"), FileConverter.Prettify(root));
        }
    }
}
