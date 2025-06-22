using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OverallAuthv1._0.EFcore.Model;

namespace OverallAuthDEMO.EFcore.Model
{
    public class Menu:CommomModel
    {
        /// <summary>
        /// 菜单ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 菜单名
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// 菜单图标
        /// </summary>
        public string? Icon { get; set; }

        /// <summary>
        /// 菜单路径
        /// </summary>
        public string? URL { get; set; }

        /// <summary>
        /// 拥有该菜单的角色
        /// </summary>
        public List<Role> Roles { get; set; } = new List<Role>();
    }
}
