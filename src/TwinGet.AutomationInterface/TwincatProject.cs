// This file is licensed to you under MIT license.

using TwinGet.AutomationInterface.Exceptions;
using TwinGet.AutomationInterface.Utils;

namespace TwinGet.AutomationInterface
{
    internal class TwincatProject
    {
        private EnvDTE.Project _project;

        public TwincatProject(EnvDTE.Project project)
        {
            if (!TwincatUtils.IsTwincatProject(project))
            {
                throw new NotATwincatProject($"The provided {project.Name} is not a TwinCAT project.");
            }

            _project = project;
        }
    }
}
