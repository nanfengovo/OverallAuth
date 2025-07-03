using Microsoft.EntityFrameworkCore;
using OverallAuthDEMO.EFcore;
using OverallAuthDEMO.EFcore.Model;
using OverallAuthv1._0.Domain.DTO;
using OverallAuthv1._0.Domain.IService;

namespace OverallAuthv1._0.Domain.Service
{
    public class RoleService : IRoleService
    {
        private readonly MyDbContext _dbContext;

        public RoleService(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<(bool success, string msg)> AddRoleAsync(AddRole role)
        {
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
                        UpdateTime = DateTime.Now
                    }; 

                    _dbContext.Roles.Add(newRole);
                    await _dbContext.SaveChangesAsync();
                    return (true, "添加成功");
                }
            }
            catch (Exception ex)
            {

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
                    return (true, result);
                }
                else if (!string.IsNullOrEmpty(search.Name) && string.IsNullOrEmpty(search.Menu))
                {
                    var result = await _dbContext.Roles.Where(r => r.Name.Contains(search.Name)).ToListAsync();
                    return (true, result);
                }
                else
                {
                    var result = await _dbContext.Roles.Where(r => r.Name.Contains(search.Name) && r.Menus.Any(m => m.Name.Contains(search.Menu))).ToListAsync();
                    return (true, result);
                }
            }
            catch (Exception ex)
            {

                return (false, ex.Message);
            }
        }
    }
}
