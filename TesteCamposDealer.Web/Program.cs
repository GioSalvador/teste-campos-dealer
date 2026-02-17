using Microsoft.Extensions.Options;
using TesteCamposDealer.Web.Configurations;
using TesteCamposDealer.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.Configure<ApiSettings>(
    builder.Configuration.GetSection("ApiSettings"));

builder.Services.AddHttpClient("ApiClient", (serviceProvider, client) =>
{
    var settings = serviceProvider
        .GetRequiredService<IOptions<ApiSettings>>().Value;

    client.BaseAddress = new Uri(settings.BaseUrl);
});

builder.Services.AddScoped<VendaService>();

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
    pattern: "{controller=Venda}/{action=Index}/{id?}");


app.Run();