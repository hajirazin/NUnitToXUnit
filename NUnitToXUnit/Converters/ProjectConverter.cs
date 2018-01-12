using System;
using System.IO;
using System.Threading.Tasks;
using NUnitToXUnit.Core;
using NUnitToXUnit.Projects;

namespace NUnitToXUnit.Converters
{
    public interface IProjectConverter
    {
        void Convert(Project project);
    }

    public class ProjectConverter : IProjectConverter
    {
        public void Convert(Project project)
        {
            if (!project.HasNUnitReference)
            {
                return;
            }

            Logger.Log($"Starting Convertion of project {Path.GetFileNameWithoutExtension(project.ProjectPath)}", ConsoleColor.Green);

            if (project.HasNUnitReference)
                project.RemoveNUnitReference();

            if (!project.HasXUnitReference)
                project.AddXUnitReference();

            Parallel.ForEach(project.Files, projectFile =>
            {
                if (project.HasNUnitReference)
                {
                    var rewriter = new Rewritter();
                    var fileConverter = new FileConverter(rewriter);
                    fileConverter.Convert(projectFile);
                }
            });
        }
    }
}
