using System;
using System.Linq;
using System.Collections.Generic;

#if NETFX_CORE
using Windows.UI;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Input;
using Windows.Foundation;
#endif
#if WINDOWS_PHONE
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Data;
#endif

namespace Bratched.Tools.RatingControl
{
    public enum Templates { Star, Heart, Smiley, Trophy, Like, None};
    public enum DefinitionCycles { Repeat, Linear }

    public partial class RatingControl : UserControl
    {
        private const bool DEBUG_MODE = false;

        public RatingControl()
        {
            EmptyItemsDefinition = new List<IRateItemDefinition>();
            FullItemsDefinition = new List<IRateItemDefinition>();
            InitializeComponent();

#if WINDOWS_PHONE
            DataContextChangedHelper.Subscribe(this, DataContextChanged);
            gridCaptureMovement.ManipulationDelta += RatingControl_ManipulationDelta;
            gridCaptureMovement.MouseLeftButtonDown += RatingControl_MouseLeftButtonDown;            
#endif

#if NETFX_CORE
            this.DataContextChanged += RatingControl_DataContextChanged;
            gridCaptureMovement.PointerMoved += gridRating_PointerMoved;
            gridCaptureMovement.ManipulationDelta += GridCaptureMovement_ManipulationDelta;
            
#endif

            this.Loaded += uc_Loaded;   
            InitDefaultValues();            
        }

      

        private void ChangeItemsValue(double x)
        {
            if (rateItems.Children.Any())
                Value = RoundSliced(x * ItemsCount / rateItems.ActualWidth);
            System.Diagnostics.Debug.WriteLine(String.Format("X={2}, New Value {0}, ActualWidth {1}, ", Value, rateItems.ActualWidth, x));
        }

#if NETFX_CORE
        void RatingControl_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            ChangeDataContextInDefinitions(sender);
        }
#endif
#if WINDOWS_PHONE
        
        private static void DataContextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as RatingControl).ChangeDataContextInDefinitions(d as FrameworkElement);
        }
#endif

        /// <summary>
        /// Apply DataContext to Definitions Children.
        /// </summary>
        /// <param name="sender"></param>
        private void ChangeDataContextInDefinitions(FrameworkElement sender)
        {
            foreach (IRateItemDefinition i in FullItemsDefinition)
                if (i is RateItemDefinition)
                    (i as RateItemDefinition).DataContext = sender.DataContext;
            foreach (IRateItemDefinition i in EmptyItemsDefinition)
                if (i is RateItemDefinition)
                    (i as RateItemDefinition).DataContext = sender.DataContext;
        }

        

        private void InitDefaultValues()
        {
            ItemsCount = 5;
            ItemsSpacing = 4;
            Padding = new Thickness(0);
            Margin = new Thickness(0);
            Background = new SolidColorBrush(Colors.Transparent);
            IsEditable = false;
            Value = 0;
        }

        /// <summary>
        /// Value of the Rate Control
        /// </summary>        
        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        /// <summary>
        /// Template of the Rate items, templates can be overrided by Definitions
        /// </summary>
        public Templates ItemTemplate
        {
            get { return (Templates)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        public DefinitionCycles DefinitionCycle
        {
            get { return (DefinitionCycles)GetValue(DefinitionCycleProperty); }
            set { SetValue(DefinitionCycleProperty, value); }
        }

        /// <summary>
        /// Spacing beetween Rate items
        /// </summary>
        public double ItemsSpacing
        {
            get { return (double)GetValue(ItemsSpacingProperty);  }
            set { SetValue(ItemsSpacingProperty, value); }          
        }

        /// <summary>
        /// Number of rate items
        /// </summary>
        public Int32 ItemsCount
        {
            get { return (Int32)GetValue(ItemsCountProperty); }
            set { SetValue(ItemsCountProperty, value); }
        }

        /// <summary>
        /// Rate items Définitions for the Full items aspect
        /// </summary>
       
        public List<IRateItemDefinition> FullItemsDefinition
        {
        
            get { return (List<IRateItemDefinition>)GetValue(FullItemsDefinitionProperty); }
            set { SetValue(FullItemsDefinitionProperty, value); }            
        }

        /// <summary>
        /// Rate items Définitions for the empty items aspect
        /// </summary>

        public List<IRateItemDefinition> EmptyItemsDefinition
        {
           
            get { return (List<IRateItemDefinition>)GetValue(EmptyItemsDefinitionProperty); }
            set { SetValue(EmptyItemsDefinitionProperty, value); }
        }


        public double RoundValueSlice
        {
            get { return (double)GetValue(RoundValueSliceProperty); }
            set { SetValue(RoundValueSliceProperty, value); }
        }

        private bool _isGenerating = false;
        /// <summary>
        /// UI rate Items elements
        /// </summary>
        public StackPanel RateItems { get { return rateItems; } }

        /// <summary>
        ///  Generate Items
        /// </summary>
        public void GenerateItems()
        {
            if (!_isLoaded || rateItems == null || _isGenerating) return;  
            _isGenerating = true;
            try
            {
                double localValue = Value;
                rateItems.Children.Clear();
                List<IRateItemDefinition> defaultFull = new List<IRateItemDefinition>();
                List<IRateItemDefinition> defaultEmpty = new List<IRateItemDefinition>();
                InitDefaultTemplate(defaultFull, defaultEmpty, ItemTemplate);
                for (int i = 0; i < ItemsCount; i++)
                {
                    localValue = GenerateRateItem(localValue, defaultFull, defaultEmpty, i);
                }
            }
            finally
            {
                _isGenerating = false;
            }
        }

        public void RefreshRateValues()
        {
            if (!rateItems.Children.Any() || Value == Double.MinValue)
                GenerateItems();
            _isGenerating = true;
            try
            {
                double localValue = RoundValue;
                foreach (RateItem item in rateItems.Children)
                {
                    if (localValue > 1)
                    item.Value = 1.0;
                    else
                        if (localValue > 0)
                            item.Value = localValue;
                        else
                            item.Value = 0.0;
                    localValue -= 1;
                }
            }
            finally
            {
                _isGenerating = false;
            }
        }

        private double RoundSliced(double value)
        {
            if (ItemsCount <= 0) return value;
            double tempValue = value;
            if (RoundValueSlice > 0)                
            {
                tempValue = value - (value % RoundValueSlice);
                tempValue = tempValue + (((value % RoundValueSlice) > (RoundValueSlice / 2)) ? RoundValueSlice : 0); 
            }
            return Math.Max(Math.Min(tempValue, ItemsCount), 0);
        }


        private IRateItemDefinition GetDefinitionFromIndex(List<IRateItemDefinition> definitions, int index)
        {
            int count = definitions.Count();
            if (count == 1)
                return definitions.First();
            if (DefinitionCycle == DefinitionCycles.Linear)
                return count > 0 ? definitions[ index * count / ItemsCount] : null;
            else
                return count > 0 ? definitions[index % count] : null;            
        }

        private double GenerateRateItem(double localValue, List<IRateItemDefinition> defaultFull, List<IRateItemDefinition> defaultEmpty, int i)
        {
            if (localValue == double.MinValue) return localValue;
            RateItem item = new RateItem();
            item.Margin = i > 0 ? new Thickness(ItemsSpacing, 0, 0, 0) : new Thickness(0);

            IRateItemDefinition fullDefaultDefinition = GetDefinitionFromIndex(defaultFull, i);
            IRateItemDefinition fullItemDefinition = GetDefinitionFromIndex(FullItemsDefinition, i); 
            item.FullBackgroundColor = fullItemDefinition != null && fullItemDefinition.BackgroundColor != null ? fullItemDefinition.BackgroundColor : fullDefaultDefinition.BackgroundColor;
            item.FullOutlineColor = fullItemDefinition != null && fullItemDefinition.OutlineColor != null ? fullItemDefinition.OutlineColor : fullDefaultDefinition.OutlineColor;
            item.FullOutlineThikness = fullItemDefinition != null && fullItemDefinition.OutlineThikness is Double ? Convert.ToDouble(fullItemDefinition.OutlineThikness) : Convert.ToDouble(fullDefaultDefinition.OutlineThikness);
            item.FullPathData = fullItemDefinition != null && fullItemDefinition.PathData is String ? Convert.ToString(fullItemDefinition.PathData) : Convert.ToString(fullDefaultDefinition.PathData);

            IRateItemDefinition emptyDefaultDefinition = GetDefinitionFromIndex(defaultEmpty, i);
            IRateItemDefinition emptyItemDefinition = GetDefinitionFromIndex(EmptyItemsDefinition, i); 
            item.EmptyBackgroundColor = emptyItemDefinition != null && emptyItemDefinition.BackgroundColor != null ? emptyItemDefinition.BackgroundColor : emptyDefaultDefinition.BackgroundColor;
            item.EmptyOutlineColor = emptyItemDefinition != null && emptyItemDefinition.OutlineColor != null ? emptyItemDefinition.OutlineColor : emptyDefaultDefinition.OutlineColor;
            item.EmptyOutlineThikness = emptyItemDefinition != null && emptyItemDefinition.OutlineThikness is Double ? Convert.ToDouble(emptyItemDefinition.OutlineThikness) : Convert.ToDouble(emptyDefaultDefinition.OutlineThikness);
            item.EmptyPathData = emptyItemDefinition != null && emptyItemDefinition.PathData is String ? Convert.ToString(emptyItemDefinition.PathData) : Convert.ToString(emptyDefaultDefinition.PathData);

            if (localValue > 1)
                item.Value = 1.0;
            else
                if (localValue > 0)
                    item.Value = localValue;
                else
                    item.Value = 0.0;
            localValue -= 1.0;
            rateItems.Children.Add(item);
            return localValue;
        }

        private void InitDefaultTemplate(List<IRateItemDefinition> defaultFull, List<IRateItemDefinition> defaultEmpty, Templates itemTemplate)
        {
            defaultFull.Clear();
            defaultEmpty.Clear();
            switch (itemTemplate)
            {
                case Templates.Star: InitTemplateStar(defaultFull, defaultEmpty); break;
                case Templates.Smiley: InitTemplateSmiley(defaultFull, defaultEmpty); break;
                case Templates.Heart: InitTemplateHeart(defaultFull, defaultEmpty); break;
                case Templates.Trophy: InitTemplateTrophy(defaultFull, defaultEmpty); break;
                case Templates.Like: InitTemplateLike(defaultFull, defaultEmpty); break;
            }
        }

        private void InitTemplateLike(List<IRateItemDefinition> defaultFull, List<IRateItemDefinition> defaultEmpty)
        {
            // 
            defaultFull.Add(new RateItemDefinitionModel
            {
                BackgroundColor = new SolidColorBrush(Colors.White),
                OutlineColor = new SolidColorBrush(Colors.Blue),
                OutlineThikness = 5,
                PathData = "m 119 427 9 0 c 1 3 -0 7 -4 7 l -5 0 c -4 0 -4 -7 -8.5e-4 -7 z m -2 16 8 0 c 4 0 5 -4 4 -7 l -12 0 c -4 0 -4 7 0 7 z m 12 3 -12 0 c -4 0 -4 7 0 7 l 7 0 c 4 -1.4e-4 5 -4 4 -7 z m -7 16 c 4 0 5 -4 5 -7 0 0 -9 0 -9 0 -4 0 -4 7 0 7 l 3 0 z m -8 -8 c -4 -2 -4 -7 -1 -10 -4 -3 -3 -9 2 -10 -2 -2 -1 -5 -0 -8 -1 -0 -2 -1 -2 -1 3 -8 3 -19 -1 -21 -4 -1 -5 4 -6 7 -3 8 -10 20 -26 20 0 0 0 17 0 24 19 0 24 7 32 8 1 0 1 0 2 0 -2 -2 -2 -6 1 -8 z"
            });
            defaultEmpty.Add(new RateItemDefinitionModel
            {
                BackgroundColor = new SolidColorBrush(Colors.Transparent),
                OutlineColor = new SolidColorBrush(Colors.White),
                OutlineThikness = 2,
                PathData = "m 119 427 9 0 c 1 3 -0 7 -4 7 l -5 0 c -4 0 -4 -7 -8.5e-4 -7 z m -2 16 8 0 c 4 0 5 -4 4 -7 l -12 0 c -4 0 -4 7 0 7 z m 12 3 -12 0 c -4 0 -4 7 0 7 l 7 0 c 4 -1.4e-4 5 -4 4 -7 z m -7 16 c 4 0 5 -4 5 -7 0 0 -9 0 -9 0 -4 0 -4 7 0 7 l 3 0 z m -8 -8 c -4 -2 -4 -7 -1 -10 -4 -3 -3 -9 2 -10 -2 -2 -1 -5 -0 -8 -1 -0 -2 -1 -2 -1 3 -8 3 -19 -1 -21 -4 -1 -5 4 -6 7 -3 8 -10 20 -26 20 0 0 0 17 0 24 19 0 24 7 32 8 1 0 1 0 2 0 -2 -2 -2 -6 1 -8 z"
            });
        }

        private void InitTemplateTrophy(List<IRateItemDefinition> defaultFull, List<IRateItemDefinition> defaultEmpty)
        {
            defaultFull.Add(new RateItemDefinitionModel
            {
                BackgroundColor = new SolidColorBrush(Colors.Yellow),
                OutlineColor = new SolidColorBrush(Colors.Orange),
                OutlineThikness = 1,
                PathData = "M 78 427 C 79 421 89 415 89 395 l -29 0 c 0 20 10 26 11 33 l 6 0 z m -9 -23 4 -1 2 -4 2 4 4 1 -3 3 1 4 L 75 410 l -4 2 1 -4 -3 -3 z m -8 8 c 1 2 1 3 2 4 C 55 414 51 406 50 398 l 8 0 c 0 1 0 2 0 3 l -5 0 c 1 4 3 9 8 12 z m 23 24 0 3 -18 0 0 -3 c 5 0 6 -4 6 -6 0 0 6 0 6 0 0 3 1 6 6 6 z M 100 398 c -1 8 -5 16 -13 19 1 -1 1 -3 2 -4 5 -3 7 -8 8 -12 l -5 0 c 0 -1 0 -2 0 -3 l 8 0 z"
            });
            defaultEmpty.Add(new RateItemDefinitionModel
            {
                BackgroundColor = new SolidColorBrush(Colors.Transparent),
                OutlineColor = new SolidColorBrush(Colors.Orange),
                OutlineThikness = 2,
                PathData = "M 78 427 C 79 421 89 415 89 395 l -29 0 c 0 20 10 26 11 33 l 6 0 z m -9 -23 4 -1 2 -4 2 4 4 1 -3 3 1 4 L 75 410 l -4 2 1 -4 -3 -3 z m -8 8 c 1 2 1 3 2 4 C 55 414 51 406 50 398 l 8 0 c 0 1 0 2 0 3 l -5 0 c 1 4 3 9 8 12 z m 23 24 0 3 -18 0 0 -3 c 5 0 6 -4 6 -6 0 0 6 0 6 0 0 3 1 6 6 6 z M 100 398 c -1 8 -5 16 -13 19 1 -1 1 -3 2 -4 5 -3 7 -8 8 -12 l -5 0 c 0 -1 0 -2 0 -3 l 8 0 z"
            });
        }

        private void InitTemplateHeart(List<IRateItemDefinition> defaultFull, List<IRateItemDefinition> defaultEmpty)
        {
            defaultFull.Add(new RateItemDefinitionModel
            {
                BackgroundColor = new SolidColorBrush(Colors.Red),
                OutlineColor = new SolidColorBrush(Color.FromArgb(255, 200, 0, 0)),
                OutlineThikness = 1,
                PathData = " m 99 409 c -5 -10 -20 -9 -24 -1 -4 -9 -19 -10 -24 1 -6 12 8 23 24 39 16 -16 29 -27 24 -39 z"
            });
            defaultEmpty.Add(new RateItemDefinitionModel
            {
                BackgroundColor = new SolidColorBrush(Colors.Transparent),
                OutlineColor = new SolidColorBrush(Colors.Red),
                OutlineThikness = 2,
                PathData = " m 99 409 c -5 -10 -20 -9 -24 -1 -4 -9 -19 -10 -24 1 -6 12 8 23 24 39 16 -16 29 -27 24 -39 z"
            });
           
        }

        private void InitTemplateStar(List<IRateItemDefinition> defaultFull, List<IRateItemDefinition> defaultEmpty)
        {
            defaultFull.Add(new RateItemDefinitionModel
            {
                BackgroundColor = new SolidColorBrush(Colors.Yellow),
                OutlineColor = new SolidColorBrush(Colors.Orange),
                OutlineThikness = 1,
                PathData = "M 5.5,0 L 4,4 L 0,4 L 3,7 L 2,11 L 5,9 L 6,9 L 9,11 L 8,7 L 11,4 L 7,4 L 5.5,0"
            });
            defaultEmpty.Add(new RateItemDefinitionModel
            {
                BackgroundColor = new SolidColorBrush(Colors.Transparent),
                OutlineColor = new SolidColorBrush(Colors.Orange),
                OutlineThikness = 2,
                PathData = "M 5.5,0 L 4,4 L 0,4 L 3,7 L 2,11 L 5,9 L 6,9 L 9,11 L 8,7 L 11,4 L 7,4 L 5.5,0"
            });
        }


        private void InitTemplateSmiley(List<IRateItemDefinition> defaultFull, List<IRateItemDefinition> defaultEmpty)
        {
            defaultFull.Add(new RateItemDefinitionModel
            {
                BackgroundColor = new SolidColorBrush(Colors.Red),
                OutlineColor = new SolidColorBrush(Color.FromArgb(255, 200, 0, 0)),
                OutlineThikness = 1,
                PathData = "m 75 412 c -14 0 -25 11 -25 25 0 14 11 25 25 25 14 0 25 -11 25 -25 0 -14 -11 -25 -25 -25 z m 8 15 c 2 0 4 2 4 4 0 2 -2 4 -4 4 -2 0 -4 -2 -4 -4 0 -2 2 -4 4 -4 z m -16 0 c 2 0 4 2 4 4 0 2 -2 4 -4 4 -2 0 -4 -2 -4 -4 0 -2 2 -4 4 -4 z m 20 22 c -5 -3 -8 -4 -12 -4 -5 0 -8 1 -12 4 l -1 -1 c 2 -4 8 -8 14 -8 6 0 11 4 14 8 l -1 1 z"
            });

            defaultFull.Add(new RateItemDefinitionModel
            {
                BackgroundColor = new SolidColorBrush(Colors.Yellow),
                OutlineColor = new SolidColorBrush(Colors.Orange),
                OutlineThikness = 1,
                PathData = "m 75 412 c -14 0 -25 11 -25 25 0 14 11 25 25 25 14 0 25 -11 25 -25 0 -14 -11 -25 -25 -25 z m -12 19 c 0 -2 2 -4 4 -4 2 0 4 2 4 4 0 2 -2 4 -4 4 -2 0 -4 -2 -4 -4 z m 23 16 -21 0 0 -5 21 0 0 5 z M 83 435 c -2 0 -4 -2 -4 -4 0 -2 2 -4 4 -4 2 0 4 2 4 4 0 2 -2 4 -4 4 z"
            });

            defaultFull.Add(new RateItemDefinitionModel
            {
                BackgroundColor = new SolidColorBrush(Color.FromArgb(255, 0, 200, 0)),
                OutlineColor = new SolidColorBrush(Colors.Green),
                OutlineThikness = 1,
                PathData = "m 75 412 c -14 0 -25 11 -25 25 0 14 11 25 25 25 14 0 25 -11 25 -25 0 -14 -11 -25 -25 -25 z m 8 15 c 2 0 4 2 4 4 0 2 -2 4 -4 4 -2 0 -4 -2 -4 -4 0 -2 2 -4 4 -4 z m -16 0 c 2 0 4 2 4 4 0 2 -2 4 -4 4 -2 0 -4 -2 -4 -4 0 -2 2 -4 4 -4 z m 8 23 c -7 0 -12 -4 -14 -9 l 1 -1 c 4 3 8 5 13 5 6 0 10 -2 13 -5 l 1 1 c -3 4 -8 9 -14 9 z"
            });


            defaultEmpty.Add(new RateItemDefinitionModel
            {
                BackgroundColor = new SolidColorBrush(Colors.Transparent),
                OutlineColor = new SolidColorBrush(Colors.Red),
                OutlineThikness = 2,
                PathData = "m 75 412 c -14 0 -25 11 -25 25 0 14 11 25 25 25 14 0 25 -11 25 -25 0 -14 -11 -25 -25 -25 z m 8 15 c 2 0 4 2 4 4 0 2 -2 4 -4 4 -2 0 -4 -2 -4 -4 0 -2 2 -4 4 -4 z m -16 0 c 2 0 4 2 4 4 0 2 -2 4 -4 4 -2 0 -4 -2 -4 -4 0 -2 2 -4 4 -4 z m 20 22 c -5 -3 -8 -4 -12 -4 -5 0 -8 1 -12 4 l -1 -1 c 2 -4 8 -8 14 -8 6 0 11 4 14 8 l -1 1 z"
            });

            defaultEmpty.Add(new RateItemDefinitionModel
            {
                BackgroundColor = new SolidColorBrush(Colors.Transparent),
                OutlineColor = new SolidColorBrush(Colors.Orange),
                OutlineThikness = 2,
                PathData = "m 75 412 c -14 0 -25 11 -25 25 0 14 11 25 25 25 14 0 25 -11 25 -25 0 -14 -11 -25 -25 -25 z m -12 19 c 0 -2 2 -4 4 -4 2 0 4 2 4 4 0 2 -2 4 -4 4 -2 0 -4 -2 -4 -4 z m 23 16 -21 0 0 -5 21 0 0 5 z M 83 435 c -2 0 -4 -2 -4 -4 0 -2 2 -4 4 -4 2 0 4 2 4 4 0 2 -2 4 -4 4 z"
            });

            defaultEmpty.Add(new RateItemDefinitionModel
            {
                BackgroundColor = new SolidColorBrush(Colors.Transparent),
                OutlineColor = new SolidColorBrush(Colors.Green),
                OutlineThikness = 2,
                PathData = "m 75 412 c -14 0 -25 11 -25 25 0 14 11 25 25 25 14 0 25 -11 25 -25 0 -14 -11 -25 -25 -25 z m 8 15 c 2 0 4 2 4 4 0 2 -2 4 -4 4 -2 0 -4 -2 -4 -4 0 -2 2 -4 4 -4 z m -16 0 c 2 0 4 2 4 4 0 2 -2 4 -4 4 -2 0 -4 -2 -4 -4 0 -2 2 -4 4 -4 z m 8 23 c -7 0 -12 -4 -14 -9 l 1 -1 c 4 3 8 5 13 5 6 0 10 -2 13 -5 l 1 1 c -3 4 -8 9 -14 9 z"
            });         
        }

        /// <summary>
        /// If True, value can be change by the user
        /// if False, control is ReadOnly
        /// </summary>
        public bool IsEditable
        {
            get { return (bool)GetValue(IsEditableProperty); }
            set { SetValue(IsEditableProperty, value); }
        }

        /// <summary>
        /// return RoundValue of Value if RoundValueSlice > 0
        /// </summary>
        public double RoundValue
        {
            get
            {
                return RoundSliced(Value);
            }
        }
        
        public static readonly  DependencyProperty ItemsCountProperty =
           DependencyProperty.Register("ItemsCount", typeof(Int32), typeof(RatingControl),
           new PropertyMetadata(Int32.MinValue, ItemsCountChanged));

        public static readonly DependencyProperty ItemsSpacingProperty =
            DependencyProperty.Register("ItemsSpacing", typeof(double), typeof(RatingControl),
            new PropertyMetadata(Double.MinValue, ItemsSpacingChanged));

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(RatingControl),
            new PropertyMetadata(Double.MinValue, ValueChanged));

        public static readonly DependencyProperty IsEditableProperty =
           DependencyProperty.Register("IsEditable", typeof(bool), typeof(RatingControl), null);

        public static readonly DependencyProperty ItemTemplateProperty =
          DependencyProperty.Register("ItemTemplate", typeof(Templates), typeof(RatingControl),
          new PropertyMetadata(Templates.Star, ItemTemplateChanged));      

        public static readonly DependencyProperty DefinitionCycleProperty =
          DependencyProperty.Register("DefinitionCycle", typeof(DefinitionCycles), typeof(RatingControl),
          new PropertyMetadata(DefinitionCycles.Linear, DefinitionCyclesChanged));

        public static readonly DependencyProperty RoundValueSliceProperty =
            DependencyProperty.Register("RoundValueSlice", typeof(double), typeof(RatingControl),
            new PropertyMetadata((double)0, RoundValueSlicePropertyChanged));

        public static readonly DependencyProperty FullItemsDefinitionProperty =
           DependencyProperty.Register("FullItemsDefinition",
           typeof(List<IRateItemDefinition>),
           typeof(RatingControl),
           new PropertyMetadata(null, FullItemsDefinitionCallback));

        public static readonly DependencyProperty EmptyItemsDefinitionProperty =
            DependencyProperty.Register("EmptyItemsDefinition",
            typeof(List<IRateItemDefinition>),
            typeof(RatingControl),
            new PropertyMetadata(null, EmptyItemsDefinitionCallback));

        private bool _isLoaded = false;



        private static void RoundValueSlicePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue != e.NewValue)
            {
                RatingControl control =  (d as RatingControl);
                control.Value = control.RoundSliced((double)e.NewValue);
                control.RefreshRateValues();
            }
        }
        
        private static void FullItemsDefinitionCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            RateItemDefinition.ParentRatingControl = d as RatingControl;
            if (e.OldValue != e.NewValue)
                (d as RatingControl).GenerateItems();          
        }

        private static void EmptyItemsDefinitionCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            RateItemDefinition.ParentRatingControl = d as RatingControl;
            if (e.OldValue != e.NewValue)
                (d as RatingControl).GenerateItems();
            
        } 

        private static void ValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            RatingControl control = (d as RatingControl);
            if (control._isLoaded)
            {
                control.RefreshRateValues();
            }
        }

        private static void ItemsCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue != e.NewValue)
                (d as RatingControl).GenerateItems();
        }

        private static void ItemsSpacingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
 	       if (e.OldValue != e.NewValue)
              (d as RatingControl).GenerateItems();  
        }

        private static void ItemTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue != e.NewValue)
                (d as RatingControl).GenerateItems();  
        }

        private static void DefinitionCyclesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue != e.NewValue)
                (d as RatingControl).GenerateItems();  
        }      
#if NETFX_CORE
        private void gridRating_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (IsEditable && IsEnabled && Visibility==Visibility.Visible && rateItems != null && rateItems.Children.Any())
            {
                e.Handled = true;                
                System.Diagnostics.Debug.WriteLine("PointerMoved {0}", DateTime.Now);   
                PointerPoint p = e.GetCurrentPoint(rateItems.Children.First());
                if (p != null && p.Position != null)
                    ChangeItemsValue(p.Position.X);

            }            
        }
#endif

#if NETFX_CORE
        private void GridCaptureMovement_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            if (IsEditable && IsEnabled && Visibility == Visibility.Visible && e != null && e.Position != null)
            {
                e.Handled = true;
                System.Diagnostics.Debug.WriteLine("ManipulationDelta {0} - {1}", DateTime.Now, e.Position.X);
                double x = e.Position.X;
                ChangeItemsValue(x);
            }
        }
#endif

#if WINDOWS_PHONE
        void RatingControl_ManipulationDelta(object sender, System.Windows.Input.ManipulationDeltaEventArgs e)
        {
            if (IsEditable && IsEnabled && Visibility == Visibility.Visible && e != null && e.ManipulationOrigin != null)
            {
                e.Handled = true;
                System.Diagnostics.Debug.WriteLine("ManipulationDelta {0} - {1}", DateTime.Now, e.ManipulationOrigin.X);
                double x = e.ManipulationOrigin.X;
                ChangeItemsValue(x);
            }
        }

        void RatingControl_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (IsEditable && IsEnabled && Visibility == Visibility.Visible && e != null && e.OriginalSource is UIElement)
            {
                e.Handled = true;
                Point p = e.GetPosition(e.OriginalSource as UIElement);
                if (p != null)
                {
                    ChangeItemsValue(p.X);
                    System.Diagnostics.Debug.WriteLine("PointerMoved {0} - {1}", DateTime.Now, p.X);
                }
            }
        }
#endif

        private void uc_Loaded(object sender, RoutedEventArgs e)
        {
            _isLoaded = true;
            GenerateItems();
        }
    }
}
