using Microsoft.EntityFrameworkCore;  //#11 add in the framework
using WeddingPlanner.Models;   //#12  You will need access to your models for your context file

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");  // #13 Create a variable to hold your connection string from appsetting.json


// Add services to the container.
builder.Services.AddControllersWithViews(); //#1
builder.Services.AddHttpContextAccessor();   //#2
builder.Services.AddSession(); 
builder.Services.AddDbContext<MyContext>(options =>  //#14 Accessing mycontext.cs file
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseSession();   //#3 

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

//#4 Create the Model View next

//#15 
