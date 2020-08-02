using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TranslatorWPF.Windows.Main;

namespace TranslatorWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_Startup(object sender, StartupEventArgs e)
        {
            ViewModel viewModel = new ViewModel();

            viewModel.Show(new View());

            Shutdown();
        }
    }
}
