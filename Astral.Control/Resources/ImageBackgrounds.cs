using Astral.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Astral.Control.Resources
{
    public class ImageBackgrounds : IUtility
    {
        private readonly RandomProvider randomProvider;

        public ImageBackgrounds(Utilities.RandomProvider randomProvider)
        {
            this.randomProvider = randomProvider;
        }

        public ImageSource RandomBackground =>
            new BitmapImage(new Uri($"pack://application:,,,/Resources/" +
                $"{Backgrounds.ToList()[randomProvider.NextInt(Backgrounds.Count())]}.png"));

        public IEnumerable<string> Backgrounds =>
            Resources.ToList().Where(x => x.StartsWith("small_background"));

        public IEnumerable<string> Resources
        {
            get
            {
                var resourceSet = Properties
                    .Resources
                    .ResourceManager
                    .GetResourceSet(CultureInfo.CurrentUICulture, true, true);

                foreach (DictionaryEntry entry in resourceSet!)
                    yield return entry.Key as string;
            }
        }

    }
}
