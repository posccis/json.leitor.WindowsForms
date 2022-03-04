using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using api.leitor.elogica.Helpers;
using FluentValidation;

namespace api.leitor.elogica.Models
{
    public class SuperHeroi
    {

        [Required]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "O nome precisa ter pelo menos 3 caracteres.")]
        public string Nome { get; set; }
        [Required]
        [StringLength(10, MinimumLength = 1, ErrorMessage = "A idade só pode possuir no máximo 10 digitos.")]
        public string Idade { get; set; }
        [Required]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "A identidade secreta precisa ter pelo menos 3 caracteres.")]
        public string IdentidadeSecreta { get; set; }
        [Required]
        public List<string> Poderes { get; set; }

        public SuperHeroi(string nome, string idade, string identidadeSecreta, List<string> poderes)
        {
            this.Nome = nome;
            this.Idade = idade;
            this.IdentidadeSecreta = identidadeSecreta;
            this.Poderes = poderes;
        }


    }
 
}

