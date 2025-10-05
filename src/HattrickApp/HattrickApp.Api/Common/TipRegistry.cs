using HattrickApp.Api.Enums;

namespace HattrickApp.Api.Common;

public static class TipRegistry
{
    public const string FirstCompetitorWin = "1";
    public const string SecondCompetitorWin = "2";
    public const string Draw = "X";
    public const string FirstCompetitorWinOrDraw = "x1";
    public const string SecondCompetitorWinOrDraw = "x2";
    public const string FirsOrSecondCompetitorWin = "12";

    // Use for validating offer creation for specifc sport
    public static readonly Dictionary<SportType, List<string>> Tips = new()
    {
        {
            SportType.Football,
            [
                FirstCompetitorWin,
                SecondCompetitorWin,
                Draw,
                FirstCompetitorWinOrDraw,
                SecondCompetitorWinOrDraw,
                FirsOrSecondCompetitorWin
            ]
        },
        {
            SportType.Tennis,
            [
                FirstCompetitorWin, 
                SecondCompetitorWin
            ]
        }
    };
}
