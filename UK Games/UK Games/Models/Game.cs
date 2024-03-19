using UK_Games.Infrastructure;

namespace UK_Games.Models;

public class Game
{
    private static string tableName = "UKGames_Games";
    
    private int id;
    private string name;
    private string refURL;
    private string embed;

    public Game(int id, string name, string refURL, string embed)
    {
        this.id = id;
        this.name = name;
        this.refURL = refURL;
        this.embed = embed;
    }

    public Game(string name, string embed)
    {
        id = DataUtil.DB.GetNextID(tableName, true);
        this.name = name;
        refURL = "";
        this.embed = embed;

        SaveNew();
    }

    public static void CreateTable()
    {
        Dictionary<string, string> fields = new Dictionary<string, string>();

        fields.Add("ID", "INT NOT NULL AUTO_INCREMENT");
        fields.Add("Name", "TEXT");
        fields.Add("ReferenceURL", "TEXT");
        fields.Add("Embed", "TEXT");

        DataUtil.DB.SmartCreate(tableName, fields);
    }

    public void SaveNew()
    {
        Dictionary<string, string> save = DBValues();
        DataUtil.DB.SmartInsert(tableName, save.Keys.ToList(), save.Values.ToList());
        DataUtil.Data.AddGame(this);
    }

    public void Save()
    {
        Dictionary<string, string> save = DBValues();
        DataUtil.DB.SmartModify(id, tableName, save.Keys.ToList(), save.Values.ToList());
        DataUtil.Data.RemoveGame(this);
        DataUtil.Data.AddGame(this);
    }

    public Dictionary<string, string> DBValues()
    {
        Dictionary<string, string> save = new Dictionary<string, string>();

        save.Add("Name", name);
        save.Add("ReferenceURL", refURL);
        save.Add("Embed", embed);

        return save;
    }

    public static string TableName => tableName;

    public int ID => id;

    public string Name
    {
        get => name;
        set => name = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string RefURL
    {
        get => refURL;
        set => refURL = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string Embed
    {
        get => embed;
        set => embed = value ?? throw new ArgumentNullException(nameof(value));
    }
}