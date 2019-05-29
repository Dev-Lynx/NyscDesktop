using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace NyscIdentify.Common.Infrastructure.Extensions
{
    public static class UIHelper
    {
        #region Methods
        public static void NavigateToView<T>(this IRegionManager regionManager, string region, NavigationParameters parameters = null) where T : class
        {
            try
            {
                if (parameters == null)
                    regionManager.RequestNavigate(region, typeof(T).Name);
                else regionManager.RequestNavigate(region, typeof(T).Name, parameters);
            }
            catch (Exception ex)
            {
                Core.Log.Error($"An error occured while attempting to navigate" +
                    $" to {typeof(T)}.\n{ex}");
            }
        }



        public static T FindParent<T>(this DependencyObject child, bool debug = false) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            //we've reached the end of the tree
            if (parentObject == null) return null;

            //check if the parent matches the type we're looking for
            if (parentObject is T parent)
                return parent;
            else
                return FindParent<T>(parentObject);
        }

        public static T FindChild<T>(this DependencyObject element, bool debug = false) where T : DependencyObject
        {
            int count = VisualTreeHelper.GetChildrenCount(element);

            for (int i = 0; i < count; i++)
            {
                object child = VisualTreeHelper.GetChild(element, i);
                if (child is T)
                    return child as T;

                if (debug && child is FrameworkElement)
                    Core.Log.Debug($"{child} - {((FrameworkElement)child).GetValue(FrameworkElement.NameProperty)}");
            }
            return null;
        }

        public static IEnumerable<T> FindVisualChildren<T>(this DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        public static IEnumerable<T> FindLogicalChildren<T>(this DependencyObject obj, bool checkObject = true) where T : DependencyObject
        {
            if (obj != null)
            {
                if (obj is T && checkObject)
                    yield return obj as T;

                foreach (DependencyObject child in LogicalTreeHelper.GetChildren(obj).OfType<DependencyObject>())
                    foreach (T c in FindLogicalChildren<T>(child))
                        yield return c;
            }
        }

        /// <summary>
        /// Finds a Child of a given item in the visual tree. 
        /// </summary>
        /// <param name="parent">A direct parent of the queried item.</param>
        /// <typeparam name="T">The type of the queried item.</typeparam>
        /// <param name="childName">x:Name or Name of child. </param>
        /// <returns>The first parent item that matches the submitted type parameter. 
        /// If not matching item can be found, 
        /// a null parent is being returned.</returns>
        public static T FindChild<T>(this DependencyObject parent, string childName)
           where T : DependencyObject
        {
            // Confirm parent and childName are valid. 
            if (parent == null) return null;

            T foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                // If the child is not of the request child type child
                T childType = child as T;
                if (childType == null)
                {
                    // recursively drill down the tree
                    foundChild = FindChild<T>(child, childName);

                    // If the child is found, break so we do not overwrite the found child. 
                    if (foundChild != null) break;
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    var frameworkElement = child as FrameworkElement;
                    // If the child's name is set for search
                    if (frameworkElement != null && frameworkElement.Name == childName)
                    {
                        // if the child's name is of the request name
                        foundChild = (T)child;
                        break;
                    }
                }
                else
                {
                    // child element found.
                    foundChild = (T)child;
                    break;
                }
            }

            return foundChild;
        }


        #region Animations
        public static async Task DoubleAnimation(this FrameworkElement element, DependencyProperty property, double? from, double? to, Duration duration)
        {
            DoubleAnimation animation = new DoubleAnimation()
            {
                From = from,
                To = to,
                FillBehavior = FillBehavior.HoldEnd,
                Duration = duration
            };

            EventHandler handler = null;
            bool completed = false;

            animation.Completed += handler = (s, e) => completed = true;

            Application.Current.Dispatcher.Invoke(() => element.BeginAnimation(property, animation));

            while (!completed)
                await Task.Delay(10);
            animation.Completed -= handler;
        }

        public static async Task AnimateWidthAndHeight(this FrameworkElement element, double destinationWidth, double destinationHeight, TimeSpan duration)
        {
            DoubleAnimation widthAnimation = new DoubleAnimation(destinationWidth, duration);
            DoubleAnimation heightAnimation = new DoubleAnimation(destinationHeight, duration);

            bool completed = false;

            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(widthAnimation);
            storyboard.Children.Add(heightAnimation);

            Storyboard.SetTarget(widthAnimation, element);
            Storyboard.SetTarget(heightAnimation, element);

            Storyboard.SetTargetProperty(widthAnimation, new PropertyPath(FrameworkElement.WidthProperty));
            Storyboard.SetTargetProperty(heightAnimation, new PropertyPath(FrameworkElement.HeightProperty));

            storyboard.Completed += (s, e) => completed = true;
            await Application.Current.Dispatcher.InvokeAsync(() => storyboard.Begin());

            while (!completed) await Task.Delay(10);
        }

        public static async Task FadeOut(this FrameworkElement element, double speed = 1, bool reappear = false)
        {
            if (speed == 0) speed = 1;

            if (element.Opacity <= 0)
                element.Opacity = 1;

            // time = distance / speed
            var time = (double)1 / speed;
            var from = element.Opacity;

            if (double.IsNaN(from) || double.IsInfinity(from))
                from = 1;

            var animation = new DoubleAnimation
            {
                From = from,
                To = 0,
                FillBehavior = FillBehavior.Stop,
                Duration = new System.Windows.Duration(TimeSpan.FromSeconds(time))
            };

            EventHandler fadeHandler = null;
            bool completed = false;

            animation.Completed += fadeHandler = (s, e) =>
            {
                if (reappear) element.Opacity = 1;
                else element.Opacity = 0;
                completed = true;
            };

            element.BeginAnimation(FrameworkElement.OpacityProperty, animation);

            while (!completed)
                await Task.Delay(10);
        }

        public static async Task FadeIn(this FrameworkElement element, double speed = 1, bool disappear = false)
        {
            if (speed <= 0) speed = 1;

            // time = distance / speed
            var time = 1.0 / speed;

            var animation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                FillBehavior = FillBehavior.Stop,
                Duration = new Duration(TimeSpan.FromSeconds(time))
            };

            EventHandler fadeHandler = null;
            bool completed = false;

            animation.Completed += fadeHandler = (s, e) =>
            {
                if (disappear) element.Opacity = 0;
                else element.Opacity = 1;
                completed = true;
            };

            element.BeginAnimation(FrameworkElement.OpacityProperty, animation);

            while (!completed)
                await Task.Delay(10);
        }

        public static async Task AnimateHeight(this FrameworkElement element, double? from, double value, double speed = 1, bool demolish = false)
        {
            if (speed <= 0) speed = 1;
            var time = 1.0 / speed;

            /*
            double oldHeight = element.ActualHeight;
            if (double.IsNaN(oldHeight)) oldHeight = 0;
            */

            Duration duration = new Duration(TimeSpan.FromSeconds(time));

            await Application.Current.Dispatcher.InvokeAsync(() => element.DoubleAnimation(FrameworkElement.HeightProperty, from, value, duration));

            if (demolish) element.Height = 0;
            else element.Height = value;
        }

        public static async Task AnimateBackground(this Control element, Color to, TimeSpan duration)
        {
            ColorAnimation animation = new ColorAnimation
            {
                To = to,
                Duration = duration,
                FillBehavior = FillBehavior.HoldEnd
            };

            bool completed = false;
            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(animation);
            Storyboard.SetTarget(animation, element);
            Storyboard.SetTargetProperty(animation, new PropertyPath("(0).(1)", Control.BackgroundProperty, SolidColorBrush.ColorProperty));

            storyboard.Completed += (s, e) => completed = true;

            await Application.Current.Dispatcher.InvokeAsync(() => storyboard.Begin());

            while (!completed) await Task.Delay(10);
        }
        #endregion

        #region Colors
        public static Color GetColor(this BitmapSource source, Point point)
        {
            try
            {
                CroppedBitmap bitmap = new CroppedBitmap(source, new Int32Rect((int)Math.Round(point.X), (int)Math.Round(point.Y), 1, 1));
                byte[] pixels = new byte[4];
                bitmap.CopyPixels(pixels, pixels.Length, 0);
                return Color.FromRgb(pixels[2], pixels[1], pixels[0]);
            }
            catch { return new Color(); }
        }

        public static SolidColorBrush ToSolidBrush(this string hex)
        {
            try
            {
                return (SolidColorBrush)(new BrushConverter().ConvertFrom(hex));
                /*
                hex = hex.Replace("#", string.Empty);

                if (hex.Length < 8) hex = "FF" + hex;
                byte a = (byte)(Convert.ToUInt32(hex.Substring(0, 2), 16));
                byte r = (byte)(Convert.ToUInt32(hex.Substring(2, 2), 16));
                byte g = (byte)(Convert.ToUInt32(hex.Substring(4, 2), 16));
                byte b = (byte)(Convert.ToUInt32(hex.Substring(6, 2), 16));
                SolidColorBrush myBrush = new SolidColorBrush(Color.FromArgb(a, r, g, b));
                return myBrush;
                */
            }
            catch
            {
                return new SolidColorBrush();
            }
        }

        public static Color ToMediaColor(this System.Drawing.Color color) => Color.FromArgb(color.A, color.R, color.G, color.B);
        public static System.Drawing.Color ToDrawingColor(this Color color) => System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);

        public static void ToHSV(this System.Drawing.Color color, out double hue, out double saturation, out double value)
        {
            int max = Math.Max(color.R, Math.Max(color.G, color.B));
            int min = Math.Min(color.R, Math.Min(color.G, color.B));

            hue = color.GetHue();
            saturation = (max == 0) ? 0 : 1d - (1d * min / max);
            value = max / 255d;
        }

        public static void ToHSL(this Color color, out double hue, out double saturation, out double lightness)
        {
            var c = color.ToDrawingColor();
            hue = c.GetHue(); // Divide by 360 when converting back...
            saturation = c.GetSaturation();
            lightness = c.GetBrightness();
//             color.A / 255.0
        }

        public static void ToHSLA(this Color color, out double hue, out double saturation, out double lightness,
        out double alpha)
        {
            var c = color.ToDrawingColor();
            hue = c.GetHue(); // Divide by 360 when converting back...
            saturation = c.GetSaturation();
            lightness = c.GetBrightness();
            alpha = color.A / 255.0;
        }

        public static void ToHSL(this System.Drawing.Color color, out double hue, out double saturation, out double lightness)
        {
            hue = color.GetHue(); // Divide by 360 when converting back...
            saturation = color.GetSaturation();
            lightness = color.GetBrightness();
        }

        public static Color HSLToColor(double hue, double saturation, double lightness)
        {
            double h = hue, s = saturation, l = lightness;
            double r = 0, g = 0, b = 0;
            double temp1, temp2;

            if (l == 0) return Color.FromRgb((byte)r, (byte)g, (byte)b);
            if (s == 0) Color.FromRgb((byte)l, (byte)l, (byte)l);

            temp2 = l <= .5 ? (l * (1.0 + s)) : l + s - (l * s);
            temp1 = 2.0 * l - temp2;

            double[] t3 = new double[] { h + (1.0 / 3.0), h, h - (1.0 / 3.0) };
            double[] clr = new double[] { 0, 0, 0 };

            for (int i = 0; i < 3; i++)
            {
                if (t3[i] < 0) t3[i] += 1.0;
                if (t3[i] > 1) t3[i] -= 1.0;

                if (6.0 * t3[i] < 1.0)
                    clr[i] = temp1 + (temp2 - temp1) * t3[i] * 6.0;
                else if (2.0 * t3[i] < 1.0)
                    clr[i] = temp2;
                else if (3.0 * t3[i] < 2.0)
                    clr[i] = (temp1 + (temp2 - temp1) * ((2.0) / (3.0) - t3[i]) * 6.0);
                else clr[i] = temp1;
            }

            r = clr[0] * 255; g = clr[1] * 255; b = clr[2] * 255;
            return Color.FromRgb((byte)r, (byte)g, (byte)b);
        }

        public static Color ColorFromHSV(double hue, double saturation, double value)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value = value * 255;
            byte v = Convert.ToByte(Convert.ToInt32(value));
            byte p = Convert.ToByte(Convert.ToInt32(value * (1 - saturation)));
            byte q = Convert.ToByte(Convert.ToInt32(value * (1 - f * saturation)));
            byte t = Convert.ToByte(Convert.ToInt32(value * (1 - (1 - f) * saturation)));

            if (hi == 0)
                return Color.FromArgb(255, v, t, p);
            else if (hi == 1)
                return Color.FromArgb(255, q, v, p);
            else if (hi == 2)
                return Color.FromArgb(255, p, v, t);
            else if (hi == 3)
                return Color.FromArgb(255, p, q, v);
            else if (hi == 4)
                return Color.FromArgb(255, t, p, v);
            else
                return Color.FromArgb(255, v, p, q);
        }

        public static Color SetSaturation(this Color color, double percent)
        {
            color.ToDrawingColor().ToHSV(out double hue, out double saturation, out double value);
            return ColorFromHSV(hue, saturation / 100.0, value);
        }

        public static double GetSaturation(this Color color)
        {
            color.ToDrawingColor().ToHSV(out double hue, out double saturation, out double value);
            return saturation;
        }
        #endregion
        #endregion
    }
}
