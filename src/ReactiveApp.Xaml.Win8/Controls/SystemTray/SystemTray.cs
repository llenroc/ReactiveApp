using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;

namespace ReactiveApp.Xaml.Controls
{
    [TemplatePart(Name = PART_ProgressIndicator, Type = typeof(ProgressBar))]
    [TemplatePart(Name = PART_Text, Type = typeof(TextBlock))]
    [TemplatePart(Name = PART_Time, Type = typeof(TextBlock))]
    public class SystemTray : Control
    {
        private const string PART_ProgressIndicator = "PART_ProgressIndicator";
        private const string PART_Text = "PART_Text";
        private const string PART_Time = "PART_Time";

        private ProgressBar progressIndicator;
        private TextBlock text;
        private TextBlock time;

        private DispatcherTimer timer;

        #region Dependency Properties        

        #region Text (Dependency Property)

        /// <summary>
        /// Using a DependencyProperty as the backing store for  Text.  This enables animation, styling, binding, etc...    
        /// </summary>
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(
                "Text",
                typeof(string),
                typeof(SystemTray),
                new PropertyMetadata(null)
            );

        /// <summary>
        /// Text showb in the systemtray.
        /// </summary>
        public string Text
        {
            get { return (string)this.GetValue(TextProperty); }
            set { this.SetValue(TextProperty, value); }
        }

        #endregion        

        #region ProgressIndicatorForeground (Dependency Property)

        /// <summary>
        /// Using a DependencyProperty as the backing store for  ProgressIndicatorForeground.  This enables animation, styling, binding, etc...    
        /// </summary>
        public static readonly DependencyProperty ProgressIndicatorForegroundProperty =
            DependencyProperty.Register(
                "ProgressIndicatorForeground",
                typeof(Brush),
                typeof(SystemTray),
                new PropertyMetadata(new SolidColorBrush(Colors.Black))
            );

        /// <summary>
        /// Brush for the ProgressIndicator.
        /// </summary>
        public Brush ProgressIndicatorForeground
        {
            get { return (Brush)this.GetValue(ProgressIndicatorForegroundProperty); }
            set { this.SetValue(ProgressIndicatorForegroundProperty, value); }
        }

        #endregion        

        #region ProgressIndicatorIsIndeterminate (Dependency Property)

        /// <summary>
        /// Using a DependencyProperty as the backing store for  ProgressIndicatorIsIndeterminate.  This enables animation, styling, binding, etc...    
        /// </summary>
        public static readonly DependencyProperty ProgressIndicatorIsIndeterminateProperty =
            DependencyProperty.Register(
                "ProgressIndicatorIsIndeterminate",
                typeof(bool),
                typeof(SystemTray),
                new PropertyMetadata(false)
            );

        /// <summary>
        /// Indicates if the ProgressIndicator is indeterminate.
        /// </summary>
        public bool ProgressIndicatorIsIndeterminate
        {
            get { return (bool)this.GetValue(ProgressIndicatorIsIndeterminateProperty); }
            set { this.SetValue(ProgressIndicatorIsIndeterminateProperty, value); }
        }

        #endregion
        
        #region ProgressIndicatorVisibility (Dependency Property)

        /// <summary>
        /// Using a DependencyProperty as the backing store for  ProgressIndicatorVisibility.  This enables animation, styling, binding, etc...    
        /// </summary>
        public static readonly DependencyProperty ProgressIndicatorVisibilityProperty =
            DependencyProperty.Register(
                "ProgressIndicatorVisibility",
                typeof(Visibility),
                typeof(SystemTray),
                new PropertyMetadata(Visibility.Collapsed)
            );

        /// <summary>
        /// Visibility of the ProgressIndicator.
        /// </summary>
        public Visibility ProgressIndicatorVisibility
        {
            get { return (Visibility)this.GetValue(ProgressIndicatorVisibilityProperty); }
            set { this.SetValue(ProgressIndicatorVisibilityProperty, value); }
        }

        #endregion        

        #region ProgressIndicatorValue (Dependency Property)

        /// <summary>
        /// Using a DependencyProperty as the backing store for  ProgressIndicatorValue.  This enables animation, styling, binding, etc...    
        /// </summary>
        public static readonly DependencyProperty ProgressIndicatorValueProperty =
            DependencyProperty.Register(
                "ProgressIndicatorValue",
                typeof(double),
                typeof(SystemTray),
                new PropertyMetadata(0.0)
            );

        /// <summary>
        /// Value of the ProgressIndicator.
        /// </summary>
        public double ProgressIndicatorValue
        {
            get { return (double)this.GetValue(ProgressIndicatorValueProperty); }
            set { this.SetValue(ProgressIndicatorValueProperty, value); }
        }

        #endregion        

        #region TrayOpacity (Dependency Property)

        /// <summary>
        /// Using a DependencyProperty as the backing store for  TrayOpacity.  This enables animation, styling, binding, etc...    
        /// </summary>
        public static readonly DependencyProperty TrayOpacityProperty =
            DependencyProperty.Register(
                "TrayOpacity",
                typeof(double),
                typeof(SystemTray),
                new PropertyMetadata(0.0)
            );

        /// <summary>
        /// Opacity of the system tray.
        /// </summary>
        public double TrayOpacity
        {
            get { return (double)this.GetValue(TrayOpacityProperty); }
            set { this.SetValue(TrayOpacityProperty, value); }
        }

        #endregion                

        #endregion

        public SystemTray()
        {
            this.DefaultStyleKey = typeof(SystemTray);
            this.timer = new DispatcherTimer();
            this.timer.Interval = TimeSpan.FromMilliseconds(500);
            
            this.Loaded += SystemTray_Loaded;
            this.Unloaded += SystemTray_Unloaded;

            this.Width = Window.Current.Bounds.Width;
            this.Height = 28;
        }

        void SystemTray_Loaded(object sender, RoutedEventArgs e)
        {
            Window.Current.SizeChanged += Current_SizeChanged;
            this.timer.Tick += timer_Tick;
            if (!this.timer.IsEnabled)
            {
                this.timer.Start();
            }
        }

        void SystemTray_Unloaded(object sender, RoutedEventArgs e)
        {
            Window.Current.SizeChanged -= Current_SizeChanged;
            this.timer.Tick -= timer_Tick; 
            if(this.timer.IsEnabled)
            {
                this.timer.Stop();              
            }
        }

        void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            this.Width = e.Size.Width;
        }

        void timer_Tick(object sender, object e)
        {
            this.time.Text = DateTime.Now.ToString("H:mm");
        }

        /// <summary>
        /// Invoked whenever application code or internal processes (such as a rebuilding layout pass) call ApplyTemplate. In simplest terms, this means the method is called just before a UI element displays in your app. Override this method to influence the default post-template logic of a class.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">
        /// PART_ProgressIndicator is missing from the template.
        /// or
        /// PART_Text is missing from the template.
        /// or
        /// PART_Time is missing from the template.
        /// </exception>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.progressIndicator = base.GetTemplateChild(PART_ProgressIndicator) as ProgressBar;
            if(this.progressIndicator == null)
            {
                throw new InvalidOperationException("PART_ProgressIndicator is missing from the template.");
            }
            this.text = base.GetTemplateChild(PART_Text) as TextBlock;
            if (this.text == null)
            {
                throw new InvalidOperationException("PART_Text is missing from the template.");
            }
            this.text.Text = this.Text ?? string.Empty;
            this.time = base.GetTemplateChild(PART_Time) as TextBlock;
            if (this.time == null)
            {
                throw new InvalidOperationException("PART_Time is missing from the template.");
            }
            this.time.Text = DateTime.Now.ToString("H:mm");

            if (!this.timer.IsEnabled)
            {
                this.timer.Start();
            }
        }
    }
}
