var builder = WebApplication.CreateBuilder(args);
//container a servisleri ekliyoruz


builder.Services.AddHttpClient();//httpclient ekledik çünkü api çaðrýsý yapacaðýz
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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
