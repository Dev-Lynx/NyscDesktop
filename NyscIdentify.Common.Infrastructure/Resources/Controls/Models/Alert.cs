using NyscIdentify.Common.Infrastructure.Attributes;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace NyscIdentify.Common.Infrastructure.Resources.Controls.Models
{
    #region Enumerations
    
    public enum AlertType
    {
        [ColorValue("#386e9f")]
        Information,
        [ColorValue("#689f38")]
        Success,
        [ColorValue("#ff6030")]
        Warning,
        [ColorValue("#FF3030")]
        Error,
    }

    #endregion

    public class Alert : BindableBase
    {
        public static TimeSpan DefaultLifeSpan { get; } = TimeSpan.FromSeconds(10);

        public AlertType AlertType { get; set; }
        public string Message { get; set; }
        public DateTime Created { get; } = DateTime.Now;
        public TimeSpan LifeSpan { get; set; } = TimeSpan.FromSeconds(10);
        public bool IsClosable { get; set; }
        public bool IsExpired => DateTime.Now < Created + LifeSpan;

        public event EventHandler AlertExpired;

        bool Ticking { get; set; }

        #region Methods
        
        public void Tick()
        {
            if (Ticking) return;
            var timer = new Timer(LifeSpan.TotalMilliseconds);
            timer.Elapsed += (s, e) =>
            {
                timer.Dispose();
                AlertExpired?.Invoke(this, EventArgs.Empty);
            };

            Ticking = true;
            timer.Start();
        }

        #endregion
    }
}
