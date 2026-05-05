using System;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SabrChecker
{
    public partial class MainWindow : Window
    {
        private string appPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App");

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadAsync();
        }

        private async System.Threading.Tasks.Task LoadAsync()
        {
            try
            {
                this.Dispatcher.Invoke(() => { LoadingStatus.Text = "Загрузка данных..."; });
                using (WebClient wc = new WebClient())
                {
                    BoxM1.Text = await wc.DownloadStringTaskAsync(new Uri("https://raw.githubusercontent.com/"));
                }
            }
            catch { BoxM1.Text = "Failed to connect to server."; }

            this.Dispatcher.Invoke(() => { LoadingStatus.Text = "Подготовка..."; });
            await System.Threading.Tasks.Task.Delay(3000);

            this.Dispatcher.Invoke(() => { LoadingOverlay.Visibility = Visibility.Collapsed; });
            RefreshSysInfo(null, null);
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void SwitchTab(object sender, RoutedEventArgs e)
        {
            string tag = (sender as Button).Tag.ToString();
            TabInfo.Visibility = Visibility.Collapsed;
            TabApps.Visibility = Visibility.Collapsed;
            TabSteam.Visibility = Visibility.Collapsed;
            TabSys.Visibility = Visibility.Collapsed;
            TabSettings.Visibility = Visibility.Collapsed;

            if (this.FindName(tag) is FrameworkElement tab) tab.Visibility = Visibility.Visible;
        }

        private void RefreshSysInfo(object sender, RoutedEventArgs e)
        {
            try
            {
                ManagementObjectSearcher mos = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor");
                foreach (ManagementObject mj in mos.Get()) TxtCPU.Text = "CPU: " + mj["Name"];

                mos = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_VideoController");
                foreach (ManagementObject mj in mos.Get()) TxtGPU.Text = "GPU: " + mj["Name"];

                mos = new ManagementObjectSearcher("root\\CIMV2", "SELECT TotalPhysicalMemory FROM Win32_ComputerSystem");
                foreach (ManagementObject mj in mos.Get())
                {
                    long ram = Convert.ToInt64(mj["TotalPhysicalMemory"]) / 1024 / 1024 / 1024;
                    TxtRAM.Text = "RAM: " + ram + " GB";
                }

                bool obs64 = Process.GetProcessesByName("obs64").Length > 0;
                bool obs32 = Process.GetProcessesByName("obs32").Length > 0;

                if (obs64 || obs32)
                {
                    TxtOBS.Text = "OBS STATUS: DETECTED";
                    TxtOBS.Foreground = System.Windows.Media.Brushes.Red;
                }
                else
                {
                    TxtOBS.Text = "OBS STATUS: NOT FOUND";
                    TxtOBS.Foreground = System.Windows.Media.Brushes.LightGreen;
                }
            }
            catch { TxtCPU.Text = "Error reading system info."; }
        }

        private void LaunchApp(object sender, RoutedEventArgs e)
        {
            string exe = (sender as Button).Tag.ToString();
            string fullPath = Path.Combine(appPath, exe);
            if (File.Exists(fullPath))
                Process.Start(fullPath);
            else
                MessageBox.Show("Файл не найден в папке App: " + exe, "Ошибка");
        }

        private void OpenLogsFolder(object sender, RoutedEventArgs e)
        {
            if (!Directory.Exists(appPath)) Directory.CreateDirectory(appPath);
            Process.Start("explorer.exe", appPath);
        }

        private void OpenGitHub(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/");
        }

        private void OpenTelegram(object sender, RoutedEventArgs e)
        {
            Process.Start("https://t.me/");
        }

        private void ExitApp(object sender, RoutedEventArgs e) => Application.Current.Shutdown();
    }
}