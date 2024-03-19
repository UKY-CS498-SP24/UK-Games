using MySqlConnector;

namespace UK_Games.Infrastructure;

public class DatabaseManager
{
    private MySqlConnectionStringBuilder cnnBuilder = new MySqlConnectionStringBuilder();

    private MySqlConnection cnn;
    public Boolean connected = false;
    
    public Boolean testDB = false;

    public DatabaseManager()
    {
        cnnBuilder.Server = "localhost";
        cnnBuilder.Port = 3306;
        cnnBuilder.UserID = "root";
        cnnBuilder.Password = "Sabumnim 2017";
        cnnBuilder.Database = "UKGames";
    }

    public void OpenConnection()
    {
        try
        {
            Console.WriteLine("Opening connection...\n\t" + cnnBuilder.ToString());
            //Console.WriteLine(Environment.StackTrace);
            cnn = new MySqlConnection(cnnBuilder.ToString());
            Connection().Open();
            Console.WriteLine("---------------------------------------------");
            Console.WriteLine("Database Connected!");
            Console.WriteLine("---------------------------------------------");
            connected = true;
        }
        catch (Exception e)
        {
            Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++");
            Console.WriteLine("");
            Console.WriteLine("Could not connect to the database at...");
            Console.WriteLine(cnnBuilder.ToString());
            Console.WriteLine("");
            Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++++++");
            Console.WriteLine(e.StackTrace);
            connected = false;
        }
    }

    public void CloseConnection()
    {
        try
        {
            Connection().Close();

            Console.WriteLine("---------------------------------------------");
            Console.WriteLine("Database Connection Closed Successfully...");
            Console.WriteLine("---------------------------------------------");
            connected = false;
        }
        catch (Exception e)
        {
            Console.WriteLine("---------------------------------------------");
            Console.WriteLine("Error Closing Database Connection...");
            Console.WriteLine(e.Message);
            Console.WriteLine(e.StackTrace);
            Console.WriteLine("---------------------------------------------");
            connected = true;
        }
    }
    
    public void SmartInsert(string tableName, string insertionStatement)
    {
        using (var transaction = Connection().BeginTransaction())
        {
            string query = "INSERT INTO " + tableName + " " + insertionStatement;
            Console.WriteLine(query);

            MySqlDataAdapter adapter = new MySqlDataAdapter();
            adapter.InsertCommand = new MySqlCommand(query, Connection());
            adapter.InsertCommand.Transaction = transaction;
            adapter.InsertCommand.ExecuteNonQuery();
            adapter.InsertCommand.Dispose();
            transaction.Commit();
        }
    }
    
    public void SmartInsert(string tableName, List<string> originalCol, List<string> originalVal)
    {
        /*
        List<string> columns = new List<string>();
        List<string> values = new List<string>();
        
        foreach (string column in originalCol)
        {
            columns.Add(GeneralMethods.GenerateEscapeString(column));
        }
        
        foreach (string value in originalVal)
        {
            values.Add(GeneralMethods.GenerateEscapeString(value));
        }
        */
        
        List<string> alteredVal = new List<string>();

        foreach (string val in originalVal)
        {
            if (val == null)
            {
                continue;
            }
            alteredVal.Add(val.Replace("\'", "\'\'"));
        }
        
        string insertColumns = String.Join(", ", originalCol);
        string insertValues = "\'" + String.Join("\', \'", alteredVal) + "\'";
        
        SmartInsert(tableName, insertColumns, insertValues);
    }
    
    public void SmartInsert(string tableName, string updateColumn, string updateValue)
    {
        string insert = "(" + updateColumn + ") VALUES (" + updateValue.Replace("\'NULL\'", "NULL") + ")";
        SmartInsert(tableName, insert);
    }

    public void SmartModify(int id, string tableName, string modificationStatement)
    {
        using (var transaction = Connection().BeginTransaction())
        {
            string query = "UPDATE " + tableName + " SET " + modificationStatement.Replace("\'NULL\'", "NULL") +
                           " WHERE ID=\'" + id + "\'";

            Console.WriteLine(query);

            MySqlDataAdapter adapter = new MySqlDataAdapter();
            adapter.UpdateCommand = new MySqlCommand(query, Connection());
            adapter.UpdateCommand.Transaction = transaction;
            adapter.UpdateCommand.ExecuteNonQuery();
            adapter.UpdateCommand.Dispose();
            
            transaction.Commit();
        }
    }
    

    public void SmartModify(int id, string tableName, string updateColumn, string updateValue)
    {
        SmartModify(id, tableName, (updateColumn + " = " + updateValue));
    }

    public void SmartModify(int id, string tableName, List<string> columns, List<string> values)
    {
        List<string> modifications = new List<string>();

        foreach (KeyValuePair<string, string> update in columns.Zip(values,(k, v) => new { k, v })
                                                               .ToDictionary(x => x.k, x => x.v))
        {
            modifications.Add(update.Key + " = \'" + update.Value.Replace("\'", "\'\'") + "\'");
        }

        string modificationString = String.Join(", ", modifications);
        
        SmartModify(id, tableName, modificationString);
    }

    public void SmartRemove(int id, string tableName)
    {
        using (var transaction = Connection().BeginTransaction())
        {
            string query = "DELETE FROM " + tableName + " WHERE ID=\'" + id + "\'";
            Console.WriteLine(query);

            MySqlDataAdapter adapter = new MySqlDataAdapter();
            adapter.DeleteCommand = new MySqlCommand(query, Connection());
            adapter.DeleteCommand.Transaction = transaction;
            adapter.DeleteCommand.ExecuteNonQuery();

            adapter.DeleteCommand.Dispose();
            
            transaction.Commit();
        }
    }

    // Uses string tableName, which is the table to be created
    // and Dictionary<string, string> fields, which should be
    // in the format: <string fieldName, fieldInfo>. For example,
    // C#:
    //      int id;
    //      string firstName;
    // To SQL:
    //      fields.add("ID", "INT NOT NULL AUTO_INCREMENT");
    //      fields.add("FirstName", "TEXT");
    //
    // NOTE: MUST USE A PRIMARY KEY LABELED "ID". OTHERWISE,
    // PLEASE SETUP OWN METHOD 
    public void SmartCreate(string tableName, Dictionary<string, string> fields)
    {
        using (var transaction = Connection().BeginTransaction())
        {
            string fieldsString = "";
            foreach (KeyValuePair<string, string> field in fields)
            {
                fieldsString += (field.Key + " " + field.Value + ",");
            }

            string query = "CREATE TABLE IF NOT EXISTS `" + tableName + "` (" + fieldsString + "PRIMARY KEY (ID))";
            MySqlCommand createTable = new MySqlCommand(query, cnn);
            createTable.Transaction = transaction;
            createTable.ExecuteNonQuery();
            
            transaction.Commit();
        }
    }

    public MySqlDataReader GetAll(string tableName, string column)
    {
        string query = "SELECT " + column + " FROM " + tableName;
        Console.WriteLine(query);

        MySqlCommand cmd = new MySqlCommand(query, Connection());
        MySqlDataReader dataReader = cmd.ExecuteReader();

        return dataReader;
    }

    public int GetNextID(string tableName, bool update)
    {
        int nextID = 0;
        int selectionID = 0;

        string query = "SELECT * FROM " + DataUtil.IDsTable + " WHERE TableName = \'" + tableName + "\'";
        Console.WriteLine(query);
        
        MySqlCommand cmd = new MySqlCommand(query, Connection());
        MySqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            selectionID = reader.GetInt32("ID");
            nextID = Int32.Parse(reader.GetString("LastID")) + 1;
        }
        reader.Close();

        if (update)
        {
            SmartModify(selectionID, DataUtil.IDsTable, "LastID", nextID.ToString());
        }
        
        return nextID;
    }
    
    public MySqlConnection Connection()
    {
        return cnn;
    }
}