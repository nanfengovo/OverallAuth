using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OverallAuthv1._0.Domain.DTO;
using OverallAuthv1._0.Domain.IService;

namespace OverallAuthv1._0.Controllers
{
    /// <summary>
    /// User Controller
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Add User
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Result> AddUser(AddUser user)
        {
            try
            {
                if(user == null) 
                {
                    return new Result
                    {
                        Code = 400,
                        Msg = "请求参数不能为空",
                        Data = null
                    };
                }
                if(user.Name == null || user.Pwd == null)
                {
                    return new Result
                    {
                        Code = 400,
                        Msg = "用户名和密码不能为空",
                        Data = null
                    };
                }
                else
                {
                    var result = await _userService.AddUserAsync(user);
                        if (result.success)
                        {
                            return new Result
                            {
                                Code = 200,
                                Msg = "添加成功",
                                Data = null
                            };
                        }
                        else
                        {
                            return new Result
                            {
                                Code = 500,
                                Msg = "添加失败: " + result.meg,
                                Data = null
                            };
                        }
                }
            }
            catch (Exception ex)
            {

                return new Result
                {
                    Code = 500,
                    Msg = "服务器错误: " + ex.Message,
                    Data = null
                };
            }
        }



        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<Result> DeleteUser(int[] id)
        {
            try
            {
                var result = await _userService.DeleteUsersAsync(id);
                if(result.success)
                {
                    return new Result
                    {
                        Code = 200,
                        Msg = "删除成功",
                    };
                }
                else
                {
                    return new Result
                    {
                        Code = 500,
                        Msg = "删除失败: " + result.msg,
                    };
                }
            }
            catch (Exception ex)
            {

                return new Result
                {
                    Code = 500,
                    Msg = "服务器错误: " + ex.Message,
                };
            }
        }
    }
}
