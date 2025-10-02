namespace HattrickApp.Api.Common.ResultPattern;

public static class ErrorMessage
{
    public static Error NotFound(string entity) => new($"{entity} not found.", nameof(ErrorCode.NotFound));
}
