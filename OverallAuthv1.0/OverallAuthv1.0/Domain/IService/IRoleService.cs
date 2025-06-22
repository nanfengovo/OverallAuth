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
    }
}
