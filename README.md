# WpfMermaidJs

Show mermaid.js flowchart in wpf.
mermaid.js のダイアグラムを WPFアプリケーションで使う。

## 参考にしたサイト
* http://www.atmarkit.co.jp/ait/articles/1807/04/news017.html
* https://code.msdn.microsoft.com/windowsapps/Windows-Store-Apps-How-to-8073b0da

## メモ
Microsoft.Toolkit.Wpf.UI.Controls.WebView をnugetでインストールする  
(昔は Microsoft.Toolkit.Win32.UI.Controls に含まれていたが分離されたようだ)

ローカルのhtmlを表示するのに苦労
```csharp
WebView.NavigateToLocalStreamUri(new Uri(mermaidHtmlFileName, UriKind.Relative), new StreamUriResolver());
```
