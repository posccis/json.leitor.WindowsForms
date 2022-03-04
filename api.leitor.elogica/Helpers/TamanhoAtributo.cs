using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace api.leitor.elogica.Helpers
{
    public class TamanhoAtributo : AbstractValidator<List<string>>
    {



        public TamanhoAtributo()
        {
            RuleFor(lista => lista.Count).GreaterThan(0).LessThanOrEqualTo(10);
        }
        
        
    }
}