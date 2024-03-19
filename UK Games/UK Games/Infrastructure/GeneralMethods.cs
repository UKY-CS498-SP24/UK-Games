using UK_Games.Models;

namespace UK_Games.Infrastructure;

public class GeneralMethods
{
    public static string ParsePlayed(Dictionary<DateTime, Game> played)
    {
        throw new NotImplementedException();
    }
    
    public static Dictionary<DateTime, Game> ParsePlayed(String played)
    {
        throw new NotImplementedException();
    }

    public static Game GetGame(int id)
    {
        foreach (var g in DataUtil.Data.GetGames())
        {
            if (g.ID == id)
            {
                return g;
            }
        }

        return null;
    }
}