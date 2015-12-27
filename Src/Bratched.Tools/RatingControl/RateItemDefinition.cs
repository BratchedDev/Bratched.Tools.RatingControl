using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#if NETFX_CORE
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
#endif
#if WINDOWS_PHONE
using System.Windows;
using System.Windows.Media;
using System.Windows.Data;
#endif

namespace Bratched.Tools.RatingControl
{
    public partial class RateItemDefinition : FrameworkElement, IRateItemDefinition//  DependencyObject //
    {
        public RateItemDefinition()
        {
            
        }


        public static RatingControl ParentRatingControl { get; set; }

        /// <summary>
        /// Background Color for the rate item
        /// </summary>
        public SolidColorBrush BackgroundColor
        {
            get { return (SolidColorBrush)GetValue(BackgroundColorProperty); }
            set { SetValue(BackgroundColorProperty, value); }
        }

        /// <summary>
        /// Outline Color for the rate item
        /// </summary>
        public SolidColorBrush OutlineColor
        {
            get { return (SolidColorBrush)GetValue(OutlineColorProperty); }
            set { SetValue(OutlineColorProperty, value); }
        }

        
        /// <summary>
        /// Outline thikness for the rate Item
        /// </summary>
        public object OutlineThickness
        {
            get { return GetValue(OutlineThicknessProperty); }
            set { SetValue(OutlineThicknessProperty, value); }
        }
       
        /// <summary>
        /// defines rate aspect
        /// </summary>
        public object PathData
        {
            get { return GetValue(PathDataProperty); }
            set { SetValue(PathDataProperty, value); }                        
        }

        /// <summary>
        /// Generate event change and redraw RatingControl
        /// </summary>
        /// <param name="d">define rate element</param>
        /// <param name="e">value property event </param>
        private static void AspectChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is Binding && ParentRatingControl != null)
                BindingOperations.SetBinding(d, BackgroundColorProperty, (Binding)e.NewValue);
            if (ParentRatingControl != null)
                ParentRatingControl.GenerateItems();
        }


        private static void AspectChanged2(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is Binding && ParentRatingControl != null)
                BindingOperations.SetBinding(d, OutlineColorProperty, (Binding)e.NewValue);
            if (ParentRatingControl != null)
                ParentRatingControl.GenerateItems();
        }

        /// <summary>
        /// Generate event change for OutlineThicknessProperty and redraw RatingControl
        /// </summary>
        /// <param name="d">define rate element</param>
        /// <param name="e">value property event </param>
        private static void OutlineThicknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            
            if (e.NewValue is Binding && ParentRatingControl != null)
                BindingOperations.SetBinding(d, OutlineThicknessProperty, (Binding)e.NewValue);
            if (e.OldValue != e.NewValue && ParentRatingControl != null)
                ParentRatingControl.GenerateItems();
        }

        /// <summary>
        /// Generate event change for PathDataProperty and redraw RatingControl
        /// </summary>
        /// <param name="d">define rate element</param>
        /// <param name="e">value property event </param>
        private static void PathDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
           
            if (e.NewValue is Binding)
                BindingOperations.SetBinding(d, PathDataProperty, (Binding)e.NewValue);
            if (e.OldValue != e.NewValue && ParentRatingControl != null)
                ParentRatingControl.GenerateItems();
        }
        
        public static readonly DependencyProperty BackgroundColorProperty =
        DependencyProperty.RegisterAttached("BackgroundColor", typeof(SolidColorBrush), typeof(RateItemDefinition),
        new PropertyMetadata(null, AspectChanged));

        public static readonly DependencyProperty OutlineColorProperty =
        DependencyProperty.RegisterAttached("OutlineColor", typeof(SolidColorBrush), typeof(RateItemDefinition),
        new PropertyMetadata(null, AspectChanged2));

    //    public static readonly DependencyProperty OutlineThicknessProperty =
    //       DependencyProperty.Register("OutlineThickness", typeof(object), typeof(RatingControl),
    //       new PropertyMetadata(null, OutlineThicknessChanged));

    //    public static readonly DependencyProperty PathDataProperty =
    //     DependencyProperty.RegisterAttached("PathData", typeof(object), typeof(RatingControl),
    //      new PropertyMetadata(null, PathDataChanged));
    
          public static readonly DependencyProperty OutlineThicknessProperty =
           DependencyProperty.Register("OutlineThickness", typeof(object), typeof(RateItemDefinition),
           new PropertyMetadata(null, OutlineThicknessChanged));

          public static readonly DependencyProperty PathDataProperty =
           DependencyProperty.RegisterAttached("PathData", typeof(object), typeof(RateItemDefinition),
            new PropertyMetadata(null, PathDataChanged));
    
        

    }
}
