using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OverallAuthv1._0.EFcore.Model;

namespace OverallAuthDEMO.EFcore.Model
{
    internal class Role:CommomModel
    {
        /// <summary>
        /// 角色id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 角色名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 该角色分配的用户
        /// </summary>
        public List<User> Users { get; set; }  = new List<User>();

        /// <summary>
        /// 该角色分配的菜单 
        /// </summary>
        public List<Menu> Menus { get; set; } = new List<Menu>();
    }
}
