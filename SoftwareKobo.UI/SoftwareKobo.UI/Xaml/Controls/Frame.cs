using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace SoftwareKobo.UI.Xaml.Controls
{
    public class Frame : Windows.UI.Xaml.Controls.Frame
    {
        public Frame() : base()
        {
        }

        public bool Navigate<TPage>() where TPage : Page
        {
            return Navigate(typeof(TPage));
        }

        public bool Navigate<TPage>(object parameter) where TPage : Page
        {
            return Navigate(typeof(TPage), parameter);
        }

        public bool Navigate<TPage>(object parameter, NavigationTransitionInfo infoOverride)
        {
            return Navigate(typeof(TPage), parameter, infoOverride);
        }
    }
}
