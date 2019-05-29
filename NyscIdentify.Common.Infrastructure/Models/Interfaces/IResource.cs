using NyscIdentify.Common.Infrastructure.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NyscIdentify.Common.Infrastructure.Models.Interfaces
{
    public interface IResource : IRequestStatusAware
    {
        bool IsBusy { get; set; }
        double Progress { get; set; }
        string Url { get; }
        string LocalPath { get; set; }
    }
}
