using NyscIdentify.Common.Infrastructure.Resources.Controls.Models;
using NyscIdentify.Common.Infrastructure.Services.Interfaces;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NyscIdentify.Common.Infrastructure.Services
{
    public class AlertService : BindableBase, IAlertService
    {
        #region Properties
        public AlertContext Context { get; } = new AlertContext();
        #endregion

        #region Methods

        #region IAlertService Implementation
        public void Error(string message, TimeSpan? duration = null, bool closable = true)
        {
            TimeSpan lifeSpan = duration.HasValue ? duration.Value : Alert.DefaultLifeSpan;

            Alert alert = NewAlert(AlertType.Error, message, lifeSpan, closable);

            Context.Add(alert);
        }

        public void Information(string message, TimeSpan? duration = null, bool closable = true)
        {
            TimeSpan lifeSpan = duration.HasValue ? duration.Value : Alert.DefaultLifeSpan;

            Alert alert = NewAlert(AlertType.Information, message, lifeSpan, closable);

            Context.Add(alert);
        }

        public void Success(string message, TimeSpan? duration = null, bool closable = true)
        {
            TimeSpan lifeSpan = duration.HasValue ? duration.Value : Alert.DefaultLifeSpan;

            Alert alert = NewAlert(AlertType.Success, message, lifeSpan, closable);

            Context.Add(alert);
        }

        public void Warning(string message, TimeSpan? duration = null, bool closable = true)
        {
            TimeSpan lifeSpan = duration.HasValue ? duration.Value : Alert.DefaultLifeSpan;

            Alert alert = NewAlert(AlertType.Warning, message, lifeSpan, closable);

            Context.Add(alert);
        }
        #endregion

        Alert NewAlert(AlertType type, string message, TimeSpan duration, bool closable) => new Alert()
        {
            Message = message,
            AlertType = type,
            LifeSpan = duration,
            IsClosable = closable
        };

        #endregion
    }
}
