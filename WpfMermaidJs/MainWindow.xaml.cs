using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
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
using EO.WebBrowser;
using Reactive.Bindings;

namespace WpfMermaidJs
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        private static string mermaidHtmlFileName = "mermaid.html";
        private static string mermaidHtmlFormat = @"
<!DOCTYPE html>
<html lang=""en"">
<head>
  <meta charset=""utf-8"">
  <link rel=""stylesheet"" href=""mermaid.min.css"">
</head>
<body>
  <div class=""mermaid"">
{0}
  </div>
  <script src=""mermaid.min.js""></script>
  <script>
    var config = {{
                startOnLoad:true,
                flowchart:{{
                        useMaxWidth:false,
                    }}
            }};
    mermaid.initialize(config);
  </script>
</body>
</html>
";

        public ReactiveProperty<string> MermaidText {get;} = new ReactiveProperty<string>(@"
graph LR
    A --- B
    B-->C[fa:fa-ban forbidden]
    B-->D(fa:fa-spinner);
");
        public ReactiveProperty<double> ZoomFactor { get; } = new ReactiveProperty<double>(1.0);

        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;

            WebView.Url = $"file:{Directory.GetCurrentDirectory()}/{mermaidHtmlFileName}";

            MermaidText
                .Throttle(TimeSpan.FromMilliseconds(1000))
                .Subscribe(x =>
                {
                    if (x == null) return;
                    WriteToHtml(x);
                    WebView.Reload();
                });

            ZoomFactor.Subscribe(x => { WebView.ZoomFactor = x; });
        }

        private static void WriteToHtml(string x)
        {
            File.WriteAllText(mermaidHtmlFileName, string.Format(mermaidHtmlFormat, x));
        }
    }
}
