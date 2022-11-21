global using Capitulo04.EntityFramework.Data;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.AspNetCore.Mvc;
global using System.Diagnostics;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<CustomerContext>(x => x.UseSqlServer("Data Source=.\\SQLEXPRESS; Initial Catalog=Northwind; trusted_connection=yes"));
builder.Services.AddDbContext<EmployeeContext>(x => x.UseSqlServer("Data Source=.\\SQLEXPRESS; Initial Catalog=Northwind; trusted_connection=yes"));

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

app.Run();
