using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BLL.DAL
{
    [Table("Games")]
    public class Game
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } 

        [Required]
        [StringLength(100, ErrorMessage = "Game name cannot exceed 100 characters.")]
        public string Name { get; set; } 

        public string photoUrl { get; set; } // Temporary game url 

        public DateTime? ReleaseDate { get; set; } 

        public decimal Price { get; set; } 

        // Publisher FK's
        public int PublisherId { get; set; } // Yayınevi Kimliği (Foreign Key)
        [ForeignKey("PublisherId")]
        public virtual Publisher Publisher { get; set; } // Yayınevi Referansı

        // Images 
        public virtual ICollection<Image> Images { get; set; } 
    }
}
