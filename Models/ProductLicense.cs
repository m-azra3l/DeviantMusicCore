using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace DeviantMusicCore.Models
{
    public class ProductLicense
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string License { get; set; }
        
        public virtual ICollection<Product> Products { get; set; }
    }
}