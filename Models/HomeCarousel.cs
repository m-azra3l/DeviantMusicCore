using System.ComponentModel.DataAnnotations;

namespace DeviantMusicCore.Models
{
    public class HomeCarousel
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        public byte[] CarouselImage { get; set; }
    }
}