namespace _0_Framework.Application;

public static class ValidationMessages
{
    public const string IsRequired = "وارد کردن این مقدار اجباری است.";

    public const string OutOfRange = "تعداد کاراکترها بیش از حد مجاز است.";

    public const string NotInRange = "عدد وارد شده خارج از محدوده است.";

    public const string ValidationFileSize = "فایل حجیم تر از مقدار مجاز است";

    public const string InvalidFileFormat = "فرمت فایل مجاز نیست. فرمت های مجاز عبارتند از: jpeg ,jpg ,png";

    public static string MaxFileSize(int maxFileSize)
    {
        return $"مقدار مجاز فایل برابر {maxFileSize} مگابایت می باشد.";
    }
}