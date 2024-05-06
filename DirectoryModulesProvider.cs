using AutoUp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using AutoUp.Entities;
using System.IO;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;

namespace AutoUp
{
    public class DirectoryModulesProvider : IModulesProvider
    {
        public DirectoryModulesProvider(string path)
        {
            filePath = path;
        }

        private string filePath ;
        List<Module> modules = new List<Module>();
        public Module[] ListModuleInfo()
        {
            modules.Clear();
            var files = readMapFile(filePath);
            foreach (var item in files)
            {
                modules.Add(item);
            }
            //List<ModulOldVerssion> result = new();

            //foreach (var module in modules)
            //{
            //    ModulOldVerssion modulOldVerssion = new ModulOldVerssion
            //    {
            //        Name_File = module.Name_File,
            //        Discription = module.Discription,
            //        Version = module.Version,
            //        Extension = module.Extension,
            //        OldVersion = string.Empty
            //    };
            //    result.Add(modulOldVerssion);
            //}
            return modules.ToArray();
          
          
        }
        public Module GetModule(Guid id)
        {
            var modules = ListModuleInfo();
            return modules?.Where(x => x.Equals(id.ToString())).FirstOrDefault();
        }

        //public List<ModulOldVerssion> MapModules()
        //{
        //    List<ModulOldVerssion> result = new();
        //    foreach (var module in modules)
        //    {
        //        ModulOldVerssion modulOldVerssion = new ModulOldVerssion
        //        {
        //            Name_File = module.Name_File,
        //            Discription = module.Discription,
        //            Version = module.Version,
        //            Extension = module.Extension,
        //            OldVersion = string.Empty
        //        };
        //        result.Add(modulOldVerssion);
        //    }
        //    return result;
        //}
        public List<Module> readMapFile(string filePath)
        {
            List<Module> LocalYml = new();
            using (var reader = new StreamReader(filePath))
            {

                var deserializer = new DeserializerBuilder()
                 .WithNamingConvention(UnderscoredNamingConvention.Instance)
                 .Build();

                var p = deserializer.Deserialize<List<Module>>(reader);
                foreach (var modul in p)
                {
                    LocalYml.Add(modul);
                }
                return LocalYml;
            }
        }

        public void Connect()
        {
            throw new NotImplementedException();
        }
        public object Download(string f)
        {
            throw new NotImplementedException();


        }
    }
}
        

            

           
        







