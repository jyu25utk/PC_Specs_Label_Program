using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SharpDX.DirectInput;
using System.Threading;
using System.IO.Ports;
using System.Windows.Threading;
using DymoSDK;
using DymoSDK.Implementations;

namespace PC_Specs_Label_Program
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    /// 
    
    public partial class MainWindow : Window
    {
        SerialPort sp = new SerialPort();
        int count = 0;
        string recieved_data;
        string mem_str = "N/A";
        string hdd_str = "N/A";
        string ssd_str = "N/A";
        string special_str = "N/A";

        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void PrintLabel(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Hello!");
            
        }
        
        private void PrintHotKey(object sender, KeyEventArgs e)
        {
            
            if (e.Key == System.Windows.Input.Key.P)
            {
                count = 0;
                if (mem_str != "N/A") count++;
                if (hdd_str != "N/A") count++;
                if (ssd_str != "N/A") count++;
                if (special_str != "N/A") count++;
            }
        }

        private void SerialConnect_Click()
        {

        }

        private void sp_list_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectdcomboitem = sender as ComboBox;
            string name = selectdcomboitem.SelectedItem as string;
        }

        private void sp_connect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string portName = sp_list.SelectedItem as string;
                sp.PortName = portName;
                sp.BaudRate = 9600;
                sp.Open();
                sp_status.Text = "Connected!";
                sp.DataReceived += new SerialDataReceivedEventHandler(Recieve);
            }
            catch(Exception)
            {
                MessageBox.Show("Please connect your device or check the device connection");
            }
        }

        private void sp_disconnect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                sp.Close();
                sp_status.Text = "Disconnect!";
            }
            catch (Exception)
            {
                MessageBox.Show("Please connect the serial first!");
            }
        }

        

        private delegate void UpdateUiTextDelegate(string text);
        private void Recieve(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            recieved_data = sp.ReadExisting();
            Dispatcher.Invoke(DispatcherPriority.Send, new UpdateUiTextDelegate(WriteData), recieved_data);
        }
        private void WriteData(string text)
        {
            controller_block.Text = text;
            split_sp(text);
        }
        private void split_sp(string in_str)
        {
            string[] words = in_str.Split(' ');

            if (words[0] == "m")
            {
                mem_str = words[1];
                mem.Text = words[1];
            }
            if (words[0] == "h")
            {
                hdd_str = words[1];
                hdd.Text = words[1];
            }
            if (words[0] == "s")
            {
                ssd_str = words[1];
                ssd.Text = words[1];
            }
            if (words[0] == "p")
            {
                special_str = words[1];
                special.Text = words[1];
            }
        }
    }
}
