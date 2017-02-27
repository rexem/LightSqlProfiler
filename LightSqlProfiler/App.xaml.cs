using LightSqlProfiler.Core;
using log4net;
using System.Windows;

namespace LightSqlProfiler
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly ILog Log = LogManager.GetLogger(nameof(App));

        protected override void OnStartup(StartupEventArgs e)
        {
            // setup log file location
            GlobalContext.Properties["LogFileName"] = Common.GetAppFilePath("log.txt");
            log4net.Config.XmlConfigurator.Configure();

            Log.Debug("START");
            base.OnStartup(e);
        }
    }
}
