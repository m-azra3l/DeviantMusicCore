using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeviantMusicCore.Models
{
    public class Product
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Required, StringLength(100), DisplayName("Title:")]
        public string Title { get; set; }

        [StringLength(100),DisplayName("Artiste:")]
        public string ArtisteName{ get; set;}

        [Required, StringLength(1000), DisplayName("About:")]
        public string Description { get; set; }

        [DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MMM/yyyy}")]
        [StringLength(50), DisplayName("Released Date:")]
        public DateTime ReleaseDate { get; set;}

        [DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MMM/yyyy}")]
        [DisplayName("Uploaded:")]
        public DateTime UploadDate{ get; set;}

        public byte[] ProductImage { get; set; }

        [StringLength(1000), DisplayName("Stream Platform Url (If any, copy embed url and paste here)")]
        public string StreamPlatformUrl { get; set; }

        [StringLength(1000), DisplayName("Fanlink (For Sponsored Only)")]
        public string Fanlink { get; set; }

        [Required, StringLength(50)]
        public string Url { get; set; }

        [DisplayName("Downloads:")]
        public int DownloadCount { get; set; }

        [StringLength(1000)]
        public string AudioPath { get; set; }

        [StringLength(1000)]
        public string ContentType { get; set; }

        public byte[] Data { get; set; }

        [StringLength(100), DisplayName("Producer(s)")]
        public string Producer { get; set; }

        [StringLength(1000), DisplayName("Featured Artsiste(s)")]
        public string Featuring { get; set; }

        [StringLength(1000), DisplayName("Writer(s)")]
        public string Writer { get; set; }

        public int TrackNumber { get; set; }

        public bool AlbumTrack { get; set; }

        public bool EPTrack { get; set; }

        [StringLength(50)]
        public string Uploader { get; set; }

        public virtual Genre Genre { get; set; }
                  
        [ForeignKey("Genre"), DisplayName("Genre:")]
        public int GenreId { get; set; }

        public virtual ProductType ProductType { get; set; }

        [Required,ForeignKey("ProductType")]
        public int ProductTypeId { get; set; }

        public virtual ProductLicense ProductLicense  { get; set; }

        [Required, ForeignKey("ProductLicense")]
        public int ProductLicenseId { get; set; }

        [ForeignKey("Product")]
        public int AlbumorEPId { get; set; }

        public virtual Product AlbumorEP  { get; set; }

        [ForeignKey("ApplicationUser")]
        public string ArtisteId { get; set; }

        public virtual ApplicationUser Artiste { get; set; }

        public virtual ICollection<Product> MyProducts { get; set; }
    }
}