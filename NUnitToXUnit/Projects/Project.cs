using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NUnitToXUnit.Projects
{
    public class Project
    {
        private readonly ProjectXml _projectXml;

        public Project(string projectFile)
        {
            _projectXml = new ProjectXml(projectFile);
            ProjectPath = projectFile;
        }

        public bool HasXUnitReference => _projectXml.HasXUnitReference;
        public bool HasNUnitReference => _projectXml.HasNUnitReference;
        
        public string ProjectPath { get; }
        
        public void AddXUnitReference() => _projectXml.AddXUnitReference();

        public void RemoveNUnitReference() => _projectXml.RemoveNUnitReference();

        public List<string> Files
        {
            get
            {
                var directory = Path.GetDirectoryName(ProjectPath);
                if (directory == null) return new List<string>();
                var filePaths = Directory.GetFiles(directory, "*.cs", SearchOption.AllDirectories);
                return filePaths.Where(f => !f.Contains("bin") && !f.Contains("obj") && !f.Contains("AssemblyInfo")).ToList();
            }
        }
    }
}
