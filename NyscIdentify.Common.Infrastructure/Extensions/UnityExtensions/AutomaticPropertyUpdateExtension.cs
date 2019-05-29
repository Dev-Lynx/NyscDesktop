using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Unity.Builder;
using Unity.Builder.Strategy;
using Unity.Extension;

namespace NyscIdentify.Common.Infrastructure.Extensions.UnityExtensions
{
    #region Extensions
    public class AutomaticPropertyUpdateExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            Context.Strategies.Add(new AutomaticPropertyUpdateStrategy(),
                UnityBuildStage.PostInitialization);
        }
    }
    #endregion

    #region Attributes
    [AttributeUsage(AttributeTargets.Property)]
    public class UpdateWith : Attribute
    {
        public string Source { get; set; }
        public string Property { get; set; }
        public string PropertyChangedMethodName { get; } = "RaisePropertyChanged";
    }
    #endregion

    #region Strategy
    class AutomaticPropertyUpdateStrategy : BuilderStrategy
    {
        public override void PostBuildUp(IBuilderContext context)
        {
            // Only work when the objects have been built.
            if (context.BuildKey.Type != typeof(object)) return;
            if (!(context.Existing is INotifyPropertyChanged)) return;

            var properties = context.Existing.GetType().GetProperties(
                BindingFlags.Public | BindingFlags.NonPublic |
                BindingFlags.GetProperty | BindingFlags.SetProperty |
                BindingFlags.Instance);

            var taggedProperties = properties.
                Where(x => x.GetCustomAttributes<UpdateWith>(false).
                Any());

            
            foreach (var prop in taggedProperties)
            {
                try
                {
                    var attribute = prop.GetCustomAttribute<UpdateWith>();

                    var sourceProperty = properties.
                        FirstOrDefault(p => p.Name == attribute.Source);

                    if (sourceProperty == null)
                        Core.Log.Debug(attribute.Property);

                    bool canNotify = sourceProperty.GetValue(context.Existing).
                        GetType().IsAssignableTo<INotifyPropertyChanged>();

                    if (!canNotify) continue;

                    MethodInfo updateMethod = context.Existing.GetType().
                        GetMethod(attribute.PropertyChangedMethodName, BindingFlags.InvokeMethod |
                        BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                    var notifier = sourceProperty.GetValue(context.Existing) as INotifyPropertyChanged;
                    notifier.PropertyChanged += (s, e) =>
                    {
                        if (e.PropertyName == attribute.Property)
                        {
                            updateMethod.Invoke(context.Existing, new object[] { prop.Name });
                        }
                    };
                }
                catch (Exception ex)
                {
                    Core.Log.Error($"An error occured while adding automatic updates to a property\n{ex}");
                }
            }
        }
    }
    #endregion
}
