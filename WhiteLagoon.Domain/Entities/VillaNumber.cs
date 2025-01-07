using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace WhiteLagoon.Domain.Entities
{
    public class VillaNumber
    {
        // this will not generate identity column
        // make sure to add unique value as it is primary key
        [Key,DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Villa_Number { get; set; }

        [ForeignKey("Villa")]
        public int VillaId { get; set; }

        // To remove it from model state validation
        [ValidateNever]
        public Villa Villa { get; set; }

        public string? SpecialDetails { get; set; }
    }
}
