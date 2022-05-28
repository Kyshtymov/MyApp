using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace DataAccessLibrary
{
    public static class DataAccess
    {
        public async static void InitializeDatabase()
        {
            await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFileAsync("sqliteSample.db", Windows.Storage.CreationCollisionOption.OpenIfExists);
            string dbpath = System.IO.Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "sqliteSample.db");
            using (SqliteConnection db =
               new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;


                insertCommand.CommandText = " DROP TABLE MyTable;";
                insertCommand.ExecuteReader();

               // String tableCommand = "CREATE TABLE IF NOT " +
                 //   "EXISTS MyTable (Primary_Key INTEGER PRIMARY KEY, " +
                 //   "Text_Entry NVARCHAR(2048) NULL, Income INT)";
                String tableCommand = "CREATE TABLE IF NOT " +
                    "EXISTS MyTable (id INTEGER PRIMARY KEY, " +
                    "Amount REAL NULL, Category NVARCHAR(2048))";
                SqliteCommand createTable = new SqliteCommand(tableCommand, db);

                createTable.ExecuteReader();
            }
        }

        public static void AddData(string Amount, string Category, int income)
        {

            string dbpath = System.IO.Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "sqliteSample.db");
            using (SqliteConnection db =
              new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                // Use parameterized query to prevent SQL injection attacks
                if (income == 1)
                {
                    insertCommand.CommandText = "INSERT INTO MyTable VALUES (NULL, @Entry, @Category);";
                    insertCommand.Parameters.AddWithValue("@Entry", Amount);
                }
                else
                {
                    insertCommand.CommandText = "INSERT INTO MyTable VALUES (NULL, @Entry, 0);";
                    insertCommand.Parameters.AddWithValue("@Entry", "-" + Amount);
                }
                insertCommand.Parameters.AddWithValue("@Category", Category);

                insertCommand.ExecuteReader();

                db.Close();
            }

        }

        public static void DeleteData(string idText)
        {
            string dbpath = System.IO.Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "sqliteSample.db");
            using (SqliteConnection db =
              new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                insertCommand.CommandText = "DELETE FROM MyTable WHERE id = @ID;";
                insertCommand.Parameters.AddWithValue("@ID", idText);
                insertCommand.ExecuteReader();

                db.Close();
            }

        }


        public static void DeleteAllData()
        {
            string dbpath = System.IO.Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "sqliteSample.db");
            using (SqliteConnection db =
              new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                insertCommand.CommandText = "DELETE FROM MyTable;";
                insertCommand.ExecuteReader();

                db.Close();
            }

        }
        public static List<String> GetData()
        {
            List<String> entries = new List<string>();

            string dbpath = System.IO.Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "sqliteSample.db");
            using (SqliteConnection db =
               new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                SqliteCommand selectCommand = new SqliteCommand
                    ("SELECT * from MyTable", db);

                SqliteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    String Output = query.GetString(0) + "  " +  query.GetString(1) + "  " + query.GetString(2);
                    entries.Add(Output);
                    //entries.Add(query.GetString(0));
                }

                db.Close();
            }

            return entries;
        }


        public static string Sum()
        {
            float sum = 0;

            string dbpath = System.IO.Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "sqliteSample.db");
            using (SqliteConnection db =
               new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                SqliteCommand selectCommand = new SqliteCommand
                    ("SELECT * from MyTable", db);

                SqliteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                      sum += query.GetFloat(1);
                }

                db.Close();
            }

            return sum.ToString();
        }
    }
}