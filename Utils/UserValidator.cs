using FluentValidation;
using Tweet.Models;

namespace Tweet.Utils
{
    public class UserValidator: AbstractValidator<UserModel>
    {
        public UserValidator()
        {
            RuleSet("Login", () => {

                RuleFor(x => x.Email).NotEmpty().WithMessage("Digite um email para iniciar sessao").EmailAddress().WithMessage("Digite umemail válido");
                RuleFor(x => x.Password).NotEmpty().WithMessage("Digite uma senha").MinimumLength(8).WithMessage("No minimo 8 digitos");
            });
     
            RuleFor(x => x.Name).NotEmpty().Length(3, 30).WithMessage("Digite um nome valido, com min 3 caracteres");
            RuleFor(x => x.Email).EmailAddress().WithMessage("Precisa intrduzir um email valido").NotEmpty().WithMessage("Digite um email");
            RuleFor(x => x.Password).NotEmpty().MinimumLength(8).WithMessage("a senha precisa de 8 digitos no minimo");
        }
    }
    
}