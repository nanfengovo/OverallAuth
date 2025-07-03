using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OverallAuthv1._0.Domain.DTO;
using OverallAuthv1._0.Domain.IService;

namespace OverallAuthv1._0.Controllers
{
    /// <summary>  
    /// 菜单控制器  
    /// </summary>  
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _menuService;

        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        /// <summary>  
        /// 获取所有菜单  
        /// </summary>  
        /// <returns></returns>  
        [HttpGet]
        public async Task<Result> GetAllMenu()
        {
            try
            {
                var result = await _menuService.GetAllMenuAsync();

                if (result.success)
                {
                    return new Result
                    {
                        Code = 200,
                        Msg = "获取成功",
                        Data = result.menus
                    };
                }
                else
                {
                    return new Result
                    {
                        Code = 500,
                        Msg = "获取失败",
                        Data = null
                    };
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
        /// 添加菜单
        /// </summary>
        /// <param name="menus"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Result> AddMenu(AddMenuDTO menus)
        {
            try
            {
                if(menus != null && !string.IsNullOrEmpty(menus.Name))
                {
                    var result = await _menuService.AddMenuAsync(menus);
                    if (result.success)
                    {
                        return new Result
                        {
                            Code = 200,
                            Msg = "添加成功",
                        };
                    }
                    else
                    {
                        return new Result
                        {
                            Code = 500,
                            Msg = "添加失败: " + result.msg,
                            Data = null
                        };
                    }
                }
                else
                {
                    return new Result
                    {
                        Code = 400,
                        Msg = "请求参数错误",
                        Data = null
                    };
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
        /// 根据角色获取菜单
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>

        [HttpGet]
        public async Task<Result> GetMenuByRole(string userName)
        {
            try
            {
                var result = await _menuService.GetMenuByRoleAsync(userName);
                if (result.success)
                {
                    return new Result
                    {
                        Code = 200,
                        Msg = "获取成功",
                        Data = result.menus
                    };
                }
                else
                {
                    return new Result
                    {
                        Code = 500,
                        Msg = ""
                    };
                }
                    
            }
            catch (Exception ex)
            {
                return new Result
                {
                    Code = 400,
                    Msg = "服务器错误: " + ex.Message,
                };


            }
        }

        /// <summary>
        /// 删除菜单（单个或多个）
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<Result> DeleteMenus(int[] ids)
        {
            try
            {
                if(ids.Count() == 0)
                {
                    return new Result
                    {
                        Code = 400,
                        Msg = "需要删除的菜单至少要有一个！"
                    };
                }
                var result = await _menuService.DeleteMenusAsync(ids);
                if (result.success)
                {
                    return new Result
                    { 
                        Code= 200,
                        Msg = "删除成功"
                    };
                }
                else
                {
                    return new Result
                    {
                        Code =500,
                        Msg = result.msg
                    };
                }
            }
            catch (Exception ex)
            {

                return new Result
                {
                    Code = 500,
                    Msg = ex.Message,
                };
            }
        }

        /// <summary>
        /// 编辑菜单
        /// </summary>
        /// <param name="id"></param>
        /// <param name="menu"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Result> EditMenu(int id, AddMenuDTO menu)
        {
            try
            {
                var result = await _menuService.EditMenuAsync(id, menu);
                if (result.success)
                {
                    return new Result
                    {
                        Code = 200,
                        Msg = result.msg
                    };
                }
                else
                {
                    return new Result
                    {
                        Code = 500,
                        Msg = result.msg
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

        /// <summary>
        /// 根据关键字搜索
        /// </summary>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Result> Search(SearchMenuDTO keyWord)
        {
            try
            {
                var result = await _menuService.SearchByKeyWordAsync(keyWord);
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
                        Code = 400,
                        Msg = result.obj.ToString()
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
