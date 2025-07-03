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

        /// <summary>
        /// 获取某个用户拥有的菜单
        /// </summary>
        /// <returns></returns>
        Task<(bool success, List<Menu> menus)> GetMenuByRoleAsync(string userName);

        /// <summary>
        /// 删除（单个/批量）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<(bool success, string msg)> DeleteMenusAsync(int[] ids);

        /// <summary>
        /// 编辑菜单
        /// </summary>
        /// <param name="id"></param>
        /// <param name="menu"></param>
        /// <returns></returns>
        Task<(bool success,string msg)> EditMenuAsync(int id, AddMenuDTO menu);
    }
}
