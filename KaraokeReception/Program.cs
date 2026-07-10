using KaraokeReception.Domain.Services.PriceCalculator;
using KaraokeReception.Infrastructure.Data;
using KaraokeReception.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<KaraokeReceptionDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("KaraokeReception")));
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IPriceRule, NormalPriceRule>();
builder.Services.AddScoped<IPriceRule, EarlyDiscountRule>();
builder.Services.AddScoped<PriceCalculator>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider
        .GetRequiredService<KaraokeReceptionDbContext>();

    await DatabaseInitializer.InitializeAsync(dbContext);
}

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
