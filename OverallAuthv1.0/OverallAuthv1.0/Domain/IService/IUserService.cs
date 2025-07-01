using OverallAuthDEMO.EFcore.Model;
using OverallAuthv1._0.Domain.DTO;

namespace OverallAuthv1._0.Domain.IService
{
    public interface IUserService
    {
        /// <summary>
        /// add user
        /// </summary>
        /// <returns></returns>
        Task<(bool success, string meg)> AddUserAsync(AddUser user);

        /// <summary>
        /// 验证是否存在
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<bool> UserIsexistAsync(string userName);

        /// <summary>
        /// 根据用户名获取用户信息
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>

        Task<UserInfo> GetUserByNameAsync(string userName);

        /// <summary>
        /// 单个或批量删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<(bool success, string msg)> DeleteUsersAsync(int[] id);

        /// <summary>
        /// 根据关键字搜索用户
        /// </summary>
        /// <param name="KeyWord"></param>
        /// <returns></returns>

        Task<(bool success, List<UserInfoDTO> Users,string msg)> SearchUsersAsync(SearchUserDTO KeyWord);


    }
}
