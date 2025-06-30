using OverallAuthDEMO.EFcore.Model;

namespace OverallAuthv1._0.Domain.DTO
{
    public class UserInfoDTO
    {
        private List<string> _rolesName; // Fix CS0029: Change type to List<string>

        /// <summary>
        /// 用户Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 该用户拥有的角色列表
        /// </summary>
        public List<string> RolesName
        {
            get
            {
                if (_rolesName == null || _rolesName.Count == 0)
                {
                    return new List<string> { "该用户还未分配角色" };
                }
                return _rolesName; // Fix CS8603: Ensure _rolesName is initialized
            }
            set
            {
                _rolesName = value ?? new List<string>();
            }
        }

        /// <summary>
        /// 描述
        /// </summary>
        public string? Describe { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable { get; set; } = true;


        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime { get; set; } 

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public string UpdateTime { get; set; }
    }
}
