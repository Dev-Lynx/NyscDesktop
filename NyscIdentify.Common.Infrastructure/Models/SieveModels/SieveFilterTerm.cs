using NyscIdentify.Common.Infrastructure.Extensions;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NyscIdentify.Common.Infrastructure.Models.SieveModels
{
    #region Enumerations
    public enum SieveFilterOperator
    {
        [Description("Equal To")]
        [SieveOperator("==")]
        Equals = 0,
        [Description("Not Equal To")]
        [SieveOperator("!=")]
        NotEquals = 1,
        [Description("Greater Than")]
        [SieveOperator(">")]
        GreaterThan = 2,
        [Description("Less Than")]
        [SieveOperator("<")]
        LessThan = 3,
        [Description("Greater Than Or Equal To")]
        [SieveOperator(">=")]
        GreaterThanOrEqualTo = 4,
        [Description("Less Than Or Equal To")]
        [SieveOperator("<=")]
        LessThanOrEqualTo = 5,
        [Description("Contains")]
        [SieveOperator("@=")]
        Contains = 6,
        [Description("Starts With")]
        [SieveOperator("_=")]
        StartsWith = 7,
        [Description("Does Not Contain")]
        [SieveOperator("!@=")]
        DoesNotContain = 8,
        [Description("Does Not Start With")]
        [SieveOperator("!_=")]
        DoesNotStartWith = 9
    }
    #endregion

    [PropertyChanged.AddINotifyPropertyChangedInterface]
    public class SieveFilterTerm : FilterTerm
    {
        #region Properties

        public SieveProperty Property { get; set; }
        public SieveFilterOperator FilterOperator { get; set; }
        public string FilterValue { get; set; }

        #endregion

        #region Methods
        public override string ToString()
        {
            object @operator = FilterOperator.GetAttribute<SieveOperator>().Value;
            return $"{Property.FullName}{@operator}{FilterValue}";
        }
        #endregion
    }
}
