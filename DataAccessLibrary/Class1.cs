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
        public async static void InitializeDatabase() //создание или открытие таблицы (ID, день, месяц, год, сумма, категория, комментарий)
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

        public static void AddData(string Day, string Month, string Year, string Amount, string Category, string Commentary, int income) //добавляет данные в таблицу, income - индикатор, доход или расход
        {
            string dbpath = System.IO.Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "sqliteSample.db");
            using (SqliteConnection db =
              new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;


                insertCommand.CommandText = "INSERT INTO MyTable VALUES (NULL, @Day, @Month, @Year, @Entry, @Category, @Commentary);";
                
                if (income == 1) insertCommand.Parameters.AddWithValue("@Entry", Amount);
                else insertCommand.Parameters.AddWithValue("@Entry", "-" + Amount); //если расход, то возвращает отрицательное значение

                insertCommand.Parameters.AddWithValue("@Day", Day);
                insertCommand.Parameters.AddWithValue("@Month", Month);
                insertCommand.Parameters.AddWithValue("@Year", Year);
                insertCommand.Parameters.AddWithValue("@Category", Category);
                insertCommand.Parameters.AddWithValue("@Commentary", Commentary);

                insertCommand.ExecuteReader();

                db.Close();
            }

        }


        public static void UpdateData(string ID, string Day, string Month, string Year, string Amount, string Category, string Commentary, int income) //функция обновления данных по ID
        {

            string dbpath = System.IO.Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "sqliteSample.db");
            using (SqliteConnection db =
              new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                
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

        public static void DeleteData(string idText) //функция удаляет данные по ID
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


        public static void DeleteAllData() //функция удаляет все данные с таблицы
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
        public static List<String> GetData(string Day1, string Month1, string Year1, string Day2, string Month2, string Year2, bool stat, string Category, out float sum, out int operations)
        { //функция возвращает список данных за период или все данные, по категориями или нет
            List<String> entries = new List<string>();

            string dbpath = System.IO.Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "sqliteSample.db");
            using (SqliteConnection db =
               new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();
                

                SqliteCommand selectCommand;
                if (!stat) selectCommand = new SqliteCommand("SELECT * from MyTable", db);
                else
                {
                    if (Category == "") selectCommand = new SqliteCommand("SELECT * from MyTable WHERE (Year BETWEEN @Year1 AND @Year2)", db);
                    else selectCommand = new SqliteCommand("SELECT * from MyTable WHERE (Year BETWEEN @Year1 AND @Year2 AND Category = @Category)", db);
                    selectCommand.Parameters.AddWithValue("@Year1", Year1);
                    selectCommand.Parameters.AddWithValue("@Year2", Year2);
                    selectCommand.Parameters.AddWithValue("@Category", Category);
                }

                SqliteDataReader query = selectCommand.ExecuteReader();

                sum = 0;
                operations = 0;
                while (query.Read())
                {
                    if (stat) //если выбрано вернуть данные за период, начинается решение, лежит ли дата в заданном диапазоне
                    {
                        bool ShowData = false;
                        int Day1I = Int32.Parse(Day1), Month1I = Int32.Parse(Month1), Year1I = Int32.Parse(Year1);
                        int Day2I = Int32.Parse(Day2), Month2I = Int32.Parse(Month2), Year2I = Int32.Parse(Year2);
                        
                        if (Year1I == Year2I)
                        {
                            if ((Month1I <= query.GetInt32(2)) && (Month2I >= query.GetInt32(2)))
                            {
                                if (Month1I == Month2I)
                                {
                                    if ((Day1I <= query.GetInt32(1)) && (Day2I >= query.GetInt32(1))) ShowData = true;
                                }

                                if ((Month1I != Month2I) && (query.GetInt32(2) == Month1I))
                                    if (Day1I <= query.GetInt32(1)) ShowData = true;

                                if ((Month1I != Month2I) && (query.GetInt32(2) == Month2I))
                                    if (Day2I >= query.GetInt32(1)) ShowData = true;
                                
                                if ((Month1I < query.GetInt32(2)) && (Month2I > query.GetInt32(2))) ShowData = true;
                            }
                        }

                        if ((Year1I != Year2I) && (query.GetInt32(3) == Year1I))
                        {
                            if (Month1I <= query.GetInt32(2))
                            {
                                if (query.GetInt32(2) == Month1I)
                                    if (Day1I <= query.GetInt32(1)) ShowData = true;

                                if (Month1I < query.GetInt32(2)) ShowData = true;
                            }
                        }

                        if ((Year1I != Year2I) && (query.GetInt32(3) == Year2I))
                        {
                            if (Month2I >= query.GetInt32(2))
                            {
                                if (query.GetInt32(2) == Month2I)
                                    if (Day2I >= query.GetInt32(1)) ShowData = true;

                                if (Month2I > query.GetInt32(2)) ShowData = true;
                            }
                        }


                        if ((Year1I < query.GetInt32(3)) && (Year2I > query.GetInt32(3))) ShowData = true;



                        if (!ShowData) continue;
                    }

                    string Day = query.GetString(1);
                    if (Day.Length == 1) Day = "0" + Day;

                    string Month = query.GetString(2);
                    if (Month.Length == 1) Month = "0" + Month;

                    string Year = query.GetString(3);
                    if (Year.Length == 1) Year = "0" + Year; //вывод данных в удобном формате
                    String Output = "ID: " + query.GetString(0) + "  Дата: " + Day + "." + Month + "." + Year + "  Сумма: " + query.GetString(4) + "  Категория: " + query.GetString(5) + "  Комментарий: " + query.GetString(6);
                    entries.Add(Output);
                    sum += query.GetFloat(4); //считает итоговую сумму по данным
                    operations++; //считает количество совершенных операций по данным
                }

                db.Close();
            }

            return entries;
        }


        public static string Sum(string ID, string Day, string Month, string Year, string Amount, string Category, string Commentary, bool total, out int Operations) //возвращает сумму, согласно выбранным критериям
        {
            float sum = 0;
            Operations = 0;
            string dbpath = System.IO.Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "sqliteSample.db");
            using (SqliteConnection db =
               new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();
                SqliteCommand selectCommand;
                if ((ID + Day + Month + Year + Amount + Category + Commentary == "") && (!total)) total = true; //проверка, если нет критерий, если поля для критерией пустые, то выводит общую сумму

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

                if (total) selectCommand = new SqliteCommand("SELECT Amount from MyTable", db); //total - индикатор, если хотим вывести полную сумму в таблице
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
                    Operations++; //подсчёт количества операций и сумму по критериям или полную сумму
                    sum += query.GetFloat(0);
                }

                db.Close();
            }

            return sum.ToString();
        }
    }
}