using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Automation.Provider;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;

namespace SoftwareKobo.UI
{
    public class RectangleButton : ButtonBase, IButton
    {
        public static readonly DependencyProperty PointerOverBrushProperty = DependencyProperty.Register(nameof(PointerOverBrush), typeof(Brush), typeof(RectangleButton), new PropertyMetadata(default(Brush)));

        public static readonly DependencyProperty PressedBrushProperty = DependencyProperty.Register(nameof(PressedBrush), typeof(Brush), typeof(RectangleButton), new PropertyMetadata(default(Brush)));

        public static readonly DependencyProperty RadiusXProperty = DependencyProperty.Register(nameof(RadiusX), typeof(double), typeof(RectangleButton), new PropertyMetadata(default(double)));

        public static readonly DependencyProperty RadiusYProperty = DependencyProperty.Register(nameof(RadiusY), typeof(double), typeof(RectangleButton), new PropertyMetadata(default(double)));

        public RectangleButton()
        {
            DefaultStyleKey = typeof(RectangleButton);
        }

        public Brush PointerOverBrush
        {
            get
            {
                return (Brush)GetValue(PointerOverBrushProperty);
            }
            set
            {
                SetValue(PointerOverBrushProperty, value);
            }
        }

        public Brush PressedBrush
        {
            get
            {
                return (Brush)GetValue(PressedBrushProperty);
            }
            set
            {
                SetValue(PressedBrushProperty, value);
            }
        }

        public double RadiusX
        {
            get
            {
                return (double)GetValue(RadiusXProperty);
            }
            set
            {
                SetValue(RadiusXProperty, value);
            }
        }

        public double RadiusY
        {
            get
            {
                return (double)GetValue(RadiusYProperty);
            }
            set
            {
                SetValue(RadiusYProperty, value);
            }
        }

        public void PerformClick()
        {
            // TODO check IsEnabled=false exception.

            var automationPeer = FrameworkElementAutomationPeer.FromElement(this) ?? FrameworkElementAutomationPeer.CreatePeerForElement(this);
            var invokeProvider = (IInvokeProvider)automationPeer.GetPattern(PatternInterface.Invoke);
            Debug.Assert(invokeProvider != null);
            invokeProvider.Invoke();
        }
    }
}