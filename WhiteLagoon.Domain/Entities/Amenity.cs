using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WhiteLagoon.Domain.Entities
{
    public class Amenity
    {
        public int Id { get; set; }

        public required string Name { get; set; }
        public string? Description { get; set; }

        [ForeignKey("Villa")]
        public int VillaId { get; set; }
        // To remove it from model state validation
        [ValidateNever]
        public Villa Villa { get; set; }
    }
}
