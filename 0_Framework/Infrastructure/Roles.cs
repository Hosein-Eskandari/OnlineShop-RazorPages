namespace _0_Framework.Infrastructure;

public static class Roles
{
    public const string Administrator = "2";
    public const string ContentUploader = "3";
    public const string SystemUser = "6";
    public const string ColleagueUser = "5";


    public static string GetRoleBy(long id)
    {
        switch (id)
        {
            case 2:
                return "مدیر سیستم";
            case 3:
                return "محتواگذار";
            default:
                return "";
        }
    }
}