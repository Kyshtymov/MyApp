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


                String tableCommand = "CREATE TABLE IF NOT " +
                    "EXISTS MyTable (id INTEGER PRIMARY KEY, " +
                    "Day INT, Month INT, Year INT, Amount REAL, Category NVARCHAR(2048), Commentary NVARCHAR(2048))";
                SqliteCommand createTable = new SqliteCommand(tableCommand, db);

                createTable.ExecuteReader();
            }
        }

        public static void AddData(string Day, string Month, string Year, string Amount, string Category, string Commentary, int income)
        {

            string dbpath = System.IO.Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "sqliteSample.db");
            using (SqliteConnection db =
              new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                // Use parameterized query to prevent SQL injection attacks

                insertCommand.CommandText = "INSERT INTO MyTable VALUES (NULL, @Day, @Month, @Year, @Entry, @Category, @Commentary);";
                
                if (income == 1) insertCommand.Parameters.AddWithValue("@Entry", Amount);
                else insertCommand.Parameters.AddWithValue("@Entry", "-" + Amount);

                insertCommand.Parameters.AddWithValue("@Day", Day);
                insertCommand.Parameters.AddWithValue("@Month", Month);
                insertCommand.Parameters.AddWithValue("@Year", Year);
                insertCommand.Parameters.AddWithValue("@Category", Category);
                insertCommand.Parameters.AddWithValue("@Commentary", Commentary);

                insertCommand.ExecuteReader();

                db.Close();
            }

        }


        public static void UpdateData(string ID, string Day, string Month, string Year, string Amount, string Category, string Commentary, int income)
        {

            string dbpath = System.IO.Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "sqliteSample.db");
            using (SqliteConnection db =
              new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                // Use parameterized query to prevent SQL injection attacks
                
                insertCommand.CommandText = "UPDATE MyTable SET id = @ID,  Day = @Day, Month = @Month, Year = @Year, Amount = @Entry, Category = @Category, Commentary = @Commentary WHERE id = @ID;";
                if (income == 1)  insertCommand.Parameters.AddWithValue("@Entry", Amount);
                else  insertCommand.Parameters.AddWithValue("@Entry", "-" + Amount);

                insertCommand.Parameters.AddWithValue("@ID", ID);
                insertCommand.Parameters.AddWithValue("@Day", Day);
                insertCommand.Parameters.AddWithValue("@Month", Month);
                insertCommand.Parameters.AddWithValue("@Year", Year);
                insertCommand.Parameters.AddWithValue("@Category", Category);
                insertCommand.Parameters.AddWithValue("@Commentary", Commentary);

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
                    string Day = query.GetString(1);
                    if (Day.Length == 1) Day = "0" + Day;

                    string Month = query.GetString(2);
                    if (Month.Length == 1) Month = "0" + Month;

                    string Year = query.GetString(3);
                    if (Year.Length == 1) Year = "0" + Year;
                    String Output = "ID: " + query.GetString(0) + "  Дата: " + Day + "." + Month + "." + Year + "  Сумма: " + query.GetString(4) + "  Категория: " + query.GetString(5) + "  Комментарий: " + query.GetString(6);
                    entries.Add(Output);
                }

                db.Close();
            }

            return entries;
        }


        public static string Sum(string ID, string Day, string Month, string Year, string Amount, string Category, string Commentary, bool total, out int Operations)
        {
            float sum = 0;
            Operations = 0;
            string dbpath = System.IO.Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "sqliteSample.db");
            using (SqliteConnection db =
               new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();
                SqliteCommand selectCommand;
                if ((ID + Day + Month + Year + Amount + Category + Commentary == "") && (!total)) return "0";

                string IDCom = "", DayCom = "", MonthCom = "", YearCom = "", AmountCom = "", CategoryCom = "", CommentaryCom = "";

                if (ID != "")
                    IDCom = " id = @ID ";
                else
                    IDCom = " id > 0 ";

                if (Day != "") DayCom = " AND Day = @Day ";
                if (Month != "") MonthCom = " AND Month = @Month ";
                if (Year != "") YearCom = " AND Year = @Year ";
                if (Amount != "") AmountCom = " AND Amount = @Amount ";
                if (Category != "") CategoryCom = " AND Category = @Category ";
                if (Commentary != "") CommentaryCom = " AND Commentary = @Commentary ";
                if (total) selectCommand = new SqliteCommand("SELECT Amount from MyTable", db);
                else
                {
                    string Command = "SELECT Amount from MyTable Where " + IDCom + DayCom + MonthCom + YearCom + AmountCom + CategoryCom + CommentaryCom;
                    selectCommand = new SqliteCommand(Command, db);
                    if (ID != "") selectCommand.Parameters.AddWithValue("@ID", ID);
                    if (Day != "") selectCommand.Parameters.AddWithValue("@Day", Day);
                    if (Month != "") selectCommand.Parameters.AddWithValue("@Month", Month);
                    if (Year != "") selectCommand.Parameters.AddWithValue("@Year", Year);
                    if (Amount != "") selectCommand.Parameters.AddWithValue("@Amount", Amount);
                    if (Category != "") selectCommand.Parameters.AddWithValue("@Category", Category);
                    if (Commentary != "") selectCommand.Parameters.AddWithValue("@Commentary", Commentary);
                }

                SqliteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    Operations++;
                    sum += query.GetFloat(0);
                }

                db.Close();
            }

            return sum.ToString();
        }
    }
}