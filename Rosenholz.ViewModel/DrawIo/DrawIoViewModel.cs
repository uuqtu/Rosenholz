using EmbedIO;
using EmbedIO.Actions;
using EmbedIO.Files;
using EmbedIO.Files.Internal;
using EmbedIO.WebApi;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;
using Rosenholz.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Services.Description;
using System.Windows;

namespace Rosenholz.ViewModel.DrawIo
{

    public class DrawIoViewModel : ViewModelBase
    {
        private Uri _source;
        private WebServer _server;
        private static int _port = 8085;
        private WebView2 _webView;
        private string _url = $"http://localhost:{_port}/";
        private string _fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "drawio");
        private string _folderPath = String.Empty;
        private RelayCommand _copyPathCommand;

        public RelayCommand CopyPathCommand
        {
            get
            {
                if (_copyPathCommand == null)
                {
                    _copyPathCommand = new RelayCommand(
                        (parameter) => CopyPathCommandExecute(parameter),
                        (parameter) => !String.IsNullOrWhiteSpace(FolderPath)
                    );
                }
                return _copyPathCommand;
            }
        }

        private void CopyPathCommandExecute(object parameter)
        {
            Clipboard.SetData(DataFormats.Text, FolderPath);
        }

        public DrawIoViewModel()
        {
            // Initialize properties or commands here if needed
        }

        public Uri Source
        {
            get { return this._source; }
            set
            {
                _source = value;
                OnPropertyChanged(nameof(Source));
            }
        }

        public string FolderPath
        {
            get { return _folderPath; }
            set
            {
                _folderPath = value;
                OnPropertyChanged(nameof(FolderPath));
            }
        }

        public void StartWebServer()
        {
            //_fullPath = @"d:\\OneDrive - Siemens AG\\Rosenholz\\Rosenholz.draw.io\\drawio\\";

            if (_server?.State != WebServerState.Listening)
            {
                _server = new WebServer(o => o
                                            .WithUrlPrefix(_url)
                                            .WithMode(HttpListenerMode.EmbedIO))
                                            .WithModule(new FileModule("/", new EmbedIO.Files.FileSystemProvider(_fullPath, true)));

                _server.RunAsync();
            }
        }



        public async void SetSource()
        {
            string localDrawioPath = "https://app.diagrams.net/?src=about";
            localDrawioPath = "http://localhost:8085/index.html";

            //Muss nur einmal gesetzt werden, danach soll sie offen bleiben.
            if (Source?.ToString()?.Equals(localDrawioPath) == false || Source?.ToString()?.Equals(localDrawioPath) == null)
            {
                var env = CoreWebView2Environment.CreateAsync(null, null);
                var uri = new Uri(localDrawioPath);
                _webView = new WebView2();
                _webView.Source = uri;
                Source = uri;

                await _webView.EnsureCoreWebView2Async();

                Thread.Sleep(2000);

                _webView.CoreWebView2.WebMessageReceived += CoreWebView2_WebMessageReceived;

                
            }

        }

        private void CoreWebView2_WebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            string xml = e.WebMessageAsJson;
            File.WriteAllText("exportedDiagram.xml", xml);
            MessageBox.Show("Diagram exported and saved.");
        }

        private async Task SendToDrawIO(string xml)
        {
            var script = $"window.chrome.webview.postMessage('{xml.Replace("'", "\\'")}');";
            await _webView.ExecuteScriptAsync(script);
        }

        public void StopWebServer()
        {
            if (_server != null && _server.State == WebServerState.Listening)
            {
                _server.Dispose();
                _server = null;
            }
        }
    }
}
