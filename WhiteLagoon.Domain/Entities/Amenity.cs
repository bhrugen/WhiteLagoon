using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhiteLagoon.Domain.Entities
{
    public class Amenity
    {
        [Key]
        public int Id { get; set; }
        
        public required string Name { get; set; }
        public string? Description { get; set; }


        [ForeignKey("Villa")]
        public int VillaId { get; set; }
        [ValidateNever]
        public Villa Villa { get; set; }
        
    }
}
