﻿#pragma checksum "C:\Visual Studio Projects\CCTV\CCTV3\CCTV3\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "E85494D5245790976E62B57BFB5B511E"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CCTV3
{
    partial class MainPage : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.17.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 2: // MainPage.xaml line 13
                {
                    this.Selection = (global::Windows.UI.Xaml.Controls.Grid)(target);
                }
                break;
            case 3: // MainPage.xaml line 48
                {
                    this.Title = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 4: // MainPage.xaml line 54
                {
                    this.ConnectionStatus = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 5: // MainPage.xaml line 55
                {
                    this.StatusBox = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 6: // MainPage.xaml line 58
                {
                    this.Mon1_Title = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                    ((global::Windows.UI.Xaml.Controls.TextBox)this.Mon1_Title).TextChanged += this.Mon1_Title_TextChanged;
                }
                break;
            case 7: // MainPage.xaml line 59
                {
                    this.Mon2_Title = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                    ((global::Windows.UI.Xaml.Controls.TextBox)this.Mon2_Title).TextChanged += this.Mon2_Title_TextChanged;
                }
                break;
            case 8: // MainPage.xaml line 60
                {
                    this.Mon3_Title = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                    ((global::Windows.UI.Xaml.Controls.TextBox)this.Mon3_Title).TextChanged += this.Mon3_Title_TextChanged;
                }
                break;
            case 9: // MainPage.xaml line 61
                {
                    this.Mon4_Title = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                    ((global::Windows.UI.Xaml.Controls.TextBox)this.Mon4_Title).TextChanged += this.Mon4_Title_TextChanged;
                }
                break;
            case 10: // MainPage.xaml line 62
                {
                    this.Mon5_Title = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                    ((global::Windows.UI.Xaml.Controls.TextBox)this.Mon5_Title).TextChanged += this.Mon5_Title_TextChanged;
                }
                break;
            case 11: // MainPage.xaml line 115
                {
                    this.PopulateTableDialog = (global::Windows.UI.Xaml.Controls.ContentDialog)(target);
                    ((global::Windows.UI.Xaml.Controls.ContentDialog)this.PopulateTableDialog).PrimaryButtonClick += this.PopulateTableDialog_PrimaryButtonClick;
                    ((global::Windows.UI.Xaml.Controls.ContentDialog)this.PopulateTableDialog).SecondaryButtonClick += this.PopulateTableDialog_SecondaryButtonClick;
                }
                break;
            case 12: // MainPage.xaml line 133
                {
                    this.SettingsDialog = (global::Windows.UI.Xaml.Controls.ContentDialog)(target);
                    ((global::Windows.UI.Xaml.Controls.ContentDialog)this.SettingsDialog).PrimaryButtonClick += this.SettingsDialog_PrimaryButtonClick;
                }
                break;
            case 14: // MainPage.xaml line 162
                {
                    this.HardwiredNameListBox = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 15: // MainPage.xaml line 163
                {
                    this.DetectCameraButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.DetectCameraButton).Click += this.DetectCameras_Click;
                }
                break;
            case 16: // MainPage.xaml line 164
                {
                    this.ResetUnitButton = (global::Windows.UI.Xaml.Controls.Button)(target);
                    ((global::Windows.UI.Xaml.Controls.Button)this.ResetUnitButton).Click += this.ResetUnit_Click;
                }
                break;
            case 18: // MainPage.xaml line 129
                {
                    this.CameraNameListBox = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 19: // MainPage.xaml line 106
                {
                    this.Mon5 = (global::Windows.UI.Xaml.Controls.ListView)(target);
                    ((global::Windows.UI.Xaml.Controls.ListView)this.Mon5).SelectionChanged += this.Mon5_SelectionChanged;
                }
                break;
            case 21: // MainPage.xaml line 96
                {
                    this.Mon4 = (global::Windows.UI.Xaml.Controls.ListView)(target);
                    ((global::Windows.UI.Xaml.Controls.ListView)this.Mon4).SelectionChanged += this.Mon4_SelectionChanged;
                }
                break;
            case 23: // MainPage.xaml line 86
                {
                    this.Mon3 = (global::Windows.UI.Xaml.Controls.ListView)(target);
                    ((global::Windows.UI.Xaml.Controls.ListView)this.Mon3).SelectionChanged += this.Mon3_SelectionChanged;
                }
                break;
            case 25: // MainPage.xaml line 76
                {
                    this.Mon2 = (global::Windows.UI.Xaml.Controls.ListView)(target);
                    ((global::Windows.UI.Xaml.Controls.ListView)this.Mon2).SelectionChanged += this.Mon2_SelectionChanged;
                }
                break;
            case 27: // MainPage.xaml line 66
                {
                    this.Mon1 = (global::Windows.UI.Xaml.Controls.ListView)(target);
                    ((global::Windows.UI.Xaml.Controls.ListView)this.Mon1).SelectionChanged += this.Mon1_SelectionChanged;
                }
                break;
            case 29: // MainPage.xaml line 40
                {
                    global::Windows.UI.Xaml.Controls.AppBarButton element29 = (global::Windows.UI.Xaml.Controls.AppBarButton)(target);
                    ((global::Windows.UI.Xaml.Controls.AppBarButton)element29).Click += this.Connect_Click;
                }
                break;
            case 30: // MainPage.xaml line 41
                {
                    global::Windows.UI.Xaml.Controls.AppBarButton element30 = (global::Windows.UI.Xaml.Controls.AppBarButton)(target);
                    ((global::Windows.UI.Xaml.Controls.AppBarButton)element30).Click += this.OpenPopulateTableDialog;
                }
                break;
            case 31: // MainPage.xaml line 42
                {
                    global::Windows.UI.Xaml.Controls.AppBarButton element31 = (global::Windows.UI.Xaml.Controls.AppBarButton)(target);
                    ((global::Windows.UI.Xaml.Controls.AppBarButton)element31).Click += this.OpenSettingsDialog;
                }
                break;
            case 32: // MainPage.xaml line 43
                {
                    global::Windows.UI.Xaml.Controls.AppBarButton element32 = (global::Windows.UI.Xaml.Controls.AppBarButton)(target);
                    ((global::Windows.UI.Xaml.Controls.AppBarButton)element32).Click += this.CloseApp_Click;
                }
                break;
            case 33: // MainPage.xaml line 44
                {
                    global::Windows.UI.Xaml.Controls.AppBarButton element33 = (global::Windows.UI.Xaml.Controls.AppBarButton)(target);
                    ((global::Windows.UI.Xaml.Controls.AppBarButton)element33).Click += this.Restart_Click;
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        /// <summary>
        /// GetBindingConnector(int connectionId, object target)
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 10.0.17.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}

