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

namespace PC_Specs_Label_Program
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
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
    }
}
