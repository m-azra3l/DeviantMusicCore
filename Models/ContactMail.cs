using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace DeviantMusicCore.Models
{
    public class ContactMail
    {
        [Required]
        public string emailTo { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Phone]
        public string Tel { get; set; }

        [Required]
        public string Message { get; set; }
    }
}