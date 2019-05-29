using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fody = PropertyChanged;

namespace NyscIdentify.Common.Infrastructure.Resources.Controls.Models
{
    public class AlertContext : BindableBase
    {
        #region Properties

        #region Bindables
        public int SelectedIndex { get; protected set; }
        public ObservableCollection<Alert> Alerts { get; } = new ObservableCollection<Alert>();


        public Alert SelectedAlert => Alerts.ElementAtOrDefault(SelectedIndex);
        public bool CanNext => SelectedIndex < Alerts.Count - 1;
        public bool CanPrevious => SelectedIndex > 0;
        public bool IsActive => SelectedAlert != null;
        #endregion

        #endregion

        #region Methods
        
        public void Add(Alert alert)
        {
            Alerts.Add(alert);

            alert.AlertExpired += (s, e) => Remove(alert);

            if (SelectedAlert != null) SelectedAlert.Tick();

            Core.Dispatcher.Invoke(() => NotifyChanges());
        }

        public void Remove(Alert alert)
        {
            try { Alerts.Remove(alert); }
            catch (Exception ex)
            {
                Core.Log.Debug($"An error occured while removing an alert\n{ex}");
            }
            
            if (Alerts.Count <= 0) SelectedIndex = 0;
            else if (SelectedIndex >= Alerts.Count && Alerts.Count > 0) SelectedIndex = Alerts.Count - 1;
            else SelectedIndex = 0; 

            if (SelectedAlert != null) SelectedAlert.Tick();

            Core.Dispatcher.Invoke(() => NotifyChanges());
        }

        public virtual void Next()
        {
            if (!CanNext) return;
            SelectedIndex++;
            if (SelectedAlert != null) SelectedAlert.Tick();

            Core.Dispatcher.Invoke(() => NotifyChanges());
        }

        public virtual void Previous()
        {
            if (!CanPrevious) return;
            SelectedIndex--;
            if (SelectedAlert != null) SelectedAlert.Tick();

            Core.Dispatcher.Invoke(() => NotifyChanges());
        }

        protected virtual void NotifyChanges()
        {
            RaisePropertyChanged(nameof(Alerts));
            RaisePropertyChanged(nameof(SelectedIndex));
            RaisePropertyChanged(nameof(SelectedAlert));
            RaisePropertyChanged(nameof(CanPrevious));
            RaisePropertyChanged(nameof(CanNext));
            RaisePropertyChanged(nameof(IsActive));
        }
        #endregion
    }
}
