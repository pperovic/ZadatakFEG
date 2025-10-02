namespace HattrickApp.Api.Common.ResultPattern;

public class Error(string message, string code)
{
    public string Message { get; } = message;
    public string Code { get; } = code;
}
