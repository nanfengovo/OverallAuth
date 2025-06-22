
namespace OverallAuthv1._0.Domain.IService
{
    public interface IOverallAuthService
    {
        Task<(bool success,string msg)> GiveRoleMenuAsync(string roleName, int[] menuIds);
    }
}
