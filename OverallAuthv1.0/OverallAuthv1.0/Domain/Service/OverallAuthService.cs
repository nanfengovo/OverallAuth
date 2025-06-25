using Microsoft.EntityFrameworkCore;
using OverallAuthDEMO.EFcore;
using OverallAuthDEMO.EFcore.Model;
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
    }
}
