using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OverallAuthv1._0.Domain.IService;

namespace OverallAuthv1._0.Controllers
{
    /// <summary>
    /// OverallAuth Controller 1、给角色分配菜单 2、给用户分配角色
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class OverallAuthController : ControllerBase
    {

        private readonly IRoleService _roleService;

        private readonly IOverallAuthService _overallAuthService;

        public OverallAuthController(IRoleService roleService, IOverallAuthService overallAuthService)
        {
            _roleService = roleService;
            _overallAuthService = overallAuthService;
        }

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

    }
}
