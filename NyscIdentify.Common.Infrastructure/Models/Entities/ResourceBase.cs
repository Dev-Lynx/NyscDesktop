using Newtonsoft.Json;
using NyscIdentify.Common.Infrastructure.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fody = PropertyChanged;

namespace NyscIdentify.Common.Infrastructure.Models.Entities
{
    [Fody.AddINotifyPropertyChangedInterface]
    public class ResourceBase : IResource
    {
        #region Properties
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Url { get; set; }
        public DateTime DateAdded { get; set; }
        public User User { get; set; }

        [NotMapped]
        public string UserIdentity { get; set; }
        public string LocalPath { get; set; }

        [NotMapped]
        public virtual bool IsBusy { get; set; }
        [NotMapped]
        public virtual string BusyMessage { get; set; }
        [NotMapped]
        public virtual double Progress { get; set; }
        [NotMapped]
        public virtual RequestStatus Status { get; set; }
        #endregion
    }
}
