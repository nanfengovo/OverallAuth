using Microsoft.EntityFrameworkCore;
using OverallAuthDEMO.EFcore;
using OverallAuthDEMO.EFcore.Model;
using OverallAuthv1._0.Domain.DTO;
using OverallAuthv1._0.Domain.IService;

namespace OverallAuthv1._0.Domain.Service
{
    public class OverallAuthService : IOverallAuthService
    {
        private readonly MyDbContext _dbcontext;

        public OverallAuthService(MyDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<(bool success, string msg)> GiveRoleMenuAsync(string roleName, int[] menuIds)
        {

            using var transaction = await _dbcontext.Database.BeginTransactionAsync();
            try
            {
                var role = await _dbcontext.Roles
                    .Include(r => r.Menus)  // 确保加载关联的菜单
                    .FirstOrDefaultAsync(x => x.Name == roleName && x.IsDeleted == false && x.IsEnable);

                if (role == null)
                {
                    await transaction.RollbackAsync();
                    return (false, "角色不存在或已被禁用");
                }

                // 获取要移除的菜单
                var menusToRemove = await _dbcontext.Menus
                    .Where(x => x.IsDeleted == false && x.IsEnable)
                    .ToListAsync();

                // 批量移除（不在循环中保存）
                foreach (var item in menusToRemove)
                {
                    role.Menus.Remove(item);
                }

                var addMenu = await _dbcontext.Menus
                    .Where(x => x.IsDeleted == false && x.IsEnable && menuIds.Contains(x.Id))
                    .ToListAsync();

                role.Menus.AddRange(addMenu);

                // 只保存一次
                await _dbcontext.SaveChangesAsync();
                await transaction.CommitAsync();

                return (true, $"已成功分配 菜单权限");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return (false, ex.Message);
            }



        }

        public async Task<(bool success, string msg)> GiveUserRoleAsync(string roleName, string[] rolesName)
        {
            using var transaction = _dbcontext.Database.BeginTransaction();
            try
            {
                var user = await _dbcontext.Users.FirstOrDefaultAsync(x => x.Name == roleName && !x.IsDeleted && x.IsEnable);
                if (user == null)
                {
                    await transaction.RollbackAsync();
                    return (false, "用户不存在或已被禁用");
                }
                // 获取要移除的角色
                var rolesToRemove = await _dbcontext.Roles.Include(x => x.Users).Where(x => x.IsDeleted == false && x.IsEnable).ToListAsync();

                foreach (var role in rolesToRemove)
                {
                    user.Roles.Remove(role);
                }

                var addRole = await _dbcontext.Roles.Where(x =>  x.IsDeleted == false && x.IsEnable && rolesName.Contains(x.Name)).ToListAsync();

                user.Roles.AddRange(addRole);

                // 只保存一次
                await _dbcontext.SaveChangesAsync();
                await transaction.CommitAsync();

                return (true, $"已成功分配角色");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return (false, ex.Message);
            }
        }


        public async Task<(bool success,List<RoleInfoDTO> roles)> GetAllRoleAsync()
        {
            try
            {
                var roles = await _dbcontext.Roles.Where(x => x.IsDeleted == false ).ToListAsync();

                var newRoles = new List<RoleInfoDTO>();
                foreach (var item in roles)
                {
                    var roleId = item.Id;
                    var roleMenus = await _dbcontext.Menus
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
                if (newRoles != null && newRoles.Count > 0)
                {

                    return (true, newRoles);
                }
                else
                {
                    return (false, new List<RoleInfoDTO>());
                }
            }
            catch (Exception ex)
            {

                return (false,new List<RoleInfoDTO>());
            }

           
        }

        public async Task<(bool success, List<UserInfoDTO> user, string Msg)> GetAllUserAsync()
        {
            try
            {
                var user = await _dbcontext.Users.Where(x => x.IsDeleted == false ).ToListAsync();
                var newUser = new List<UserInfoDTO>();
                foreach (var item in user)
                {
                    var userid = item.Id;
                    var userRoles = await _dbcontext.Roles
                        .Where(x => x.IsDeleted == false && x.IsEnable && x.Users.Any(u => u.Id == userid))
                        .ToListAsync();
                    var userInfo = new UserInfoDTO
                    {
                        Name = item.Name,
                        Describe = item.Describe,
                        RolesName = userRoles.Select(x => x.Name).ToList(),
                        IsEnable = item.IsEnable,
                        CreateTime = item.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                        UpdateTime = item.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss")
                    };
                    newUser.Add(userInfo);
                }
                if (user != null && user.Count > 0)
                {
                    return (true, newUser, "查询成功");
                }
                else
                {
                    return (false, new List<UserInfoDTO>(), "查询失败");
                }
            }
            catch (Exception ex)
            {

                return (false, new List<UserInfoDTO>(), "查询失败"+ex.Message);
            }
        }
    }
}
