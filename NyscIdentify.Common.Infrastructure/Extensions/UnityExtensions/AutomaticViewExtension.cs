using NyscIdentify.Common.Infrastructure.Resources.Controls.Interfaces;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Unity;
using Unity.Builder;
using Unity.Builder.Strategy;
using Unity.Extension;

namespace NyscIdentify.Common.Infrastructure.Extensions.UnityExtensions
{
    #region Extension
    public class AutomaticViewExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            Context.Strategies.Add(new AutomaticViewStrategy(),
                UnityBuildStage.PostInitialization);
        }
    }
    #endregion


    #region Attributes
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class AutoView : Attribute
    {
        public Type View { get; set; }
        public string Region { get; set; }
    }
    #endregion

    #region Strategy
    class AutomaticViewStrategy : BuilderStrategy
    {
        #region Properties
        RoutedEventHandler LoadedHandler;
        #endregion

        public override void PostBuildUp(IBuilderContext context)
        {
            if (context.BuildKey.Type == typeof(object)) return;

            bool auto = Attribute.IsDefined(context.BuildKey.Type, typeof(AutoView));

            if (!auto) return;

            if (context.Existing is IView)
            {
                IView view = context.Existing as IView;

                if (!view.IsLoaded)
                    view.Loaded += LoadedHandler = (s, e) =>
                    {
                        if (!view.IsLoaded) return;
                        view.Loaded -= LoadedHandler;
                        ResolveViews(context);
                    };
                else ResolveViews(context);
            }
            else if (context.Existing != null) ResolveViews(context);
        }

        void ResolveViews(IBuilderContext context)
        {
            try
            {
                var regionManager = context.Container.Resolve<IRegionManager>();

                if (regionManager == null) return;

                var autoViews = Attribute.GetCustomAttributes(context.BuildKey.Type).OfType<AutoView>();

                foreach (var view in autoViews)
                    regionManager.AddToRegion(view.Region, context.Container.Resolve(view.View));
            }
            catch (Exception ex)
            {
                Core.Log.Error($"An error occured while resolving an auto view.\n{ex}");
            }
        }
    }
    #endregion
}
