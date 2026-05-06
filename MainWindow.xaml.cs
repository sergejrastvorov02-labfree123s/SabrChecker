using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace SabrChecker
{
    public partial class MainWindow : Window
    {
        private string method1Text = "vilonity|avalon|kitekat|paste.cc|skyline.fix|masonlite|amphetamine|simplicity|apfetamine|trinity|clarity|NovazBesting|vlone|invis.hack|AMTH|skidware|infinity|AdolfRust|ComputeStringHash|dummy_ptr|facepunch.graphics|norecoil|ExternalCheat_NoRecoil|GxOne|RustExploit_Injector|KaboomCheat|UnderHack|Facepunch.Sharp|BasicLand|GOPOTA|invis|money_rain|superiority|infinity.|astrahookie|geroin|dolbaebfree|novazbesting|CatChair|0xcheat|Dootpeaker.space|skyline.one|lghub|brend|extreme|UnityCrashHandler64|imgui|halal.exe|reg.exe|ak47|berda|Deluxe|Nova|keyran|com.swiftsoft|ANW|UG.dll|cartine.html|plague.dll|plaguecrack.dll|plaguepast.dll|suckmaster|spermaHookie|winhttp.dll|skidware.cc|laze.dll|mortemsuck|AnywareFree|MyCheat.dll|Dast";

        private Dictionary<string, string> tools = new Dictionary<string, string>
        {
            { "Everything", "Everything.exe" },
            { "ShellBagsView", "ShellBags.exe" },
            { "USBDeview", "USBDeview.exe" },
            { "LastActivityView", "LastActivityView.exe" },
            { "BrowserDownloadsView", "BrowserDownloadsView.exe" },
            { "BrowsingHistoryView", "BrowsingHistoryView.exe" }
        };

        public MainWindow()
        {
            InitializeComponent();

            this.MouseLeftButtonDown += (s, e) => DragMove();

            LoadWindowIcon();
            LoadLogo();
            LoadButtons();
            LoadSocialIcons();
            Method1Text.Text = method1Text;

            StartGradientAnimation();

            AppsContent.Visibility = Visibility.Collapsed;
            InfoContent.Visibility = Visibility.Visible;
            HighlightTab(InfoTabButton);
        }

        private void LoadWindowIcon()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App", "logo.jpg");
            if (File.Exists(path))
            {
                try
                {
                    BitmapImage img = new BitmapImage();
                    img.BeginInit();
                    img.UriSource = new Uri(path);
                    img.CacheOption = BitmapCacheOption.OnLoad;
                    img.EndInit();
                    this.Icon = img;
                }
                catch { }
            }
        }

        private void LoadLogo()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App", "logo.jpg");
            if (File.Exists(path))
            {
                try
                {
                    BitmapImage img = new BitmapImage();
                    img.BeginInit();
                    img.UriSource = new Uri(path);
                    img.CacheOption = BitmapCacheOption.OnLoad;
                    img.EndInit();
                    ServerLogo.Source = img;
                }
                catch { }
            }
        }

        private void LoadButtons()
        {
            MainToolsPanel.Children.Clear();
            string appFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App");

            foreach (var t in tools)
            {
                Button btn = new Button();
                btn.Content = t.Key;
                btn.Width = 180;
                btn.Height = 40;
                btn.Margin = new Thickness(5);
                btn.Tag = Path.Combine(appFolder, t.Value);
                btn.Click += RunProgram;
                btn.Style = (Style)TryFindResource("ActionButtonStyle");
                MainToolsPanel.Children.Add(btn);
            }
        }

        private void RunProgram(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            string path = btn.Tag.ToString();

            if (!File.Exists(path))
            {
                MessageBox.Show("Файл не найден: " + path, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                Process.Start(new ProcessStartInfo(path) { Verb = "runas", UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadSocialIcons()
        {
            LoadIcon(GitHubIcon, "github.ico");
            LoadIcon(TelegramIcon, "telegram.ico");
            LoadIcon(DiscordIcon, "discord.ico");
        }

        private void LoadIcon(Image img, string name)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App", name);
            if (File.Exists(path))
            {
                try
                {
                    BitmapImage bmp = new BitmapImage();
                    bmp.BeginInit();
                    bmp.UriSource = new Uri(path);
                    bmp.CacheOption = BitmapCacheOption.OnLoad;
                    bmp.EndInit();
                    img.Source = bmp;
                }
                catch { }
            }
        }

        private void CopyMethod1_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(Method1Text.Text);
            MessageBox.Show("Скопировано", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void OpenLink_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            string url = "";

            switch (btn.Tag.ToString())
            {
                case "github_url": url = "https://github.com/sergejrastvorov02-labfree123s"; break;
                case "telegram_url": url = "https://t.me/SABRRUST"; break;
                case "discord_url": url = "https://discord.gg/pcHMTqtEdB"; break;
            }

            if (!string.IsNullOrEmpty(url))
                Process.Start(url);
        }

        private void OpenWebsite_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("http://sabrrust.gamestores.app/");
        }

        private void Services_Click(object sender, RoutedEventArgs e) => Process.Start("services.msc");
        private void DataUsage_Click(object sender, RoutedEventArgs e) => Process.Start("resmon.exe");
        
        private void Nvidia_Click(object sender, RoutedEventArgs e)
        {
            try { Process.Start("nvcplui.exe"); }
            catch
            {
                try { Process.Start("rundll32.exe", "shell32.dll,Control_RunDLL nvcplui.dll"); }
                catch { MessageBox.Show("NVIDIA не найдена", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning); }
            }
        }

        private void AppsTab_Click(object sender, RoutedEventArgs e)
        {
            AppsContent.Visibility = Visibility.Visible;
            InfoContent.Visibility = Visibility.Collapsed;
            HighlightTab(AppsTabButton);
        }

        private void InfoTab_Click(object sender, RoutedEventArgs e)
        {
            AppsContent.Visibility = Visibility.Collapsed;
            InfoContent.Visibility = Visibility.Visible;
            HighlightTab(InfoTabButton);
        }

        private void HighlightTab(Button activeTab)
        {
            AppsTabButton.Background = Brushes.Transparent;
            InfoTabButton.Background = Brushes.Transparent;
            activeTab.Background = new SolidColorBrush(Color.FromRgb(58, 58, 58));
        }

        private void Minimize_Click(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;
        private void Close_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();

        private void StartGradientAnimation()
        {
            var gradient = AnimatedGradient;
            var stop1 = gradient.GradientStops[0];
            var stop2 = gradient.GradientStops[1];

            DoubleAnimation anim1 = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(4)) { RepeatBehavior = RepeatBehavior.Forever, AutoReverse = true };
            DoubleAnimation anim2 = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(4)) { RepeatBehavior = RepeatBehavior.Forever, AutoReverse = true };

            stop1.BeginAnimation(GradientStop.OffsetProperty, anim1);
            stop2.BeginAnimation(GradientStop.OffsetProperty, anim2);
        }
    }
}