using Microsoft.EntityFrameworkCore;
using OverallAuthDEMO.EFcore;
using OverallAuthDEMO.EFcore.Model;
using OverallAuthv1._0.Domain.DTO;
using OverallAuthv1._0.Domain.IService;

namespace OverallAuthv1._0.Domain.Service
{
    public class UserService : IUserService
    {
        private readonly MyDbContext _dbcontext;

        public UserService(MyDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<(bool success, string meg)> AddUserAsync(AddUser user)
        {
            try
            {
                var existuser = await _dbcontext.Users.FirstOrDefaultAsync(u => u.Name == user.Name);
                if (existuser != null)
                {
                    return (false, "用户已存在");
                }
                else
                {
                    var newuser = new User()
                    {
                        Name = user.Name,
                        Pwd = user.Pwd,
                        Describe = user.Describe,
                        IsEnable = user.IsEnable,
                        CreateTime = DateTime.Now,
                        UpdateTime = DateTime.Now,
                    };
                    _dbcontext.Add(newuser);
                    await _dbcontext.SaveChangesAsync();
                    return (true, "添加成功");
                }
            }
            catch (Exception ex)
            {

                return (false, ex.Message);
            }
        }

        public async Task<bool> UserIsexistAsync(string userName)
        {
            var exist = await _dbcontext.Users.FirstOrDefaultAsync(u => u.Name == userName && u.IsEnable && !u.IsDeleted);
            return exist != null;
        }


        public async Task<User> GetUserByNameAsync(string userName)
        {
            var exist = await _dbcontext.Users.FirstOrDefaultAsync(u => u.Name == userName && u.IsEnable && !u.IsDeleted);
            return exist;
        }
    }
}
