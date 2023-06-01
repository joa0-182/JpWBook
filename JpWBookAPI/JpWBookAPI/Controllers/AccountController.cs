using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using JpWBookAPI.Data;
using JpWBookAPI.DataTransferObjects;
using JpWBookAPI.Helpers;
using JpWBookAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JpWBookAPI.Controllers;

    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] LoginDto login)
        {
            if (login == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest();

            AppUser user = new();

            if(IsEmail(login.Email))
            {
                // acha o usuario pelo email
                user = await _context.Users.FirstOrDefaultAsync(
                    u => u.Email.Equals(login.Email)
                );
            }
            else
            {
                // acha o usuario pelo username
                user = await _context.Users.FirstOrDefaultAsync(
                    u => u.UserName.Equals(login.Email)
                );
            };

            if (user == null)
                return NotFound(new { Message = "Usuário e/ou Senha inválidos !!!"});


            if (!PasswordHasher.VerifyPassword(login.Password, user.Password))
                return NotFound(new { Message = "Usuário e/ou Senha inválidos !!!"});

            return Ok(new {Message = "Usuário Autenticado"});
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto register)
        {
            if (register == null)
            return BadRequest();

            if (!ModelState.IsValid)
            return BadRequest();

            //checar se o email já existe
            if (await _context.Users.AnyAsync(u => u.Email.Equals(register.Email)))
                return BadRequest(new {Message = "Email já cadastrado! Tente recuperar sua senha ou entre em contato com os Administradores"});

            // checar a força da senha
            var pass = CheckPasswordStrength(register.Password);
            if (!string.IsNullOrEmpty(pass))
                return BadRequest( new { Message = pass});

            // criar o usuario

            return Ok ();
        }


        private bool IsEmail(string email)
        {
            try
            {
                MailAddress mail = new(email);
                return true;
            }

            catch
            {
                return false;
            }
        }

        private string CheckPasswordStrength(string password)
        {
            StringBuilder sb = new();
            if (password.Length < 6)
                sb.Append("A senha deve possuir no minimo 6 Caracteres " + Environment.NewLine);

            if (!(Regex.IsMatch(password, "[a-z]") && Regex.IsMatch(password, "[A-Z]")
                && (Regex.IsMatch(password, "[0-9]"))))
                sb.Append("A senha deve ser alfanumérica " + Environment.NewLine);
            if (!Regex.IsMatch(password, "[<,>,!,@,#,$,%,&,*,(,),_,+,-,/,=,:,;,|,\\,?,{,},`,^,~,\\],\\[,."))
                sb.Append("A senha deve conter um caracter especial " + Environment.NewLine);
                return sb.ToString();

        }
    }
