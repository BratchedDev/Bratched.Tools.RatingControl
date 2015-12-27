using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#if NETFX_CORE
using Windows.UI.Xaml.Media;
#endif
#if WINDOWS_PHONE
using System.Windows.Media;
#endif


namespace Bratched.Tools.RatingControl
{
    public sealed class RateItemDefinitionModel: IRateItemDefinition
    {
        public SolidColorBrush BackgroundColor { get; set; }
        public SolidColorBrush OutlineColor { get;set; }
        public object OutlineThickness { get; set; }
        public object PathData { get; set; }
        
    }
}
