using System.ComponentModel.DataAnnotations;

namespace BerberWeb.UI.Models
{
    public class CustomerCreateModel
    {
        [Required(ErrorMessage = "Email zorunludur")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "İsim boş geçilemez")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Soyadı boş geçilemez")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Şifre girmek zorunludur")]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Kullanıcı adı boş geçilemz")]
        public string UserName { get; set; }
    }
}
