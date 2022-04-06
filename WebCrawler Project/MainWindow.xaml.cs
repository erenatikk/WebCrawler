using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;
using static WebCrawler_Project.csGetSource; //2021112204

namespace WebCrawler_Project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        public MainWindow()
        {
            InitializeComponent();


        }
        private static List<Task> lstTaskPr = new List<Task>(); //2021112250 ,2021112209
        public void btnStart_Click(object sender, RoutedEventArgs e)
        {
            var vrTask = Task.Factory.StartNew(() =>
                {
                    csDatabase database = new csDatabase(); //2021112202
                    database.CreateDatabase();

                    foreach (var item in lstDomain.Items)
                    {
                        Task.Factory.StartNew(() =>
                        {
                            var temp = Convert.ToString(item).Split(' ').Last();
                            Dispatcher.BeginInvoke((Action)(() =>
                            {
                                switch (cmbScan.SelectedIndex)
                                {
                                    case 0:
                                        csInternalScan internalscan = new csInternalScan(temp);
                                        internalscan.Scanner();
                                        break;
                                    case 1:
                                        csExternalScan externalscan = new csExternalScan(temp);
                                        externalscan.Scanner();
                                        break;
                                }
                            }));
                        });
                    }
                }
            );
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            string srcurl = txtSearchUrl.Text;
            lstDomain.Items.Add(srcurl);
            txtSearchUrl.Clear();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)//2021112241
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {

            Dispatcher.BeginInvoke((Action)(() =>
            {
                csDatabase database = new csDatabase();
                dgShowDB.ItemsSource = database.ReadData();
            }));
        }
    }
}
