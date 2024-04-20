using UK_Games.Models;

namespace UK_Games.Infrastructure;

public class DatabaseAggregate
{
    public Dictionary<int,Game> games;
    public Dictionary<int,Game>.ValueCollection Games => games.Values;
    public Dictionary<int,User> users;
    public Dictionary<int,User>.ValueCollection Users => users.Values;

    public DatabaseAggregate()
    {
        games = new Dictionary<int,Game>();
        users = new Dictionary<int,User>();
    }

    public void WipeCache()
    {
        games.Clear();
        users.Clear();
    }

    public void AddGame(Game game)
    {
        games[game.ID] = game;
    }

    public void AddUser(User user)
    {
        users[user.ID] = user;
    }

    public void RemoveGame(Game game)
    {
        RemoveGame(game.ID);
    }

    public void RemoveGame(int id)
    {
        games.Remove(id);
    }

    public void RemoveUser(User user)
    {
        RemoveUser(user.ID);
    }

    public void RemoveUser(int id)
    {
        users.Remove(id);
    }
}