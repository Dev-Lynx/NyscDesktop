using Mono.Reflection;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unity;
using Unity.Builder;
using Unity.Builder.Strategy;
using Unity.Extension;

namespace NyscIdentify.Common.Infrastructure.Extensions.UnityExtensions
{
    #region Extensions
    public class AutomaticCommandExtension : UnityContainerExtension
    {
        protected override void Initialize()
        {
            Context.Strategies.Add(new AutomaticCommandStrategy(),
                UnityBuildStage.PostInitialization);
        }
    }
    #endregion

    #region Attributes
    /// <summary>
    /// Automatically implements a delegate command and
    /// attaches it to the method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class CommandMethod : Attribute
    {
        /// <summary>
        /// Specifies that the current method uses a parameter
        /// of type object. At the time of implementation,
        /// C# did not yet support generic attributes.
        /// </summary>
        public bool ObjectAsParameter { get; set; }
        /// <summary>
        /// Name of the command property to implement
        /// </summary>
        public string CommandName { get; }
        public CommandMethod(string commandName) => CommandName = commandName;
        public CommandMethod(string commandName, bool objectAsParameter) : this(commandName)
        {
            ObjectAsParameter = objectAsParameter;
        }
    }
    #endregion

    #region Strategy
    class AutomaticCommandStrategy : BuilderStrategy
    {
        public override void PostBuildUp(IBuilderContext context)
        {
            // Only work when the objects have been built.
            if (context.BuildKey.Type != typeof(object)) return;

            var methods = context.Existing.GetType().
                GetMethods(BindingFlags.InvokeMethod | BindingFlags.Public |
                BindingFlags.NonPublic | BindingFlags.Instance).
                Where(x => x.GetCustomAttributes<CommandMethod>().Any());

            var properties = context.Existing.GetType().GetProperties(
                BindingFlags.Public | BindingFlags.NonPublic |
                BindingFlags.GetProperty | BindingFlags.SetProperty |
                BindingFlags.Instance).
                Where(p => p.PropertyType.IsAssignableTo<ICommand>());

            foreach (var method in methods)
            {
                try
                {
                    var attribute = method.GetCustomAttribute<CommandMethod>();

                    var command = properties.FirstOrDefault(p =>
                    p.Name == attribute.CommandName);

                    if (!attribute.ObjectAsParameter)
                    {
                        Action methodAction = (Action)method.CreateDelegate(typeof(Action), context.Existing);

                        DelegateCommand delegateCommand = new DelegateCommand(methodAction);

                        if (command.CanWrite)
                            command.SetValue(context.Existing, delegateCommand);
                        else command.GetBackingField().SetValue(context.Existing, delegateCommand);
                    }
                    else
                    {
                        Action<object> methodAction = (Action<object>)method.
                            CreateDelegate(typeof(Action<object>), context.Existing);

                        DelegateCommand<object> delegateCommand = new DelegateCommand<object>(methodAction);

                        if (command.CanWrite)
                            command.SetValue(context.Existing, delegateCommand);
                        else command.GetBackingField().SetValue(context.Existing, delegateCommand);
                    }
                }
                catch(Exception ex)
                {
                    Core.Log.Debug($"An error occured while creating delegate command\n{ex}");
                }
               
            }
        }
    }
    #endregion


}
