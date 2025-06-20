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
    internal class MyDbContext:DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Menu> Menus { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=OverallAuth.db").LogTo(Console.WriteLine, LogLevel.Information); // 输出到控制台;  // 连接字符串
        }

        //加载单独配置的所以程序

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 加载所有单独配置类
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

    }
}
