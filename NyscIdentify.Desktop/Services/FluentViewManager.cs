using NyscIdentify.Common.Infrastructure;
using NyscIdentify.Common.Infrastructure.Extensions;
using NyscIdentify.Common.Infrastructure.Extensions.UnityExtensions;
using NyscIdentify.Common.Infrastructure.Services.Interfaces;
using Prism.Logging;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fody = PropertyChanged;

namespace NyscIdentify.Desktop.Services
{
    [AutoBuild]
    public class FluentViewManager : BindableBase, IViewManager
    {
        #region Properties

        #region Services
        [DeepDependency]
        ILoggerFacade Logger { get; }
        [DeepDependency]
        IRegionManager RegionManager { get; }
        #endregion
        
        #region Bindables
        public bool BackstageActive { get; set; }
        TabMenu _activeTab = TabMenu.Home;
        [Fody.DoNotNotify]
        public TabMenu ActiveTab
        {
            get => _activeTab;
            set => ActivateTab(value);
        }
        #endregion

        #region Internals
        TabMenu PreviouslyActiveTab { get; set; }
        #endregion

        #endregion

        #region Methods

        void ActivateTab(TabMenu tab)
        {
            PreviouslyActiveTab = ActiveTab;
            
            switch (tab)
            {
                case TabMenu.Home:
                    RegionManager.NavigateToView<Views.IdentityView>(Core.MAIN_REGION);
                    break;

                case TabMenu.SingleView:
                    RegionManager.NavigateToView<Views.SingleIdentityView>(Core.MAIN_REGION);
                    break;
            }

            _activeTab = tab;
            RaisePropertyChanged(nameof(ActiveTab));
        }

        #endregion

    }
}
