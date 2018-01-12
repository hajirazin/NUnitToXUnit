using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Formatting;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using System;
using System.IO;
using System.Text.RegularExpressions;
using NUnitToXUnit.Converters;

namespace NUnitToXUnit
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger.Init();

            var converter = new SolutionConverter();
            converter.Convert(@"C:\TTL\web\source\WebTestComponents\src\Test.Common");

            //File convert
            Logger.Log("End ...");
            System.Console.ReadLine();
        }
    }
}
