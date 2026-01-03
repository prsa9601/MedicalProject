using MedicalProject.Infrastructure;
using MedicalProject.Infrastructure.Utils.Decryption;
using MedicalProject.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.RegisterApiServices();
builder.Services.AddHttpContextAccessor();

builder.Services.AddControllersWithViews();
//builder.Services.AddRazorPages(options =>
//{
//    options.Conventions.ConfigureFilter(new AuthRedirectFilter());
//}).AddRazorRuntimeCompilation();
//builder.Services.Configure<RazorViewEngineOptions>(options =>
//{
//    options.ViewLocationFormats.Add("/Pages/Shared/{0}.cshtml");
//});

builder.Services.AddAuthorization(option =>
{

    //option.AddPolicy("AdminAreaAccess", policy =>
    //{
    //    policy.RequireAuthenticatedUser();
    //    policy.RequireAssertion(context =>
    //    {
    //        var roles = context.User.Claims
    //            .Where(c => c.Type == ClaimTypes.Role)
    //            .Select(c => c.Value);

    //        return roles.Any(r =>
    //            r.Equals("SuperAdmin", StringComparison.OrdinalIgnoreCase) ||
    //            r.Equals("Admin", StringComparison.OrdinalIgnoreCase));
    //    });
    //});

    option.AddPolicy("Account", builder =>
    {
        builder.RequireAuthenticatedUser();
    });
    option.AddPolicy("SuperAdmin", builder =>
    {
        builder.RequireAuthenticatedUser();
    
        builder.RequireAssertion(f => f.User.Claims
            .Any(c => c.Type == ClaimTypes.Role && c.Value.Contains("SuperAdmin")));
    });
    option.AddPolicy("Programmer", builder =>
    {
        builder.RequireAuthenticatedUser();
        builder.RequireAssertion(f => f.User.Claims
            .Any(c => c.Type == ClaimTypes.Role && c.Value.Contains("Programmer")));
    });
});


builder.Services.AddRazorPages()
    .AddRazorRuntimeCompilation()
    .AddRazorPagesOptions(options =>
    {
        options.Conventions.AuthorizeFolder("/Account", "Account");
        //options.Conventions.AuthorizeFolder("/Admin", "Programmer");
        options.Conventions.AuthorizeFolder("/Admin", "SuperAdmin");
    });

//builder.Services.AddAuthentication(option =>
//{
//    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    option.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
//    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//});

builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

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
}).AddCookie(options =>
      {
          options.Cookie.Name = "auth-Token";
          options.Cookie.HttpOnly = true;
          options.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.Always;
          options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict;
          options.Cookie.MaxAge = TimeSpan.FromMinutes(60);

          options.Events = new CookieAuthenticationEvents
          {
              // ۶. هنگامی که کاربر لاگین نکرده
              OnRedirectToLogin = async context =>
              {
                  // مدیریت redirect به صفحه لاگین
                  if (context.Request.Path.StartsWithSegments("/api"))
                  {
                      context.Response.StatusCode = 401;
                      await context.Response.WriteAsync("Unauthorized");
                      context.Response.Redirect($"/Auth/VerificationPhoneNumber?action={ForAuthAction.Login}");

                  }
                  else
                  {
                      context.Response.Redirect("/Login");
                  }
              },
              //
              //// ۷. هنگامی که کاربر از لاگ اوت بازدید می‌کند
              //OnRedirectToLogout = async context =>
              //{
              //    // مدیریت redirect به صفحه خروج
              //    context.Response.Redirect("/Logout");
              //},
              //
              //// ۸. هنگامی که کوکی منقضی شده
              //OnRedirectToReturnUrl = async context =>
              //{
              //    // مدیریت بازگشت به URL اصلی
              //    context.Response.Redirect(context.RedirectUri);
              //}

          };




          //option.Events = new JwtBearerEvents
          //{
          //    OnChallenge = async context =>
          //    {
          //        // جلوگیری از پاسخ پیش‌فرض
          //        context.HandleResponse();

          //        //if (context.Request.Path.StartsWithSegments("/api"))
          //        //{
          //        //    context.Response.StatusCode = 401;
          //        //    await context.Response.WriteAsync("Unauthorized");
          //        //}
          //        if ((int)context.Response.StatusCode == 401)
          //        {
          //            context.Response.Redirect($"/Auth/VerificationPhoneNumber?action={ForAuthAction.Login}");
          //        }
          //    }
          //};
      });






builder.Services.AddDataProtection();
builder.Services.AddScoped<DecryptionService>();

builder.Services.Configure<IISServerOptions>(options =>
{
    options.MaxRequestBodySize = 6 * 1024 * 1024; // 100 مگابایت
});

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.MaxRequestBodySize = 100 * 1024 * 1024;
});
var app = builder.Build();

// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}





app.Use(async (context, next) =>
{
    var token = context.Request.Cookies["auth-Token"]?.ToString();
    var refreshToken = context.Request.Cookies["RefreshToken"]?.ToString();
    if (string.IsNullOrWhiteSpace(token) == false)
    {
        context.Request.Headers.Append("Authorization", $"Bearer {token}");
        context.Request.Headers.Append("AuthToken", $"Bearer {token}");
    }
    if (string.IsNullOrWhiteSpace(refreshToken) == false)
    {
        context.Request.Headers.Append("RefreshToken", $"{refreshToken}");
    }
    await next();

});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

//app.UseStatusCodePages(async context =>
//{
//    if (context.HttpContext.Response.StatusCode == 401)
//    {
//        app.Use(async (context, next) =>
//        {
//            context.Request.Path = $"/Auth/VerificationPhoneNumber?action={ForAuthAction.Login}";
//            context.Response.Redirect($"/Auth/VerificationPhoneNumber?action={ForAuthAction.Login}");
//            await next();
//        });
//    }
//});
//کار نمیکنه
app.Use(async (context, next) =>
{
    await next();

    var status = context.Response.StatusCode;
    if (status == 401)
    {
        var path = context.Request.Path;
        context.Response.Redirect($"/Auth/VerificationPhoneNumber?action={ForAuthAction.Login}");
    }
});

app.UseAuthentication();

app.UseAuthorization();

app.MapStaticAssets();



app.MapRazorPages()
   .WithStaticAssets();


app.Run();
