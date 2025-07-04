using OverallAuthDEMO.EFcore.Model;

namespace OverallAuthv1._0.Domain.DTO
{
    public class RoleInfoDTO
    {
        private List<string> _menusName; // Fix CS0029: Change type to List<string>

        public int Id { get; set; }

        /// <summary>
        /// 角色名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string? Describe { get; set; }

        /// <summary>
        /// 拥有的菜单
        /// </summary>
        public List<string> menusName
        {
            get
            {
                if (_menusName == null || _menusName.Count == 0)
                {
                    return new List<string> { "该角色还未分配菜单" };
                }
                return _menusName; // Fix CS8603: Ensure _menusName is initialized
            }
            set
            {
                _menusName = value ?? new List<string>();
            }
        }

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
