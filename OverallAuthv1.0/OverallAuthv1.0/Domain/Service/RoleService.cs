using Microsoft.EntityFrameworkCore;
using OverallAuthDEMO.EFcore;
using OverallAuthDEMO.EFcore.Model;
using OverallAuthv1._0.Domain.DTO;
using OverallAuthv1._0.Domain.IService;
using System.Data;

namespace OverallAuthv1._0.Domain.Service
{
    public class RoleService : IRoleService
    {
        private readonly MyDbContext _dbContext;

        private readonly IOverallAuthService _overallAuthService;

        public RoleService(MyDbContext dbContext, IOverallAuthService overallAuthService)
        {
            _dbContext = dbContext;
            _overallAuthService = overallAuthService;
        }

        /// <summary>
        /// 添加角色 （添加基本的角色信息+给角色分配菜单）
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public async Task<(bool success, string msg)> AddRoleAsync(AddRoleDTO role)
        {
            // 开启事务
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                
                var exist = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Name == role.Name);
                if(exist != null)
                {
                    return (false, "角色已存在");
                }
                else
                {
                    var newRole = new Role 
                    {
                        Name = role.Name,
                        Describe = role.Describe,
                        IsEnable = role.IsEnable,
                        CreateTime = DateTime.Now,
                        UpdateTime = DateTime.Now,
                        IsDeleted = false
                        
                    };

                    _dbContext.Roles.Add(newRole);
                    await _dbContext.SaveChangesAsync();

                    #region 给角色分配菜单
                    var result = await _overallAuthService.GiveRoleMenuAsync(role.Name, role.menuIds);
                    #endregion

                  
                    

                    if(result.success)
                    {
                        await transaction.CommitAsync();//提交事务
                        return (true, "添加成功");
                    }
                    else
                    {
                        await transaction.RollbackAsync();//回滚事务
                        return (false,"添加失败"+result.msg);
                    }

                    
                }
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();//回滚事务
                return (false, ex.Message);
            }
        }

        public async Task<(bool success, string msg)> DeleteRoleByIdAsync(int[] ids)
        {
            try
            {
                var roles = await _dbContext.Roles.Where(x => ids.Contains(x.Id) && !x.IsDeleted).ToListAsync();  
                if(roles.Count == 0 || roles ==null)
                {
                    return (false, "角色不存在");
                }
                else
                {
                    foreach (var role in roles)
                    {
                        role.IsDeleted = true;
                        role.UpdateTime = DateTime.Now; 
                    }
                    await _dbContext.SaveChangesAsync();
                    return (true, "删除成功");
                }

            }
            catch (Exception ex)
            {

                return (false,"服务端异常"+ ex.Message);
            }
        }

        public async Task<(bool success, string msg)> EditRoleAsync(int id, AddRoleDTO editRole)
        {
            //开启事务
            await using var tran = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var exist = await _dbContext.Roles.FirstOrDefaultAsync(x =>x.Id == id && !x.IsDeleted );
                if(exist == null)
                {
                    return (false, "该角色被删除或不存在！！");
                }
                else
                {
                    var rolename = exist.Name;
                    #region 给角色分配菜单
                    var result = await _overallAuthService.GiveRoleMenuAsync(rolename, editRole.menuIds);
                    #endregion
                    exist.Name = editRole.Name;
                    var isSaneName = await _dbContext.Roles.AnyAsync(x => x.Id != id && x.Name == editRole.Name); 
                    if(isSaneName)
                    {
                        return (false, "需要修改的角色名已经存在！！");
                    }
                    else
                    {
                        exist.IsEnable = editRole.IsEnable;
                        exist.Describe = editRole.Describe;
                        exist.UpdateTime = DateTime.Now;
                        await _dbContext.SaveChangesAsync();
                    }

                    




                    if (result.success)
                    {
                        await tran.CommitAsync();//提交事务
                        return (true, "编辑成功");
                    }
                    else
                    {
                        await tran.RollbackAsync();//回滚事务
                        return (false, "编辑失败" + result.msg);
                    }
                }
            }
            catch (Exception ex)
            {
                await tran.RollbackAsync();//回滚事务
                return (false,ex.Message);
            }
        }

        public async Task<bool> GetRoleByNameAsync(string roleName)
        {
            var exist = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Name == roleName  && !r.IsDeleted);
            return exist != null;
        }

        public async Task<(bool success, object obj)> SearchByKeyWordAsync(searchRoleDTO search)
        {
            try
            {
                if(string.IsNullOrEmpty(search.Name) && string.IsNullOrEmpty(search.Menu))
                {
                    return (false,"请输入搜索的关键字");
                }
                else if(string.IsNullOrEmpty(search.Name) && !string.IsNullOrEmpty(search.Menu))
                {
                    var result = await _dbContext.Roles.Where(r => r.Menus.Any(m => m.Name.Contains(search.Menu))).ToListAsync();
                    var newRoles = new List<RoleInfoDTO>();
                    foreach (var item in result)
                    {
                        var roleId = item.Id;
                        var roleMenus = await _dbContext.Menus
                            .Where(x => x.IsDeleted == false && x.IsEnable && x.Roles.Any(r => r.Id == roleId))
                            .ToListAsync();

                        var role = new RoleInfoDTO
                        {

                            Name = item.Name,
                            Describe = item.Describe,
                            menusName = roleMenus.Select(x => x.Name).ToList(),
                            IsEnable = item.IsEnable,
                            CreateTime = item.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                            UpdateTime = item.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss")
                        };
                        newRoles.Add(role);
                    }
                    return (true, newRoles);
                }
                else if (!string.IsNullOrEmpty(search.Name) && string.IsNullOrEmpty(search.Menu))
                {
                    var result = await _dbContext.Roles.Where(r => r.Name.Contains(search.Name)).ToListAsync();
                    var newRoles = new List<RoleInfoDTO>();
                    foreach (var item in result)
                    {
                        var roleId = item.Id;
                        var roleMenus = await _dbContext.Menus
                            .Where(x => x.IsDeleted == false && x.IsEnable && x.Roles.Any(r => r.Id == roleId))
                            .ToListAsync();

                        var role = new RoleInfoDTO
                        {

                            Name = item.Name,
                            Describe = item.Describe,
                            menusName = roleMenus.Select(x => x.Name).ToList(),
                            IsEnable = item.IsEnable,
                            CreateTime = item.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                            UpdateTime = item.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss")
                        };
                        newRoles.Add(role);
                    }
                    return (true, newRoles);
                }
                else
                {
                    var result = await _dbContext.Roles.Where(r => r.Name.Contains(search.Name) && r.Menus.Any(m => m.Name.Contains(search.Menu))).ToListAsync();
                    var newRoles = new List<RoleInfoDTO>();
                    foreach (var item in result)
                    {
                        var roleId = item.Id;
                        var roleMenus = await _dbContext.Menus
                            .Where(x => x.IsDeleted == false && x.IsEnable && x.Roles.Any(r => r.Id == roleId))
                            .ToListAsync();

                        var role = new RoleInfoDTO
                        {

                            Name = item.Name,
                            Describe = item.Describe,
                            menusName = roleMenus.Select(x => x.Name).ToList(),
                            IsEnable = item.IsEnable,
                            CreateTime = item.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                            UpdateTime = item.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss")
                        };
                        newRoles.Add(role);
                    }
                    return (true, newRoles);
                }
            }
            catch (Exception ex)
            {

                return (false, ex.Message);
            }
        }
    }
}
