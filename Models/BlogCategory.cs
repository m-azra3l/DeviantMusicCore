using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeviantMusicCore.Models
{
    public class BlogCategory
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Category { get; set; }

        public virtual ICollection<BlogItem> BlogItems { get; set; }
    }
}