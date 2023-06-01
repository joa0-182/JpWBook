using System.ComponentModel.DataAnnotations;

namespace JpWBookAPI.DataTransferObjects
{
    public class LoginDto
    {
        [Required(ErrorMessage =" Email ou Nome de Usuário requerido")]
        public string Email { get; set;}

        [DataType(DataType.Password)]
        [Required(ErrorMessage =" A senha é requerida")]
        public string Password {get; set;}
        
    }
}