using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeviantMusicCore.Models
{
    public class Software
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Required, StringLength(100), DisplayName("Title:")]
        public string Name { get; set; }
        
        [Required, StringLength(1000), DisplayName("About:")]
        public string Description { get; set; }

        [DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MMM/yyyy}")]
        [StringLength(50), DisplayName("Released Date:")]
        public DateTime ReleaseDate { get; set;}

        [DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MMM/yyyy}")]
        [DisplayName("Uploaded:")]
        public DateTime UploadDate{ get; set;}

        public byte[] Image { get; set; }

        [Required, StringLength(50)]
        public string Url { get; set; }

        [DisplayName("Downloads:")]
        public int DownloadCount { get; set; }

        [StringLength(1000)]
        public string FilePath { get; set; }

        [StringLength(1000)]
        public string ContentType { get; set; }

        public byte[] Data { get; set; }

        [DisplayName("Price")]
        public double Price { get; set; }

        [StringLength(100), DisplayName("Developer(s)")]
        public string ExternalDeveloper { get; set; }

        [StringLength(1000)]
        public string PurchaseURL { get; set; }

        [StringLength(1000)]
        public string DownloadURL { get; set; }


        [StringLength(50)]
        public string Uploader { get; set; }

        [Required, ForeignKey("ExtrasLicense")]
        public int ExtrasLicenseId { get; set; }

        public virtual ExtrasLicense ExtrasLicense  { get; set; }

        [Required,ForeignKey("SoftwareType")]
        public int SoftwareTypeId { get; set; }       

        public virtual SoftwareType SoftwareType { get; set; }

        [ForeignKey("ApplicationUser")]
        public string DeveloperId { get; set; }

        public virtual ApplicationUser Developer { get; set; }
    }
}