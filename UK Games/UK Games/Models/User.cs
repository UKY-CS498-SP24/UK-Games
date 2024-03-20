using UK_Games.Infrastructure;

namespace UK_Games.Models;

public class User
{
    private static string tableName = "UKGames_Users";
    
    private int id;
    private string username;
    
    private string firstName;
    private string lastName;
    
    private DateTime DOB;
    
    private string email;
    private string password;

    private Dictionary<DateTime, Game> played;

    public User(int id, string username, string firstName, string lastName, DateTime dob, string email, string password, Dictionary<DateTime, Game> played)
    {
        this.id = id;
        this.username = username;
        this.firstName = firstName;
        this.lastName = lastName;
        DOB = dob;
        this.email = email;
        this.password = password;
        this.played = played;
    }

    public User(string username, string firstName, string lastName, DateTime dob, string email, string password)
    {
        id = DataUtil.DB.GetNextID(tableName, true);
        
        this.username = username;
        this.firstName = firstName;
        this.lastName = lastName;
        DOB = dob;
        this.email = email;
        this.password = password;
        this.played = new Dictionary<DateTime, Game>();

        SaveNew();
    }

    public static void CreateTable()
    {
        Dictionary<string, string> fields = new Dictionary<string, string>();

        fields.Add("ID", "INT NOT NULL AUTO_INCREMENT");
        fields.Add("Username", "TEXT");
        fields.Add("FirstName", "TEXT");
        fields.Add("LastName", "TEXT");
        fields.Add("DateOfBirth", "TEXT");
        fields.Add("Email", "TEXT");
        fields.Add("Password", "TEXT");
        fields.Add("PlayedGames", "TEXT");

        DataUtil.DB.SmartCreate(tableName, fields);
    }

    public void SaveNew()
    {
        Dictionary<string, string> save = DBValues();
        DataUtil.DB.SmartInsert(tableName, save.Keys.ToList(), save.Values.ToList());
        DataUtil.Data.AddUser(this);
    }

    public void Save()
    {
        Dictionary<string, string> save = DBValues();
        DataUtil.DB.SmartModify(id, tableName, save.Keys.ToList(), save.Values.ToList());
        DataUtil.Data.RemoveUser(this);
        DataUtil.Data.AddUser(this);
    }

    public Dictionary<string, string> DBValues()
    {
        Dictionary<string, string> save = new Dictionary<string, string>();

        save.Add("Username", username);
        save.Add("FirstName", firstName);
        save.Add("LastName", lastName);
        save.Add("DateOfBirth", DOB.ToString());
        save.Add("Email", email);
        save.Add("Password", password);
        save.Add("PlayedGames", GeneralMethods.ParsePlayed(played));

        return save;
    }

    public static string TableName => tableName;

    public int ID => id;

    public string Username => username;

    public Dictionary<DateTime, Game> Played => played;

    public string FirstName
    {
        get => firstName;
        set => firstName = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string LastName
    {
        get => lastName;
        set => lastName = value ?? throw new ArgumentNullException(nameof(value));
    }

    public DateTime Birthday
    {
        get => DOB;
        set => DOB = value;
    }

    public string Email
    {
        get => email;
        set => email = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string Password
    {
        get => password;
        set => password = value ?? throw new ArgumentNullException(nameof(value));
    }

    public void AddPlayed(Game game)
    {
        if (game != null)
        {
            played.Add(DateTime.Now, game);
        }
    }

    public void AddPlayed(int id)
    {
        Game game = GeneralMethods.GetGame(id);

        if (game != null)
        {
            played.Add(DateTime.Now,game);
        }
    }
}