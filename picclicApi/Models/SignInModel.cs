using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace picclicApi.Models
{
    public class SignInModel
    {
        [Key]
        public string UserId { get; set; }
        public string Password { get; set; }

        [ForeignKey("UserId")]
        internal virtual SignUpUserModel SignUpUser { get; set; }
    }
}