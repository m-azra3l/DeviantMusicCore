using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeviantMusicCore.Models
{
    public class BlogItem
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Title { get; set; }

        [Required, StringLength(10000)]
        public string Article { get; set; }

        [DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MMM/yyyy}")]
        [DisplayName("Published On")]
        public DateTime PublishDate { get; set; }

        public byte[] PostImage { get; set; }

        [Required, StringLength(50)]
        public string BlogItemUrl { get; set; }

        public int Views { get; set; }

        [Required, ForeignKey("BlogCategory")]
        public int CategoryId { get; set; }

        public virtual BlogCategory BlogCategory { get; set; }

        public string AuthorId { get; set; }

        public virtual ApplicationUser Author { get; set; }
        
    }
}