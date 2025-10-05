namespace HattrickApp.Api.Common.ResultPattern;

public static class ErrorMessage
{
    public static Error NotFound(string entity) => new($"{entity} not found.", nameof(ErrorCode.NotFound));

    public static Error InvalidValues(string entity, string property) =>
        new($"One or more {entity} do not have a valid {property}", nameof(ErrorCode.InvalidValues));

    public static Error MoreThanOneNotAllowed(string entity, string property) => new(
        $"A {entity} cannot contain more than one {property}", nameof(ErrorCode.MoreThanOneNotAllowed));
}
