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
            var menus = await _dbContext.Menus.ToListAsync();
            return (true, menus);
        }
    }
}
