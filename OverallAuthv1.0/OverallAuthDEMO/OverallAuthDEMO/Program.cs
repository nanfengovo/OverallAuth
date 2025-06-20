using OverallAuthDEMO.EFcore;
using OverallAuthDEMO.EFcore.Model;

namespace OverallAuthDEMO
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (MyDbContext ctx = new MyDbContext())
            {
                #region 角色菜单模块
                //准备用户数据
                var admin = new User { Name = "admin", Pwd = "123456" };

                //准备角色数据
                var Admin = new Role { Name = "Admin" };
                var User = new Role { Name = "User" };

                //准备菜单数据
                var Home = new Menu { Name = "Home" };

                var UserManager = new Menu { Name = "UserManager" };

                var RoleManager = new Menu { Name = "RoleManager" };

                var MenuManager = new Menu { Name = "MenuManager" };


                ////1、给Admin角色分配所有的菜单
                //Admin.Menus.AddRange(new[] { Home, UserManager, RoleManager, MenuManager });

                ////2、给User角色只分配Home菜单
                //User.Menus.Add(Home);

                //// 添加到上下文
                //ctx.Users.Add(admin);
                //ctx.Roles.Add(Admin);
                //ctx.Roles.Add(User);
                //ctx.Menus.AddRange(Home, UserManager, RoleManager, MenuManager);

                #endregion

                #region 用户角色模块
                // 添加到上下文
                ctx.Users.Add(admin);
                ctx.Roles.Add(Admin);
                //给admin用户分配Admin角色
                admin.Roles.Add(Admin);
                #endregion

                ctx.SaveChanges();
                Console.ReadKey();

            }
        }
    }
}
