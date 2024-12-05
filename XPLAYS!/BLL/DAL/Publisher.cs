

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DAL

{
    [Table("Publishers")]
    public class Publisher
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } 

        [Required]
        [StringLength(100, ErrorMessage = "Publisher name cannot exceed 100 characters.")]
        public string Name { get; set; } 


        // Games ile ilişkisi
        public virtual ICollection<Game> Games { get; set; } // Yayınevinin Yayınladığı Oyunlar
    }
}

