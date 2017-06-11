using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using static SubtitleFixer.SubtitleFixerViewModel;

namespace SubtitleFixer
{
    /// <summary>
    /// This class convert bool to PathType enum
    /// </summary>
    public class PathTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((PATH_TYPE)value) == PATH_TYPE.File ? true : false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((bool)value) ? PATH_TYPE.File : PATH_TYPE.Folder;
        }
    }
}
