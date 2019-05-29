using Prism.Mvvm;
using Sieve.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NyscIdentify.Common.Infrastructure.Models.SieveModels
{
    #region Attributes
    [AttributeUsage(AttributeTargets.Field)]
    public class SieveOperator : Attribute
    {
        public object Value { get; set; }

        public SieveOperator(object value)
        {
            Value = value;
        }
    }
    #endregion

    public class SieveProperty : BindableBase
    {
        public string Name { get; }
        public string FullName { get; }

        public SieveProperty(string name, string fullName)
        {
            Name = name;
            FullName = fullName;
        }

        #region Methods
        public override string ToString() => Name;
        #endregion
    }
}
