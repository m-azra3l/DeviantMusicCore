using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeviantMusicCore.Models
{
    public class Download
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string DownloadTitle { get; set; }

        [StringLength(500)]
        public string Url { get; set; }

        [StringLength(50)]
        public string DownloadToken { get; set; }

        public int? Hits { get; set; }

        public virtual bool ExpireAfterDownload { get; set; }

        public virtual bool Downloaded { get; set; }

        public virtual Beat Beat { get; set; }

        [Required, ForeignKey("Beat")]
        public int BeatId { get; set; }

    }
}