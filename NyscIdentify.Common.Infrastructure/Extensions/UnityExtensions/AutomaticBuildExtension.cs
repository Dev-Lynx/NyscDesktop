using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Unity;
using Unity.Builder;
using Unity.Builder.Strategy;
using Unity.Extension;
using Unity.ObjectBuilder.BuildPlan.DynamicMethod.Creation;
using Unity.Policy;

namespace NyscIdentify.Common.Infrastructure.Extensions.UnityExtensions
{
    #region Extension
    public class AutomaticBuildExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            Context.Strategies.Add(new AutomaticBuildStrategy(),
                UnityBuildStage.Initialization);
        }
    }
    #endregion

    #region Attributes
    [AttributeUsage(AttributeTargets.Class)]
    /// <summary>
    /// Notifies Unity to automatically build the class once it is initialized.
    /// </summary>
    public class AutoBuild : Attribute { }
    #endregion

    #region Strategy
    class AutomaticBuildStrategy : BuilderStrategy
    {
        public override void PostBuildUp(IBuilderContext context)
        {
            if (context.BuildKey.Type == typeof(object)) return;

            bool build = Attribute.IsDefined(context.BuildKey.Type, typeof(AutoBuild));

            if (build) context.Container.BuildUp(context.Existing);
        }
    }
    #endregion
}
