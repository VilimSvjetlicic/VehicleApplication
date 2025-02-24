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


var app = builder.Build();


// Verify elements in database can be populated and deleted. Will be removed
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<VehicleContext>();
    dbContext.Database.Migrate();

    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

    var vehicleMakeRepository = app.Services.GetRequiredService<IGenericRepository<VehicleMake>>();
    var vehicleModelRepository = app.Services.GetRequiredService<IGenericRepository<VehicleModel>>();

    var vehicleMakeService = app.Services.GetRequiredService<IVehicleService<VehicleMake>>();
    var vehicleModelService = app.Services.GetRequiredService<IVehicleService<VehicleModel>>();

    var make = new VehicleMake { Name = "Volkswagen", Abrv = "VW" };
    await vehicleMakeService.AddAsync(make);
    await vehicleMakeService.CommitAsync();

    var model = new VehicleModel { Name = "Polo", Abrv = "PLO", MakeId = make.Id };
    await vehicleModelService.AddAsync(model);
    await vehicleModelService.CommitAsync();

    await vehicleModelService.DeleteAsync(model.Id);
    await vehicleModelService.CommitAsync();

    await vehicleMakeService.DeleteAsync(make.Id);
    await vehicleMakeService.CommitAsync();
}

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

app.Run();