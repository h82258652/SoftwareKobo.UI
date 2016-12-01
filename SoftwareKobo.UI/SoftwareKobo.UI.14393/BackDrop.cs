using Microsoft.Graphics.Canvas.Effects;
using System.Numerics;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;

namespace SoftwareKobo.UI
{
    public class Backdrop : Control
    {
        public static readonly DependencyProperty BlurAmountProperty = DependencyProperty.Register(nameof(BlurAmount), typeof(double), typeof(Backdrop), new PropertyMetadata(default(double), BlurAmountChanged));

        public static readonly DependencyProperty TintColorProperty = DependencyProperty.Register(nameof(TintColor), typeof(Color), typeof(Backdrop), new PropertyMetadata(Colors.Transparent, TintColorChanged));

        private readonly SpriteVisual _blurVisual;

        private readonly Compositor _compositor;

        public Backdrop()
        {
            DefaultStyleKey = typeof(Backdrop);

            SizeChanged += (sender, e) =>
            {
                UpdateVisualSize();
            };

            var rootVisual = ElementCompositionPreview.GetElementVisual(this);
            _compositor = rootVisual.Compositor;
            _blurVisual = _compositor.CreateSpriteVisual();
            var brush = CreateBlurBrush();
            brush.SetSourceParameter("Source", _compositor.CreateBackdropBrush());
            _blurVisual.Brush = brush;

            ElementCompositionPreview.SetElementChildVisual(this, _blurVisual);
        }

        public double BlurAmount
        {
            get
            {
                return (double)GetValue(BlurAmountProperty);
            }
            set
            {
                SetValue(BlurAmountProperty, value);
            }
        }

        public Color TintColor
        {
            get
            {
                return (Color)GetValue(TintColorProperty);
            }
            set
            {
                SetValue(TintColorProperty, value);
            }
        }

        private static void BlurAmountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = (Backdrop)d;
            var value = (double)e.NewValue;
            obj._blurVisual.Brush.Properties.InsertScalar("BlurEffect.BlurAmount", (float)value);
        }

        private static void TintColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = (Backdrop)d;
            var value = (Color)e.NewValue;
            obj._blurVisual.Brush.Properties.InsertColor("ColorEffect.Color", value);
        }

        private CompositionEffectBrush CreateBlurBrush()
        {
            var blurEffect = new GaussianBlurEffect()
            {
                Name = "BlurEffect",
                BlurAmount = (float)BlurAmount,
                Optimization = EffectOptimization.Balanced,
                Source = new CompositionEffectSourceParameter("Source"),
                BorderMode = EffectBorderMode.Hard
            };
            var colorEffect = new ColorSourceEffect()
            {
                Name = "ColorEffect",
                Color = TintColor
            };
            var blendEffect = new BlendEffect()
            {
                Background = blurEffect,
                Foreground = colorEffect,
                Mode = BlendEffectMode.SoftLight
            };
            var effectFactory = _compositor.CreateEffectFactory(blendEffect, new[]
            {
                "BlurEffect.BlurAmount",
                "ColorEffect.Color"
            });
            return effectFactory.CreateBrush();
        }

        private void UpdateVisualSize()
        {
            _blurVisual.Size = new Vector2((float)ActualWidth, (float)ActualHeight);
        }
    }
}