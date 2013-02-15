using System;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using System.Windows.Media.Imaging;

namespace ScottIsAFool.WindowsPhone.Controls
{
    public class TileSlider : ContentControl
    {
        Storyboard slideTile;
        Storyboard spinTile;
        TextBlock tb;
        StackPanel theStack;
        Grid im;

        DispatcherTimer dispatchTimer;

        int min = 18;
        int max = 40;

        public TileSlider() 
        {            
            DefaultStyleKey = typeof(TileSlider);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            tb = GetTemplateChild("theText") as TextBlock;
            im = GetTemplateChild("theImage") as Grid;
            theStack = GetTemplateChild("theStack") as StackPanel;
            TileSlider_Loaded(this, new RoutedEventArgs());
            MouseLeftButtonUp += TileSlider_MouseLeftButtonUp;
        }

        void TileSlider_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (Click != null)
                Click(this, new RoutedEventArgs());
        }

        void TileSlider_Loaded(object sender, RoutedEventArgs e)
        {
            theStack.Orientation = (TextSlidesFrom == SlideDirection.Bottom || TextSlidesFrom == SlideDirection.Top) ? Orientation.Vertical : Orientation.Horizontal;
            //tb.Margin = new Thickness(0, -ActualHeight, 0, 0);
            SetStackPanel(theStack, im, tb);
            RectangleGeometry rect = new RectangleGeometry { Rect = new Rect(0, 0, ActualWidth, ActualHeight) };
            Clip = rect;
            if (!string.IsNullOrEmpty(Text))
            {
                CreateStoryBoard();

                dispatchTimer = new DispatcherTimer();
                dispatchTimer.Tick += dispatchTimer_Tick;
                dispatchTimer.Interval = new TimeSpan(0, 0, RandomNumber(1, 10));
                dispatchTimer.Start();
            }
        }

        private void SetStackPanel(StackPanel sp, Grid im, TextBlock tb)
        {
            sp.Children.Clear();
            switch (TextSlidesFrom)
            {
                case SlideDirection.Top:
                    tb.Margin = new Thickness(0, -ActualHeight, 0, 0);
                    sp.Children.Add(tb);
                    sp.Children.Add(im);
                    break;
                case SlideDirection.Bottom:
                    sp.Children.Add(im);
                    sp.Children.Add(tb);
                    break;
                case SlideDirection.Left:
                    tb.Margin = new Thickness(-ActualWidth, 0, 0, 0);
                    sp.Children.Add(tb);
                    sp.Children.Add(im);
                    break;
                case SlideDirection.Right:
                    sp.Children.Add(im);
                    sp.Children.Add(tb);
                    break;
                default:
                    tb.Margin = new Thickness(0, -ActualHeight, 0, 0);
                    sp.Children.Add(tb);
                    sp.Children.Add(im);
                    break;
            }
        }

        private static int RandomNumber(int min, int max)
        {
            Random r = new Random();
            return r.Next(min, max);
        }

        void dispatchTimer_Tick(object sender, EventArgs e)
        {
            dispatchTimer.Stop();
            
            dispatchTimer.Interval = new TimeSpan(0, 0, RandomNumber(min, max));
            if (!IsStartTile)
                slideTile.Begin();
            else
            {
                if (spinTile != null)
                    spinTile.Begin();
            }
            dispatchTimer.Start();
        }

        void slideTile_Completed(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(LongText) && spinTile != null)
            {
                spinTile.Begin();
            }
        }

        /// <summary>
        /// Creates the story board.
        /// </summary>
        private void CreateStoryBoard()
        {
            double halfHeight = ActualHeight / 2;
            double halfWidth = ActualWidth / 2;
            
            #region TextSlidesFrom Switch
            switch (TextSlidesFrom)
            {
                case SlideDirection.Top:
                    slideTile = GetTemplateChild("moveTileDown") as Storyboard;
                    foreach (var animation in slideTile.Children)
                    {
                        DoubleAnimationUsingKeyFrames ani = (DoubleAnimationUsingKeyFrames)animation;
                        EasingDoubleKeyFrame kf1 = ani.KeyFrames[1] as EasingDoubleKeyFrame;
                        EasingDoubleKeyFrame kf2 = ani.KeyFrames[2] as EasingDoubleKeyFrame;
                        EasingDoubleKeyFrame kf3 = ani.KeyFrames[3] as EasingDoubleKeyFrame;
                        EasingDoubleKeyFrame kf4 = ani.KeyFrames[4] as EasingDoubleKeyFrame;
                        kf1.Value = halfHeight;
                        kf2.Value = halfHeight;
                        kf3.Value = ActualHeight;
                        kf4.Value = ActualHeight;
                    }
                    break;
                case SlideDirection.Bottom:
                    slideTile = GetTemplateChild("moveTileUp") as Storyboard;
                    foreach (var animation in slideTile.Children)
                    {
                        DoubleAnimationUsingKeyFrames ani = (DoubleAnimationUsingKeyFrames)animation;
                        EasingDoubleKeyFrame kf1 = ani.KeyFrames[1] as EasingDoubleKeyFrame;
                        EasingDoubleKeyFrame kf2 = ani.KeyFrames[2] as EasingDoubleKeyFrame;
                        EasingDoubleKeyFrame kf3 = ani.KeyFrames[3] as EasingDoubleKeyFrame;
                        EasingDoubleKeyFrame kf4 = ani.KeyFrames[4] as EasingDoubleKeyFrame;
                        kf1.Value = -halfHeight;
                        kf2.Value = -halfHeight;
                        kf3.Value = -ActualHeight;
                        kf4.Value = -ActualHeight;
                    }
                    break;
                case SlideDirection.Left:
                    slideTile = GetTemplateChild("moveTileRight") as Storyboard;
                    foreach (var animation in slideTile.Children)
                    {
                        DoubleAnimationUsingKeyFrames ani = (DoubleAnimationUsingKeyFrames)animation;
                        EasingDoubleKeyFrame kf1 = ani.KeyFrames[1] as EasingDoubleKeyFrame;
                        EasingDoubleKeyFrame kf2 = ani.KeyFrames[2] as EasingDoubleKeyFrame;
                        EasingDoubleKeyFrame kf3 = ani.KeyFrames[3] as EasingDoubleKeyFrame;
                        EasingDoubleKeyFrame kf4 = ani.KeyFrames[4] as EasingDoubleKeyFrame;
                        kf1.Value = halfWidth;
                        kf2.Value = halfWidth;
                        kf3.Value = ActualWidth;
                        kf4.Value = ActualWidth;
                    }
                    break;
                case SlideDirection.Right:
                    slideTile = GetTemplateChild("moveTileLeft") as Storyboard;
                    foreach (var animation in slideTile.Children)
                    {
                        DoubleAnimationUsingKeyFrames ani = (DoubleAnimationUsingKeyFrames)animation;
                        EasingDoubleKeyFrame kf1 = ani.KeyFrames[1] as EasingDoubleKeyFrame;
                        EasingDoubleKeyFrame kf2 = ani.KeyFrames[2] as EasingDoubleKeyFrame;
                        EasingDoubleKeyFrame kf3 = ani.KeyFrames[3] as EasingDoubleKeyFrame;
                        EasingDoubleKeyFrame kf4 = ani.KeyFrames[4] as EasingDoubleKeyFrame;
                        kf1.Value = -halfWidth;
                        kf2.Value = -halfWidth;
                        kf3.Value = -ActualWidth;
                        kf4.Value = -ActualWidth;
                    }
                    break;
                default:
                    slideTile = GetTemplateChild("moveTileDown") as Storyboard;
                    foreach (var animation in slideTile.Children)
                    {
                        DoubleAnimationUsingKeyFrames ani = (DoubleAnimationUsingKeyFrames)animation;
                        EasingDoubleKeyFrame kf1 = ani.KeyFrames[1] as EasingDoubleKeyFrame;
                        EasingDoubleKeyFrame kf2 = ani.KeyFrames[2] as EasingDoubleKeyFrame;
                        EasingDoubleKeyFrame kf3 = ani.KeyFrames[3] as EasingDoubleKeyFrame;
                        EasingDoubleKeyFrame kf4 = ani.KeyFrames[4] as EasingDoubleKeyFrame;
                        kf1.Value = halfHeight;
                        kf2.Value = halfHeight;
                        kf3.Value = ActualHeight;
                        kf4.Value = ActualHeight;
                    }
                    break;
            }
            #endregion

            slideTile.Completed += slideTile_Completed;

            if (!string.IsNullOrEmpty(LongText))
            {
                spinTile = GetTemplateChild("spinTile") as Storyboard;
                var thing = GetTemplateChild("gridTransform") as ScaleTransform;
                thing.CenterX = ActualHeight / 2;
                thing.CenterY = ActualWidth / 2;
            }
            else
            {
                min = 11;
                max = 30;
            }
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
                                                  "Text",
                                                  typeof(string),
                                                  typeof(TileSlider),
                                                  new PropertyMetadata(""));

        public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register(
                                                  "ImageSource",
                                                  typeof(ImageSource),
                                                  typeof(TileSlider),
                                                  new PropertyMetadata(new BitmapImage(new Uri("TileSlider;component/images/TS.png", UriKind.RelativeOrAbsolute))));

        public static readonly DependencyProperty TileBackgroundProperty = DependencyProperty.Register(
                                                  "TileBackground",
                                                  typeof(SolidColorBrush),
                                                  typeof(TileSlider),
                                                  new PropertyMetadata((SolidColorBrush)Application.Current.Resources["PhoneAccentBrush"]));

        public static readonly DependencyProperty TextStyleProperty = DependencyProperty.Register(
                                                  "TextStyle",
                                                  typeof(Style),
                                                  typeof(TileSlider),
                                                  new PropertyMetadata((Style)Application.Current.Resources["PhoneTextLargeStyle"]));

        public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(
                                                  "Size",
                                                  typeof(double),
                                                  typeof(TileSlider),
                                                  new PropertyMetadata((double)-1, OnSizeChanged));

        public static readonly DependencyProperty LongTextProperty = DependencyProperty.Register(
                                                  "LongText",
                                                  typeof(string),
                                                  typeof(TileSlider),
                                                  new PropertyMetadata(""));

        public static readonly DependencyProperty LongTextStyleProperty = DependencyProperty.Register(
                                                  "LongTextStyle",
                                                  typeof(Style),
                                                  typeof(TileSlider),
                                                  new PropertyMetadata((Style)Application.Current.Resources["PhoneTextLargeStyle"]));

        public static readonly DependencyProperty StretchProperty = DependencyProperty.Register(
                                                  "Stretch",
                                                  typeof(Stretch),
                                                  typeof(TileSlider),
                                                  new PropertyMetadata(Stretch.Uniform));

        public static readonly DependencyProperty SlideDirectionProperty = DependencyProperty.Register(
                                                  "SlideDirection",
                                                  typeof(SlideDirection),
                                                  typeof(TileSlider),
                                                  new PropertyMetadata(SlideDirection.Top));

        public static readonly DependencyProperty IsStartTileProperty = DependencyProperty.Register(
                                                  "IsStartTile",
                                                  typeof(bool),
                                                  typeof(TileSlider),
                                                  new PropertyMetadata(false, OnStartTileChanged));

        public static readonly DependencyProperty BackTitleProperty = DependencyProperty.Register(
                                                  "BackTitle",
                                                  typeof(string),
                                                  typeof(TileSlider),
                                                  new PropertyMetadata(""));

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
                                                  "Title",
                                                  typeof(string),
                                                  typeof(TileSlider),
                                                  new PropertyMetadata(""));

        public static readonly DependencyProperty BackImageSourceProperty = DependencyProperty.Register(
                                                  "BackImageSource",
                                                  typeof(ImageSource),
                                                  typeof(TileSlider),
                                                  new PropertyMetadata(new BitmapImage(new Uri("TileSlider;component/images/TS.png", UriKind.RelativeOrAbsolute))));        

        private static void OnStartTileChanged(DependencyObject theTarget, DependencyPropertyChangedEventArgs e)
        {
            TileSlider ts = theTarget as TileSlider;
            if ((bool)e.NewValue)
            {
                if (ts.slideTile != null)
                    ts.slideTile.Stop();
            }
            else
            {
                if (ts.slideTile != null)
                    ts.slideTile.Begin();
            }
        }

        private static void OnSizeChanged(DependencyObject theTarget, DependencyPropertyChangedEventArgs e)
        {
            TileSlider ts = theTarget as TileSlider;
            ts.Height = (double)e.NewValue;
            ts.Width = (double)e.NewValue;
        }

        /// <summary>
        /// Gets or sets the text. If this isn't set, the tile will just show the image
        /// </summary>
        /// <value>The text.</value>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        /// <summary>
        /// Gets or sets the image source.
        /// </summary>
        /// <value>The image source. </value>
        public ImageSource ImageSource
        {
            get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        public ImageSource BackImageSource
        {
            get { return (ImageSource)GetValue(BackImageSourceProperty); }
            set { SetValue(BackImageSourceProperty, value); }
        }

        /// <summary>
        /// Gets or sets the tile background.
        /// </summary>
        /// <value>The tile background.</value>
        public SolidColorBrush TileBackground
        {
            get { return (SolidColorBrush)GetValue(TileBackgroundProperty); }
            set { SetValue(TileBackgroundProperty, value); }
        }

        /// <summary>
        /// Gets or sets the text style.
        /// </summary>
        /// <value>The text style.</value>
        public Style TextStyle
        {
            get { return (Style)GetValue(TextStyleProperty); }
            set { SetValue(TextStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the size of the control. This overrules Height/Width properties
        /// </summary>
        /// <value>The size.</value>
        public double Size
        {
            get { return (double)GetValue(SizeProperty); }
            set { SetValue(SizeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the long text. This is displayed when the tile flips
        /// </summary>
        /// <value>The long text.</value>
        public string LongText
        {
            get { return (string)GetValue(LongTextProperty); }
            set { SetValue(LongTextProperty, value); }
        }

        /// <summary>
        /// Gets or sets the long text style.
        /// </summary>
        /// <value>The long text style.</value>
        public Style LongTextStyle
        {
            get { return (Style)GetValue(LongTextStyleProperty); }
            set { SetValue(LongTextStyleProperty, value); }
        }

        public string BackTitle
        {
            get { return (string)GetValue(BackTitleProperty); }
            set { SetValue(BackTitleProperty, value); }
        }

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the stretch.
        /// </summary>
        /// <value>
        /// The stretch.
        /// </value>
        public Stretch Stretch
        {
            get { return (Stretch)GetValue(StretchProperty); }
            set { SetValue(StretchProperty, value); }
        }

        /// <summary>
        /// Gets or sets where the text slides from.
        /// </summary>
        /// <value>
        /// Slidedirection
        /// </value>
        public SlideDirection TextSlidesFrom
        {
            get { return (SlideDirection)GetValue(SlideDirectionProperty); }
            set { SetValue(SlideDirectionProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is start tile.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is start tile; otherwise, <c>false</c>.
        /// </value>
        public bool IsStartTile
        {
            get { return (bool)GetValue(IsStartTileProperty); }
            set { SetValue(IsStartTileProperty, value); }
        }

        //private SlideDirection _textSlidesFrom = SlideDirection.Top;
        //public SlideDirection TextSlidesFrom {
        //    get { return _textSlidesFrom; }
        //    set
        //    {
        //        if (value != _textSlidesFrom)
        //        {
        //            _textSlidesFrom = value;
        //            CreateStoryBoard();
        //        }
        //    }
        //}

        /// <summary>
        /// Occurs when the tile is clicked.
        /// </summary>
        public event RoutedEventHandler Click;
    }

    public enum SlideDirection
    {
        Top,
        Bottom,
        Left,
        Right
    }
}
