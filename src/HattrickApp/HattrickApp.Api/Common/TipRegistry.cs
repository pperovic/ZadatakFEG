using HattrickApp.Api.Enums;

namespace HattrickApp.Api.Common;

public static class TipRegistry
{
    public static readonly Dictionary<SportType, List<string>> Tips = new()
    {
        { SportType.Football, ["1", "2", "X", "x2", "x1", "12"] },
        { SportType.Tennis, ["1", "2"] }
    };
}
