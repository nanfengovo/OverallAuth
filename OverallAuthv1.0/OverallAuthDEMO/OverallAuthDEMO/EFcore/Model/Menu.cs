using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OverallAuthDEMO.EFcore.Model
{
    internal class Menu
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<Role> Roles { get; set; } = new List<Role>();
    }
}
