using System.Globalization;
using UK_Games.Models;

namespace UK_Games.Infrastructure;

public class GeneralMethods
{
    public static string ParsePlayed(Dictionary<DateTime, Game> played)
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        
        foreach(KeyValuePair<DateTime, Game> p in played)
        {
            string timestamp = string.Format("{0:MM/dd/yyyy HH:mm:ss.fff}", p.Key);
            data.Add(timestamp, p.Value.ID.ToString());
        }

        return StringFromMetaData(data);
    }
    
    public static Dictionary<DateTime, Game> ParsePlayed(String p)
    {
        Dictionary<DateTime, Game> played = new Dictionary<DateTime, Game>();

        foreach (KeyValuePair<string, string> data in MetaDataFromString(p))
        {
            played.Add(DateTime.ParseExact(data.Key, "MM/dd/yyyy HH:mm:ss.fff", CultureInfo.CurrentCulture), GetGame(Int32.Parse(data.Value)));
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

    public static User GetUser(string username)
    {
        foreach (var u in DataUtil.Data.GetUsers())
        {
            if (u.Username == username)
            {
                return u;
            }
        }

        return null;
    }
    
    public static void HandleException(Exception e)
    {
        Console.WriteLine("\n\nError Occured...");
        Console.WriteLine(e.StackTrace);
    }

    public static Dictionary<bool, string> isLoggedIn(ISession session)
    {
        Dictionary<bool, string> status = new Dictionary<bool, string>();

        string loggedIn = (session.GetString("loggedIn"));

        if (loggedIn == "true")
        {
            Console.WriteLine("[ LOGIN ATTEMPT ] User Already Logged In: " +
                              session.GetString("loggedInUser"));
            status.Add(true, session.GetString("loggedInUser"));
        }
        else
        {
            status.Add(false, null);
        }

        return status;
    }

    public static Dictionary<bool, User> ConfirmUser(string username, string password, ISession session)
    {
        Dictionary<bool, User> status = new Dictionary<bool, User>();

        foreach (User user in DataUtil.Data.GetUsers())
        {
            if (user != null)
            {
                if (username.ToUpper() == user.Username.ToUpper())
                {
                    username = user.Username;

                    Console.WriteLine("[ LOGIN ATTEMPT ] Matches user: " + username);

                    if (password != user.Password)
                    {
                        Console.WriteLine("[ LOGIN FAIL ] Incorrect password for: " + username);
                        status.Add(false, null);
                    }
                    else
                    {
                        Console.WriteLine("[ LOGIN SUCCESS ] Logged in: " + username);
                        session.SetString("loggedIn", "true");
                        session.SetString("loggedInUser", username);
                        status.Add(true, user);
                    }
                }
            }
        }

        if (status.Count == 0)
        {
            Console.WriteLine("[ LOGIN FAIL ] Unknown user " + username + "... does this user exist?");
        }
        
        return status;
    }

    public static User GetLoggedInUser(ISession session)
    {
        string username = session.GetString("loggedInUser");

        if (username != null)
        {
            return GetUser(username);
        }

        return null;
    }
}