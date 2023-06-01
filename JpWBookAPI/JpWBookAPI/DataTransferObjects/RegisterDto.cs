using System.ComponentModel.DataAnnotations;

namespace JpWBookAPI.DataTransferObjects
{
    public class RegisterDto
    {
        [Required(ErrorMessage ="Informme o Nome")]
        [StringLength(60, ErrorMessage = "O Nome deve possuir no máximo 60 caracteres")]
        public string Name { get; set;}

        [Required(ErrorMessage ="Informe o Email")]
        [EmailAddress(ErrorMessage ="Informe um Email Válido")]
        [StringLength(100, ErrorMessage = "O Email deve possuir no máximo 100 caracteres")]
        public string Email { get; set;}

        [DataType(DataType.Password)]
        [Required(ErrorMessage ="Informe a senha")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "A senha deve possuir no minimo 6 e no maximo 20 caracteres")]
        public string Password {get; set;}
        
    }
}