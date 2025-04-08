using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using _0_Framework.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace _0_Framework.Application;

public class AuthHelper : IAuthHelper
{
    private readonly IHttpContextAccessor _contextAccessor;

    public AuthHelper(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    public List<int> GetPermissions()
    {
        if (!IsAuthenticated())
            return new List<int>();

        var permissions = _contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "permissions")
            ?.Value;
        return JsonConvert.DeserializeObject<List<int>>(permissions);
    }

    public long CurrentAccountId()
    {
        return IsAuthenticated()
            ? long.Parse(_contextAccessor.HttpContext.User.Claims.First(x => x.Type == "AccountId")?.Value)
            : 0;
    }

    public string GetCurrentAccountMobile()
    {
        return IsAuthenticated()
            ? _contextAccessor.HttpContext.User.Claims.First(x => x.Type == "Mobile")?.Value
            : " ";
    }

    public string CurrentAccountRole()
    {
        if (IsAuthenticated())
        {
            var role = _contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
            return role;
        }

        return null;
    }

    public AuthViewModel CurrentAccountInfo()
    {
        var result = new AuthViewModel();
        if (!IsAuthenticated()) return result;

        var claims = _contextAccessor.HttpContext.User.Claims.ToList();

        result.Id = long.Parse(claims.FirstOrDefault(x => x.Type == "AccountId").Value);
        result.UserName = claims.FirstOrDefault(x => x.Type == "Username").Value;
        result.RoleId = long.Parse(claims.FirstOrDefault(x => x.Type == ClaimTypes.Role).Value);
        result.FullName = claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value.ToString();

        result.Role = Roles.GetRoleBy(result.RoleId);

        return result;
    }

    public bool IsAuthenticated()
    {
        return _contextAccessor.HttpContext.User.Identity.IsAuthenticated;
        //var claims = _contextAccessor.HttpContext.User.Claims.ToList();
        ////if (claims.Count > 0)
        ////    return true;
        ////return false;
        //return claims.Count > 0;
    }

    public void SignIn(AuthViewModel account)
    {
        var permissions = JsonConvert.SerializeObject(account.Permissions);
        var claims = new List<Claim>
        {
            new("AccountId", account.Id.ToString()),
            new(ClaimTypes.Name, account.FullName),
            new(ClaimTypes.Role, account.RoleId.ToString()),
            new("Username", account.UserName), // Or Use ClaimTypes.NameIdentifier
            new("permissions", permissions),
            new("Mobile", account.Mobile)
            //new Claim("Mobile", account.Mobile)
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var authProperties = new AuthenticationProperties
        {
            ExpiresUtc = DateTimeOffset.UtcNow.AddDays(1)
        };

        _contextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);
    }

    public void SignOut()
    {
        _contextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }

    public string CurrentAccountMobile()
    {
        return IsAuthenticated()
            ? _contextAccessor.HttpContext.User.Claims.First(x => x.Type == "Mobile")?.Value
            : "";
    }
}