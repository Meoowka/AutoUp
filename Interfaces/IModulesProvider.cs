using System;
using AutoUp.Entities;

namespace AutoUp.Interfaces
{
    public interface IModulesProvider
    {
        public Module[] ListModuleInfo();
        public Module GetModule(Guid id);
        public object Download(string path);
        public void Connect();
    }
}
