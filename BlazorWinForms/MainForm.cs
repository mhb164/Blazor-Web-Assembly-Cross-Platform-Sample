using BlazorShared;
using BlazorShared.Services;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.WebView.WindowsForms;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorWinForms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            var services = new ServiceCollection();
            services.AddWindowsFormsBlazorWebView();
            services.AddSingleton<WeatherForecastService>();

#if DEBUG
            services.AddBlazorWebViewDeveloperTools();
#endif

            InitializeComponent();

            blazorWebView1.HostPage = @"wwwroot\index.html";
            blazorWebView1.Services = services.BuildServiceProvider();
            blazorWebView1.RootComponents.Add<App>("#app");
        }
    }
}
