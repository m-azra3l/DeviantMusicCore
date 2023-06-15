using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DeviantMusicCore.Models
{
    public class SoftwareType
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string Type { get; set; }
        
        public virtual ICollection<Software> Softwares { get; set; }   
    }
}