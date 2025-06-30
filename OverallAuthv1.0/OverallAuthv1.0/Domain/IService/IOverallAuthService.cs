
using OverallAuthDEMO.EFcore.Model;
using OverallAuthv1._0.Domain.DTO;

namespace OverallAuthv1._0.Domain.IService
{
    public interface IOverallAuthService
    {
        /// <summary>
        /// 给角色分配菜单
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="menuIds"></param>
        /// <returns></returns>
        Task<(bool success,string msg)> GiveRoleMenuAsync(string roleName, int[] menuIds);

        /// <summary>
        /// 给用户分配角色
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
        Task<(bool success, string msg)> GiveUserRoleAsync(string roleName, string[] rolesName);

        /// <summary>
        /// 获取所有角色
        /// </summary>
        /// <returns></returns>
        Task<(bool success, List<RoleInfoDTO> roles)> GetAllRoleAsync();


        /// <summary>
        /// 获取所有的用户
        /// </summary>
        /// <returns></returns>
        Task<(bool success, List<UserInfoDTO> user, string Msg)> GetAllUserAsync();

        /// <summary>
        /// 编辑用户信息
        /// </summary>
        /// <returns></returns>
        Task<(bool success, string msg)> EditUserInfoAsync(int id, EditUserInfoDTO editUserInfo);
    }
}
