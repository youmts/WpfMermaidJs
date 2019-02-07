using System;

using System.Diagnostics;
using System.IO;
using System.Reactive.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;
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

            MermaidText
                .Throttle(TimeSpan.FromMilliseconds(1000))
                .Subscribe(x =>
                {
                    if (x == null) return;
                    WriteToHtml(x);
                    Dispatcher.Invoke(WebView.Refresh);
                });

            ZoomFactor
                .Throttle(TimeSpan.FromMilliseconds(100))
                .Subscribe(x =>
                {
                    Dispatcher.Invoke(() =>
                        WebView.InvokeScriptAsync("eval", $"document.body.style.zoom = {x};")
                    );
                });
        }

        private static void WriteToHtml(string x)
        {
            File.WriteAllText(mermaidHtmlFileName, string.Format(mermaidHtmlFormat, x));
        }

        private class StreamUriResolver : IUriToStreamResolver
        {
            public Stream UriToStream(Uri uri)
            {
                Uri baseDir = new Uri(AppDomain.CurrentDomain.BaseDirectory);
                Uri target = new Uri(baseDir, uri.LocalPath.TrimStart('/'));
                return new FileStream(target.AbsolutePath, FileMode.Open);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WriteToHtml(MermaidText.Value);
            WebView.NavigateToLocalStreamUri(new Uri(mermaidHtmlFileName, UriKind.Relative), new StreamUriResolver());
        }
    }
}
