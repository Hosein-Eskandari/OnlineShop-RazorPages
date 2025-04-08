using System;

namespace _0_Framework.Infrastructure;

public class NeedsPermissionAttribute : Attribute
{
    public NeedsPermissionAttribute(int permission)
    {
        Permission = permission;
    }

    public int Permission { get; set; }
}