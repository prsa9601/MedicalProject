using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;

namespace MedicalProject.Infrastructure;

public class HttpClientAuthorizationDelegatingHandler : DelegatingHandler
{

    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpClientAuthorizationDelegatingHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (_httpContextAccessor.HttpContext != null)
        {
            var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
            var refreshToken = _httpContextAccessor.HttpContext.Request.Headers["RefreshToken"].ToString();

            if (string.IsNullOrWhiteSpace(token) == false)
            {
                request.Headers.Add("Authorization", token);

                request.Headers.Add("AuthToken", token);
            }
            if (string.IsNullOrWhiteSpace(refreshToken) == false)
            {
                request.Headers.Add("RefreshToken", refreshToken);
            }

        }

        
        var response = await base.SendAsync(request, cancellationToken);

        if (response.Headers.TryGetValues("X-Auth-Token", out var tokens))
        {
            var newToken = tokens.First();
            _httpContextAccessor.HttpContext.Response.Cookies.Append("auth-Token", newToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(30),
                Path = "/"
            });
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(newToken);
            var claims = jwtToken.Claims;
            var identity = new ClaimsIdentity(claims, "jwt");
            var principal = new ClaimsPrincipal(identity);
            _httpContextAccessor.HttpContext.User = principal;
        }

        //if (!request.Headers.TryGetValues("Authorization", out var i))
        //{
        //    if (i == null)
        //    {
        //        return new HttpResponseMessage(HttpStatusCode.Unauthorized);

        //    }
        //}
        return response;
    }

}


