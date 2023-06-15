using System;
using System.ComponentModel.DataAnnotations;

namespace DeviantMusicCore.Models
{
    public class AdsB
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Title { get; set; }

        [Required, StringLength(1000)]
        public string Url { get; set; }

        [Required, StringLength(1000)]
        public string AlternateText { get; set; }

        [Required, StringLength(100)]
        public string NavigateUrl { get; set; }

        public byte[] ImageUrl { get; set; }

        [DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MMM/yyyy}")]
        public DateTime Date{ get; set; }

        public int Hits { get; set; }
             
    }
}