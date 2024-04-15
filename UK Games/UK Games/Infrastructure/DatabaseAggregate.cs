using UK_Games.Models;

namespace UK_Games.Infrastructure;

public class DatabaseAggregate
{
    private List<Game> games;
    public List<Game> Games => games;
    private List<User> users;
    public List<User> Users => users;

    public DatabaseAggregate()
    {
        games = new List<Game>();
        users = new List<User>();
    }

    public void WipeCache()
    {
        games.Clear();
        users.Clear();
    }


    public void AddGame(Game game)
    {
        foreach (var g in games)
        {
            if (g.ID == game.ID)
                return;
        }
        games.Add(game);
    }

    public void AddUser(User user)
    {
        foreach (var u in users)
        {
            if (u.ID == user.ID)
                return;
        }
        users.Add(user);
    }

    public void RemoveGame(Game game)
    {
        games.Remove(game);
    }

    public void RemoveGame(int id)
    {
        foreach (var g in games)
        {
            if (g.ID == id)
            {
                games.Remove(g);
                return;
            }
        }
    }

    public void RemoveUser(User user)
    {
        foreach (var u in users)
        {
            if (u.ID == user.ID)
            {
                users.Remove(user);
                return;
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
                return;
            }
        }
    }
}