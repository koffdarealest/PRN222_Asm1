using System.ComponentModel.DataAnnotations;

namespace PRN222.Assignment2.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập Username")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập Mật khẩu")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
