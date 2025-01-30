using Microsoft.EntityFrameworkCore;
using VehicleApplication.Common;
using VehicleApplication.DAL;
using VehicleApplication.Model;
using VehicleApplication.Repository;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<VehicleContext>(options =>
    options.UseSqlServer("Server=.\\SQLEXPRESS03;Database=VehicleDb;Trusted_Connection=True;TrustServerCertificate=True;"));

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();


// Verify elements in database can be populated and deleted. Will be removed
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<VehicleContext>();
    dbContext.Database.Migrate();

    var unitOfWork = new UnitOfWork(dbContext);
    var vehicleMakeRepository = new GenericRepository<VehicleMake>(unitOfWork, dbContext);
    var vehicleModelRepository = new GenericRepository<VehicleModel>(unitOfWork, dbContext);


    var make = new VehicleMake { Name = "Volkswagen", Abrv = "VW" };
    await vehicleMakeRepository.AddAsync(make);
    await vehicleMakeRepository.CommitAsync();

    var model = new VehicleModel { Name = "Polo", Abrv = "PLO", MakeId = make.Id };
    await vehicleModelRepository.AddAsync(model);
    await vehicleModelRepository.CommitAsync();

    await vehicleModelRepository.DeleteAsync(model.Id);
    await vehicleModelRepository.CommitAsync();

    await vehicleMakeRepository.DeleteAsync(make.Id);
    await vehicleMakeRepository.CommitAsync();
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
