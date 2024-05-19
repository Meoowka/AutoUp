using AutoUp.Entities;
using AutoUp.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;

namespace AutoUp
{
    public class IgnorModule : IModulesProvider
    {
        public IgnorModule(string path)
        {
            filePath = path;
        }

        private string filePath;
        List<Module> Ignors = new List<Module>();
        public Module[] ListModuleInfo()
        {
            Ignors.Clear();
            if (File.Exists(filePath))
            {
                var files = readMapFile(filePath);
                foreach (var item in files)
                {
                    Ignors.Add(item);
                }
            }

            return Ignors.ToArray();
        }
        public List<Module> readMapFile(string filePath)
        {
            List<Module> LocalIgnor = new();
            using (var reader = new StreamReader(filePath))
            {

                var deserializer = new DeserializerBuilder()
                 .WithNamingConvention(UnderscoredNamingConvention.Instance)
                 .Build();

                var p = deserializer.Deserialize<List<Module>>(reader);
                foreach (var modul in p)
                {
                    LocalIgnor.Add(modul);
                }
                return LocalIgnor;
            }
        }
        public void Connect()
        {
            throw new NotImplementedException();
        }
        public object Download(string path)
        {
            throw new NotImplementedException();
        }
        public Module GetModule(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
