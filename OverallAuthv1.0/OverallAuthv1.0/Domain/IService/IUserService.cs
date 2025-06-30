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

        Task<User> GetUserByNameAsync(string userName);


    }
}
