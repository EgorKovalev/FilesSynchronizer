using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class AuthorizationModel
    {
        [Required]
        public string Code { get; set; }
    }

    public class RequestTokenModel
    {
        [Required]
        public string Token { get; set; }
    }
}
