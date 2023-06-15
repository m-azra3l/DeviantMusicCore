using System.ComponentModel.DataAnnotations;

namespace DeviantMusicCore.Models
{
    public class Social
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [/*Index("IX_UniqueName", IsUnique = true),*/Required, StringLength(100)]
        public string SMName { get; set; }

        [Required, StringLength(100)]
        public string Url { get; set; }

        public byte[] SMImage { get; set; }
    }
}