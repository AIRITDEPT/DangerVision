using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Devices.Enumeration;
using Windows.Devices.SerialCommunication;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using System.Net;
using System.Net.Sockets;
using System.Data;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.Storage;
using Windows.UI.Xaml.Media;
using Windows.Devices.Input; 
using Windows.UI.Input;
using Windows.UI.Xaml.Input;
using Windows.ApplicationModel.Background;
using Windows.Networking.Sockets;
using System.Text;
using System.Runtime.InteropServices.WindowsRuntime;
using System.IO;
using Windows.System.Threading;
using ThreadPool = System.Threading.ThreadPool;
using Windows.Foundation;
using Windows.Media.Devices;


namespace CCTV3
{

   

    public sealed partial class MainPage : Page
    {



        public static MainPage Current;
                
        DataWriter dataWriterObject; //= null;
        SerialDevice device;
        DataReader dataReaderObject;

        public bool DeviceConnected = false;

        Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
        
        public MainPage()
        {
            InitializeComponent();

            Windows.Globalization.Language.TrySetInputMethodLanguageTag("en-US");

            //this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            Connect();
                   
            PopulateMonitorTitles();
            PopulateCameraTable();

            //ExecuteCommandSync("devcon disable ROOT\rpiwav");
            


        }

        //public void ExecuteCommandSync(object command)
        //{
        //    try
        //    {
        //        // create the ProcessStartInfo using "cmd" as the program to be run,
        //        // and "/c " as the parameters.
        //        // Incidentally, /c tells cmd that we want it to execute the command that follows,
        //        // and then exit.
        //        System.Diagnostics.ProcessStartInfo procStartInfo =
        //            new System.Diagnostics.ProcessStartInfo("PowerShell", "/c " + command);

        //        // The following commands are needed to redirect the standard output.
        //        // This means that it will be redirected to the Process.StandardOutput StreamReader.
        //        procStartInfo.RedirectStandardOutput = true;
        //        procStartInfo.UseShellExecute = false;
        //        // Do not create the black window.
        //        procStartInfo.CreateNoWindow = true;
        //        // Now we create a process, assign its ProcessStartInfo and start it
        //        System.Diagnostics.Process proc = new System.Diagnostics.Process();
        //        proc.StartInfo = procStartInfo;
        //        proc.Start();
        //        // Get the output into a string
        //        string result = proc.StandardOutput.ReadToEnd();
        //        // Display the command output.
        //        Console.WriteLine(result);
        //        MessageBox("Passed");
        //    }
        //    catch (Exception objException)
        //    {
        //        MessageBox("failed");
        //    }
        //}


        /// <summary>
        /// Connects to serial port adapter
        /// </summary>
        public async void Connect()
        {
            if (device != null)
            {
                device.Dispose();
            }
                        

            try
            {
                /// This section gets the device information. Ideally, "GetDeviceSelector()" would contain the actual name of the device, 
                /// but I could never find this. Using the VId/PId also did not work...
                string aqs = SerialDevice.GetDeviceSelector();
                var dis = await DeviceInformation.FindAllAsync(aqs, null);
                device = await SerialDevice.FromIdAsync(dis[0].Id);

                //Configure serial settings
                //The Bosch LTC8200 uses BaudRate of 19200 or 9600. As long as this has been preset, there should no issues.
                device.WriteTimeout = TimeSpan.FromMilliseconds(1000);    //mS before a time-out occurs when a write operation does not finish (default=InfiniteTimeout).
                device.ReadTimeout = TimeSpan.FromMilliseconds(1000);     //mS before a time-out occurs when a read operation does not finish (default=InfiniteTimeout).
                device.BaudRate = 19200;
                device.Parity = SerialParity.None;
                device.StopBits = SerialStopBitCount.One;
                device.DataBits = 8;
                               
                dataReaderObject = new DataReader(device.InputStream);
                dataReaderObject.InputStreamOptions = InputStreamOptions.Partial;

                dataWriterObject = new DataWriter(device.OutputStream);

                if (device != null)
                {
                    DeviceConnected = true;

                    ///UART0 are pins on the Raspberry Pi board. We do not want to use this.
                    if (device.PortName == "UART0")
                    {
                        ConnectionText("Connected to " + device.PortName + ". This is wrong. Please try reconnect or restart device.");                     
                    }
                    ///The Raspberry Pi seemed to give a blank name for the serial port adapater.
                    else if (device.PortName =="")
                    {
                        ConnectionText("Connected to serial converter.");
                    }
                    ///This is added incase it passes through the previous stages. Helpful for troubleshooting.
                    else
                    {
                        ConnectionText("Connected to " + device.PortName);
                    }

                    ///Disables Monitor Overlays
                    ///data1 = monitor #
                    ///data2 = enable Camera/title information
                    ///data3 = enable time and date information
                    Transmit("MON-OVL-ENABLE 1 0 0");
                    Transmit("MON-OVL-ENABLE 2 0 0");
                    Transmit("MON-OVL-ENABLE 3 0 0");
                    Transmit("MON-OVL-ENABLE 4 0 0");
                    Transmit("MON-OVL-ENABLE 5 0 0");

                    ///Sets Time and Date
                    ///MessageBox("!TIME " + DateTime.Now);

                }
                else
                {
                    ConnectionText("Error: No connection to device...");
                    DeviceConnected = false;
                }
            }
            catch
            {
                StatusText("Error: Can not connect to device...");
                DeviceConnected = false;
            }
        }


        /// <summary>
        /// Passing data to this will transmit to the serial port.
        /// </summary>
        /// <param name="TxData"></param>
        public async void Transmit(String TxData)
        {
            ///Ensures the serial port adapter is still connected, otherwise throws exception.
            if (device == null)
            {
                try
                {
                    Connect();
                }
                catch
                {
                    StatusText("Error: Can not connect to serial device.");
                    DeviceConnected = false;
                }
            }
            else
            {
                try
                {
                    ///Clears previous messages.
                    dataWriterObject.DetachStream();
                    dataWriterObject = null;

                    ///Creates new DataWriter object
                    dataWriterObject = new DataWriter(device.OutputStream);

                    //Sends data to UART
                    dataWriterObject.WriteString(TxData);
                    dataWriterObject.WriteDouble(0D);

                    await dataWriterObject.StoreAsync();

                    ///Not the best confirmation. Ideally, it would read the current state of the box. I had not implimented the serial port reader at this point.
                    StatusText("Transmitted " + TxData);
                }
                catch
                {
                    StatusText("Error: Can not transmit to device...");
                }
                finally
                {
                    //dataWriterObject.DetachStream();
                    //dataWriterObject = null;

                    ///Clears data.
                    TxData = null;
                }
            }
        }

        /// <summary>
        /// This section is the code for the camera selection. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///Changed camera on monitor 1 to selection
        private void Mon1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ///Index begins at 0, this corrects number for the LTC Programming Commands
            int sC = Mon1.SelectedIndex + 1;
            string selectedCamera = sC.ToString();

             ///Transmit("LCM " + selectedCamera + " 1");
            Transmit("MON+CAM 1 " + selectedCamera);
        }

        ///Changed camera on monitor 2 to selection
        private void Mon2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ///Index begins at 0, this corrects number for the LTC Programming Commands
            int sC = Mon2.SelectedIndex + 1;
            string selectedCamera = sC.ToString();

            ///Transmit("LCM " + selectedCamera + " 2");
            Transmit("MON+CAM 2 " + selectedCamera);
        }

        ///Changed camera on monitor 3 to selection
        private void Mon3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ///Index begins at 0, this corrects number for the LTC Programming Commands
            int sC = Mon3.SelectedIndex + 1;
            string selectedCamera = sC.ToString();

            ////Transmit("LCM " + selectedCamera + " 3");
            Transmit("MON+CAM 3 " + selectedCamera);
        }

        ///Changed camera on monitor 4 to selection
        private void Mon4_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ///Index begins at 0, this corrects number for the LTC Programming Commands
            int sC = Mon4.SelectedIndex + 1;
            string selectedCamera = sC.ToString();

            ///Transmit("LCM " + selectedCamera + " 4");
            Transmit("MON+CAM 4 " + selectedCamera);
        }

        ///Changed camera on monitor 5 to selection
        private void Mon5_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ///Index begins at 0, this corrects number for the LTC Programming Commands
            int sC = Mon5.SelectedIndex + 1;
            string selectedCamera = sC.ToString();

            ///Transmit("LCM " + selectedCamera + " 5");
            Transmit("MON+CAM 5 " + selectedCamera);
        }
                        
        public async void OpenPopulateTableDialog(object sender, RoutedEventArgs e)
        {
                       
            if (localSettings.Values["CameraNames"] == null)
            {

            }
            else
            {
                CameraNameListBox.Text = localSettings.Values["CameraNames"].ToString();
            }

            await PopulateTableDialog.ShowAsync();                                   
        }

        private void HardwiredCameras_Click(object sender, RoutedEventArgs e)
        {
            if (localSettings.Values["HardwiredNames"] == null)
            {

            }
            else
            {
                CameraNameListBox.Text = localSettings.Values["HardwiredNames"].ToString();
            }
        }

        private void PopulateTableDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {            
            localSettings.Values["CameraNames"] = CameraNameListBox.Text;
            PopulateCameraTable();         
        }

              

        //public async void PopulateCameraTable(string cameraNames, CreationCollisionOption mode)
        public void PopulateCameraTable()        {

            string[] CameraNames = null;
            if (localSettings.Values["CameraNames"] == null)
            {

            }
            else
            {

                char[] separators = { ',' }; 
                CameraNames = localSettings.Values["CameraNames"].ToString().Split(separators);

                Mon1.Items.Clear();
                Mon2.Items.Clear();
                Mon3.Items.Clear();
                Mon4.Items.Clear();
                Mon5.Items.Clear();

                int i = 1;

                foreach (string element in CameraNames)
                {
                    ListViewItem item = new ListViewItem();
                    item.Content = element.Trim();
                    Mon1.Items.Add(item);
                    i = i + 1;


                    //if (i % 2 == 0)
                    //{
                    //    item.Background = new SolidColorBrush(Windows.UI.Colors.Yellow);
                    //    item.Foreground = new SolidColorBrush(Windows.UI.Colors.Black);
                    //}
                    //else
                    //{
                    //    item.Background = new SolidColorBrush(Windows.UI.Colors.Black);
                    //    item.Foreground = new SolidColorBrush(Windows.UI.Colors.White);
                    //}
                }

                i = 1;

                foreach (string element in CameraNames)
                {
                    ListViewItem item = new ListViewItem();
                    item.Content = element.Trim();
                    Mon2.Items.Add(item);
                    i = i + 1;

                    //if (i % 2 == 0)
                    //{
                    //    item.Background = new SolidColorBrush(Windows.UI.Colors.Yellow);
                    //    item.Foreground = new SolidColorBrush(Windows.UI.Colors.Black);
                    //}
                    //else
                    //{
                    //    item.Background = new SolidColorBrush(Windows.UI.Colors.Black);
                    //    item.Foreground = new SolidColorBrush(Windows.UI.Colors.White);
                    //}
                }

                i = 1;

                foreach (string element in CameraNames)
                {
                    ListViewItem item = new ListViewItem();
                    item.Content = element.Trim();
                    Mon3.Items.Add(item);
                    i = i + 1;

                    //if (i % 2 == 0)
                    //{
                    //    item.Background = new SolidColorBrush(Windows.UI.Colors.Yellow);
                    //    item.Foreground = new SolidColorBrush(Windows.UI.Colors.Black);
                    //}
                    //else
                    //{
                    //    item.Background = new SolidColorBrush(Windows.UI.Colors.Black);
                    //    item.Foreground = new SolidColorBrush(Windows.UI.Colors.White);
                    //}
                }

                i = 1;

                foreach (string element in CameraNames)
                {
                    ListViewItem item = new ListViewItem();
                    item.Content = element.Trim();
                    Mon4.Items.Add(item);
                    i = i + 1;

                    //if (i % 2 == 0)
                    //{
                    //    item.Background = new SolidColorBrush(Windows.UI.Colors.Yellow);
                    //    item.Foreground = new SolidColorBrush(Windows.UI.Colors.Black);
                    //}
                    //else
                    //{
                    //    item.Background = new SolidColorBrush(Windows.UI.Colors.Black);
                    //    item.Foreground = new SolidColorBrush(Windows.UI.Colors.White);
                    //}
                }

                i = 1;

                foreach (string element in CameraNames)
                {
                    ListViewItem item = new ListViewItem();
                    item.Content = element.Trim();
                    Mon5.Items.Add(item);
                    i = i + 1;

                    //if (i % 2 == 0)
                    //{
                    //    item.Background = new SolidColorBrush(Windows.UI.Colors.Yellow);
                    //    item.Foreground = new SolidColorBrush(Windows.UI.Colors.Black);
                    //}
                    //else
                    //{
                    //    item.Background = new SolidColorBrush(Windows.UI.Colors.Black);
                    //    item.Foreground = new SolidColorBrush(Windows.UI.Colors.White);
                    //}
                }

            }
            

        }

        public void PopulateMonitorTitles()
        {

            if (localSettings.Values["Mon1 Title"] == null)
            {

            }
            else
            {
                Mon1_Title.Text = localSettings.Values["Mon1 Title"].ToString();
            }

            if (localSettings.Values["Mon2 Title"] == null)
            {

            }
            else
            {
                Mon2_Title.Text = localSettings.Values["Mon2 Title"].ToString();
            }
            if (localSettings.Values["Mon3 Title"] == null)
            {

            }
            else
            { Mon3_Title.Text = localSettings.Values["Mon3 Title"].ToString(); }
            if (localSettings.Values["Mon4 Title"] == null)
            {

            }
            else
            {
                Mon4_Title.Text = localSettings.Values["Mon4 Title"].ToString();
            }
            if (localSettings.Values["Mon5 Title"] == null)
            {

            }
            else
            {
                Mon5_Title.Text = localSettings.Values["Mon5 Title"].ToString();
            }



        }

        private void Mon1_Title_TextChanged(object sender, RoutedEventArgs e)
        {
            localSettings.Values["Mon1 Title"] = Mon1_Title.Text;           
        }

        private void Mon2_Title_TextChanged(object sender, RoutedEventArgs e)
        {
            localSettings.Values["Mon2 Title"] = Mon2_Title.Text;          
        }

        private void Mon3_Title_TextChanged(object sender, RoutedEventArgs e)
        {
            localSettings.Values["Mon3 Title"] = Mon3_Title.Text;            
        }

        private void Mon4_Title_TextChanged(object sender, RoutedEventArgs e)
        {
            localSettings.Values["Mon4 Title"] = Mon4_Title.Text;            
        }

        private void Mon5_Title_TextChanged(object sender, RoutedEventArgs e)
        {
            localSettings.Values["Mon5 Title"] = Mon5_Title.Text;            
        }

        public void Connect_Click(object sender, RoutedEventArgs e)
        {
            Connect();
        }

        public async void OpenSettingsDialog(object sender, RoutedEventArgs e)
        {
            if (localSettings.Values["HardwiredNames"] == null)
            {

            }
            else
            {
                HardwiredNameListBox.Text = localSettings.Values["HardwiredNames"].ToString();
            }

            await SettingsDialog.ShowAsync();
                        
        }

        private void SettingsDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            localSettings.Values["HardwiredNames"] = HardwiredNameListBox.Text;           
        }
             
        public void DetectCameras_Click(object sender, RoutedEventArgs e)
        {
            Transmit("WORKING-CAMS");
            StatusText("Displaying Working Cameras");
        }

        public void ResetUnit_Click(object sender, RoutedEventArgs e)
        {
            Transmit("RESET-SYSTEM");
            StatusText("RESETTING SYSTEM");
        }
            
        
        /// <summary>
        /// Shows status in bottom right of screen
        /// </summary>
        /// <param name="data"></param>
        public void StatusText(String data)
        {
            StatusBox.Text = data;
        }

        /// <summary>
        /// Shows connection status on bottom right of screen. Ideally, this would auto-refresh.
        /// </summary>
        /// <param name="data"></param>
        private void ConnectionText(String data)
        {
            ConnectionStatus.Text = data;
        }

        /// <summary>
        /// Restarts application. Duh.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void Restart_Click(object sender, RoutedEventArgs e)
        {

            var result = await CoreApplication.RequestRestartAsync("Application Restart Programmatically ");

            if (result == AppRestartFailureReason.NotInForeground ||
                result == AppRestartFailureReason.RestartPending ||
                result == AppRestartFailureReason.Other)
            {
                var msgBox = new MessageDialog("Restart Failed", result.ToString());
                await msgBox.ShowAsync();
            }

        }

        /// <summary>
        /// Yep.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CloseApp_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }

        /// <summary>
        /// Made this to help with troubleshooting. Left it in as it was quite handy.
        /// </summary>
        /// <param name="text"></param>
        public async void MessageBox(String text)
        {
            var dialog = new MessageDialog(text);
            await dialog.ShowAsync();
        }



private void AudioModuleManager_ModuleNotificationReceived(AudioDeviceModulesManager sender, AudioDeviceModuleNotificationEventArgs args)
        {
            
            MessageBox("Audio notification received.");            
        }
    }

}

