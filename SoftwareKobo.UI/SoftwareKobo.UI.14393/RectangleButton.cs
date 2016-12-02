using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Automation.Provider;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;

namespace SoftwareKobo.UI
{
    [TemplatePart(Name = ContentPresenterTemplateName, Type = typeof(ContentPresenter))]
    public class RectangleButton : ButtonBase, IButton
    {
        public new static readonly DependencyProperty BorderThicknessProperty = DependencyProperty.Register(nameof(BorderThickness), typeof(double), typeof(RectangleButton), new PropertyMetadata(default(double)));

        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(nameof(CornerRadius), typeof(double), typeof(RectangleButton), new PropertyMetadata(default(double), CornerRadiusChanged));

        public static readonly DependencyProperty DisabledBackgroundProperty = DependencyProperty.Register(nameof(DisabledBackground), typeof(Brush), typeof(RectangleButton), new PropertyMetadata(default(Brush)));

        public static readonly DependencyProperty PointerOverBackgroundProperty = DependencyProperty.Register(nameof(PointerOverBackground), typeof(Brush), typeof(RectangleButton), new PropertyMetadata(default(Brush)));

        public static readonly DependencyProperty PressedBackgroundProperty = DependencyProperty.Register(nameof(PressedBackground), typeof(Brush), typeof(RectangleButton), new PropertyMetadata(default(Brush)));

        private const string ContentPresenterTemplateName = "PART_ContentPresenter";

        private ContentPresenter _contentPresenter;

        public RectangleButton()
        {
            DefaultStyleKey = typeof(RectangleButton);
        }

        public new double BorderThickness
        {
            get
            {
                return (double)GetValue(BorderThicknessProperty);
            }
            set
            {
                SetValue(BorderThicknessProperty, value);
            }
        }

        public double CornerRadius
        {
            get
            {
                return (double)GetValue(CornerRadiusProperty);
            }
            set
            {
                SetValue(CornerRadiusProperty, value);
            }
        }

        public Brush DisabledBackground
        {
            get
            {
                return (Brush)GetValue(DisabledBackgroundProperty);
            }
            set
            {
                SetValue(DisabledBackgroundProperty, value);
            }
        }

        public Brush PointerOverBackground
        {
            get
            {
                return (Brush)GetValue(PointerOverBackgroundProperty);
            }
            set
            {
                SetValue(PointerOverBackgroundProperty, value);
            }
        }

        public Brush PressedBackground
        {
            get
            {
                return (Brush)GetValue(PressedBackgroundProperty);
            }
            set
            {
                SetValue(PressedBackgroundProperty, value);
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

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _contentPresenter = (ContentPresenter)GetTemplateChild(ContentPresenterTemplateName);
            UpdateCornerRadius();
        }

        private static void CornerRadiusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = (RectangleButton)d;
            obj.UpdateCornerRadius();
        }

        private void UpdateCornerRadius()
        {
            if (_contentPresenter != null)
            {
                _contentPresenter.CornerRadius = new CornerRadius(CornerRadius);
            }
        }
    }
}