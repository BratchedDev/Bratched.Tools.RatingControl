using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Media;
using System.Windows;


namespace RatingControlDemo_wp7.ViewModel
{
    public class RatingControlExampleViewModel: INotifyPropertyChanged
    {
        private double _value;
        private double _spaces = 4;
        private Int32 _nb;
        private static UInt32[] uintColors =
        {
            0x00FFFFFF, 0x20FFFFFF,
          0xFFFFFF00,0xFFFFE135,0xFFFFFF66,0xFFF8DE7E,0xFF008000,0xFF008A00,
          0xFFADFF2F,0xFF00FF00,0xFF7FFF00,0xFF32CD32,0xFF00FF7F,0xFF90EE90,
          0xFF3CB371,0xFF00FA9A,0xFF808000,0xFF2E8B57,0xFFFF0000,0xFFFF4500,
          //0xFFFF8C00,0xFFFFA500,0xFFED2939,0xFF800000,0xFFA52A2A,0xFFD2691E,
          //0xFFFF7F50,0xFFDC143C,0xFFE9967A,0xFFFF1493,0xFFB22222,0xFFFF69B4,
          //0xFFCD5C5C,0xFFF08080,0xFFFFB6C1,0xFFFFA07A,0xFFFF00FF,0xFFC71585,
          //0xFFDA70D6,0xFFDB7093,0xFFFA8072,0xFFF4A460,0xFF000080,0xFF4B0082,
          //0xFF191970,0xFF0000FF,0xFF800080,0xFF8A2BE2,0xFF6495ED,0xFF00FFFF,
          //0xFF008B8B,0xFF483D8B,0xFF00BFFF,0xFF1E90FF,0xFFADD8E6,0xFF20B2AA,
          //0xFF87CEFA,0xFFB0C4DE,0xFF76608A,0xFF7B68EE,0xFF4169E1,0xFF6A5ACD,
          //0xFF708090,0xFF4682B4,0xFF008080,0xFF40E0D0,0xFFA9A9A9,0xFFD3D3D3,

          0x55FFFF00,0x55FFE135,0x55FFFF66,0x55F8DE7E,0x55008000,0x55008A00,
          0x55ADFF2F,0x5500FF00,0x557FFF00,0x5532CD32,0x5500FF7F,0x5590EE90,
          0x553CB371,0x5500FA9A,0x55808000,0x552E8B57,0x55FF0000,0x55FF4500,
          //0x55FF8C00,0x55FFA500,0x55ED2939,0x55800000,0x55A52A2A,0x55D2691E,
          //0x55FF7F50,0x55DC143C,0x55E9967A,0x55FF1493,0x55B22222,0x55FF69B4,
          //0x55CD5C5C,0x55F08080,0x55FFB6C1,0x55FFA07A,0x55FF00FF,0x55C71585,
          //0x55DA70D6,0x55DB7093,0x55FA8072,0x55F4A460,0x55000080,0x554B0082,
          //0x55191970,0x550000FF,0x55800080,0x558A2BE2,0x556495ED,0x5500FFFF,
          //0x55008B8B,0x55483D8B,0x5500BFFF,0x551E90FF,0x55ADD8E6,0x5520B2AA,
          //0x5587CEFA,0x55B0C4DE,0x5576608A,0x557B68EE,0x554169E1,0x556A5ACD,
          //0x55708090,0x554682B4,0x55008080,0x5540E0D0,0x55A9A9A9,0x55D3D3D3

        };
        

        private ObservableCollection<SolidColorBrush> _listColors;
        public ObservableCollection<SolidColorBrush> ListColors { get { return _listColors; } }

        public RatingControlExampleViewModel()
        {
            _listColors = new ObservableCollection<SolidColorBrush>(uintColors.Select(c => 
                new SolidColorBrush(Color.FromArgb(
                    (byte)(c >> 24),                    
                    (byte)((c & 0x00FF0000) >> 16),
                    (byte)((c & 0x0000FF00) >> 8),
                    (byte)((c & 0x000000FF) >> 0)))));
                    
            
            Nb = 2;
            IsEdit = true;  
            FullOutlineThikness = 5;
            EmptyOutlineThikness = 1;
            PathData = "M 5.5,0 L 4,4 L 0,4 L 3,7 L 2,11 L 5,9 L 6, 9 L 9,11 L 8,7 L 11,4 L 7,4 L 5.5,0";
            Value = 0;
            RoundValueSlice = 0.25;
        }

        public double Value
        {
            get { return _value; }
            set
            {
                _value = value;
                RaisePropertyChanged("Value");
            }
        }

        public Int32 Nb
        {
            get { return _nb; }
            set
            {
                _nb = value;
                RaisePropertyChanged("Nb");
            }
        }

        public double Spaces
        {
            get { return _spaces; }
            set
            {
                _spaces = value;
                RaisePropertyChanged("Spaces");
            }
        }

        public bool IsEdit
        {
            get { return _isEdit; }
            set
            {
                _isEdit = value;
                RaisePropertyChanged("IsEdit");
            }
        }

        public SolidColorBrush FullColor
        {
            get { return _fullColor; }
            set 
            { 
                _fullColor = value; 
                RaisePropertyChanged("FullColor");
            }
        }

        public SolidColorBrush EmptyColor
        {
            get { return _emptyColor; }
            set
            {
                _emptyColor = value;
                RaisePropertyChanged("EmptyColor");
            }
        }


        public SolidColorBrush FullOutlineColor
        {
            get { return _fullOutlineColor; }
            set
            {
                _fullOutlineColor = value;
                RaisePropertyChanged("FullOutlineColor");
            }
        }

        public SolidColorBrush EmptyOutlineColor
        {
            get { return _emptyOutlineColor; }
            set
            {
                _emptyOutlineColor = value;
                RaisePropertyChanged("EmptyOutlineColor");
            }
        }

        public Brush BackgroundColor
        {
            get { return _backgroundColor; }
            set
            {
                _backgroundColor = value;
                RaisePropertyChanged("BackgroundColor");
            }
        }


        public double FullOutlineThikness
        {
            get { return _fullOutlineThikness; }
            set
            {
                _fullOutlineThikness = value;
                RaisePropertyChanged("FullOutlineThikness");
            }
        }

        public double EmptyOutlineThikness
        {
            get { return _emptyOutlineThikness; }
            set
            {
                _emptyOutlineThikness = value;
                RaisePropertyChanged("EmptyOutlineThikness");
            }
        }


        public string PathData
        {
            get { return _pathData; }
            set 
            {
                _pathData = value;
                RaisePropertyChanged("PathData");

            }
        }

        public Thickness Padding
        {
            get { return _padding; }
            set
            {
                _padding = value;
                RaisePropertyChanged("Padding");

            }
        }

        public double PaddingDouble
        {
            get { return _paddingDouble; }
            set
            {
                _paddingDouble = value;
                Padding = new Thickness(value);
            }
        }

        public double RoundValueSlice
        {
            get { return _roundValueSlice; }
            set
            {
                _roundValueSlice = value;
                RaisePropertyChanged("RoundValueSlice");
            }
        }

        #region IPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private bool _isEdit;
        private SolidColorBrush _fullColor; 
        private SolidColorBrush _fullOutlineColor;
        private SolidColorBrush _emptyOutlineColor;
        private SolidColorBrush _emptyColor;

        private double _emptyOutlineThikness;
        private double _fullOutlineThikness;
        private string _pathData;
        private Brush _backgroundColor;
        private Thickness _padding;
        private double _paddingDouble = 0;
        private double _roundValueSlice;
        

        private void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

    }
}
