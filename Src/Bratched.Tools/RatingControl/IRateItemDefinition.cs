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
    public interface IRateItemDefinition
    {
       SolidColorBrush BackgroundColor { get; set; }
       SolidColorBrush OutlineColor { get; set; }
        object OutlineThikness { get; set; }        
        object PathData { get; set; }
    }
}
