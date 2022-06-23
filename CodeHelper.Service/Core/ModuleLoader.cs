using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using CodeHelper.Service.Core;

namespace CodeHelper.Service
{
    internal class ModuleLoader
    {
        IEnumerable<string> ModuleDirectories => File.ReadAllLines("Configs/ModuleDirectories.cfg");

        public IEnumerable<Module> ModulesList =>
            ModuleDirectories.Where(str => str.Any())
                .SelectMany(moduleDirectory => new DirectoryInfo(moduleDirectory)
                .GetDirectories())
                .Select(modules => new Module(modules.Name, modules.FullName));

        public void StartProcess(string name, IEnumerable<string> args)
        {
            Process.Start(name, args);

        }

    }

}
