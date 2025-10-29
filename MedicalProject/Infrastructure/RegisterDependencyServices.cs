using MedicalProject.Infrastructure.CookieUtils;
using MedicalProject.Infrastructure.FileUtil.Interfaces;
using MedicalProject.Infrastructure.FileUtil.Services;
using MedicalProject.Infrastructure.RazorUtils;
using MedicalProject.Services.Auth;
using MedicalProject.Services.Product;
using MedicalProject.Services.User;

namespace MedicalProject.Infrastructure;

public static class RegisterDependencyServices
{
    public static IServiceCollection RegisterApiServices(this IServiceCollection services)
    {
        var baseAddress = "http://localhost:5000/api/";
        //var baseAddress = "http://localhost:5290/api/";

        services.AddHttpContextAccessor();

        services.AddScoped<HttpClientAuthorizationDelegatingHandler>();
        services.AddTransient<IRenderViewToString, RenderViewToString>();
        services.AddTransient<IFileService, FileService>();


        // اضافه کردن TelegramService به DI
        //services.AddScoped<ITelegramService, TelegramService>();

        // services.AddAutoMapper(typeof(RegisterDependencyServices).Assembly);
        //services.AddScoped<IMainPageService, MainPageService>();

        services.AddScoped<ShopCartCookieManager>();

        //services.AddCookieManager();

        services.AddHttpClient<IAuthService, AuthService>(httpClient =>
        {
            httpClient.BaseAddress = new Uri(baseAddress);
        }).AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();
        
      
        services.AddHttpClient<IUserService, UserService>(httpClient =>
        {
            httpClient.BaseAddress = new Uri(baseAddress);
        }).AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();
        
        services.AddHttpClient<IProductService, ProductService>(httpClient =>
        {
            httpClient.BaseAddress = new Uri(baseAddress);
        }).AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();
        
      

        return services;
    }
}


