using BLL.DAL;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BLL.DAL
{
    [Table("Images")]
    public class Image
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } 

        [Required]
        public byte[] Data { get; set; } // Binary data of image

        public string FileName { get; set; } // image.jpg 

        public string ContentType { get; set; } //  "image/jpeg..."

        // FK's
        public int GameId { get; set; } // Game Foreign Key
        [ForeignKey("GameId")]
        public Game game { get; set; } // Game Object as foreign 
    }
}


