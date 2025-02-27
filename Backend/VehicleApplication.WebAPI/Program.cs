using Microsoft.EntityFrameworkCore;
using VehicleApplication.Common;
using VehicleApplication.DAL;
using VehicleApplication.Model;
using VehicleApplication.Repository;
using VehicleApplication.Service;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using VehicleApplication.Service.Common;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<VehicleContext>(options =>
    options.UseSqlServer("Server=.\\SQLEXPRESS03;Database=VehicleDb;Trusted_Connection=True;TrustServerCertificate=True;"));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.Register(c => c.Resolve<VehicleContext>())
                .As<DbContext>()
                .InstancePerLifetimeScope();

    containerBuilder.RegisterType<UnitOfWork>()
                    .As<IUnitOfWork>()
                    .InstancePerLifetimeScope();

    containerBuilder.RegisterGeneric(typeof(GenericRepository<>))
                    .As(typeof(IGenericRepository<>))
                    .InstancePerLifetimeScope();

    containerBuilder.RegisterGeneric(typeof(VehicleService<>))
                    .As(typeof(IVehicleService<>))
                    .InstancePerLifetimeScope();
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontendOrigin",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000")
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseCors("AllowFrontendOrigin");

app.Run();