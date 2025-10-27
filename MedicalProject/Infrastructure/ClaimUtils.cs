using System.Security.Claims;

namespace MedicalProject.Infrastructure;

public static class ClaimUtils
{
    public static long GetUserId(this ClaimsPrincipal principal)
    {
        if (principal == null)
            throw new ArgumentNullException(nameof(principal));

        return Convert.ToInt64(principal.FindFirst(ClaimTypes.NameIdentifier)?.Value);
    }
    public static string GetUserPhoneNumber(this ClaimsPrincipal principal)
    {
        if (principal == null)
            throw new ArgumentNullException(nameof(principal));

        return Convert.ToString(principal.FindFirst(ClaimTypes.MobilePhone)?.Value)!;
    }
    public static string GetUserName(this ClaimsPrincipal principal)
    {
        if (principal == null)
            throw new ArgumentNullException(nameof(principal));

        return Convert.ToString(principal.FindFirst(ClaimTypes.Name)?.Value)!;
    }
}