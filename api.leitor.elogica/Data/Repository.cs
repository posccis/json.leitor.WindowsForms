using System.Collections.Generic;
using api.leitor.elogica.Helpers;
using api.leitor.elogica.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace api.leitor.elogica.Data
{
    public class Repository : IRepository
    {

        private Encrypt crypt = new Encrypt();
        public SuperHeroi[] SuperHerois;

        public SuperHeroi[] Adicionar(SuperHeroi[] ArrayOriginal) 
        { 


            return ArrayOriginal;
        }
        public SuperHeroi[] cryptAll(SuperHeroi[] ArrayOriginal) 
        { 
            for (int i = 0; i < ArrayOriginal.Length; i++)
            {

                crypt.Text = ArrayOriginal[i].IdentidadeSecreta;
                ArrayOriginal[i].IdentidadeSecreta = crypt.EncryptFunc();
            }

            return ArrayOriginal;
        }

        public SuperHeroi[] decryptAll(SuperHeroi[] ArrayOriginal) 
        { 
            for (int i = 0; i < ArrayOriginal.Length; i++)
            {

                crypt.Text = ArrayOriginal[i].IdentidadeSecreta;
                ArrayOriginal[i].IdentidadeSecreta = crypt.DesencryptFunc();
            }

            return ArrayOriginal;            
        }
        public SuperHeroi cryptAlone(SuperHeroi heroi)
        {
            crypt.Text = heroi.IdentidadeSecreta;
            heroi.IdentidadeSecreta = crypt.EncryptFunc();
            return heroi;
        }
        public SuperHeroi decryptAlone(SuperHeroi heroi) 
        { 
            crypt.Text = heroi.IdentidadeSecreta;
            heroi.IdentidadeSecreta = crypt.DesencryptFunc();
            return heroi;
        }
        public SuperHeroi LoadToEdit(SuperHeroi heroi) 
        {
            
            return heroi;
        }

        public List<SuperHeroi> Remover(SuperHeroi superHeroi, List<SuperHeroi> ArrayOriginal) 
        {
            
            if(ArrayOriginal.Remove(superHeroi)){
                
                return ArrayOriginal;
            };
            return ArrayOriginal;

        }




    }
}