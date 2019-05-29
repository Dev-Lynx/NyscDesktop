using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using Unity.Builder;
using Unity.Builder.Strategy;
using Unity.ObjectBuilder;
using Unity.Extension;
using Unity.Policy;

namespace NyscIdentify.Common.Infrastructure.Extensions.UnityExtensions
{
    #region Extension
    public class BuildTrackingExtension : UnityContainerExtension
    {

        protected override void Initialize()
        {
            Context.Strategies.Add(new BuildTrackingStrategy(), UnityBuildStage.TypeMapping);
        }

        public static IBuildTrackingPolicy GetPolicy(IBuilderContext context)
        {
            return context.Policies.Get<IBuildTrackingPolicy>(context.BuildKey, out IPolicyList containingPolicyList);
        }

        public static IBuildTrackingPolicy SetPolicy(IBuilderContext context)
        {
            IBuildTrackingPolicy policy = new BuildTrackingPolicy();
            context.Policies.SetDefault(policy);
            return policy;
        }
    }
    #endregion

    #region Strategy
    public class BuildTrackingStrategy : BuilderStrategy
    {

        public override void PreBuildUp(IBuilderContext context)
        {
            var policy = BuildTrackingExtension.GetPolicy(context)
                ?? BuildTrackingExtension.SetPolicy(context);

            policy.BuildKeys.Push(context.BuildKey);
        }

        public override void PostBuildUp(IBuilderContext context)
        {
            IBuildTrackingPolicy policy = BuildTrackingExtension.GetPolicy(context);
            if ((policy != null) && (policy.BuildKeys.Count > 0))
            {
                policy.BuildKeys.Pop();
                policy.BuildUp(context);
            }
        }
    }
    #endregion

    #region TrackingPolicy
    public interface IBuildTrackingPolicy : IBuildPlanPolicy
    {
        Stack<object> BuildKeys { get; }
    }

    public class BuildTrackingPolicy : IBuildTrackingPolicy
    {

        public BuildTrackingPolicy()
        {
            BuildKeys = new Stack<object>();
        }

        public Stack<object> BuildKeys { get; private set; }

        public void BuildUp(IBuilderContext context)
        {
            Core.Log.Debug($"Finally Building Context -- {context.BuildKey.Type}");
        }
    }
    #endregion
}
