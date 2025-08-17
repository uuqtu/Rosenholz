using EmbedIO;
using EmbedIO.Actions;
using EmbedIO.Files;
using EmbedIO.Files.Internal;
using EmbedIO.WebApi;
using Microsoft.Web.WebView2.Core;
using Rosenholz.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
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
        private string _url = $"http://localhost:{_port}/";
        private string _fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "drawio");

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

        public void StartWebServer()
        {
            _fullPath = @"d:\\OneDrive - Siemens AG\\Rosenholz\\Rosenholz.draw.io\\drawio\\";

            if (_server?.State != WebServerState.Listening)
            {
                _server = new WebServer(o => o
                                            .WithUrlPrefix(_url)
                                            .WithMode(HttpListenerMode.EmbedIO))
                                            .WithModule(new FileModule("/", new EmbedIO.Files.FileSystemProvider(_fullPath, true)));

                _server.RunAsync();
            }
        }



        public async Task LoadSourceAsync()
        {
            string localDrawioPath = "https://app.diagrams.net/?src=about";
            localDrawioPath = "http://localhost:8085/index.html";

            //Muss nur einmal gesetzt werden, danach soll sie offen bleiben.
            if (Source?.ToString()?.Equals(localDrawioPath) == false || Source?.ToString()?.Equals(localDrawioPath) == null)
            {
                var env = await CoreWebView2Environment.CreateAsync(null, null);

                var uri = new Uri(localDrawioPath).AbsoluteUri;
                Source = new Uri(uri);
                var a = Source.ToString();
            }

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
