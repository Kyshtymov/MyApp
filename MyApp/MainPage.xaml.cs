using DataAccessLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x419

namespace MyApp
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    /// 

public sealed partial class MainPage : Page
    {

        bool History = false; //отображение истории операций
        bool Update = false; //обновление по ID
        bool ShowSum = false; //отображение суммы
        bool CheckDay = true; //проверка корректности ввода данных
        bool CheckMonth = true; 
        bool CheckYear = true; 
        bool CheckAmount = true;

        List<String> categories_income = new List<String>();
        List<String> categories_outcome = new List<String>();
        public MainPage()
        {
            this.InitializeComponent();
            categories_income.Add(""); //инициализация списков на категории
            categories_income.Add("Заработная плата");
            categories_income.Add("Возврат долга");
            categories_income.Add("Дивиденты");
            categories_outcome.Add("");
            categories_outcome.Add("Развлечение");
            categories_outcome.Add("Еда");
            categories_outcome.Add("Транспорт");
        }





    void CheckData(string DayS, string MonthS, string YearS, out bool CheckDay, out bool CheckMonth, out bool CheckYear) //функция для проверки корретности даты
        {
            int Day;
            CheckDay = int.TryParse(DayS, out Day);
            if ((CheckDay) && (Day > 0) && (Day < 32)) CheckDay = true;
            else CheckDay = false;

            int Month;
            CheckMonth = int.TryParse(MonthS, out Month);
            if ((CheckMonth) && (Month > 0) && (Month < 12)) CheckMonth = true;
            else CheckMonth = false;

            int Year;
            CheckYear = int.TryParse(YearS, out Year);
            if ((CheckYear) && (Year > 0) && (Year < 23)) CheckYear = true;
            else CheckYear = false;
        }

        private void Log(string LogText) //функция отображения текста на экран
        {
            List<String> entries = new List<String>();
            int pos = 0;

            bool stat = true;

            bool CheckDayFilter1 = true, CheckMonthFilter1 = true, CheckYearFilter1 = true;
            bool CheckDayFilter2 = true, CheckMonthFilter2 = true, CheckYearFilter2 = true; //для проверки корректности ввода даты для периодов


            if ((Day_filter_1.Text != "") && (Month_filter_1.Text != "") && (Year_filter_1.Text != "") && (Day_filter_2.Text != "") && (Month_filter_2.Text != "") && (Year_filter_2.Text != ""))
            {
                CheckData(Day_filter_1.Text, Month_filter_1.Text, Year_filter_1.Text, out CheckDayFilter1, out CheckMonthFilter1, out CheckYearFilter1);
                CheckData(Day_filter_2.Text, Month_filter_2.Text, Year_filter_2.Text, out CheckDayFilter2, out CheckMonthFilter2, out CheckYearFilter2);
            }
            else stat = false;

            bool CheckFilter = CheckDayFilter1 && CheckMonthFilter1 && CheckYearFilter1 && CheckDayFilter2 && CheckMonthFilter2 && CheckYearFilter2;
            if (!CheckFilter) stat = false;

            float sum = 0;
            int operations_filter = 0;

            if (History) entries = DataAccess.GetData(Day_filter_1.Text, Month_filter_1.Text, Year_filter_1.Text, Day_filter_2.Text, Month_filter_2.Text, Year_filter_2.Text, stat, "", out sum, out operations_filter);
            //вывод истории на экран, если надо, если введены даты периода для статистики, отображение операций за период



            //выводы информации по вводу некорректных данных

            if (!CheckDayFilter1)
            {
                entries.Insert(pos, "Введите день от 1 до 31 числа для начала периода отчёта");
                pos++;
            }
            
            if (!CheckMonthFilter1)
            {
                entries.Insert(pos, "Введите месяц от 1 до 12 для начала периода отчёта");
                pos++;
            }
            
            if (!CheckYearFilter1)
            {
                entries.Insert(pos, "Введите год от 1 до 23 для начала периода отчёта");
                pos++;
            }

            if (!CheckDayFilter2)
            {
                entries.Insert(pos, "Введите день от 1 до 31 числа для конца периода отчёта");
                pos++;
            }
            
            if (!CheckMonthFilter2)    
            {
                entries.Insert(pos, "Введите месяц от 1 до 12 для конца периода отчёта");
                pos++;
            }
            
            if (!CheckYearFilter2)
            {
                entries.Insert(pos, "Введите год от 1 до 23 для конца периода отчёта");
                 pos++;
            }
            
            if (!CheckDay)
            {
                entries.Insert(pos, "Введите день от 1 до 31 числа");
                pos++;
            }
            if (!CheckMonth)
            {
                entries.Insert(pos, "Введите месяц от 1 до 12");
                pos++;
            }
            if (!CheckYear)
            {
                entries.Insert(pos, "Введите год от 1 до 23");
                pos++;
            }
            if (!CheckAmount)
            {
                entries.Insert(pos, "Введите сумму от 0");
                pos++;
            }

            if (ShowSum) //вывод суммы по критериям (если введены в поля для ввода данных значения, то выводит сумму и количество операций по этим критериям), полной суммы или суммы за период
            {
                int Operations, OperationsTotal;
                //функция поиска суммы по критериям
                string sum1 = DataAccess.Sum(Delete_ID.Text, Input_Day.Text, Input_Month.Text, Input_Year.Text, Input_Amount.Text, Input_Category.Text, Input_Commentary.Text, false, out Operations);
                string sum2 = DataAccess.Sum(Delete_ID.Text, Input_Day.Text, Input_Month.Text, Input_Year.Text, Input_Amount.Text, Input_Category.Text, Input_Commentary.Text, true, out OperationsTotal);
                string CommandSum = "Прибыль по критериям = " + sum1 + "  Количество операций по критериям = " + Operations;
                string CommandSum2 = "Полная прибыль = " + sum2 + "  Полное количество операций = " + OperationsTotal;
                string CommandSum3 = "Прибыль за период = " + sum.ToString() + "  Количество операций за период = " + operations_filter.ToString();

                entries.Insert(pos, CommandSum);
                if (!stat) entries.Insert(pos + 1, CommandSum2);
                if (stat) entries.Insert(pos + 1, CommandSum3);
                pos += 2;
            }

            if (stat)
            {
                float[] sumCategory = new float[4];
                int[] operationCategory = new int[4];
                int k = 0;
                foreach (string Category in categories_income)
                {
                    if (Category != "")
                    {
                        DataAccess.GetData(Day_filter_1.Text, Month_filter_1.Text, Year_filter_1.Text, Day_filter_2.Text, Month_filter_2.Text, Year_filter_2.Text, stat, Category, out sumCategory[k], out operationCategory[k]);
                        entries.Insert(pos, "Доход по категории   " + Category + " = " + sumCategory[k].ToString() + "     Операций = " + operationCategory[k].ToString());
                        pos++;
                        k++;
                    }
                }
               
                k = 0;
                foreach (string Category in categories_outcome)
                {
                    if (Category != "")
                    {
                        DataAccess.GetData(Day_filter_1.Text, Month_filter_1.Text, Year_filter_1.Text, Day_filter_2.Text, Month_filter_2.Text, Year_filter_2.Text, stat, Category, out sumCategory[k], out operationCategory[k]);
                        entries.Insert(pos, "Расход по категории   " + Category + " = " + sumCategory[k].ToString() + "     Операций = " + operationCategory[k].ToString());
                        pos++;
                        k++;
                    }
                }
                DataAccess.GetData(Day_filter_1.Text, Month_filter_1.Text, Year_filter_1.Text, Day_filter_2.Text, Month_filter_2.Text, Year_filter_2.Text, stat, "Прочее", out sumCategory[k], out operationCategory[k]);
                entries.Insert(pos, "Прибыль по категории   " + "Прочее" + " = " + sumCategory[k].ToString() + "     Операций = " + operationCategory[k].ToString());
                pos++;
            }

            if ((History) && (!stat)) entries.Insert(pos, "История операций:");
            if ((History) && (stat)) entries.Insert(pos, "История операций за период:");

            if (LogText != "") entries.Insert(0, LogText); //вывод текста, если необходимо
            Output.ItemsSource = entries;
            entries = null;
        }


        private void WriteInTable(int income) //запись данных в таблицу, income - индикатор дохода или расхода
        {
            string Category = Input_Category.Text;
            if (Input_Category.Text == "")  Category = "Прочее"; //если в категории пусто, то его закидывает в прочее

            CheckData(Input_Day.Text, Input_Month.Text, Input_Year.Text, out CheckDay, out CheckMonth, out CheckYear); //проверки входных данных

            float Amount = 0;
            CheckAmount = float.TryParse(Input_Amount.Text, out Amount);
            if ((CheckAmount) && (Amount > 0)) CheckAmount = true;
            else CheckAmount = false;

            bool Check = CheckDay && CheckMonth && CheckYear && CheckAmount;
            if (!Check) Log("");

            if ((Check) && (!Update))
            {
                DataAccess.AddData(Input_Day.Text, Input_Month.Text, Input_Year.Text, Input_Amount.Text, Category, Input_Commentary.Text, income); //заносит данные в таблицу
                Log("");
            }
        
            if ((Check) && (Update))
            {
                if (Delete_ID.Text != "")
                {
                    int ID = 0;
                    bool CheckID = int.TryParse(Delete_ID.Text, out ID);
                    if ((CheckID) && (ID > 0))
                    {
                        DataAccess.UpdateData(Delete_ID.Text, Input_Day.Text, Input_Month.Text, Input_Year.Text, Input_Amount.Text, Category, Input_Commentary.Text, income); //если введён ID и нажата кнопка обновить, обновляет данные по ID
                        Log("");
                    }
                }
                else Log("Введите ID для обновления");
            }
        }

        private void Add(object sender, RoutedEventArgs e) //кнопка для начала процесса записи данных в таблицу
        {
            ComboBoxItem selectedItem = (ComboBoxItem) OperationComboBox.SelectedItem;
            if (selectedItem != null)
            {
                String Operation = selectedItem.Content.ToString();
                if (Operation == "Доход") WriteInTable(1);
                if (Operation == "Расход") WriteInTable(0);
            }
            else Log("Выберите операцию");
        }

        private void DeleteData(object sender, RoutedEventArgs e) //кнопка удаляет данные по ID
        {
            if (Delete_ID.Text != "")
            {
                DataAccess.DeleteData(Delete_ID.Text);
                Log("");
            }
            else Log("Введите ID для удаления");
        }

        private void DeleteAllData(object sender, RoutedEventArgs e) //кнопка стирает все данные из таблицы
        {
            DataAccess.DeleteAllData();
            Log("");
        }
        private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e) //кнопка фиксирует, показывать операции или нет
        {
            ToggleSwitch toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {
                if (toggleSwitch.IsOn == true)
                {
                    History = true;
                    Log("");
                }
                else
                {
                    History = false;
                    Log("");
                }
            }
        }

        private void ToggleSwitch_Toggled_Update(object sender, RoutedEventArgs e) //кнопка фиксирует, обновлять данные по ID или нет
        {
            ToggleSwitch toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {
                if (toggleSwitch.IsOn == true)
                { 
                    Update = true;
                }
                else
                {
                    Update = false;
                }
            }
        }

        private void ToggleSwitch_Toggled_Sum(object sender, RoutedEventArgs e) //кнопка фиксирует, показывать сумму или нет
        {
            ToggleSwitch toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {
                if (toggleSwitch.IsOn == true)
                {
                    ShowSum = true;
                    Log("");
                }
                else
                {
                    ShowSum = false;
                    Log("");
                }
            }

        }

        private void UpdateTable(object sender, RoutedEventArgs e) //Обновляет вывод данных
        {
            Log("");
        }

        
    private void OperationComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) //список категорий, согласно выбранной операции
        {
            string Operation;
            ComboBox comboBox = (ComboBox)sender;
            ComboBoxItem selectedItem = (ComboBoxItem)comboBox.SelectedItem;
            Operation = selectedItem.Content.ToString();
            if (Operation == "Доход")
            {
                Input_Category.ItemsSource = categories_income;
            }
            if (Operation == "Расход")
            {
                Input_Category.ItemsSource = categories_outcome;
            }
        }
        
        
        private void Category_TextSubmitted(ComboBox sender, ComboBoxTextSubmittedEventArgs e)
        {

        }

        private void ComboBox_Category_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        
        }

        private void Clear_filter(object sender, RoutedEventArgs e) //кнопка очищяет данные из периодов
        {
            Day_filter_1.Text = "";
            Month_filter_1.Text = "";
            Year_filter_1.Text = "";
            Day_filter_2.Text = "";
            Month_filter_2.Text = "";
            Year_filter_2.Text = "";
        }
    }
}
