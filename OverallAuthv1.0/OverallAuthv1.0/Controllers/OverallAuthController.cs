using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OverallAuthDEMO.EFcore.Model;
using OverallAuthv1._0.Domain.IService;

namespace OverallAuthv1._0.Controllers
{
    /// <summary>
    /// OverallAuth Controller 1、给角色分配菜单 2、给用户分配角色
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OverallAuthController : ControllerBase
    {
        private readonly IUserService _userService;

        private readonly IRoleService _roleService;

        private readonly IOverallAuthService _overallAuthService;

        public OverallAuthController(IRoleService roleService, IOverallAuthService overallAuthService, IUserService userService)
        {
            _roleService = roleService;
            _overallAuthService = overallAuthService;
            _userService = userService;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Result> Login(string userName, string password)
        {
            
                var user = await _userService.GetUserByNameAsync(userName);
                if (user == null)
                {
                    return new Result
                    {
                        Code = 404,
                        Msg = "用户不存在",
                        Data = null
                    };
                }
                if (user.Pwd != password)
                {
                    return new Result
                    {
                        Code = 404,
                        Msg = "密码错误",
                        Data = null
                    };
                }
                return new Result
                {
                    Code = 200,
                    Msg = "登录成功",
                    Data = user
                };
            
        }














        /// <summary>
        /// 给角色分配菜单
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="menuIds"></param>
        /// <returns></returns>
        //1、给角色分配菜单；现在有三个菜单页，给Admin角色分配所有菜单
        [HttpPost]
        public async Task<Result> GiveRoleMenu(string roleName, int[] menuIds)
        {
            #region 验证参数
            //1、这个角色是否存在且是启用的需要过滤删除状态
            var success = await _roleService.GetRoleByNameAsync(roleName);
            #endregion
            if (!success)
            {
                return new Result
                {
                    Code = 404,
                    Msg = "角色不存在或未启用无法分配权限",
                    Data = null
                };
            }
            else
            {
                //2、给角色分配菜单
                var result = await _overallAuthService.GiveRoleMenuAsync(roleName, menuIds);
                if (result.success)
                {
                    return new Result
                    {
                        Code = 200,
                        Msg = "分配成功",
                        Data = null
                    };
                }
                else
                {
                    return new Result
                    {
                        Code = 500,
                        Msg = "分配失败: " + result.msg,
                        Data = null
                    };
                }
            }

        }

        /// <summary>
        /// 给用户分配角色
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Result> GiveUserRole(string userName, string[] rolesName)
        {
            #region 验证参数的合法性
            //1、这个角色是否存在且是启用的需要过滤删除状态
            foreach (var item in rolesName)
            {
                var existRole = await _roleService.GetRoleByNameAsync(item);
                if (!existRole)
                {
                    return new Result
                    {
                        Code = 404,
                        Msg = "角色不存在或未启用无法分配权限",
                        Data = null
                    };
                }
            }
            
            var existUser = await _userService.UserIsexistAsync(userName);
           
            if (!existUser)
            {
                return new Result
                {
                    Code = 404,
                    Msg = "用户不存在或未启用无法分配权限",
                    Data = null
                };
            }
            #endregion
            else
            {
                //2、给用户分配角色
                var result = await _overallAuthService.GiveUserRoleAsync(userName,  rolesName);
                if (result.success)
                {
                    return new Result
                    {
                        Code = 200,
                        Msg = "分配成功",
                        Data = null
                    };
                }
                else
                {
                    return new Result
                    {
                        Code = 500,
                        Msg = "分配失败: " + result.msg,
                        Data = null
                    };
                }
            }
        }

    }
}
