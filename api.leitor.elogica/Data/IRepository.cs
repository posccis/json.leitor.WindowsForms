using System.Collections.Generic;
using api.leitor.elogica.Models;

namespace api.leitor.elogica.Data
{
    public interface IRepository
    {

        public SuperHeroi[] Adicionar(SuperHeroi[] t);
        public SuperHeroi cryptAlone(SuperHeroi t);
        public SuperHeroi LoadToEdit(SuperHeroi t);
        public List<SuperHeroi> Remover(SuperHeroi superheroi, List<SuperHeroi> t);
    }
}