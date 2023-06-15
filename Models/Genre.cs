using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DeviantMusicCore.Models
{
    public class Genre
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Required, StringLength(50), Display(Name = "Genre")]
        public string Name { get; set; }

        public virtual ICollection<Beat> Beats { get; set; }
        
        public virtual ICollection<Product> Products { get; set; }
    }
}