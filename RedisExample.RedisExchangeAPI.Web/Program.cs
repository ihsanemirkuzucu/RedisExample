using System.Runtime.CompilerServices;
using RedisExample.RedisExchangeAPI.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<RedisService>();
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

using (var serviceScope = app.Services.CreateScope())

{

    var services = serviceScope.ServiceProvider;

    var redisService = services.GetRequiredService<RedisService>();

    redisService.Connect();

}

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=HashType}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
