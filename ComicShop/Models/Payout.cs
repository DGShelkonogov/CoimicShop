using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ComicShop.Models
{
    public class Payout
    {

        [Key]
        public int Id { get; set; }

        [Required]
        public int Sum { get; set; }

        [Required]
        public Employee Employee { get; set; }
    }
}
