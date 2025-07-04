using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OverallAuthv1._0.Domain.DTO;
using OverallAuthv1._0.Domain.IService;

namespace OverallAuthv1._0.Controllers
{
    /// <summary>
    /// Role Controller
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Result> AddRole(AddRoleDTO role)
        {
            try
            {
                if (role == null)
                {
                    return new Result
                    {
                        Code = 400,
                        Msg = "请求参数不能为空",
                        Data = null
                    };
                }
                if (string.IsNullOrEmpty(role.Name))
                {
                    return new Result
                    {
                        Code = 400,
                        Msg = "角色名称不能为空",
                        Data = null
                    };
                }
                else
                {
                    var result = await _roleService.AddRoleAsync(role);
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
                            Msg = result.msg,
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
        /// 搜索
        /// </summary>
        /// <param name="keyWord"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<Result> Search(searchRoleDTO keyWord )
        {
            try
            {
                var result = await _roleService.SearchByKeyWordAsync(keyWord);
                if(result.success)
                {
                    return new Result
                    { 
                        Code = 200,
                        Data = result.obj
                    };
                }
                else
                {
                    return new Result
                    {
                        Code = 500,
                        Msg = result.obj.ToString(),
                    };
                }

            }
            catch (Exception ex)
            {

                return new Result
                {
                    Code=500,
                    Msg = "服务器错误: " + ex.Message,
                };
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<Result> DeleteRole(int[] ids)
        {
            try
            {
                var result = await _roleService.DeleteRoleByIdAsync(ids);
                if (result.success)
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
