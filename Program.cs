var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// FIX: Serve sitemap.xml even if static files fails
app.MapGet("/sitemap.xml", async (HttpContext context) =>
{
    var path = Path.Combine(app.Environment.WebRootPath, "sitemap.xml");
    if (File.Exists(path))
    {
        context.Response.ContentType = "application/xml";
        await context.Response.SendFileAsync(path);
    }
    else
    {
        context.Response.StatusCode = 404;
    }
});

app.MapGet("/ads.txt", async (HttpContext context) =>
{
    var path = Path.Combine(app.Environment.WebRootPath, "ads.txt");
    if (File.Exists(path))
    {
        context.Response.ContentType = "text/plain";
        await context.Response.SendFileAsync(path);
    }
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();