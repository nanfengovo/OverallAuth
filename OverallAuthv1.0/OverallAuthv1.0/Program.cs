
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

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
