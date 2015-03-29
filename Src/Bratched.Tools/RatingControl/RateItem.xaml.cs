using System;

#if NETFX_CORE
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Markup;
#endif
#if WINDOWS_PHONE
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Markup;
#endif

namespace Bratched.Tools.RatingControl
{
    public sealed partial class RateItem : UserControl
    {
        private const Int32 RATEITEM_SIZE = 100;

        public RateItem()
        {           
            InitializeComponent();
            Width = RATEITEM_SIZE;
            Height = RATEITEM_SIZE;
        }

        private double _value;
        private SolidColorBrush _fullBackgroundColor;
        private SolidColorBrush _emptyBackgroundColor;
        private SolidColorBrush _fullOutlineColor;
        private SolidColorBrush _emptyOutlineColor;
        private double _fullOutlineThikness;
        private double _emptyOutlineThikness;
        private string _fullPathData;
        private string _emptyPathData;
        /// <summary>
        /// Value of the rate item must be between 0 and 1.
        /// </summary>
        public double Value
        {
            get { return _value; } 
            set 
            { 
                _value = value;
                RectRateFull.Rect = new Rect(0, 0, Width * value, Height);
            }
        }

        /// <summary>
        /// Backgroud Color of the full rate item
        /// </summary>
        public SolidColorBrush FullBackgroundColor
        {
            get { return _fullBackgroundColor; }
            set 
            { 
                _fullBackgroundColor = value;
                rateFull.Fill = value; 
            }
        }

        /// <summary>
        /// Background Color of the empty rate item
        /// </summary>
        public SolidColorBrush EmptyBackgroundColor
        {
            get { return _emptyBackgroundColor; }
            set 
            { 
                _emptyBackgroundColor = value;
                rateEmpty.Fill = value;
            }
        }

        /// <summary>
        /// Outline color of the full rate item
        /// </summary>
        public SolidColorBrush FullOutlineColor
        {
            get { return _fullOutlineColor; }
            set 
            { 
                _fullOutlineColor = value;
                rateFull.Stroke = value;
            }                   
        }

        /// <summary>
        /// Outline color of the empty rate item
        /// </summary>
        public SolidColorBrush EmptyOutlineColor
        {
            get { return _emptyOutlineColor; }
            set 
            {
                _emptyOutlineColor = value;
                rateEmpty.Stroke = value; 
            }
        }

        /// <summary>
        /// Outline stroke thikness of the full rate item
        /// </summary>
        public double FullOutlineThikness
        {
            get { return _fullOutlineThikness; }
            set 
            {
                _fullOutlineThikness = value;
                rateFull.StrokeThickness = value;      
            }
        }

        /// <summary>
        /// Outline stroke thikness of the empty rate item
        /// </summary>
        public double EmptyOutlineThikness
        {
            get { return _emptyOutlineThikness; }
            set
            { 
                _emptyOutlineThikness = value;
               rateEmpty.StrokeThickness = value;      
            }
        }
        
        /// <summary>
        /// Data Geometry Path for the full rate item
        /// </summary>
        public string FullPathData
        {
            get { return _fullPathData; }
            set 
            { 
                _fullPathData = value;
                rateFull.Data = StringToPath(value);              
            }                            
        }

        /// <summary>
        /// Data Geometry Path for the empty rate item
        /// </summary>
        public string EmptyPathData
        {
            get { return _emptyPathData; }
            set 
            { 
                _emptyPathData = value;
                rateEmpty.Data = StringToPath(value);       
            }
        }

        /// <summary>
        /// Convert string Data Path to Geometrey Data Path
        /// </summary>
        /// <param name="pathData">string data path</param>
        /// <returns></returns>
        private static Geometry StringToPath(string pathData)
        {
            if (String.IsNullOrEmpty(pathData)) return null;
            string xamlPath =
                "<Geometry xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'>"
                + pathData + "</Geometry>";
            return XamlReader.Load(xamlPath) as Geometry;

            //return Windows.UI.Xaml.Markup.XamlReader.Load(xamlPath) as Geometry;
        }

        //private static void ValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    //RateItem rateItem = d as RateItem;
        //    //rateItem.RectRateFull.Rect = new Rect(0, 0, rateItem.Width * (double)e.NewValue, rateItem.Height);
        //}

        //private static void FullBackgroundColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    (d as RateItem).rateFull.Fill = (SolidColorBrush)e.NewValue;
        //}

        //private static void EmptyBackgroundColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    (d as RateItem).rateEmpty.Fill = (SolidColorBrush)e.NewValue;
        //}

        //private static void FullOutlineColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    (d as RateItem).rateFull.Stroke = (SolidColorBrush)e.NewValue;
        //}

        //private static void EmptyOutlineColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    (d as RateItem).rateEmpty.Stroke = (SolidColorBrush)e.NewValue; 
        //}

        //private static void FullOutlineThiknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    (d as RateItem).rateFull.StrokeThickness = (double)e.NewValue;              
        //}

        //private static void EmptyOutlineThiknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    (d as RateItem).rateEmpty.StrokeThickness = (double)e.NewValue;      
        //}

        //private static void FullPathDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    (d as RateItem).rateFull.Data = StringToPath((string)e.NewValue);              
        //}        

        //private static void EmptyPathDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    (d as RateItem).rateEmpty.Data = StringToPath((string)e.NewValue);       
        //}      

        //public static readonly DependencyProperty ValueProperty = 
        //    DependencyProperty.Register("Value", typeof(double), typeof(RateItem),
        //    new PropertyMetadata(null, ValuePropertyChanged));

        //public static readonly DependencyProperty FullBackgroundColorProperty =
        //    DependencyProperty.Register("FullBackgroundColor", typeof(SolidColorBrush), typeof(RateItem),
        //    new PropertyMetadata(null, FullBackgroundColorChanged));

        //public static readonly DependencyProperty EmptyBackgroundColorProperty =
        //   DependencyProperty.Register("EmptyBackgroundColor", typeof(SolidColorBrush), typeof(RateItem),
        //   new PropertyMetadata(null, EmptyBackgroundColorChanged));

        //public static readonly DependencyProperty EmptyOutlineColorProperty =
        //   DependencyProperty.Register("EmptyOutlineColor", typeof(SolidColorBrush), typeof(RateItem),
        //   new PropertyMetadata(null, EmptyOutlineColorChanged));

        //public static readonly DependencyProperty FullOutlineColorProperty =
        //   DependencyProperty.Register("FullOutlineColor", typeof(SolidColorBrush), typeof(RateItem),
        //   new PropertyMetadata(null, FullOutlineColorChanged));

        //public static readonly DependencyProperty FullOutlineThiknessProperty =
        //    DependencyProperty.Register("FullOutlineThikness", typeof(double), typeof(RateItem),
        //    new PropertyMetadata(null, FullOutlineThiknessChanged));

        //public static readonly DependencyProperty EmptyOutlineThiknessProperty =
        //    DependencyProperty.Register("EmptyOutlineThikness", typeof(double), typeof(RateItem),
        //    new PropertyMetadata(null, EmptyOutlineThiknessChanged));

        //public static readonly DependencyProperty FullPathDataProperty =
        //  DependencyProperty.Register("FullPathData", typeof(string), typeof(RateItem), 
        //  new PropertyMetadata(null, FullPathDataChanged));

        //public static readonly DependencyProperty EmptyPathDataProperty =
        //  DependencyProperty.Register("EmptyPathData", typeof(string), typeof(RateItem), 
        //  new PropertyMetadata(null, EmptyPathDataChanged));
    }
}
