using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Hardcodet.Wpf.Samples.Pages
{
    /// <summary>
    /// Interaction logic for Stopwatch.xaml
    /// </summary>
    public partial class StopwatchPage 
    {
        DispatcherTimer dt = new DispatcherTimer();
        Stopwatch sw = new Stopwatch();
        string currentTime = string.Empty;
        public StopwatchPage()
        {
            InitializeComponent();
            dt.Tick += new EventHandler(dt_Tick);
            dt.Interval = new TimeSpan(0, 0, 0, 0, 1);
            timelbl.Content = DateTime.Now.ToLongTimeString();
            datelbl.Content = DateTime.Now.ToLongDateString();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
           // timer.Tick += Timer_Tick;
            timer.Start();
        }

        //public void Timer_Tick(object sender, EventArgs e)
        //{
        //    timelbl.Content = DateTime.Now.ToLongTimeString();
        //    datelbl.Content = DateTime.Now.ToLongDateString();
        //}

        void dt_Tick(object sender, EventArgs e)
        {
            if (sw.IsRunning)
            {
                TimeSpan ts = sw.Elapsed;
                currentTime = String.Format("{0:00}:{1:00}:{2:00}",
                ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                timelbl.Content = currentTime;
                datelbl.Content = DateTime.Now.ToLongDateString();
            }
        }

        private void startbtn_Click(object sender, RoutedEventArgs e)
        {
            sw.Start();
            dt.Start();
        }

        private void stopbtn_Click(object sender, RoutedEventArgs e)
        {
            if (sw.IsRunning)
            {
                sw.Stop();
            }
            //elapsedtimeitem.Items.Add(currentTime);
        }

        private void resetbtn_Click(object sender, RoutedEventArgs e)
        {
            sw.Reset();
            timelbl.Content = DateTime.Now.ToLongTimeString();
        }

        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void quitbtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
