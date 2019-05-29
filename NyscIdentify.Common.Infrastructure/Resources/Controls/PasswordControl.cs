using NyscIdentify.Common.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace NyscIdentify.Common.Infrastructure.Resources.Controls
{
    public class PasswordControl : Control
    {
        #region Properties

        #region Dependency Properties



        public string PasswordHint
        {
            get { return (string)GetValue(PasswordHintProperty); }
            set { SetValue(PasswordHintProperty, value); }
        }

        public static readonly DependencyProperty PasswordHintProperty =
            DependencyProperty.Register("PasswordHint", typeof(string), typeof(PasswordControl), new PropertyMetadata("Password"));



        public SecureString Password
        {
            get { return (SecureString)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }

        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(SecureString),
                typeof(PasswordControl), new UIPropertyMetadata(new SecureString(), 
                    (s, e) =>
                    {
                        if (!(s is PasswordControl)) return;
                        if (!(e.NewValue is SecureString)) return;

                        SecureString password = (SecureString)e.NewValue;

                        PasswordControl control = (PasswordControl)s;

                        if (control.InternalEdit)
                        {
                            control.InternalEdit = false;
                            return;
                        }

                        if (!control.IsActive) return;

                        string simplePassword = password.ToSimpleString();
                        if (control.ShowPassword)
                        {
                            control.InternalBox.Text = simplePassword;
                        }
                        else
                        {
                            control.PasswordBox.Password = simplePassword;
                        }
                    }));



        public bool ShowPassword
        {
            get { return (bool)GetValue(ShowPasswordProperty); }
            set { SetValue(ShowPasswordProperty, value); }
        }

        public static readonly DependencyProperty ShowPasswordProperty =
            DependencyProperty.Register("ShowPassword", typeof(bool),
                typeof(PasswordControl), new FrameworkPropertyMetadata(false,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, (s, e) => 
                    {

                        if (!(s is PasswordControl)) return;

                        PasswordControl control = (PasswordControl)s;
                        bool showPassword = e.NewValue is bool ? (bool)e.NewValue : false;

                        if (showPassword && control.IsActive)
                        {
                            control.InternalBox.Text = control.PasswordBox.Password;
                            Keyboard.Focus(control.InternalBox);
                        }
                        else if (control.IsActive)
                        {
                            control.PasswordBox.Password = control.InternalBox.Text;
                            Keyboard.Focus(control.PasswordBox);
                        }
                            
                    }));

        #endregion

        #region Internals
        bool IsActive { get; set; }
        bool InternalEdit { get; set; }
        TextBox InternalBox { get; set; }
        PasswordBox PasswordBox { get; set; }
        #endregion

        #endregion

        #region Methods

        #region Overrides
        public override void OnApplyTemplate()
        {
            try
            {
                InternalBox = (TextBox)GetTemplateChild("InternalBox");
                PasswordBox = (PasswordBox)GetTemplateChild("PasswordBox");

                PasswordBox.PasswordChanged += (s, e) =>
                {
                    InternalEdit = true;
                    Password = PasswordBox.SecurePassword;
                };

                InternalBox.TextChanged += (s, e) =>
                {
                    if (!ShowPassword) return;

                    SecureString password = new SecureString();
                    foreach (char c in InternalBox.Text.ToCharArray())
                        password.AppendChar(c);
                    Password = password;
                };

                Focusable = false;
                IsActive = true;
            }
            catch (Exception ex)
            {
                Core.Log.Error($"An error occured while apply templates to PasswordControl\n{ex}");
            }
           

            base.OnApplyTemplate();
        }
        #endregion

        #endregion
    }
}
