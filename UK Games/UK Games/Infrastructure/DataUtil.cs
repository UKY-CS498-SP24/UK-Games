using System.Runtime.CompilerServices;
using MySqlConnector;
using UK_Games.Models;

namespace UK_Games.Infrastructure;

public class DataUtil
{
    private static DatabaseManager db;
    private static DatabaseAggregate data;
    public static readonly string IDsTable = "LastUsedIDs"; 

    public DataUtil()
    {
        db = new DatabaseManager();
        data = new DatabaseAggregate();
    }

    public static DatabaseManager DB => db;

    public static DatabaseAggregate Data => data;

    public static void CreateTables()
    {
        //  0. IDsTable
        Dictionary<string, string> lastUsedIDs = new Dictionary<string, string>();
        lastUsedIDs.Add("ID", "INT NOT NULL AUTO_INCREMENT");
        lastUsedIDs.Add("TableName", "VARCHAR(255)");
        lastUsedIDs.Add("LastID", "TEXT");
        lastUsedIDs.Add("CONSTRAINT Unique_Table", "UNIQUE (TableName)");
        DB.SmartCreate(IDsTable, lastUsedIDs);
        
        //  1. Game - Base Data Types
        Game.CreateTable();
        DB.SmartInsert(IDsTable, "TableName, LastID", "\'" + Game.TableName + "\', " + "\'0\'");
        
        //  2. User - Base Data Types, List of GameIDs and DateTimes
        User.CreateTable();
        DB.SmartInsert(IDsTable, "TableName, LastID", "\'" + User.TableName + "\', " + "\'0\'");
    }

    public static void PullDataFromDB()
    {
        Data.WipeCache();
        
        MySqlDataReader reader;

        //  1. Game - Base Data Types
       reader = db.GetAll(Game.TableName, "*");
       while (reader.Read())
       {
           data.AddGame(
                new Game(
                reader.GetInt32(0),
                reader.GetString(1),
                reader.GetString(2),
                reader.GetString(3),
                reader.GetString(4)
                ));
        }
        reader.Close();
        
        //  2. User - Base Data Types, List of GameIDs and DateTimes
        reader = db.GetAll(User.TableName, "*");
        while (reader.Read())
        {
            Dictionary<DateTime, Game> played = new Dictionary<DateTime, Game>();
            
            if (reader.GetValue(7) != DBNull.Value)
            {
                played = GeneralMethods.ParsePlayed(reader.GetString(7));
            }
            
            data.AddUser(
                new User(
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetString(3),
                    DateTime.Parse(reader.GetString(4)),
                    reader.GetString(5),
                    reader.GetString(6),
                    played
                ));
        }
        reader.Close();
    }
}