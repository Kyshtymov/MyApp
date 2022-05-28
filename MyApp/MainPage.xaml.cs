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
        public MainPage()
        {
            this.InitializeComponent();
            Sum_Box.Text = "Total = 0" + DataAccess.Sum();
        }

        private async void Income_Click(object sender, RoutedEventArgs e)
        {
            MediaElement mediaElement = new MediaElement();
            var synth = new Windows.Media.SpeechSynthesis.SpeechSynthesizer();
            Windows.Media.SpeechSynthesis.SpeechSynthesisStream stream = await synth.SynthesizeTextToStreamAsync("Hello, World!");
            mediaElement.SetSource(stream, stream.ContentType);
            mediaElement.Play();
        }
        private void IncomeData(object sender, RoutedEventArgs e)
        {
            DataAccess.AddData(Input_Amount.Text, Input_Category.Text, 1);

            if (Output.Visibility == Visibility.Visible) Output.ItemsSource = DataAccess.GetData();
            Sum_Box.Text = "Total = " + DataAccess.Sum();
        }


        private void OutcomeData(object sender, RoutedEventArgs e)
        {
            DataAccess.AddData(Input_Amount.Text, Input_Category.Text, 0);

            List<String> entries = new List<string>();
            if (Output.Visibility == Visibility.Visible)
            {
                entries = DataAccess.GetData();

                entries.Insert(0, "Total = " + DataAccess.Sum());
                Output.ItemsSource = entries;
            }
            Sum_Box.Text = "Total = " + DataAccess.Sum();
        }

        private void DeleteData(object sender, RoutedEventArgs e)
        {
            if (Delete_ID.Text != "")
            {
                DataAccess.DeleteData(Delete_ID.Text);

                if (Output.Visibility == Visibility.Visible) Output.ItemsSource = DataAccess.GetData();
                Sum_Box.Text = DataAccess.Sum();
            }
        }

        private void DeleteAllData(object sender, RoutedEventArgs e)
        {
            DataAccess.DeleteAllData();

            if (Output.Visibility == Visibility.Visible) Output.ItemsSource = DataAccess.GetData();
            Sum_Box.Text = "Total = 0";
        }
        private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch toggleSwitch = sender as ToggleSwitch;
            if (toggleSwitch != null)
            {
                if (toggleSwitch.IsOn == true)
                {
                    Output.Visibility = Visibility.Visible; 
                    Output.ItemsSource = DataAccess.GetData();
                }
                else
                {
                    Output.Visibility = Visibility.Collapsed;
                }
            }
        }
    }
}
