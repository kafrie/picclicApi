using System.ComponentModel.DataAnnotations;

namespace picclicApi.Models
{
    public class SignUpUserModel
    {
        [Key]
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public virtual SignInModel SignInUser => new SignInModel
        {
            UserId = UserId,
            Password = Password
        };
    }
}