using AutoUp.Entities;
using AutoUp.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace AutoUp
{
    public class FtpModulesProvider : IModulesProvider
    {

        public FtpModulesProvider(string pathload, string pathdown, string serpath,string host,string login,string pass)
        {
            pathLoad = pathload;
            pathDownload = pathdown;
            ServerPath = serpath;
            Host = host;
            Login = login;
            Pass = pass;
        }


        FtpConnectionHandler ftp = new FtpConnectionHandler();
        List<Module> modules_ftp = new List<Module>();
        List<ModulOldVerssion> modulOldVerssions = new List<ModulOldVerssion>();
        private string pathLoad;
        private string pathDownload;
        private string ServerPath;
        private string Host, Login, Pass;
        public void Connect()
        {
            ftp.ConnectFtp(Host, new NetworkCredential(Login, Pass)); 
        }



        public Module[] ListModuleInfo()
        {
            modules_ftp.Clear();
            var files = readMapFile(ServerPath);
            foreach (var item in files)
            {
                modules_ftp.Add(item);
            }
            return modules_ftp.ToArray();
        }
        public Module GetModule(Guid id) {
            var modules = ListModuleInfo();
            return modules?.Where(x => x.Equals(id.ToString())).FirstOrDefault();
        }
        public object Download(string f)
        {
            
            return ftp.client.DownloadFile(pathLoad + "\\" + f, pathDownload + "/" + f);
        }

        public List<Module> readMapFile(string filePath)
        {
            List<Module> ServerYaml = new();
            var reader = new StreamReader(filePath);
            var deserializer = new DeserializerBuilder()
                 .WithNamingConvention(UnderscoredNamingConvention.Instance)
                 .Build();

            var p = deserializer.Deserialize<List<Module>>(reader);
            foreach (var modul in p)
            {
                ServerYaml.Add(modul);
            }
            return ServerYaml;
        }
    }
}
