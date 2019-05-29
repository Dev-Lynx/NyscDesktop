using NyscIdentify.Common.Infrastructure.Models.Entities;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NyscIdentify.Common.Infrastructure.Models.ViewModels
{
    public class UserLoginViewModel : BindableBase
    {
        #region Properties
        public UserRole Role { get; set; } = UserRole.Coordinator;
        public string Username { get; set; }
        public string Password { get; set; }
        public string Origin { get; } = Core.ORIGIN;
        #endregion
    }
}
