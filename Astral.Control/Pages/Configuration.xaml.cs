using Astral.Control.Resources;
using Astral.Control.WindowsAPI;
using Astral.Models;
using Astral.Utilities;
using System;
using System.Collections.Generic;
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

namespace Astral.Control.Pages
{
    /// <summary>
    /// Interaction logic for Configuration.xaml
    /// </summary>
    public partial class Configuration : UserControl
    {
        private readonly IConfig configuration;

        public Configuration(
            OSCheck osCheck,
            ImageBackgrounds imageBackgrounds,
            IConfig configuration)
        {
            this.DataContext = this;

            OsCheck =
                osCheck;
            ImageBackgrounds =
                imageBackgrounds;
            this.configuration =
                configuration;

            InitializeComponent();

            // I'm not good at UI code.
            // ConfigBackground.Source = ImageBackgrounds.RandomBackground;
            ConfigurationValues.ToList();
        }

        public OSCheck OsCheck { get; }

        public ImageBackgrounds ImageBackgrounds { get; }

        public string ConfigurationTypeName =>
            configuration.GetType().Name.ToSentence();

        public Dictionary<string, string> ConfigurationValues =>
                configuration
                .GetType()
                .GetProperties()
                .ToDictionary(x => x.Name.ToSentence(),
                    x => x.GetValue(configuration, null)!.ToString()!);

        public event EventHandler<IPage> Replaced;
    }
}
