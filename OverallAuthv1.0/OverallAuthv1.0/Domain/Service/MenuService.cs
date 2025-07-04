﻿using Microsoft.EntityFrameworkCore;
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
            var menus = await _dbContext.Menus.Where(x => !x.IsDeleted).ToListAsync();
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

        public async Task<(bool success, string msg)> EditMenuAsync(int id, AddMenuDTO menu)
        {
            try
            {
                // 验证必要字段
                if (string.IsNullOrWhiteSpace(menu.Name) ||
                    string.IsNullOrWhiteSpace(menu.URL) ||
                    string.IsNullOrWhiteSpace(menu.Describe))
                {
                    return (false, "菜单名、路由和菜单描述均不能为空！");
                }

                // 获取当前菜单（不限制启用状态，但需未删除）
                var existingMenu = await _dbContext.Menus
                    .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

                if (existingMenu == null)
                {
                    return (false, "菜单不存在或已被删除");
                }

                // 检查重复性（排除当前菜单自身）
                bool isDuplicate = await _dbContext.Menus
                    .AnyAsync(x => !x.IsDeleted &&
                                    x.Id != id &&
                                    (x.Name == menu.Name ||
                                    x.URL == menu.URL ||
                                    x.Describe == menu.Describe));

                if (isDuplicate)
                {
                    return (false, "修改失败！名称/URL/描述与其他菜单冲突");
                }

                // 更新字段
                existingMenu.Name = menu.Name;
                existingMenu.URL = menu.URL;
                existingMenu.Describe = menu.Describe;
                existingMenu.Icon = menu.Icon;
                existingMenu.IsEnable = menu.IsEnable;
                existingMenu.UpdateTime = DateTime.Now;

                // 提交更改
                await _dbContext.SaveChangesAsync();
                return (true, "菜单修改成功");
            }
            catch (Exception ex)
            {
                // 实际项目中建议记录日志
                return (false, $"服务器错误: {ex.Message}");
            }
        }

        public async Task<(bool success, object obj)> SearchByKeyWordAsync(SearchMenuDTO searchMenu)
        {
            try
            {
                if(string.IsNullOrEmpty(searchMenu.Name) && string.IsNullOrEmpty(searchMenu.Describe))
                {
                    return (false, "请输入查询条件");
                }
                else if(!string.IsNullOrEmpty(searchMenu.Name) && string.IsNullOrEmpty(searchMenu.Describe))
                {
                    var menus = _dbContext.Menus.Where(x => x.Name.Contains(searchMenu.Name) && !x.IsDeleted).ToList();
                    
                    return (true,menus);
                }
                else if(string.IsNullOrEmpty(searchMenu.Name) && !string.IsNullOrEmpty(searchMenu.Describe))
                {
                    var menus = _dbContext.Menus.Where(x => x.Describe.Contains(searchMenu.Describe) && !x.IsDeleted).ToList();
                    return (true, menus);
                }
                else
                {
                    var menus = _dbContext.Menus.Where(x => x.Name.Contains(searchMenu.Name) && x.Describe.Contains(searchMenu.Describe) && !x.IsDeleted).ToList();
                    return (true,menus);
                }
            }
            catch (Exception ex)
            {

                return (false, "服务端发生异常，异常信息为："+ex.Message);
            }
        }


    }
}
