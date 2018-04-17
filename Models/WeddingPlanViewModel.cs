
using System.ComponentModel.DataAnnotations;
using System;

namespace WeddingPlanner.Models
{
    public class WeddingPlanViewModel : BaseEntity
    {
        [Display(Name = "Wedder One")]
        [Required]
        [MinLength(2)]
        [RegularExpression(@"^[a-zA-Z]+$")]
        public string Wedder1 { get; set; }

        [Display(Name = "Wedder Two")]
        [Required]
        [MinLength(2)]
        [RegularExpression(@"^[a-zA-Z]+$")]
        public string Wedder2 { get; set; }

        [Display(Name = "Date")]
        [Required]
        [CurrentDate(ErrorMessage = "Date must be after or equal to current date")]
        public DateTime WeddingDate { get; set; }

        [Display(Name = "Wedding Address")]
        [Required]
        [MinLength(8)]
        public string Address { get; set; }
    }
}