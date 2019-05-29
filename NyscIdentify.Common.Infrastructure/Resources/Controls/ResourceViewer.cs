using CommonServiceLocator;
using NyscIdentify.Common.Infrastructure.Models.Interfaces;
using NyscIdentify.Common.Infrastructure.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace NyscIdentify.Common.Infrastructure.Resources.Controls
{
    public class ResourceViewer : Control
    {
        #region Properties

        #region Dependency Properties
        public IResource Resource
        {
            get { return (IResource)GetValue(ResourceProperty); }
            set { SetValue(ResourceProperty, value); }
        }

        public static readonly DependencyProperty ResourceProperty =
            DependencyProperty.Register("Resource", typeof(IResource), typeof(ResourceViewer), new PropertyMetadata(null, (s, e) =>
            {
                if (!(s is ResourceViewer viewer)) return;
                if (!(e.NewValue is IResource resource)) return;

                if (viewer.IsLoaded)
                    viewer.ResolveResource();
            }));



        public bool UsePlaceholder
        {
            get { return (bool)GetValue(UsePlaceholderProperty); }
            set { SetValue(UsePlaceholderProperty, value); }
        }

        public static readonly DependencyProperty UsePlaceholderProperty =
            DependencyProperty.Register("UsePlaceholder", typeof(bool), typeof(ResourceViewer), new PropertyMetadata(false));



        public bool HighPriority
        {
            get { return (bool)GetValue(HighPriorityProperty); }
            set { SetValue(HighPriorityProperty, value); }
        }

        public static readonly DependencyProperty HighPriorityProperty =
            DependencyProperty.Register("HighPriority", typeof(bool), typeof(ResourceViewer), new PropertyMetadata(false));



        #endregion

        #region Internals

        #region Elements
        Button TryAgainButton { get; set; }
        #endregion

        #endregion

        #endregion

        #region Constructors
        public ResourceViewer()
        {
            Loaded += (s, e) =>
            {
                ResolveResource();
            };
        }
        #endregion

        #region Methods
        
        void ResolveResource()
        {
            try
            {
                var resourceManager = ServiceLocator.Current.TryResolve<IResourceManager>();

                resourceManager.ResolveResource(Resource);
            }
            catch { }
        }

        public override void OnApplyTemplate()
        {
            TryAgainButton = GetTemplateChild("PART_TryAgainButton") as Button;

            TryAgainButton.Click += (s, e) =>
            {
                var resourceManager = ServiceLocator.Current.TryResolve<IResourceManager>();
                resourceManager.ResolveResource(Resource, true);
            };

            base.OnApplyTemplate();
        }

        #endregion
    }
}
