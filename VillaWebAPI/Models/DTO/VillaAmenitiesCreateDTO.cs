using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RoyalVilla.DTO
{
    internal class VillaAmenitiesCreateDTO
    {
        
        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }
        public string? Description { get; set; }

       
        public int VillaId { get; set; }
        
    }
}
