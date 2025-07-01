using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OverallAuthv1._0.EFcore.Model;

namespace OverallAuthDEMO.EFcore.Model
{
    public class UserInfo:CommomModel
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Pwd { get; set; }

        /// <summary>
        /// 该用户拥有的角色列表
        /// </summary>
        public List<Role> Roles { get; set; } = new List<Role>(); 
    }
}
