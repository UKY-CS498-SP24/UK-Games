namespace UK_Games.Models;

public class User
{
    private string username, password;
    public string Username { get => username; }
    public string Password { get => password; }

    public User(string _username, string _password)
    {
        this.username = _username;
        this.password = _password;
    }
}