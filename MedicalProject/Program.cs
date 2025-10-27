using MedicalProject.Infrastructure;
using MedicalProject.Infrastructure.Utils.Decryption;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
//builder.Services.Configure<RazorViewEngineOptions>(options =>
//{
//    options.ViewLocationFormats.Add("/Pages/Shared/{0}.cshtml");
//});

builder.Services.RegisterApiServices();
builder.Services.AddHttpContextAccessor();

//builder.Services.AddAuthorization();

//builder.Services.AddRazorPages()
//    .AddRazorRuntimeCompilation()
//    .AddRazorPagesOptions(options =>
//    {
//        // options.Conventions.AuthorizeFolder("/Profile", "Account");
//        options.Conventions.AuthorizeFolder("/SellerPanel", "SellerPanel");
//    });

//builder.Services.AddAuthentication(option =>
//{
//    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    option.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
//    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//});
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(option =>
{
    option.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidAudience = builder.Configuration["JwtConfig:Audience"],
        ValidIssuer = builder.Configuration["JwtConfig:Issuer"],
        IssuerSigningKey =
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtConfig:SecretKey"]!)),
        ValidateLifetime = true,
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidateIssuerSigningKey = true
    };
});



builder.Services.AddDataProtection();
builder.Services.AddScoped<DecryptionService>();

// Program.cs
//builder.Services.Configure<IISServerOptions>(options =>
//{
//    options.MaxRequestBodySize = 52428800; // 50MB
//});
builder.Services.Configure<IISServerOptions>(options =>
{
    options.MaxRequestBodySize = 100 * 1024 * 1024; // 100 مگابایت
});

//builder.WebHost.ConfigureKestrel(serverOptions =>
//{
//    serverOptions.Limits.MaxRequestBodySize = 52428800; // 50MB
//});
builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.MaxRequestBodySize = 100 * 1024 * 1024;
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.Use(async (context, next) =>
{
    var token = context.Request.Cookies["token"]?.ToString();
    if (string.IsNullOrWhiteSpace(token) == false)
    {
        context.Request.Headers.Append("Authorization", $"Bearer {token}");
    }
    await next();
});

app.UseHttpsRedirection();

app.UseRouting();
app.Use(async (context, next) =>
{

    var status = context.Response.StatusCode;
    if (status == 401)
    {
        var path = context.Request.Path;
        context.Response.Redirect($"../../../Auth/Login?redirectTo={path}");
    }
    await next();
});
app.UseAuthentication();


app.UseAuthorization();

app.MapStaticAssets();



app.MapRazorPages()
   .WithStaticAssets();


app.Run();
