using NLog;
using NyscIdentify.Common.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Builder;
using Unity.Builder.Strategy;
using Unity.Extension;
using Unity.Policy;
using Unity.Policy.Mapping;

namespace NyscIdentify.Common.Infrastructure.Extensions.UnityExtensions
{
    public class LogCreationExtension : UnityContainerExtension
    {

        protected override void Initialize()
        {
            Context.Strategies.Add(new LogCreationStrategy(), UnityBuildStage.PreCreation);
        }
    }

    public class LogCreationStrategy : BuilderStrategy
    {
        public bool IsPolicySet { get; private set; }

        public override void PreBuildUp(IBuilderContext context)
        {
            Type typeToBuild = context.BuildKey.Type;
            // var plan = context.Policies.Get<IBuildPlanPolicy>(context.BuildKey);

            if (!typeof(NLogger).Equals(typeToBuild))
            {
                base.PreBuildUp(context);
                return;
            }

            Type typeForLog = GetLogType(context);
            IBuildPlanPolicy policy = new LogBuildPlanPolicy(typeForLog);
            context.Policies.Set(policy, context.BuildKey);

            IsPolicySet = true;
        }

        public override void PostBuildUp(IBuilderContext context)
        {
            if (IsPolicySet)
            {
                context.Policies.Clear<IBuildPlanPolicy>(context.BuildKey);
                IsPolicySet = false;
            }
        }

        private static Type GetLogType(IBuilderContext context)
        {
            Type logType = null;
            IBuildTrackingPolicy buildTrackingPolicy = BuildTrackingExtension.GetPolicy(context);
            if ((buildTrackingPolicy != null) && (buildTrackingPolicy.BuildKeys.Count >= 2))
            {
                logType = ((BuildKeyMappingPolicy)buildTrackingPolicy.BuildKeys.ElementAt(1)).Type;
            }
            else
            {
                StackTrace stackTrace = new StackTrace();
                //first two are in the log creation strategy, can skip over them
                for (int i = 2; i < stackTrace.FrameCount; i++)
                {
                    StackFrame frame = stackTrace.GetFrame(i);
                    logType = frame.GetMethod().DeclaringType;
                    if (!logType.FullName.StartsWith("Microsoft.Practices"))
                        break;
                }
            }
            return logType;
        }
    }

    public class LogBuildPlanPolicy : IBuildPlanPolicy
    {

        public LogBuildPlanPolicy(Type logType)
        {
            LogType = logType;
        }

        public Type LogType { get; private set; }

        public void BuildUp(IBuilderContext context)
        {
            if (context.Existing == null)
            {
                var log = LogManager.GetCurrentClassLogger();
                context.Existing = log;
                //context.
            }
        }
    }
}
