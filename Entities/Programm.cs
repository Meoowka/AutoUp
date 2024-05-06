using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoUp.Entities
{
    public class Programm : Users
    {
        public int idProgrmm { get; set; }
        public Users idUser { get; set; }
        public string NameProgramm {get;set;}
        public string NameVersion { get;set;}
        public DateTime DateChanges { get; set; }
        public long SizeProgramm { get; set; }
        public string StatusProgramm { get; set; }

        public Programm(Users users, string nameProgramm,string nameVersion,DateTime dataChanges,long sizeProgramm, string Status)
        {
            idUser = users;
            NameProgramm = nameProgramm;
            NameVersion = nameVersion;
            DateChanges = dataChanges;
            SizeProgramm = sizeProgramm;
            StatusProgramm = Status;            
        }

    }
}
