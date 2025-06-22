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
    }
}
