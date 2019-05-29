using Mono.Reflection;
using Prism.Unity;
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
    #region Extension
    public class DeepDependencyExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            Context.Strategies.Add(new DeepDependencyStrategy(),
                UnityBuildStage.Initialization);   
        }
    }
    #endregion

    #region Attributes
    [AttributeUsage(AttributeTargets.Property)]
    /// <summary>
    /// Automatically resolves a property on initialization.
    /// </summary>
    public class DeepDependency : Attribute { }
    #endregion

    #region Strategy
    class DeepDependencyStrategy : BuilderStrategy
    {
        public override void PostBuildUp(IBuilderContext context)
        {
            var properties = context.Existing.GetType().GetProperties(
                BindingFlags.Public | BindingFlags.NonPublic |
                BindingFlags.GetProperty | BindingFlags.SetProperty |
                BindingFlags.Instance)
                .Where(x => x.GetCustomAttributes(typeof(DeepDependency), false)
                .Any());

            foreach (var prop in properties)
            {
                try
                {
                    if (prop.CanWrite)
                    {
                        prop.SetValue(context.Existing,
                        context.Container.TryResolve(prop.PropertyType));
                    }
                    else
                    {
                        prop.GetBackingField().SetValue(context.Existing,
                            context.Container.TryResolve(prop.PropertyType));
                    }
                }
                catch (Exception ex)
                {
                    Core.Log.Error($"An error occured during deep property resolutiion\n{ex}");
                }
            }
        }
    }
    #endregion
}
