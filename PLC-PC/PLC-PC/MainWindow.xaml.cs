using S7.Net;
using System;

using System.Windows;

using System.Windows.Threading;

namespace PLC_PC
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
      
       
        public MainWindow()
        {
            InitializeComponent();
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();

        }
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (plc == null || plc.IsConnected == false)
            {
                return;
            }

            try
            {
                RefreshTags();
            }
            finally
            {
                if (WB_Fire == true)
                {
                    wb_fire.Fill = System.Windows.Media.Brushes.Red;
                    //Chay huong west bount
                }
                else
                {
                    wb_fire.Fill = System.Windows.Media.Brushes.White;

                }
                if (EB_Fire == true)
                {
                    eb_fire.Fill = System.Windows.Media.Brushes.Red;

                    //Chay huong east bount
                }
                else
                {
                    eb_fire.Fill = System.Windows.Media.Brushes.White;

                }
            }
        }
        Plc plc;
        bool WB_Fire, EB_Fire;



        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {

            plc = new Plc(CpuType.S7300, "192.168.0.106", 0, 2);
            try
            {
                plc.Open();
                btnConnect.IsEnabled = false;
                btnDisconnect.IsEnabled = true;
            }
            catch (PlcException ex)
            {

                MessageBox.Show(this, ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }
        private void btnDisconnect_Click(object sender, RoutedEventArgs e)
        {
 
            try
            {
                if (plc != null)
                {
                    plc.Close();
                    btnConnect.IsEnabled = true;
                    btnDisconnect.IsEnabled = false;
                }
            }
            catch (PlcException ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
         
        }

        private void RefreshTags()
        {

            var u = (byte[])plc.ReadBytes(DataType.DataBlock,2,18,1);  
            var WB_bitnumber = 2;
            var EB_bitnumber = 3;
            WB_Fire = (u[0] & (1 << WB_bitnumber - 1)) != 0;  
            EB_Fire = (u[0] & (1 << EB_bitnumber - 1)) != 0;            
        }
    }
}
