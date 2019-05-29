using SharpVectors.Converters;
using SharpVectors.Renderers.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Resources;
using System.Xml;
using System.Xml.Linq;

namespace NyscIdentify.Common.Infrastructure.Resources.Sources
{
    public class SvgSource : DependencyObject, IUriContext
    {
        #region Properties

        #region IUriContext Implementation
        public Uri BaseUri { get; set; }
        #endregion

        #region Dependency Properties
        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            private set { SetValue(IsActiveProperty, value); }
        }
        public static readonly DependencyProperty IsActiveProperty =
            DependencyProperty.Register("IsActive", typeof(bool), typeof(SvgSource), new PropertyMetadata(false));



        public Uri Source
        {
            get { return (Uri)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public static readonly DependencyProperty SourceProperty =
        DependencyProperty.Register("Source", typeof(Uri), typeof(SvgSource), new UIPropertyMetadata(null, (s, e) =>
        {
            if (s is SvgSource && e.NewValue is Uri)
                ((SvgSource)s).GenerateDrawing((Uri)e.NewValue);
        }));

        public DrawingGroup Drawing
        {
            get { return (DrawingGroup)GetValue(DrawingProperty); }
            private set
            {
                SetFill(Fill, value);
                SetValue(DrawingProperty, value);
            }
        }
        public static readonly DependencyProperty DrawingProperty =
            DependencyProperty.Register("Drawing", typeof(DrawingGroup), typeof(SvgSource), new PropertyMetadata(null));



        public ImageSource ImageSource
        {
            get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(SvgSource), new PropertyMetadata(null));



        public Brush Fill
        {
            get { return (Brush)GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }

        public static readonly DependencyProperty FillProperty =
            DependencyProperty.Register("Fill", typeof(Brush), typeof(SvgSource), new UIPropertyMetadata(null, (s, e) =>
            {
                if (s is SvgSource) ((SvgSource)s).SetFill((Brush)e.NewValue, ((SvgSource)s).Drawing);
            }));

        public bool TextAsGeometry
        {
            get { return (bool)GetValue(TextAsGeometryProperty); }
            set { SetValue(TextAsGeometryProperty, value); }
        }
        public static readonly DependencyProperty TextAsGeometryProperty =
            DependencyProperty.Register("TextAsGeometry", typeof(bool), typeof(SvgSource), new UIPropertyMetadata(false));



        public bool IncludeRuntime
        {
            get { return (bool)GetValue(IncludeRuntimeProperty); }
            set { SetValue(IncludeRuntimeProperty, value); }
        }
        public static readonly DependencyProperty IncludeRuntimeProperty =
            DependencyProperty.Register("IncludeRuntime", typeof(bool), typeof(SvgSource), new UIPropertyMetadata(false));



        public bool OptimizePath
        {
            get { return (bool)GetValue(OptimizePathProperty); }
            set { SetValue(OptimizePathProperty, value); }
        }

        public static readonly DependencyProperty OptimizePathProperty =
            DependencyProperty.Register("OptimizePath", typeof(bool), typeof(SvgSource), new UIPropertyMetadata(true));
        #endregion

        #region Internals
        WpfDrawingSettings DrawingSettings { get; set; }
        #endregion

        #endregion

        #region Methods

        public void Activate() => GenerateDrawing(Source);

        void GenerateDrawing(Uri source)
        {
            try
            {
                DrawingSettings = new WpfDrawingSettings()
                {
                    IncludeRuntime = IncludeRuntime,
                    OptimizePath = OptimizePath,
                    TextAsGeometry = TextAsGeometry
                };

                StreamResourceInfo svgStreamInfo = null;

                Uri folder = new Uri(BaseUri, ".");
                source = new Uri(folder, source.OriginalString);

                if (source.ToString().IndexOf("siteoforigin", StringComparison.OrdinalIgnoreCase) >= 0)
                    svgStreamInfo = Application.GetRemoteStream(source);
                else svgStreamInfo = Application.GetResourceStream(source);

                if (svgStreamInfo == null) return;

                string fileExt = Path.GetExtension(source.ToString());
                bool isCompressed = !String.IsNullOrEmpty(fileExt) &&
                    String.Equals(fileExt, ".svgz",
                    StringComparison.OrdinalIgnoreCase);

                DrawingGroup drawing = null;
                if (isCompressed)
                {
                    using (var svgStream = svgStreamInfo.Stream)
                    using (GZipStream zipStream = new GZipStream(svgStream, CompressionMode.Decompress))
                    using (FileSvgReader reader = new FileSvgReader(DrawingSettings))
                        if ((drawing = reader.Read(zipStream)) != null)
                            Drawing = drawing;
                }
                else
                {
                    using (var svgStream = svgStreamInfo.Stream)
                    using (FileSvgReader reader = new FileSvgReader(DrawingSettings))
                        if ((drawing = reader.Read(svgStream)) != null)
                            Drawing = drawing;

                    /*
                    if (source.ToString().IndexOf("siteoforigin", StringComparison.OrdinalIgnoreCase) >= 0)
                        svgStreamInfo = Application.GetRemoteStream(source);
                    else svgStreamInfo = Application.GetResourceStream(source);

                    using (var svgStream = svgStreamInfo.Stream)
                    using (FileSvgReader reader = new FileSvgReader(DrawingSettings))
                    {
                        reader.Read(svgStream);

                        MemoryStream stream = new MemoryStream();
                        
                        reader.Drawing.image
                    }
                    */
                }
                

                string xaml = Path.Combine(Core.TEMP_DIR, Path.GetFileNameWithoutExtension(source.OriginalString) + ".xaml");
                File.WriteAllText(xaml, PrettyXml(XamlWriter.Save(Drawing)));


                IsActive = true;
            }
            catch (Exception ex)
            {
                Core.Log.Debug(ex);
            }
            return;
        }

        void SetFill(Brush fill, DrawingGroup drawing)
        {
            if (fill == null || drawing == null) return;
            foreach (Drawing d in drawing.Children)
            {
                if (d is DrawingGroup)
                    SetFill(fill, d as DrawingGroup);
                else if (d is GeometryDrawing)
                {
                    GeometryDrawing geo = d as GeometryDrawing;
                    
                    geo.Brush = fill;

                    if (geo.Pen == null) continue;

                    geo.Pen.Brush = fill;
                    geo.Pen.Thickness = 1;
                }
                else if (d is GlyphRunDrawing)
                {
                    GlyphRunDrawing glyph = d as GlyphRunDrawing;
                    glyph.ForegroundBrush = fill;
                }
            }
        }

        static string PrettyXml(string xml)
        {
            var stringBuilder = new StringBuilder();

            var element = XElement.Parse(xml);

            var settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.Indent = true;
            settings.NewLineOnAttributes = true;

            using (var xmlWriter = XmlWriter.Create(stringBuilder, settings))
            {
                element.Save(xmlWriter);
            }

            return stringBuilder.ToString();
        }
        #endregion
    }
}
