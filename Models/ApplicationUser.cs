using System;
using System.Collections.Generic;
using DeviantMusicCore.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeviantMusicCore.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required, StringLength(100)]
        public string Name { get; set; }

        public string StageName { get; set; }

        [Required, StringLength(1000)]
        public string Bio { get; set; }

        [Required, StringLength(1000)]
        public string Designation { get; set; }

        [Required, StringLength(1000)]
        public string Url { get; set; }

        public byte[] UserImage { get; set; }

        public bool IsArtiste { get; set; }
        
        public bool IsTeam { get; set; }

        [NotMapped]
        public bool IsAdmin { get; set; }

        [NotMapped]
        public bool IsSuperAdmin { get; set; }
        
        [NotMapped]
        public bool IsMaster { get; set; }

        [NotMapped]
        public bool IsMember { get; set; }

        [StringLength(1000)]
        public string SocialUrl { get; set; }
        
        public virtual ICollection<BlogItem> BlogItems { get; set; }

        public virtual ICollection<Beat> Beats { get; set; }
        
        public virtual ICollection<Product> Products { get; set; }

        public virtual ICollection<Software> Softwares { get; set; }
    }
}
