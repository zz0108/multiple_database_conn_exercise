using System.Reflection;
using Microsoft.Extensions.Configuration;
using MultipleDatabaseConn.Data;
using MultipleDatabaseConn.Enum;
using MultipleDatabaseConn.Models;
using MultipleDatabaseConn.Repositories;

namespace MultipleDatabaseConn
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();



            builder.Services.AddSingleton<IDynamicDbContextFactory<ApplicationDbContext>, DynamicDbContextFactory<ApplicationDbContext>>();
            builder.Services.AddSingleton<IConnectionStringProvider, ConnectionStringProvider>(p =>
            {
                var connectionStringProvider = new ConnectionStringProvider();
                connectionStringProvider.SetConnectionStrings(typeof(ServerNames)
                    .GetFields(BindingFlags.Public | BindingFlags.Static)
                    .ToDictionary(
                        f => f.GetValue(null) as string ?? throw new InvalidOperationException("Server name cannot be null"),
                        f => typeof(Conn).GetField(f.Name)!.GetValue(null) as string ?? throw new InvalidOperationException("Connection string cannot be null")
                    ));
                return connectionStringProvider;
            });
            builder.Services.Configure<DbContextOptions>(builder.Configuration.GetSection("DbContextOptions"));
            builder.Services.AddScoped<IMemberRepository, MemberRepository>();
            builder.Services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
