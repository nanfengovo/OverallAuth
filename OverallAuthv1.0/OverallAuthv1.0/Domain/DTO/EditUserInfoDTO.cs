using OverallAuthDEMO.EFcore.Model;

namespace OverallAuthv1._0.Domain.DTO
{
    public class EditUserInfoDTO
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        //public string Pwd { get; set; }

        /// <summary>
        /// 该用户拥有的角色列表
        /// </summary>
        public string[] Roles { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string? Describe { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable { get; set; } = true;

    }
}
