using Microsoft.EntityFrameworkCore;
using OverallAuthDEMO.EFcore;
using OverallAuthDEMO.EFcore.Model;
using OverallAuthv1._0.Domain.DTO;
using OverallAuthv1._0.Domain.IService;
using System.Xml.Linq;
using System;

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
                    var newuser = new UserInfo()
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


        public async Task<UserInfo> GetUserByNameAsync(string userName)
        {
            var exist = await _dbcontext.Users.FirstOrDefaultAsync(u => u.Name == userName && u.IsEnable && !u.IsDeleted);
            return exist;
        }

        public async Task<(bool success, string msg)> DeleteUsersAsync(int[] id)
        {
            var users = await _dbcontext.Users.Where(u => id.Contains(u.Id)&& !u.IsDeleted).ToListAsync();
            if (users == null || users.Count == 0)
            {
                return (false, "用户不存在或已被删除");
            }
            else
            {
                foreach (var user in users)
                {
                    user.IsDeleted = true;
                    user.UpdateTime = DateTime.Now;
                }
                await _dbcontext.SaveChangesAsync();
                return (true, "删除成功");
            }
        }

        public async Task<(bool success, List<UserInfoDTO> Users,string msg)> SearchUsersAsync(SearchUserDTO KeyWord)
        {
            try
            {
                if(string.IsNullOrEmpty(KeyWord.name) && string.IsNullOrEmpty(KeyWord.role))
                {
                    return(false, new List<UserInfoDTO>(),"请提供搜索的关键字！！");
                }
                else if(KeyWord.name != null && string.IsNullOrEmpty(KeyWord.role))
                {
                    var users = await _dbcontext.Users.Where(u => u.Name.Contains(KeyWord.name) && !u.IsDeleted).ToListAsync();
                    var newUser = new List<UserInfoDTO>();
                    foreach (var item in users)
                    {
                        var userid = item.Id;
                        var userRoles = await _dbcontext.Roles
                            .Where(x => x.IsDeleted == false && x.IsEnable && x.Users.Any(u => u.Id == userid))
                            .ToListAsync();
                        var userInfo = new UserInfoDTO
                        {
                            Id = item.Id,
                            Name = item.Name,
                            Describe = item.Describe,
                            RolesName = userRoles.Select(x => x.Name).ToList(),
                            IsEnable = item.IsEnable,
                            CreateTime = item.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                            UpdateTime = item.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss")
                        };
                        newUser.Add(userInfo);
                    }
                    return (true, newUser, "查询成功");
                }
                else if ( string.IsNullOrEmpty(KeyWord.name) && KeyWord.role != null)
                {
                    var roleid = _dbcontext.Roles.FirstOrDefault(r => r.Name == KeyWord.role);
                    var users = await _dbcontext.Users.Where(u => u.Roles.Contains(roleid) && !u.IsDeleted).ToListAsync();
                    var newUser = new List<UserInfoDTO>();
                    foreach (var item in users)
                    {
                        var userid = item.Id;
                        var userRoles = await _dbcontext.Roles
                            .Where(x => x.IsDeleted == false && x.IsEnable && x.Users.Any(u => u.Id == userid))
                            .ToListAsync();
                        var userInfo = new UserInfoDTO
                        {
                            Id = item.Id,
                            Name = item.Name,
                            Describe = item.Describe,
                            RolesName = userRoles.Select(x => x.Name).ToList(),
                            IsEnable = item.IsEnable,
                            CreateTime = item.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                            UpdateTime = item.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss")
                        };
                        newUser.Add(userInfo);
                    }
                    return (true, newUser, "查询成功");
                }
                else
                {
                    var roleid = _dbcontext.Roles.FirstOrDefault(r => r.Name == KeyWord.role);
                    var users = await _dbcontext.Users.Where(u => u.Name.Contains(KeyWord.name) && u.Roles.Contains(roleid) && !u.IsDeleted).ToListAsync();
                    var newUser = new List<UserInfoDTO>();
                    foreach (var item in users)
                    {
                        var userid = item.Id;
                        var userRoles = await _dbcontext.Roles
                            .Where(x => x.IsDeleted == false && x.IsEnable && x.Users.Any(u => u.Id == userid))
                            .ToListAsync();
                        var userInfo = new UserInfoDTO
                        {
                            Id = item.Id,
                            Name = item.Name,
                            Describe = item.Describe,
                            RolesName = userRoles.Select(x => x.Name).ToList(),
                            IsEnable = item.IsEnable,
                            CreateTime = item.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                            UpdateTime = item.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss")
                        };
                        newUser.Add(userInfo);
                    }
                    return (true, newUser, "查询成功");
                }
            }
            catch (Exception ex)
            {

                return(false, new List<UserInfoDTO>(), ex.Message);
            }
        }
    }
}
