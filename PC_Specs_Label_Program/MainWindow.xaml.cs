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

namespace PC_Specs_Label_Program
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        SerialPort sp = new SerialPort();

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
                MessageBox.Show("Hello2!");
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
            }
            catch(Exception)
            {
                MessageBox.Show("Please connect your device or check the device connection");
            }
        }
    }
}
