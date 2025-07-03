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

        public async Task<bool> GetRoleByNameAsync(string roleName)
        {
            var exist = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Name == roleName  && !r.IsDeleted);
            return exist != null;
        }
    }
}
