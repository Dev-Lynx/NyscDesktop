using Prism.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Unity.Builder;
using Unity.Extension;

namespace NyscIdentify.Common.Infrastructure.Extensions.UnityExtensions
{
    #region Extensions
    /// <summary>
    /// Invokes methods marked with <see cref="DeepInjectionMethod"/>
    /// once the object has been built.
    /// </summary>
    public class DeepMethodExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            Context.Strategies.Add(new DeepMethodStrategy(),
                UnityBuildStage.PostInitialization);
        }
    }
    #endregion

    #region Attributes
    [AttributeUsage(AttributeTargets.Method)]
    /// <summary>
    /// Automatically resolves a method after initialization.
    /// </summary>
    public class DeepInjectionMethod : Attribute { }
    #endregion

    #region Strategy
    class DeepMethodStrategy : BuildTrackingStrategy
    {
        public override void PostBuildUp(IBuilderContext context)
        {
            // Only invoke methods when the objects have been built.
            if (context.BuildKey.Type != typeof(object)) return;

            var methods = context.Existing.GetType().GetMethods(BindingFlags.InvokeMethod
                | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(x => x.GetCustomAttributes(typeof(DeepInjectionMethod), true)
                .Any());

            foreach (var method in methods)
            {
                try
                {
                    List<object> parameters = new List<object>();
                    foreach (var param in method.GetParameters())
                    {
                        if (param.HasDefaultValue) continue;
                        parameters.Add(context.Container.TryResolve(param.ParameterType));
                    }

                    method.Invoke(context.Existing, parameters.ToArray());
                }
                catch (Exception ex)
                {
                    Core.Log.Error($"An error occured during a deep method resolution\n{ex}");
                }
            }
        }
    }
    #endregion
}
