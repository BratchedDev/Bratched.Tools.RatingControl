using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using RatingControlDemo_wp7.ViewModel;

namespace RatingControlDemo_wp7
{
    public partial class MVVMPage : PhoneApplicationPage
    {
        public MVVMPage()
        {
            InitializeComponent();
            DataContext = new RatingControlExampleViewModel();
        }
    }
}