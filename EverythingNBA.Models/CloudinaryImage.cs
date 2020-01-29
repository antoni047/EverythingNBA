namespace EverythingNBA.Models
{
    using System.ComponentModel.DataAnnotations;

    public class CloudinaryImage
    {
        [Key]
        public int Id { get; set; }

        public string ImagePublicId { get; set; }

        public string ImageURL { get; set; }

        public string ImageThumbnailURL{ get; set; }

        public long Length { get; set; }
    }
}
