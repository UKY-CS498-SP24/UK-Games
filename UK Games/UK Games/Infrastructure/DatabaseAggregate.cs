using UK_Games.Models;

namespace UK_Games.Infrastructure;

public class DatabaseAggregate
{
    private List<Game> games;
    private List<User> users;

    public DatabaseAggregate()
    {
        games = new List<Game>();
        users = new List<User>();
    }

    public void WipeCache()
    {
        games = new List<Game>();
        users = new List<User>();
    }

    public List<Game> GetGames()
    {
        return games;
    }

    public List<User> GetUsers()
    {
        return users;
    }

    public void AddGame(Game game)
    {
        bool exists = false;
        foreach (var g in games)
        {
            if (g.ID == game.ID)
            {
                exists = true;
                break;
            }
        }

        if (!exists)
        {
            games.Add(game);
        }
    }

    public void AddUser(User user)
    {
        bool exists = false;
        foreach (var u in users)
        {
            if (u.ID == user.ID)
            {
                exists = true;
                break;
            }
        }

        if (!exists)
        {
            users.Add(user);
        }
    }

    public void RemoveGame(Game game)
    {
        int id = game.ID;

        foreach (var g in games)
        {
            if (g.ID == id)
            {
                games.Remove(g);
            }
        }
    }

    public void RemoveGame(int id)
    {
        foreach (var g in games)
        {
            if (g.ID == id)
            {
                games.Remove(g);
            }
        }
    }

    public void RemoveUser(User user)
    {
        int id = user.ID;

        foreach (var u in users)
        {
            if (u.ID == id)
            {
                users.Remove(u);
            }
        }
    }

    public void RemoveUser(int id)
    {
        foreach (var u in users)
        {
            if (u.ID == id)
            {
                users.Remove(u);
            }
        }
    }
}