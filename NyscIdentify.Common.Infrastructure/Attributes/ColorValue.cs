using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace NyscIdentify.Common.Infrastructure.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ColorValue : Attribute
    {
        public string Color { get; set; } = "#000000";

        public ColorValue(string color)
        {
            Color = color;
        }
    }
}
