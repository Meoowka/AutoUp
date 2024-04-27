using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoUp.Entities
{
    public class ModulOldVerssion : Module
    {
       public string? OldVersion { get; set; }

        public override string ToString()
        {
            return string.Format("- name_file : {0}\n" +
                "  discription : {1}\n" +
                "  oldVersion: { 2}\n" +
                "  version : \"{3}\"\n" +
                "  extension : {4}"
                ,this.Name_File, this.Discription, this.OldVersion, this.Version, this.Extension);
        }

    }
}
