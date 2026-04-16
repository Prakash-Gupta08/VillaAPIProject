using System.ComponentModel.DataAnnotations;

namespace VillaWebAPI.DTO
{
    public class UserDto
    {
        public int id { get; set; }
        public string Email { get; set; } = default!;
        public string Name { get; set; } = default!;
        public  string Role { get; set; } = default!;
      
    }
}
