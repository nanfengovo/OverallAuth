using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OverallAuthDEMO.EFcore.Model
{
    internal class User
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Pwd { get; set; }

        public List<Role> Roles { get; set; } = new List<Role>(); 
    }
}
