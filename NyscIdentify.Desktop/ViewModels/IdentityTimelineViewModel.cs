using NyscIdentify.Common.Infrastructure.Extensions;
using NyscIdentify.Common.Infrastructure.Extensions.UnityExtensions;
using NyscIdentify.Common.Infrastructure.Models.ViewModels;
using NyscIdentify.Common.Infrastructure.Resources.Collections;
using NyscIdentify.Common.Infrastructure.Services.Interfaces;
using Prism.Logging;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NyscIdentify.Desktop.ViewModels
{
    [AutoBuild]
    public class IdentityTimelineViewModel : BindableBase, IWaitable
    {
        #region Properties

        #region Bindables
        public AsyncObservableCollection<SimpleUserActivity> Activities { get; } 
            = new AsyncObservableCollection<SimpleUserActivity>();
        #endregion

        #region Services
        [DeepDependency]
        ILoggerFacade Logger { get; }
        [DeepDependency]
        IIdentityManager IdentityManager { get; }
        #endregion

        #region Commands
        public ICommand UpdateActivitiesCommand { get; }
        public ICommand DisposeTimelineCommand { get; }

        public bool IsBusy { get; set; } = true;
        public string BusyMessage { get; set; }
        public double Progress => 50;
        #endregion

        #endregion

        #region Methods
        [CommandMethod(nameof(UpdateActivitiesCommand))]
        void OnUpdateTimeline()
        {
            Task.Run(async () =>
            {
                IsBusy = true;
                BusyMessage = "Acquiring Timelines...";

                var activities = await IdentityManager.GetActivities();

                foreach (var activity in activities)
                    Logger.Debug(activity.Description);

                if (activities != null) await Activities.AddRangeAsync(activities);


                IsBusy = false;
            });
        }

        [CommandMethod(nameof(DisposeTimelineCommand))]
        void OnDisposeTimeline() => Activities.Clear();

        #endregion
    }
}
