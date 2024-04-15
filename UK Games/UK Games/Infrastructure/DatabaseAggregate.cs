using UK_Games.Models;

namespace UK_Games.Infrastructure;

public class DatabaseAggregate
{
    private List<Game> games;
    public List<Game> Games { get => games; }
    private List<User> users;
    public List<User> Users { get => users; }

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
        bool canRemove = false;

        foreach (var g in games)
        {
            if (g.ID == id)
            {
                canRemove = true;
            }
        }

        if (canRemove)
        {
            games.Remove(game);
        }
    }

    public void RemoveGame(int id)
    {
        bool canRemove = false;
        Game toRemove = null;

        foreach (var g in games)
        {
            if (g.ID == id)
            {
                canRemove = true;
                toRemove = g;
            }
        }

        if (canRemove && toRemove != null)
        {
            games.Remove(toRemove);
        }
    }

    public void RemoveUser(User user)
    {
        int id = user.ID;
        bool canRemove = false;

        foreach (var u in users)
        {
            if (u.ID == id)
            {
                canRemove = true;
            }
        }
        
        if (canRemove)
        {
            users.Remove(user);
        }
    }

    public void RemoveUser(int id)
    {
        bool canRemove = false;
        User toRemove = null;

        foreach (var u in users)
        {
            if (u.ID == id)
            {
                canRemove = true;
                toRemove = u;
            }
        }

        if (canRemove && toRemove != null)
        {
            users.Remove(toRemove);
        }
    }
}