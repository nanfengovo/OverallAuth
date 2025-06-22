using Microsoft.EntityFrameworkCore;
using OverallAuthDEMO.EFcore;
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
                var role = await _dbcontext.Roles.FirstOrDefaultAsync(x => x.Name == roleName && x.IsDeleted == false && x.IsEnable);
                // 一次性查询所有匹配的菜单项
                var menus = _dbcontext.Menus
                    .Where(x => menuIds.Contains(x.Id))  // 批量查询
                    .AsNoTracking()                      // 禁用变更跟踪（仅读场景）
                    .ToList();                           // 立即执行查询

                role.Menus.Clear();     // 清除原有权限
                role.Menus.AddRange(menus); // 批量添加新权限

                await _dbcontext.SaveChangesAsync();
                await transaction.CommitAsync();

                return (true, $"已成功分配 {menus.Count} 个菜单权限");
            }
            catch (DbUpdateException dbEx)
            {
                await transaction.RollbackAsync();
                return (false, $"数据库更新错误: {dbEx.Message}");
            }
            catch (InvalidOperationException invOpEx)
            {
                await transaction.RollbackAsync();
                return (false, $"操作错误: {invOpEx.Message}");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
          

        }
    }
}
