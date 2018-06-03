using System;
using System.Collections.Generic;
using System.IO;
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
using EO.WebBrowser;

namespace WpfMermaidJs
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;

            WebView.Url = $"file:{Directory.GetCurrentDirectory()}/mermaid.html";
        }

        public string Url { get; set; }

        private void WebView_OnLoadFailed(object sender, LoadFailedEventArgs e)
        {
            MessageBox.Show($"{e.ErrorMessage}\r\n{e.Url}");
        }
    }
}
