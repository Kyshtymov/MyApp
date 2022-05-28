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
    public sealed partial class MainPage : Page
    {

        bool History = false;
        bool Update = false;
        bool ShowSum = false;
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void Log(string LogText)
        {
            int pos = 0;
            List<String> entries = new List<string>();
            if (History) entries = DataAccess.GetData();
         
            if (ShowSum)
            {
                int Operations, OperationsTotal;
                string sum1 = DataAccess.Sum(Delete_ID.Text, Input_Day.Text, Input_Month.Text, Input_Year.Text, Input_Amount.Text, Input_Category.Text, Input_Commentary.Text, false, out Operations);
                string sum2 = DataAccess.Sum(Delete_ID.Text, Input_Day.Text, Input_Month.Text, Input_Year.Text, Input_Amount.Text, Input_Category.Text, Input_Commentary.Text, true, out OperationsTotal);
                string CommandSum = "Прибыль по критериям = " + sum1 + "  Количество операций по критериям = " + Operations;
                string CommandSum2 = "Полная прибыль = " + sum2 + "  Полное количество операций = " + OperationsTotal;

                entries.Insert(0, CommandSum);
                entries.Insert(1, CommandSum2);
                pos += 2;
            }
            if (LogText != "")
            {
                entries.Insert(pos, LogText);
                pos++;
            }
            if (History) entries.Insert(pos, "История операций:");
            Output.ItemsSource = entries;
        }


        private void WriteInTable(int income)
        {
            string Category = Input_Category.Text;
            if (Input_Category.Text == "")  Category = "Прочее";

            int Day = 0;
            bool CheckDay = int.TryParse(Input_Day.Text, out Day);
            if ((CheckDay) && (Day > 0) && (Day < 32)) CheckDay = true;
            else
            {
                CheckDay = false;
                Log("Введите день от 1 до 31 числа");
            }


            int Month = 0;
            bool CheckMonth = int.TryParse(Input_Month.Text, out Month);
            if ((CheckMonth) && (Month > 0) && (Month < 13)) CheckMonth = true;
            else
            {
                CheckMonth = false;
                Log("Введите месяц от 1 до 12");

            }

            int Year = 0;
            bool CheckYear = int.TryParse(Input_Year.Text, out Year);
            if ((CheckYear) && (Year > 0) && (Year < 32)) CheckYear = true;
            else
            {
                CheckYear = false;
                Log("Введите год от 0");
            }


            float Amount = 0;
            bool CheckAmount = float.TryParse(Input_Amount.Text, out Amount);
            if ((CheckAmount) && (Amount > 0) && (Year < 32)) CheckAmount = true;
            else
            {
                CheckYear = false;
                Log("Введите сумму от 0");
            }

            bool Check = CheckDay && CheckMonth && CheckYear && CheckAmount;
            if ((Check) && (!Update))
            {
                DataAccess.AddData(Input_Day.Text, Input_Month.Text, Input_Year.Text, Input_Amount.Text, Category, Input_Commentary.Text, income);
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
                        DataAccess.UpdateData(Delete_ID.Text, Input_Day.Text, Input_Month.Text, Input_Year.Text, Input_Amount.Text, Category, Input_Commentary.Text, income);
                        Log("");
                    }
                }
                else Log("Введите ID для обновления");
            }
        }

        private void IncomeData(object sender, RoutedEventArgs e)
        {
            WriteInTable(1);
        }


        private void OutcomeData(object sender, RoutedEventArgs e)
        {

            WriteInTable(0);
        }

        private void DeleteData(object sender, RoutedEventArgs e)
        {
            if (Delete_ID.Text != "")
            {
                DataAccess.DeleteData(Delete_ID.Text);
                Log("");
           }
        }

        private void DeleteAllData(object sender, RoutedEventArgs e)
        {
            DataAccess.DeleteAllData();
            Log("");
        }
        private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
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

        private void ToggleSwitch_Toggled_Update(object sender, RoutedEventArgs e)
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

        private void ToggleSwitch_Toggled_Sum(object sender, RoutedEventArgs e)
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
                }
            }

        }

        private void UpdateSum(object sender, RoutedEventArgs e)
        {
            Log("");
        }
    }
}
