using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OverallAuthDEMO.EFcore.Model;

namespace OverallAuthDEMO.EFcore
{
    public class MyDbContext:DbContext
    {
        /// <summary>
        /// 用户
        /// </summary>
        public DbSet<UserInfo> Users { get; set; }
        /// <summary>
        /// 角色
        /// </summary>
        public DbSet<Role> Roles { get; set; }
        /// <summary>
        /// 菜单
        /// </summary>
        public DbSet<Menu> Menus { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=OverallAuthV1.0.db").LogTo(Console.WriteLine, LogLevel.Information); // 输出到控制台;  // 连接字符串
        }

        //加载单独配置的所以程序

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 加载所有单独配置类
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

    }
}
