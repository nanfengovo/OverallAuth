using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OverallAuthDEMO.EFcore.Model
{
    internal class Role
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<User> Users { get; set; }  = new List<User>();

        public List<Menu> Menus { get; set; } = new List<Menu>();
    }
}
