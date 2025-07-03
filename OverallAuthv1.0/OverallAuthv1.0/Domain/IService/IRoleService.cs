using OverallAuthv1._0.Domain.DTO;

namespace OverallAuthv1._0.Domain.IService
{
    public interface IRoleService
    {
        /// <summary>
        /// add role
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        Task<(bool success, string msg)> AddRoleAsync(AddRole role);

        /// <summary>
        /// 角色是否存在且启用
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        Task<bool> GetRoleByNameAsync(string roleName);

        /// <summary>
        /// 根据关键字搜索
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        Task<(bool success, string msg)> SearchByKeyWordAsync(searchRoleDTO search);
    }
}
