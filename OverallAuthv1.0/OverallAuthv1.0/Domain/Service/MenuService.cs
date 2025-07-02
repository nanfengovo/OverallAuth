using Microsoft.EntityFrameworkCore;
using OverallAuthDEMO.EFcore;
using OverallAuthDEMO.EFcore.Model;
using OverallAuthv1._0.Domain.DTO;
using OverallAuthv1._0.Domain.IService;

namespace OverallAuthv1._0.Domain.Service
{
    public class MenuService : IMenuService
    {
        public readonly MyDbContext _dbContext;

        public MenuService(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<(bool success, string msg)> AddMenuAsync(AddMenuDTO menus)
        {
            try
            {
                var existingMenu = await _dbContext.Menus.FirstOrDefaultAsync(m => m.Name == menus.Name || m.URL == menus.URL);
                if(existingMenu != null)
                    return (false, "菜单已存在");
                else
                {
                    var menu = new Menu
                    {
                        Name = menus.Name,
                        Icon = menus.Icon,
                        URL = menus.URL,
                        Describe = menus.Describe,
                        IsEnable = menus.IsEnable,
                        CreateTime = menus.CreateTime,
                        UpdateTime = menus.UpdateTime
                    };
                    _dbContext.Menus.Add(menu);
                    await _dbContext.SaveChangesAsync();
                    return (true, "添加成功");
                }
            }
            catch (Exception ex)
            {
                return (false, $"添加失败: {ex.Message}");
            }
        }



        async Task<(bool success, List<Menu> menus)> IMenuService.GetAllMenuAsync()
        {
            var menus = await _dbContext.Menus.Where(x => x.IsEnable && !x.IsDeleted).ToListAsync();
            return (true, menus);
        }


        public async Task<(bool success, List<Menu> menus)> GetMenuByRoleAsync(string userName)
        {
            try
            {
                // 1. 通过用户名获取用户及其关联的所有角色ID
                var roleIds = await _dbContext.Users
                    .Where(u => u.Name == userName)
                    .SelectMany(u => u.Roles.Select(r => r.Id)) // 提取所有角色ID
                    .Distinct()
                    .ToListAsync();

                if (!roleIds.Any())
                    return (false, new List<Menu>());

                // 2. 通过角色ID列表查询菜单
                var menus = await _dbContext.Menus
                    .Where(m => m.Roles.Any(r => roleIds.Contains(r.Id))).ToListAsync(); // 匹配任意角色
  

                return (true, menus);
            }
            catch (Exception ex)
            {
                return (false, new List<Menu>());
            }
        }

        public async Task<(bool success, string msg)> DeleteMenusAsync(int[] ids)
        {
            try
            {
                var menus = await _dbContext.Menus.Where(m => ids.Contains(m.Id)).ToListAsync();
                foreach (var menu in menus)
                {
                    menu.IsDeleted = true;
                    menu.UpdateTime = DateTime.Now;
                }
                await _dbContext.SaveChangesAsync();
                return (true, "删除成功");

            }
            catch (Exception ex)
            {

                return (false, "服务端发生异常，异常信息为：" + ex.Message);
            }
        }
    }
}
