﻿using System;
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
using Microsoft.Win32;
using DymoSDK.Implementations;
using System.Runtime.CompilerServices;
using System.ComponentModel;

namespace PC_Specs_Label_Program
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    /// 
    
    public partial class MainWindow : Window
    {
        #region Props
        IEnumerable<DymoSDK.Interfaces.IPrinter> _printers;

        public IEnumerable<DymoSDK.Interfaces.IPrinter> Printers
        {
            get
            {
                if (_printers == null)
                    _printers = new List<DymoSDK.Interfaces.IPrinter>();
                return _printers;
            }
            set
            {
                _printers = value;
                NotifyPropertyChanged("Printers");
            }
        }

        public int PrintersFound
        {
            get { return Printers.Count(); }
        }

        string _fileName;
        public string FileName
        {
            get
            {
                if (string.IsNullOrEmpty(_fileName))
                    return "No file selected";

                return _fileName;
            }
            set
            {
                _fileName = value;
                NotifyPropertyChanged("FileName");
            }
        }

        private BitmapImage _imageSourcePreview;

        public BitmapImage ImageSourcePreview
        {
            get { return _imageSourcePreview; }
            set
            {
                _imageSourcePreview = value;
                NotifyPropertyChanged("ImageSourcePreview");
            }
        }


        List<DymoSDK.Interfaces.ILabelObject> _labelObjects;
        public List<DymoSDK.Interfaces.ILabelObject> LabelObjects
        {
            get
            {
                if (_labelObjects == null)
                    _labelObjects = new List<DymoSDK.Interfaces.ILabelObject>();
                return _labelObjects;
            }
            set
            {
                _labelObjects = value;
                NotifyPropertyChanged("LabelObjects");
            }
        }

        private DymoSDK.Interfaces.ILabelObject _selectedLabelObject;
        public DymoSDK.Interfaces.ILabelObject SelectedLabelObject
        {
            get { return _selectedLabelObject; }
            set
            {
                _selectedLabelObject = value;
                NotifyPropertyChanged("SelectedLabelObject");
            }
        }

        private string _objectValue;
        public string ObjectValue
        {
            get { return _objectValue; }
            set
            {
                _objectValue = value;
                NotifyPropertyChanged("ObjectValue");
            }
        }


        DymoSDK.Interfaces.IPrinter _selectedPrinter;
        public DymoSDK.Interfaces.IPrinter SelectedPrinter
        {
            get { return _selectedPrinter; }
            set
            {
                _selectedPrinter = value;
                NotifyPropertyChanged("FileName");
            }
        }

        List<string> _twinTurboRolls;
        public List<string> TwinTurboRolls
        {
            get
            {
                if (_twinTurboRolls == null)
                    _twinTurboRolls = new List<string>();
                return _twinTurboRolls;
            }
            set
            {
                _twinTurboRolls = value;
                NotifyPropertyChanged("TwinTurboRolls");
            }
        }

        private string _selectedRoll;
        public string SelectedRoll
        {
            get { return _selectedRoll; }
            set
            {
                _selectedRoll = value;
                NotifyPropertyChanged("SelectedRoll");
            }
        }
        #endregion
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        SerialPort sp = new SerialPort();
        DymoLabel dymoSDKLabel;
        
        int count = 0;
        string recieved_data;
        string mem_str = "N/A";
        string hdd_str = "N/A";
        string ssd_str = "N/A";
        string special_str = "N/A";

        public MainWindow()
        {
            InitializeComponent();
            SelectedPrinter = DymoPrinter.Instance.GetPrinters().ToList()[0];
            Console.WriteLine(SelectedPrinter.Name);
        }

        private void PrintLabel(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Hello!");
            
        }
        
        private void PrintHotKey(object sender, KeyEventArgs e)
        {
            int copies = 1;
            bool barcodeOrGraphsquality = false;
            if (e.Key == System.Windows.Input.Key.P)
            {
                count = 0;
                if (mem_str != "N/A") count++;
                if (hdd_str != "N/A") count++;
                if (ssd_str != "N/A") count++;
                if (special_str != "N/A") count++;
            }

            dymoSDKLabel = new DymoLabel();
            
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "DYMO files |*.label;*.dymo|All files|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                FileName = openFileDialog.FileName;
                Console.WriteLine(FileName);
                //C:\Users\Jinxiao Yu\Desktop\Label\3 lines.dymo
                //Load label from file path
                dymoSDKLabel.LoadLabelFromFilePath(FileName);
                //Get object names list
                LabelObjects = dymoSDKLabel.GetLabelObjects().ToList();
                SelectedLabelObject = LabelObjects[0];
                ObjectValue = "Test";
                dymoSDKLabel.UpdateLabelObject(SelectedLabelObject, ObjectValue);
                Console.WriteLine("Update Finish!");
                DymoPrinter.Instance.PrintLabel(dymoSDKLabel, SelectedPrinter.Name, copies, barcodeGraphsQuality: barcodeOrGraphsquality);
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
