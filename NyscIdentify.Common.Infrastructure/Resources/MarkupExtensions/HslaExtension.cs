using NyscIdentify.Common.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Windows.Media;

namespace NyscIdentify.Common.Infrastructure.Resources.MarkupExtensions
{
    [MarkupExtensionReturnType(typeof(SolidColorBrush))]
    public class HslaExtension : MarkupExtension
    {
        #region Properties
        public SolidColorBrush Source { get; set; }
        public double H { get; set; }
        public double S { get; set; }
        public double L { get; set; }
        public double A { get; set; }
        #endregion

        #region Methods
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            H = ReAlign(H, 0, 360);
            S = ReAlign(S, 0, 100);
            L = ReAlign(L, 0, 100);
            A = ReAlign(A, 0, 100);

            Source.Color.ToHSLA(out double hue, out double saturation,
                out double lightness, out double alpha);
            // TODO: Finish this up



            return new SolidColorBrush();
        }

        /// <summary>
        /// Makes sure a value stays within it's limit
        /// </summary>
        /// <returns></returns>
        double ReAlign(double value, double min, double max)
        {
            value = Math.Max(value, min);
            value = Math.Min(value, max);
            return value;
        }

        double CalculateDifference(double percentageOffset, double currentValue, double maxValue)
        {

            double currentPercentage = (currentValue / maxValue) * 100;

            double destinationPercentage = ((percentageOffset - 50.0) / 50.0) * 100;
            double difference = percentageOffset >= 50 ? maxValue - currentValue : currentValue;

            double percentageDifference = difference * destinationPercentage / 100;
            double newPercentage = currentPercentage + percentageDifference;

            return (newPercentage / 100) * maxValue;
        }
        #endregion
    }
}
