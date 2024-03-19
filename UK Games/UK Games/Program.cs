using UK_Games.Infrastructure;

new DataUtil();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

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

// BEGIN DATABASE INITIALIZATION

// 1. Open Connection
DataUtil.DB.OpenConnection();

try
{
// 2. Create tables if they don't exist    
    DataUtil.CreateTables();
}
catch (Exception e)
{
    Console.WriteLine("Tables already created... moving on");
}

// 3. Import Data
DataUtil.PullDataFromDB();

// 4. Load Default Data (if no users)
if (DataUtil.Data.GetUsers().Count == 0)
{
    new LoadDefaultData();
}

app.Run();