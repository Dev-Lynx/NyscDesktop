using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NyscIdentify.Common.Infrastructure.Extensions;
using Prism.Mvvm;
using Sieve.Attributes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using ZXing;

namespace NyscIdentify.Common.Infrastructure.Models.Entities
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum UserRole
    {
        [Description("Administrator")]
        Administrator,
        [Description("Coordinator")]
        Coordinator,
        [Description("Regular User")]
        RegularUser
    }

    public enum ApprovalStatus
    {
        Idle, Approved, Rejected
    }


    public class User : BindableBase
    {
        #region Properties
        public string Id { get; set; }
        public string Username { get; set; }
        [Sieve(CanFilter = true, CanSort = true, Name = "Last Name")]
        public string LastName { get; set; }
        [Sieve(CanFilter = true, CanSort = true, Name = "Other Names")]
        public string OtherNames { get; set; }
        [JsonIgnore]
        public string Password { get; set; }

        [Sieve(CanFilter = true, CanSort = true, Name = "File Number")]
        public int FileNo { get; set; }
        [JsonIgnore]
        public UserRole Role { get; set; }
        [Sieve(CanFilter = true, CanSort = true, Name = "State Of Origin")]
        public string StateOfOrigin { get; set; }
        [Sieve(CanFilter = true, CanSort = true, Name = "Date Of Birth")]
        public DateTime DateOfBirth { get; set; }

        [JsonIgnore]
        public int Age { get; }

        public string Qualification { get; set; }
        [Sieve(CanFilter = true, CanSort = true)]
        public string Gender { get; set; }
        [Sieve(CanFilter = true, CanSort = true)]
        public string Rank { get; set; }
        [Sieve(CanFilter = true, CanSort = true)]
        public string Department { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string Location { get; set; }

        [JsonIgnore]
        public string AccessToken { get; set; }

        string _displayName = string.Empty;
        public string DisplayName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_displayName))
                    return FullName;
                return _displayName;
            }
            set => _displayName = value;
        }

        public ICollection<Photo> Photos { get; set; } = new Collection<Photo>();

        public string PhoneNumber { get; set; }

        public ApprovalStatus ApprovalStatus { get; set; }

        [JsonIgnore]
        public bool PhoneNumberConfirmed { get; }
        [JsonIgnore]
        public string Initials => $"{OtherNames.FirstOrDefault()}{LastName.FirstOrDefault()}";
        [JsonIgnore]
        public string FullName => $"{LastName} {OtherNames}";
        [JsonIgnore]
        public Photo Passport => Photos.FirstOrDefault(p => p.Type == PhotoType.Passport);
        [JsonIgnore]
        public Photo Signature => Photos.FirstOrDefault(p => p.Type == PhotoType.Signature);

        [JsonIgnore]
        public BitmapSource BarcodeSource
        {
            get
            {
                try
                {
                    string data = JsonConvert.SerializeObject(this, Formatting.Indented);
                    BarcodeWriter writer = new BarcodeWriter();
                    writer.Format = BarcodeFormat.PDF_417;
                    var bitmap = writer.Write(data);
                    return bitmap.ToBitmapSource();
                }
                catch (Exception ex)
                {
                    Core.Log.Error($"An error occured while generating barcode for {this}.\n{ex}");
                }
                return null;
            }
        }
        #endregion

        #region Constructors
        public User() { }
        #endregion

        #region Methods

        #endregion
    }
}
