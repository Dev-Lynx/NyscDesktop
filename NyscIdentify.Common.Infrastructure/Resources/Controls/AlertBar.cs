using NyscIdentify.Common.Infrastructure.Resources.Controls.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using NyscIdentify.Common.Infrastructure.Extensions;
using Fody = PropertyChanged;
using System.ComponentModel;
using System.Windows.Media;

namespace NyscIdentify.Common.Infrastructure.Resources.Controls
{
    public class AlertBar : Control
    {
        #region Properties

        #region Events
        PropertyChangedEventHandler ChangedHandler;
        #endregion

        #region Dependency Properties


        public TimeSpan AnimationDuration
        {
            get { return (TimeSpan)GetValue(AnimationDurationProperty); }
            set { SetValue(AnimationDurationProperty, value); }
        }

        public static readonly DependencyProperty AnimationDurationProperty =
            DependencyProperty.Register("AnimationDuration", typeof(TimeSpan), typeof(AlertBar), new PropertyMetadata(TimeSpan.FromSeconds(1)));



        public AlertContext Context
        {
            get { return (AlertContext)GetValue(ContextProperty); }
            set { SetValue(ContextProperty, value); }
        }

        public static readonly DependencyProperty ContextProperty =
            DependencyProperty.Register("Context", typeof(AlertContext), 
                typeof(AlertBar), new UIPropertyMetadata(null, (s, e) =>
                {
                    if (!(s is AlertBar bar)) return;
                    if (!(e.NewValue is AlertContext context)) return;

                    if (e.OldValue is AlertContext oldContext)
                        oldContext.PropertyChanged -= bar.ChangedHandler;

                    context.PropertyChanged += bar.ChangedHandler;
                }));
        #endregion

        #region Internals

        #region Elements
        Button NextButton { get; set; }
        Button PreviousButton { get; set; }
        Button CloseButton { get; set; }
        #endregion

        #endregion

        #endregion

        #region Constructors
        public AlertBar()
        {
            Background = Brushes.Transparent;
            Height = 0;

            ChangedHandler = (s, e) =>
            {
                switch (e.PropertyName)
                {
                    case nameof(AlertContext.SelectedAlert):
                        Core.Log.Debug($"Selected Alert is {Context.SelectedAlert}");
                        if (Context.SelectedAlert == null)
                        {
                            Core.Dispatcher.Invoke(async () =>
                            {
                                this.AnimateHeight(null, 0, 2, true);
                                this.AnimateBackground(Brushes.Transparent.Color, AnimationDuration);
                            });
                            return;
                        }


                        string color = Context.SelectedAlert.AlertType.ToColorValue();

                        Core.Dispatcher.Invoke(() =>
                        {
                            double? from = double.IsNaN(ActualHeight) ? 0 : ActualHeight;
                            this.AnimateHeight(from, 30, 2, false);
                            this.AnimateBackground(color.ToSolidBrush().Color, AnimationDuration);
                        });
                        break;
                }
            };
        }
        #endregion

        #region Methods

        #region Overrides
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            try
            {
                NextButton = GetTemplateChild("PART_NextButton") as Button;
                PreviousButton = GetTemplateChild("PART_PreviousButton") as Button;
                CloseButton = GetTemplateChild("PART_CloseButton") as Button;

                PreviousButton.Click += (s, e) => Context.Previous();
                NextButton.Click += (s, e) => Context.Next();
                CloseButton.Click += (s, e) => Context.Remove(Context.SelectedAlert);
            }
            catch (Exception ex)
            {
                Core.Log.Debug($"An error occured while " +
                    $"apply a content template to {this}.\n{ex}");
            }
        }
        #endregion

        #endregion
    }
}
