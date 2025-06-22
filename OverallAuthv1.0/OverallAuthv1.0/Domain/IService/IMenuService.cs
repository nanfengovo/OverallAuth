using OverallAuthDEMO.EFcore.Model;
using OverallAuthv1._0.Domain.DTO;

namespace OverallAuthv1._0.Domain.IService
{
    /// <summary>
    /// 菜单服务接口
    /// </summary>
    public interface IMenuService
    {
        /// <summary>
        /// 获取所有菜单
        /// </summary>
        /// <returns></returns>
        Task<(bool success, List<Menu> menus)> GetAllMenuAsync();

        /// <summary>
        /// 添加菜单
        /// </summary>
        /// <param name="menus"></param>
        /// <returns></returns>

        Task<(bool success, string msg)> AddMenuAsync(AddMenuDTO menus); 
    }
}
