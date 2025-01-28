using Microsoft.EntityFrameworkCore;
using VehicleApplication.DAL;
using VehicleApplication.Model;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<VehicleContext>(options =>
    options.UseSqlServer("Server=.\\SQLEXPRESS03;Database=VehicleDb;Trusted_Connection=True;TrustServerCertificate=True;"));

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();


// Verify database can be created and populated. Will be removed
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<VehicleContext>();
    dbContext.Database.Migrate();

    var makes = new List<VehicleMake>
        {
            new VehicleMake { Name = "BMW", Abrv = "BMW" },
            new VehicleMake { Name = "Ford", Abrv = "FRD" },
            new VehicleMake { Name = "Volkswagen", Abrv = "VW" }
        };

    dbContext.VehicleMakes.AddRange(makes);
    dbContext.SaveChanges();

    var models = new List<VehicleModel>
        {
            new VehicleModel { Name = "3 Series", Abrv = "M3", MakeId = makes[0].Id },
            new VehicleModel { Name = "Fiesta", Abrv = "FST", MakeId = makes[1].Id },
            new VehicleModel { Name = "Golf", Abrv = "GLF", MakeId = makes[2].Id }
        };

    dbContext.VehicleModels.AddRange(models);
    dbContext.SaveChanges();
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
