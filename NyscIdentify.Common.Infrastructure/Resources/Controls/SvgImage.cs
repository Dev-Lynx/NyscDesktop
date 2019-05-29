using NyscIdentify.Common.Infrastructure.Resources.Sources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace NyscIdentify.Common.Infrastructure.Resources.Controls
{
    public class SvgImage : Image
    {
        #region Properties

        #region Dependency Properties
        public SvgSource SvgSource
        {
            get { return (SvgSource)GetValue(SvgSourceProperty); }
            set { SetValue(SvgSourceProperty, value); }
        }

        public static readonly DependencyProperty SvgSourceProperty =
            DependencyProperty.Register("SvgSource", typeof(SvgSource), typeof(SvgImage), new UIPropertyMetadata(null, (s, e) =>
            {
                if (!(s is SvgImage) || !(e.NewValue is SvgSource)) return;
                
                var svg = (SvgImage)s;
                svg.SnapsToDevicePixels = true;
                

                if (!(svg.Source is DrawingImage))
                    svg.Source = new DrawingImage();

                if (!(e.NewValue is SvgSource)) return;
                SvgSource svgSource = ((SvgSource)e.NewValue);

                if (!svgSource.IsActive) svgSource.Activate();
                if (!svgSource.IsActive) return;


                svg.DrawingSource = svgSource.Drawing;
                svg.Source = new DrawingImage(svg.DrawingSource);
            }));

        public DrawingGroup DrawingSource
        {
            get { return (DrawingGroup)GetValue(DrawingSourceProperty); }
            private set { SetValue(DrawingSourceProperty, value); }
        }

        public static readonly DependencyProperty DrawingSourceProperty =
            DependencyProperty.Register("DrawingSource", typeof(DrawingGroup), typeof(SvgImage), new PropertyMetadata(null));
        #endregion

        #endregion

        #region Constructors
        public SvgImage()
        {
            RenderOptions.SetBitmapScalingMode(this, BitmapScalingMode.HighQuality);
            Stretch = Stretch.Uniform;
        }
        #endregion
    }
}
