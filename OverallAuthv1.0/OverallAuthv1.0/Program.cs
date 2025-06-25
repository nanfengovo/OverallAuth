
using OverallAuthDEMO.EFcore;
using OverallAuthv1._0.Domain.IService;
using OverallAuthv1._0.Domain.Service;

namespace OverallAuthv1._0
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            // 示例：允许特定源（生产环境推荐）
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("MyPolicy", builder =>
                {
                    builder.WithOrigins("https://your-frontend.com", "http://localhost:3000")
                           .AllowAnyMethod() // 允许所有HTTP方法
                           .AllowAnyHeader() // 允许所有请求头
                           .AllowCredentials(); // 允许携带凭证（如Cookie）
                });
            });

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(x =>
            {
                string path = Path.Combine(System.AppContext.BaseDirectory, "OverallAuthv1.0.xml");
                x.IncludeXmlComments(path, true);
            });
            builder.Services.AddSwaggerGen();

            

            builder.Services.AddScoped<MyDbContext>();
            builder.Services.AddScoped<IMenuService,MenuService>();
            builder.Services.AddScoped<IUserService,UserService>();
            builder.Services.AddScoped<IRoleService,RoleService>();
            builder.Services.AddScoped<IOverallAuthService,OverallAuthService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            app.UseHttpsRedirection();
            app.UseCors("MyPolicy"); // 应用命名策略
            app.UseAuthorization();
            

            app.MapControllers();

            app.Run();
        }
    }
}
