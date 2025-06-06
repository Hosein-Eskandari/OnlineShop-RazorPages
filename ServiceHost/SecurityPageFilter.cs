﻿using System.Linq;
using System.Reflection;
using _0_Framework.Application;
using _0_Framework.Infrastructure;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ServiceHost;

public class SecurityPageFilter : IPageFilter
{
    private readonly IAuthHelper _authHelper;

    public SecurityPageFilter(IAuthHelper authHelper)
    {
        _authHelper = authHelper;
    }

    public void OnPageHandlerSelected(PageHandlerSelectedContext context)
    {
    }

    public void OnPageHandlerExecuting(PageHandlerExecutingContext context)
    {
        //var permission = context.HandlerMethod.MethodInfo.GetCustomAttribute(
        //        typeof(NeedsPermissionAttribute));

        //var handlerPermission =
        //    (NeedsPermissionAttribute)context.HandlerMethod.MethodInfo.GetCustomAttribute(
        //        typeof(NeedsPermissionAttribute));

        //if (handlerPermission == null)
        //    return;

        //var accountPermissions = _authHelper.GetPermissions();

        //if (accountPermissions.All(x => x != handlerPermission.Permission))
        //    context.HttpContext.Response.Redirect("/Account");

        if (context?.HandlerMethod?.MethodInfo == null)
            return; // اگر HandlerMethod یا MethodInfo null باشد، تابع را ترک کن

        var handlerPermission = (NeedsPermissionAttribute)context.HandlerMethod.MethodInfo.GetCustomAttribute(typeof(NeedsPermissionAttribute));

        if (handlerPermission == null)
            return;

        var accountPermissions = _authHelper?.GetPermissions() ?? Enumerable.Empty<int>(); // اگر _authHelper null باشد، یک لیست خالی برگردان

        if (accountPermissions.All(x => x != handlerPermission.Permission))
            context.HttpContext.Response.Redirect("/Account");
    }


    public void OnPageHandlerExecuted(PageHandlerExecutedContext context)
    {
    }
}