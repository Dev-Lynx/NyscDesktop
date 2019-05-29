using Sieve.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NyscIdentify.Common.Infrastructure.Extensions;

namespace NyscIdentify.Common.Infrastructure.Models.SieveModels
{
    #region Enumerations
    public enum SieveSortOperator
    {
        [SieveOperator("")]
        Ascending,
        [SieveOperator("-")]
        Descending
    }
    #endregion

    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class SieveSortTerm : SortTerm
    {
        #region Properties
        public SieveProperty Property { get; set; }
        public SieveSortOperator SortOperator { get; set; }
        #endregion

        #region Methods

        #region Overrides
        public override string ToString()
        {
            object @operator = SortOperator.GetAttribute<SieveOperator>().Value;
            return $"{@operator}{Property.FullName}";
        }
        #endregion

        #endregion
    }
}
