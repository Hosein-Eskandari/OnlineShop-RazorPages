namespace _0_Framework.Application;

public class OperationResult
{
    public OperationResult()
    {
        IsSuccedded = false;
    }

    public bool IsSuccedded { get; set; }

    public string Message { get; set; }

    public OperationResult Succeeded(string message = "عملیات با موفقیت انجام شد")
    {
        IsSuccedded = true;
        Message = message;
        return this;
    }

    public OperationResult Failed(string message)
    {
        IsSuccedded = false;
        Message = message;
        return this;
    }
}