#pragma checksum "C:\Users\Lagrange\Desktop\MyApp\MyApp\MyApp\MainPage.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "3A3BE5A94466F1D971CA2C8748AA35DEF0A2A204B248A26086E41CD68287629D"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MyApp
{
    partial class MainPage : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                {
                    this.Delete_ID = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 2:
                {
                    this.ToggleSwitchUpdate = (global::Windows.UI.Xaml.Controls.ToggleSwitch)(target);
                    #line 107 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.ToggleSwitch)this.ToggleSwitchUpdate).Toggled += this.ToggleSwitch_Toggled_Update;
                    #line default
                }
                break;
            case 3:
                {
                    global::Windows.UI.Xaml.Controls.Button element3 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 108 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)element3).Click += this.DeleteData;
                    #line default
                }
                break;
            case 4:
                {
                    global::Windows.UI.Xaml.Controls.Button element4 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 109 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)element4).Click += this.DeleteAllData;
                    #line default
                }
                break;
            case 5:
                {
                    this.ToggleSwitch = (global::Windows.UI.Xaml.Controls.ToggleSwitch)(target);
                    #line 110 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.ToggleSwitch)this.ToggleSwitch).Toggled += this.ToggleSwitch_Toggled;
                    #line default
                }
                break;
            case 6:
                {
                    this.ToggleSwitchSum = (global::Windows.UI.Xaml.Controls.ToggleSwitch)(target);
                    #line 111 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.ToggleSwitch)this.ToggleSwitchSum).Toggled += this.ToggleSwitch_Toggled_Sum;
                    #line default
                }
                break;
            case 7:
                {
                    global::Windows.UI.Xaml.Controls.Button element7 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 112 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)element7).Click += this.UpdateSum;
                    #line default
                }
                break;
            case 8:
                {
                    this.Output = (global::Windows.UI.Xaml.Controls.ListView)(target);
                }
                break;
            case 9:
                {
                    this.Input_Commentary = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 10:
                {
                    this.Input_Category = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 11:
                {
                    this.ComboBox_Category = (global::Windows.UI.Xaml.Controls.ComboBox)(target);
                    #line 75 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.ComboBox)this.ComboBox_Category).SelectionChanged += this.ComboBox_Category_SelectionChanged;
                    #line default
                }
                break;
            case 12:
                {
                    this.Input_Amount = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 13:
                {
                    this.Input_Year = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 14:
                {
                    this.Input_Month = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 15:
                {
                    this.Input_Day = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 16:
                {
                    this.OperationComboBox = (global::Windows.UI.Xaml.Controls.ComboBox)(target);
                    #line 24 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.ComboBox)this.OperationComboBox).SelectionChanged += this.OperationComboBox_SelectionChanged;
                    #line default
                }
                break;
            case 17:
                {
                    global::Windows.UI.Xaml.Controls.Button element17 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 29 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)element17).Click += this.IncomeData;
                    #line default
                }
                break;
            case 18:
                {
                    global::Windows.UI.Xaml.Controls.Button element18 = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 30 "..\..\..\MainPage.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)element18).Click += this.OutcomeData;
                    #line default
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}

