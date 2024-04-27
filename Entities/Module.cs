using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoUp.Entities
{
    public class Module
    {


        public string Name_File { get; set; }
        public string Discription { get; set; }
        public string? Version { get; set; }
        public string Extension { get; set; }

        public Module() { }
        public Module(string name_file, string discription, string version, string extension)
        {

            Name_File = name_file;
            Discription = discription;
            Version = version;
            Extension = extension;
        }
        public override string ToString()
        {
            return string.Format("- name_file : {0}\n" +
                "  discription : \"{1}\"\n" +
                "  version : \"{2}\"\n" +
                "  extension : {3}", this.Name_File, this.Discription, this.Version, this.Extension);
        }

    }
}
