using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeviantMusicCore.Models
{
    public class Subscriber
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Required, EmailAddress, StringLength(100), DisplayName("Email")]
        public string Email { get; set; }
    }
}