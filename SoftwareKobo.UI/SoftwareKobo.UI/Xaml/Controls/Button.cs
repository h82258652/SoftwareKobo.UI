using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Automation.Provider;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace SoftwareKobo.UI.Xaml.Controls
{
    public class Button : ButtonBase
    {
        // TODO
        // Ripple
        // Shadow

        public Button()
        {
            this.DefaultStyleKey = typeof(Button);
        }

        public void PerformClick()
        {
            var peer = FrameworkElementAutomationPeer.FromElement(this) ?? FrameworkElementAutomationPeer.CreatePeerForElement(this);
            var provider = (IInvokeProvider)peer.GetPattern(PatternInterface.Invoke);
            provider.Invoke();
        }
    }
}
