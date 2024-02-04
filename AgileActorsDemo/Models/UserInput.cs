using System.ComponentModel.DataAnnotations;

namespace AgileActorsDemo.Models
{
    public class UserInput
    {
        [Required(AllowEmptyStrings = false)]
        public string Username { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Password { get; set; }
    }
}
