using UK_Games.Models;

namespace UK_Games.Infrastructure;

public class GeneralMethods
{
    public static string ParsePlayed(Dictionary<DateTime, Game> played)
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        
        foreach(KeyValuePair<DateTime, Game> p in played)
        {
            data.Add(p.Key.ToString(), p.Value.ID.ToString());
        }

        return StringFromMetaData(data);
    }
    
    public static Dictionary<DateTime, Game> ParsePlayed(String p)
    {
        Dictionary<DateTime, Game> played = new Dictionary<DateTime, Game>();

        foreach (KeyValuePair<string, string> data in MetaDataFromString(p))
        {
            played.Add(DateTime.Parse(data.Key), GetGame(Int32.Parse(data.Value)));
        }
        
        return played;
    }
    
    // meta data format: field1, value; field2, value; ...
    public static string StringFromMetaData(Dictionary<string, string> metaData)
    {
        string data = "";

        foreach (KeyValuePair<string, string> set in metaData)
        {
            data += set.Key + "," + set.Value + ";";
        }

        data.TrimEnd(';');

        if (data.Length == 0)
        {
            data = "NULL";
        }

        return data;
    }
    
    // meta data format: field1, value; field2, value; ...
    public static Dictionary<string, string> MetaDataFromString(string data)
    {
        // meta data format: field1, value; field2, value; ...
        Dictionary<string, string> metaData = new Dictionary<string, string>();
        if (data.Contains(';'))
        {
            foreach (string set in data.Split(";"))
            {
                if (set.Contains(","))
                {
                    metaData.Add(set.Split(",")[0], set.Split(",")[1]);
                }
            }
        }
        else
        {
            if (data.Contains(","))
            {
                metaData.Add(data.Split(",")[0], data.Split(",")[1]);
            }
        }


        return metaData;
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

    public static User GetUser(int id)
    {
        foreach (var u in DataUtil.Data.GetUsers())
        {
            if (u.ID == id)
            {
                return u;
            }
        }

        return null;
    }
}