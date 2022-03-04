using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using api.leitor.elogica.Models;
using api.leitor.elogica.Data;
using Microsoft.AspNetCore.Http;


namespace api.leitor.elogica.Controllers
{
/*---- Inicio da Controller----*/
    public class JsonController : ControllerBase
    {
        //Instancia do Repository
        private readonly Repository _repo = new Repository();


        public SuperHeroi[] superherois{get; set;}
        
        //Retorna a lista de superherois
        public SuperHeroi[] Get() 
        {
            return superherois;
        }

        //Recebe a lista do forms
        public SuperHeroi[] Post(SuperHeroi[] all) 
        {
            superherois = _repo.Adicionar(all);

            return superherois;
        }

        //Descriptografa todos os itens de um array
        public SuperHeroi[] decrypt(SuperHeroi[] all) 
        {
            superherois = _repo.decryptAll(all);

            return superherois;
        }

        //Cryptografa todos os mebros de um array
        public SuperHeroi[] crypt(SuperHeroi[] all) 
        { 
            superherois = _repo.cryptAll(all);

            return superherois;            
        }

        //Cryptografa todos um Heroi especifico pelo nome e idade
        public SuperHeroi cryptAlone(string nome, string idade)
         { 
            List<SuperHeroi> supers = superherois.ToList();

            SuperHeroi heroi = supers.Where(a => a.Idade == idade && a.Nome == nome).FirstOrDefault();

            if (heroi != null)
            {
                //Chama a função cryptAlone do Repository
                SuperHeroi heroiToload = _repo.cryptAlone(heroi);
                int index = supers.IndexOf(heroi);
                supers[index] = heroiToload;
                superherois = supers.ToArray();
                return heroiToload;
            }else{
                return heroi;
            }

         }

         //Descryptograda um item especifico
        public SuperHeroi decryptAlone(string nome, string idade)
         { 
            List<SuperHeroi> supers = superherois.ToList();

            SuperHeroi heroi = supers.Where(a => a.Idade == idade && a.Nome == nome).FirstOrDefault();

            if (heroi != null)
            {
                SuperHeroi heroiToload = _repo.decryptAlone(heroi);
                return heroiToload;
            }else{
                return heroi;
            }
         }

        //Função LoadToEdit que retorna o heroi para a janela de edição
        public SuperHeroi LoadToEdit(string nome, string idade){
            List<SuperHeroi> supers = superherois.ToList();

            SuperHeroi heroi = supers.Where(a => a.Idade == idade && a.Nome == nome).FirstOrDefault();

            if (heroi != null)
            {
                SuperHeroi heroiToload = _repo.LoadToEdit(heroi);
                return heroiToload;
            }else{
                return heroi;
            }
        }

        // Função que adiciona um novo SuperHeroi
        public SuperHeroi[] Create(SuperHeroi heroi)
        {

            heroi = _repo.cryptAlone(heroi);
            List<SuperHeroi> temporary = superherois.ToList();
            temporary.Add(heroi);
            superherois = temporary.ToArray();
            return superherois;
        }

        //Função que atualiza um SuperHeroi
        public SuperHeroi[] Put(int indexo, SuperHeroi heroi) 
        {
            heroi = _repo.cryptAlone(heroi);
            superherois[indexo] = heroi;
            
            return superherois;
        }

        //Função para Remover um SuperHeroi
        public SuperHeroi[] Remover(string nome, string idade) 
        { 
            List<SuperHeroi> supers = superherois.ToList();

            SuperHeroi heroi = supers.Where(a => a.Idade == idade && a.Nome == nome).FirstOrDefault();
            if (heroi != null)
            {

                supers = _repo.Remover(heroi, supers);
                superherois = supers.ToArray();
                return superherois;
            }else{
                return superherois;
            }

            
        }



      

    }
}