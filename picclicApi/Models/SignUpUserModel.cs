using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace picclicApi.Models
{
    public class SignUpUserModel
    {
        [Key]
        public string UserId { get; set; }
        [Index(IsUnique = true)]
        [StringLength(450)]
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}