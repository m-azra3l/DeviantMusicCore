using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeviantMusicCore.Models
{
    public class Beat
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Required, StringLength(100), DisplayName("Title")]
        public string Title { get; set; }

        [Required, StringLength(1000), DisplayName("About")]
        public string Description { get; set; }

        [StringLength(1000), DisplayName("Stream Platform Url (If any, copy embed url and paste here)")]
        public string StreamPlatformUrl { get; set; }

        [DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MMM/yyyy}")]
        [StringLength(50), DisplayName("Released Date")]
        public DateTime ReleaseDate { get; set;}

        [DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MMM/yyyy}")]
        [DisplayName("Uploaded")]
        public DateTime UploadDate{ get; set;}

        public byte[] Image { get; set; }

        [Required, StringLength(50)]
        public string Url { get; set; }

        [DisplayName("Downloads")]
        public int DownloadCount { get; set; }

        [StringLength(1000)]
        public string AudioPath { get; set; }

        [StringLength(1000)]
        public string ContentType { get; set; }

        public byte[] Data { get; set; }

        [DisplayName("Price")]
        public double Price { get; set; }

        [StringLength(100), DisplayName("Producer")]
        public string ExternalProducer { get; set; }

        [StringLength(1000)]
        public string PurchaseURL { get; set; }

        [Required, ForeignKey("Genre"), DisplayName("Genre:")]
        public int GenreId { get; set; }

        public virtual Genre Genre { get; set; }

        [Required, ForeignKey("ExtrasLicense")]
        public int ExtrasLicenseId { get; set; }

        public virtual ExtrasLicense ExtrasLicense  { get; set; }

        [ForeignKey("ApplicationUser")]
        public string ProducerId { get; set; }

        public virtual ApplicationUser Producer { get; set; }
    }
}