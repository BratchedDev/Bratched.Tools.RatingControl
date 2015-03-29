using System;
using System.Windows;
using System.Windows.Data;


namespace Bratched.Tools.RatingControl
{

    /// <summary>
    /// Provides subscription to DataContext changes.
    /// Very thanks to Charles Strahan
    /// </summary>

    public class DataContextChangedHelper
    {
        /// <summary>
        /// Subscribes the given callback to any changes made to the specified FrameworkElement's DataContext.
        /// </summary>
        /// <param name="element">The FrameworkElement to monitor.</param>
        /// <param name="callback">The callback to use when the DataContext changes.</param>

        public static void Subscribe(FrameworkElement element, PropertyChangedCallback callback)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            else if (callback == null)
            {
                throw new ArgumentNullException("callback");
            }

            // Compose this callback with the others
            PropertyChangedCallback callbacks = (PropertyChangedCallback)element.GetValue(DataContextChangedHelper.CallbacksProperty);
            callbacks += callback;
            element.SetValue(DataContextChangedHelper.CallbacksProperty, callbacks);
            // If the we haven't bound element.DataContext to DataContextChangedHelper.DataContextProperty, do so now
            if (element.GetBindingExpression(DataContextProperty) == null)
            {
                element.SetBinding(DataContextChangedHelper.DataContextProperty, new Binding());
            }
        }

        /// <summary>
        /// Unsubscribes the given callback to any changes made to the specified FrameworkElement's DataContext.
        /// </summary>
        /// <param name="element">The FrameworkElement to monitor.</param>
        /// <param name="callback">The callback to use when the DataContext changes.</param>
        public static void Unsubscribe(FrameworkElement element, PropertyChangedCallback callback)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            else if (callback == null)
            {
                throw new ArgumentNullException("callback");
            }

            // Remove this callback from the others
            PropertyChangedCallback callbacks = (PropertyChangedCallback)element.GetValue(DataContextChangedHelper.CallbacksProperty);
            callbacks -= callback;
            element.SetValue(DataContextChangedHelper.CallbacksProperty, callbacks);
        }

        private static void DataContextChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            // Get the callbacks for this FrameworkElement
            FrameworkElement element = sender as FrameworkElement;
            PropertyChangedCallback callbacks = (PropertyChangedCallback)element.GetValue(CallbacksProperty);

            // Invoke the callbacks
            callbacks(sender, e);
        }


        // We'll bind this attached DP to the DataContext(s) we want to monitor for changes
        private static readonly DependencyProperty DataContextProperty =
            DependencyProperty.RegisterAttached("DataContextProperty",
                                                 typeof(object),
                                                 typeof(FrameworkElement),
                                                 new PropertyMetadata(new PropertyChangedCallback(DataContextChanged)));

        // We'll use this to keep track of each set of callbacks
        private static readonly DependencyProperty CallbacksProperty =
            DependencyProperty.RegisterAttached("CallbacksProperty",
                                                 typeof(PropertyChangedCallback),
                                                 typeof(FrameworkElement),
                                                 new PropertyMetadata((object)((PropertyChangedCallback)((DependencyObject sender, DependencyPropertyChangedEventArgs e) => { }))));  // We'll use this to store all of the callbacks
    }
}
