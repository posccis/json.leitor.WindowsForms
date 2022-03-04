using api.leitor.elogica.Controllers;
using api.leitor.elogica.Helpers;
using api.leitor.elogica.Models;
using interfaceg.elogica;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Windows.Forms;



namespace leitor_json.elogica
{
    public partial class Form1 : Form
    {
/* ---- Inicio do Forms ----*/

        //Criando a lista que vai será usada por todo o forms
        public SuperHeroi[] superHeroisList;
        //Instanciando o OpenFile
        OpenFileDialog openFile = new OpenFileDialog();
        //Instanciando a controller
        public JsonController jsonController = new JsonController();
        //Variavel que irá receber o valor da ação do OpenFile
        DialogResult resultado;
        //Variável para armazenar o nome do arquivo
        string nameFile;








        public Form1()
        {
            InitializeComponent();

        }
        //Evento de CLIQUE do botão CARREGAR
        private void button1_Click(object sender, EventArgs e)
        {
            //Chamada da função Carrega
            Carrega();
            //Caso o resultado do openfile tiver sido OK ou seja tiver aberto algum arquivo...
            if (resultado == DialogResult.OK) 
            {
                //...essas funções serão chamadas
                Post();
                checkTable();
                Get();
                CriaLista();
            };
 
        }
        //Função Carrega
        private void Carrega()
        {
            //Receber apenas arquivos JSON
            openFile.Filter = "Json Files (*.json) | *.json";
            //Variável para receber o conteudo do arquivo
            string fileContent = string.Empty;
            resultado = openFile.ShowDialog();
            //Caso tenha recebido o arquivo...
            if (resultado == DialogResult.OK)
            {
                //...ele armazena o arquivo...
                Stream resultao = openFile.OpenFile();
                nameFile = openFile.FileName;

                
                
                using (StreamReader reader = new StreamReader(resultao))
                {
                    //...e em seguida lê e armazena o conteudo
                    fileContent = reader.ReadToEnd();

                }
                //Pegando cada objeto dentro do arquivo que seja uma instancia da classe SuperHeroi
                superHeroisList = JsonConvert.DeserializeObject<SuperHeroi[]>(fileContent);
            }


        }
        //Função Post
        //Apenas para adicionar a lista do forms para a lista da controller instanciada
        private void Post()
        {
            jsonController.Post(superHeroisList);

        }
        //Função Get
        //Apenas para obter a lista da controller instanciada
        private void Get()
        {
            superHeroisList = jsonController.Get();
        }
        //Função Remove
        public void Remove(string nome, string idade)
        {
            //Chama a função Remover da controller
            //que irá procurar o SuperHeroi com esse nome e essa idade para ser removido
            SuperHeroi[] retorno = jsonController.Remover(nome, idade);
            if(retorno == superHeroisList)
            {
                MessageBox.Show("Ocorreu um erro.\nProvavelmente o objeto não existe no arquivo.");
            }
            else if (retorno != superHeroisList)
            {
                superHeroisList = retorno;
            }
            //Em seguida chama a função CriaLista para atualizar a lista do forms
            CriaLista();
            

        }

        private void CriaLista()
        {
            //Confere se a lista está vazia antes de colocar a lsita atualizada
            checkTable();
 

            //Adiciona para cada linha do DataGridView o nome e a idade do objeto
            for (int i = 0; i < superHeroisList.Length; i++)
            {
                dataGridView1.Rows.Add(superHeroisList[i].Nome, superHeroisList[i].Idade);
                

            }

        }
        //Função checkTable
        //Caso a tabela esteja populada, a função irá chamar a função Clear
        private void checkTable()
        {
            if(dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Rows.Clear();
            }
        }


        //Função LoadToEdit
        private SuperHeroi LoadToEdit(string nome, string idade) 
        {
            //Apenas localiza o heroi dentro da lista para a janela de edição
            SuperHeroi descryptedHeroi = jsonController.LoadToEdit(nome, idade);
            
            return descryptedHeroi;
        }

        //Função Update
        private void Update(int indexo, SuperHeroi heroi)
        {
            //Chama a função update da controller que irá localizar o heroi e atualiza-lo
            jsonController.Put(indexo, heroi);
            //Função Get() para pegar a lista atualizada
            Get();
            
            
        }

        //Função Create
        private void Create(SuperHeroi superHeroi)
        {
            //Chama a função Create da controller
            //Onde recebe um SuperHeroi e o adiciona a lista
            jsonController.Create(superHeroi);
            
            Get();
            
        }

        //Função ConvertList
        //Serve para preparar os dados para serem salvos
        private string ConvertList()
        {
            //Para cada SuperHeroi na lista
            //vai ser checado se já foi criptografado
            foreach(SuperHeroi heroi in superHeroisList)
            {
                try
                {
                    jsonController.decryptAlone(heroi.Nome, heroi.Idade);
                }
                catch (System.FormatException)
                {

                
                }
                finally
                {
                    jsonController.cryptAlone(heroi.Nome, heroi.Idade);
                }
            }

            
            //Dados sendo convertidos para Json
            string datateste = JsonConvert.SerializeObject(superHeroisList);
            
            return datateste;
            
        }

        //Função SaveFile
        //Salva definitivamento os dados
        private void SaveFile(string datafile)
        {
            //Cria um arquivo tempoarario para receber os dados
            string path = Environment.CurrentDirectory + "/" + "filetemp.json";
            FileStream temporaryfile = File.Create(path);
            
            

            using(StreamWriter sw = new StreamWriter(temporaryfile))
            {
                sw.Write(datafile);
            }
            //Em seguida o arquivo original é sobrescrito pelo temporario
            File.Replace(path, nameFile, null);
        }

        
        //Função de CLIQUE do Excluirbtn
        private void Excluirbtn_Click(object sender, EventArgs e)
        {
            //Confere se existe algum dado a ser excluido
            if (nameFile == null)
            {
                MessageBox.Show("Nenhum arquivo foi carregado ainda para que algo seja excluido.\nClique em CARREGAR para carregar um arquivo antes.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewSelectedRowCollection selected = dataGridView1.SelectedRows;
            //Para cada dado selecionado ele aplica a remoção
            foreach (DataGridViewRow item in selected)
            {
                if(item.Cells[0] == null)
                {
                    MessageBox.Show("Selecione um item");
                    break;
                }

                string nome = item.Cells[0].Value.ToString();
                string idade = item.Cells[1].Value.ToString();
                Remove(nome, idade);
                nome = string.Empty;
                idade = string.Empty;
            }

        }

        //Função de CLIQUE do Editarbtn
        private void Editarbtn_Click(object sender, EventArgs e)
        {
            if (nameFile == null)
            {
                MessageBox.Show("Nenhum arquivo foi carregado ainda para que algo seja editado.\nClique em CARREGAR para carregar um arquivo antes.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DataGridViewCellCollection selected = dataGridView1.CurrentRow.Cells;

            //Confere se alguma linha foi selecionada para ser editada
            foreach (DataGridViewCell item in selected)
            {
                if (item.Value == null)
                {
                    MessageBox.Show("Selecione um item");
                    break;
                }

            }

            string nome = selected[0].Value.ToString();
            string idade = selected[1].Value.ToString();
            SuperHeroi decrypted = LoadToEdit(nome, idade);
            //Confere se a Identidade Secreta precisa ser descriptografada
            try
            {
                decrypted = jsonController.decryptAlone(nome, idade);
            }
            catch (System.FormatException)
            {
                LoadToEdit(nome, idade);
            }
            

            


            nome = string.Empty;
            idade = string.Empty;
            //Cria a janela de edição
            //e preenche os TextBoxs com os dados do Heroi a ser editado
            Form2 form2 = new Form2();
            form2.Text = "Editar";
            form2.textBox1.Text = decrypted.Nome;
            form2.textBox2.Text = decrypted.IdentidadeSecreta;
            form2.textBox3.Text = decrypted.Idade.ToString();
            foreach (string item in decrypted.Poderes)
            {
                form2.dataGridView1.Rows.Add(item);
            }

            form2.Show();
            DataGridViewRow atual = dataGridView1.CurrentRow;
            int indexdata = dataGridView1.Rows.IndexOf(atual);
            
            //Evento de CLIQUE do botão SALVAR da janela de edição
            form2.OnEdit +=(sender, e) =>
            {

                string nome = form2.textBox1.Text;

                string identidadeScrt = form2.textBox2.Text;
                
                string idade_def = form2.textBox3.Text;

                



                
                DataGridViewRowCollection selected = form2.dataGridView1.Rows;

                List<string> poderes = new List<string>();

                //Criação da lista de poderes
                foreach (DataGridViewRow item in selected)
                {
                    if (item.Cells[0].Value != null)
                    {
                        poderes.Add(item.Cells[0].Value.ToString());
                    }
                }
                //Validação da lista de poderes
                TamanhoAtributo validationRules = new TamanhoAtributo();
                var result = validationRules.Validate(poderes);
                if (!result.IsValid)
                {
                    foreach(var erro in result.Errors)
                    {
                        
                        MessageBox.Show("É necessário pelo menos 1 poder ou no máximo 10 poderes.","Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    
                }

                //Validação de todos os outros atributos via DataAnnotation
                SuperHeroi heroi = new SuperHeroi(nome, idade_def, identidadeScrt, poderes);
                ValidationContext validationContext = new ValidationContext(heroi, null, null);
                IList<ValidationResult> errors = new List<ValidationResult>();
                if (!Validator.TryValidateObject(heroi, validationContext, errors, true))
                {
                    foreach(ValidationResult error in errors)
                    {
                        MessageBox.Show(error.ErrorMessage, "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                        
                    }
                }
                else
                {
                    //Caso não tenha ocorrido nenhum erro
                    //tudo é atualizado
                    DataGridViewRow atual = dataGridView1.CurrentRow;
                    int indexdata = dataGridView1.Rows.IndexOf(atual);

                    Update(indexdata, heroi);
                    form2.Close();
                    CriaLista();
                    Get();
                }

            };
            //Tudo é atualizado mesmo que nada seja alterado
            Update(indexdata, decrypted);





        }

        //Função de CLIQUE do Inserirbtn
        //tem a função de inserir novos Herois
        private void Inserirbtn_Click(object sender, EventArgs e)
        {
            //Confere se algum arquivo foi carregado
            if (nameFile == null)
            {
                MessageBox.Show("Nenhum arquivo foi carregado ainda para que algo seja inserido.\nClique em CARREGAR para carregar um arquivo antes.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Form2 form2 = new Form2();
            form2.Text = "Inserir";
            form2.textBox1.Text = String.Empty;
            form2.textBox2.Text = String.Empty;
            form2.textBox3.Text = String.Empty;
            form2.Show();
            form2.OnEdit += (sender, e) =>
            {
                string nome = form2.textBox1.Text;
                string identidadeScrt = form2.textBox2.Text;
                string idade = form2.textBox3.Text;

                DataGridViewRowCollection selected = form2.dataGridView1.Rows;
                List<string> poderes = new List<string>();

                foreach(DataGridViewRow item in selected)
                {
                    if (item.Cells[0].Value != null)
                    {
                        poderes.Add(item.Cells[0].Value.ToString());
                    }
                }
                TamanhoAtributo validationRules = new TamanhoAtributo();
                var result = validationRules.Validate(poderes);
                if (!result.IsValid)
                {
                    foreach (var erro in result.Errors)
                    {

                        MessageBox.Show("É necessário pelo menos 1 poder ou no máximo 10 poderes.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                }


                SuperHeroi heroi = new SuperHeroi(nome, idade, identidadeScrt, poderes);
                ValidationContext validationContext = new ValidationContext(heroi, null, null);
                IList<ValidationResult> errors = new List<ValidationResult>();
                if (!Validator.TryValidateObject(heroi, validationContext, errors, false))
                {
                    foreach (ValidationResult error in errors)
                    {
                        MessageBox.Show(error.ErrorMessage, "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;

                    }
                }
                else
                {
                    Create(heroi);
                    form2.Close();
                    CriaLista();
                    Get();
                }



            };

        } 

        //Função de CLIQUE do button4
        //tem a tarefa de salvar o arquivo chamando as funções de ConvertList e SavFile
        private void button4_Click(object sender, EventArgs e)
        {
            if (nameFile == null)
            {
                MessageBox.Show("Não há dados ou arquivo para ser salvo.\nClique em carregar para adicionar um arquivo Json.", "Messagem", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string datafile = ConvertList();
                SaveFile(datafile);
                MessageBox.Show("Arquivo salvo com sucesso!", "Messagem", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }


        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
