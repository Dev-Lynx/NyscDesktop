using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NyscIdentify.Common.Infrastructure.Models.ViewModels
{
    public enum Activity
    {
        AccountCreated,
        AccountUpdated,
        UpdateRequested,
        AccountApproved,
    }

    public class SimpleUserActivity
    {
        public Activity ActivityType { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
    }
}
