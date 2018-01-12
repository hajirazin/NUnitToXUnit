using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace NUnitToXUnit.Projects
{
    public class ProjectXml
    {
        private readonly string _projectPath;
        private string _projectFileText;
        private const string BuildNameSpace = "xmlns=\"http://schemas.microsoft.com/developer/msbuild/2003\"";
        private const string BuildTarget = "DefaultTargets=\"Build\"";

        public ProjectXml(string projectPath)
        {
            _projectPath = projectPath;
            _projectFileText = File.ReadAllText(projectPath);
            HasXUnitReference = _projectFileText.Contains("xunit");
            HasNUnitReference = _projectFileText.Contains("nunit");
        }

        public bool HasXUnitReference { get; }
        public bool HasNUnitReference { get; }
        
        public void RemoveNUnitReference()
        {
            RemoveReference("nunit");
        }

        private void RemoveReference(string mockString)
        {
            var xml = XDocument.Parse(_projectFileText.Replace(BuildNameSpace, ""));
            xml.Descendants("Reference").Where(f => f.Attribute("Include")?.Value.Contains(mockString) ?? false).Remove();
            var sb = new StringBuilder();
            xml.Save(new StringWriter(sb));
            sb = sb.Replace(BuildTarget, $"{BuildTarget} {BuildNameSpace}").Replace("utf-16", "utf-8");
            
            _projectFileText = sb.ToString();
            File.WriteAllText(_projectPath, _projectFileText, Encoding.UTF8);
        }

        public void AddXUnitReference()
        {
            var xml = XDocument.Parse(_projectFileText.Replace(BuildNameSpace, ""));
            var x = xml.Descendants("ItemGroup").ToList();
            var d = x.First();
            var import = new XElement("Import");
            import.SetAttributeValue("Project", @"$(SolutionDir)\..\packages\xunit.core\build\xunit.core.props");
            import.SetAttributeValue("Condition", @"Exists('$(SolutionDir)\..\packages\xunit.core\build\xunit.core.props')");
            d.AddBeforeSelf(import);
            d = x.FirstOrDefault(f => f.Descendants("Reference").Any());
            if (d != null)
            {
                var xElement = new XElement("Reference",
                    new XElement("HintPath", @"$(SolutionDir)\..\packages\xunit.abstractions\lib\net35\xunit.abstractions.dll"));
                xElement.SetAttributeValue("Include", "xunit.abstractions");
                d.Add(xElement);

                xElement = new XElement("Reference",
                    new XElement("HintPath", @"$(SolutionDir)\..\packages\xunit.assert\lib\netstandard1.1\xunit.assert.dll"));
                xElement.SetAttributeValue("Include", "xunit.assert");
                d.Add(xElement);

                xElement = new XElement("Reference",
                    new XElement("HintPath", @"$(SolutionDir)\..\packages\xunit.extensibility.core\lib\netstandard1.1\xunit.core.dll"));
                xElement.SetAttributeValue("Include", "xunit.core");
                d.Add(xElement);

                xElement = new XElement("Reference",
                    new XElement("HintPath", @"$(SolutionDir)\..\packages\xunit.extensibility.execution.2.3.1\lib\net452\xunit.execution.desktop.dll"));
                xElement.SetAttributeValue("Include", "xunit.execution.desktop");
                d.Add(xElement);

                var sb = new StringBuilder();
                xml.Save(new StringWriter(sb));
                sb = sb.Replace(BuildTarget, $"{BuildTarget} {BuildNameSpace}").Replace("utf-16", "utf-8");
                _projectFileText = sb.ToString();
                File.WriteAllText(_projectPath, _projectFileText);
            }
        }
    }
}
