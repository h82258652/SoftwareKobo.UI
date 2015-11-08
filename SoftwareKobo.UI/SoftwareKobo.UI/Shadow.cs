using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Graphics.DirectX;
using Windows.Graphics.Display;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace SoftwareKobo.UI
{
    [ContentProperty(Name = nameof(Content))]
    [TemplatePart(Name = CanvasControlPartName, Type = typeof(CanvasControl))]
    public class Shadow : Control
    {
        public static readonly DependencyProperty BlurAmountProperty = DependencyProperty.Register(nameof(BlurAmount), typeof(float), typeof(Shadow), new PropertyMetadata(2.0f));
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(nameof(Color), typeof(Color), typeof(Shadow), new PropertyMetadata(Colors.Black));
        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register(nameof(Content), typeof(FrameworkElement), typeof(Shadow), new PropertyMetadata(null, ContentChanged));
        public static readonly DependencyProperty ContentTemplateProperty = DependencyProperty.Register(nameof(ContentTemplate), typeof(DataTemplate), typeof(Shadow), new PropertyMetadata(null));

        public static readonly DependencyProperty DepthProperty = DependencyProperty.Register(nameof(Depth), typeof(double), typeof(Shadow), new PropertyMetadata(2.0d, DepthChanged));

        public static readonly DependencyProperty DirectionProperty = DependencyProperty.Register(nameof(Direction), typeof(double), typeof(Shadow), new PropertyMetadata(270.0d));

        public static readonly DependencyProperty OptimizationProperty = DependencyProperty.Register(nameof(Optimization), typeof(EffectOptimization), typeof(Shadow), new PropertyMetadata(EffectOptimization.Balanced));

        private const string CanvasControlPartName = @"PART_CanvasControl";

        private CanvasControl _canvas;

        private int _pixelHeight;

        private byte[] _pixels;

        private int _pixelWidth;

        public Shadow()
        {
            this.DefaultStyleKey = typeof(Shadow);
            this.Unloaded += this.OnUnloaded;
        }

        public float BlurAmount
        {
            get
            {
                return (float)this.GetValue(BlurAmountProperty);
            }
            set
            {
                this.SetValue(BlurAmountProperty, value);
            }
        }

        public Color Color
        {
            get
            {
                return (Color)this.GetValue(ColorProperty);
            }
            set
            {
                this.SetValue(ColorProperty, value);
            }
        }

        public FrameworkElement Content
        {
            get
            {
                return (FrameworkElement)this.GetValue(ContentProperty);
            }
            set
            {
                this.SetValue(ContentProperty, value);
            }
        }

        public DataTemplate ContentTemplate
        {
            get
            {
                return (DataTemplate)this.GetValue(ContentTemplateProperty);
            }
            set
            {
                this.SetValue(ContentTemplateProperty, value);
            }
        }

        public double Depth
        {
            get
            {
                return (double)this.GetValue(DepthProperty);
            }
            set
            {
                this.SetValue(DepthProperty, value);
            }
        }

        public double Direction
        {
            get
            {
                return (double)this.GetValue(DirectionProperty);
            }
            set
            {
                this.SetValue(DirectionProperty, value);
            }
        }

        public EffectOptimization Optimization
        {
            get
            {
                return (EffectOptimization)this.GetValue(OptimizationProperty);
            }
            set
            {
                this.SetValue(OptimizationProperty, value);
            }
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this._canvas = (CanvasControl)this.GetTemplateChild(CanvasControlPartName);
            this._canvas.Draw += this.Canvas_Draw;
            this.ExpendCanvas();
        }

        private static void ContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Shadow obj = (Shadow)d;

            FrameworkElement oldValue = (FrameworkElement)e.OldValue;
            if (oldValue != null)
            {
                oldValue.LayoutUpdated -= obj.Content_LayoutUpdated;
            }

            FrameworkElement newValue = (FrameworkElement)e.NewValue;
            if (newValue != null)
            {
                newValue.LayoutUpdated += obj.Content_LayoutUpdated;
            }
        }

        private static void DepthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Shadow obj = (Shadow)d;
            obj.ExpendCanvas();
        }

        private void Canvas_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            if (this.Content == null || this._pixels == null || this._pixelWidth <= 0 || this._pixelHeight <= 0)
            {
                args.DrawingSession.Clear(sender.ClearColor);
            }
            else
            {
                GeneralTransform transform = this.Content.TransformToVisual(sender);
                Vector2 location = transform.TransformPoint(new Point()).ToVector2();

                using (CanvasCommandList cl = new CanvasCommandList(sender))
                {
                    using (CanvasDrawingSession clds = cl.CreateDrawingSession())
                    {
                        using (CanvasBitmap bitmap = CanvasBitmap.CreateFromBytes(sender, this._pixels, this._pixelWidth, this._pixelHeight, DirectXPixelFormat.B8G8R8A8UIntNormalized, DisplayInformation.GetForCurrentView().LogicalDpi))
                        {
                            clds.DrawImage(bitmap, location);
                        }
                    }

                    float translateX = (float)(Math.Cos(Math.PI / 180.0d * this.Direction) * this.Depth);
                    float translateY = 0 - (float)(Math.Sin(Math.PI / 180.0d * this.Direction) * this.Depth);
                    Transform2DEffect finalEffect = new Transform2DEffect()
                    {
                        Source = new ShadowEffect()
                        {
                            Source = cl,
                            BlurAmount = this.BlurAmount,
                            ShadowColor = this.GetShadowColor(),
                            Optimization = this.Optimization
                        },
                        TransformMatrix = Matrix3x2.CreateTranslation(translateX, translateY)
                    };

                    args.DrawingSession.DrawImage(finalEffect);
                }
            }
        }

        private async void Content_LayoutUpdated(object sender, object e)
        {
            if (DesignMode.DesignModeEnabled || this.Visibility == Visibility.Collapsed || this.Content.Visibility == Visibility.Collapsed)
            {
                this._pixels = null;
                this._pixelWidth = 0;
                this._pixelHeight = 0;
            }
            else
            {
                RenderTargetBitmap bitmap = new RenderTargetBitmap();
                await bitmap.RenderAsync(this.Content);

                int pixelWidth = bitmap.PixelWidth;
                int pixelHeight = bitmap.PixelHeight;
                if (pixelWidth > 0 && pixelHeight > 0)
                {
                    this._pixels = (await bitmap.GetPixelsAsync()).ToArray();
                    this._pixelWidth = pixelWidth;
                    this._pixelHeight = pixelHeight;
                }
                else
                {
                    this._pixels = null;
                    this._pixelWidth = 0;
                    this._pixelHeight = 0;
                }
            }

            if (this._canvas != null)
            {
                this._canvas.Invalidate();
            }
        }

        private void ExpendCanvas()
        {
            if (this._canvas != null)
            {
                this._canvas.Margin = new Thickness(0 - (this.Depth + 10));
            }
        }

        private Color GetShadowColor()
        {
            if (this.Content.Visibility == Visibility.Collapsed)
            {
                return Colors.Transparent;
            }
            double alphaRatio = Math.Max(0, Math.Min(1, this.Content.Opacity));
            byte newAlpha = (byte)(this.Color.A * alphaRatio);
            return Color.FromArgb(newAlpha, this.Color.R, this.Color.G, this.Color.B);
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            if (this._canvas != null)
            {
                this._canvas.RemoveFromVisualTree();
                this._canvas = null;
            }
        }
    }
}