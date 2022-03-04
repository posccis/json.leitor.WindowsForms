using api.leitor.elogica.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace interfaceg.elogica
{
    public partial class Form2 : Form
    {
        public delegate void EditHandler(object sender, EventArgs e);
        public event EditHandler OnEdit;
        public Form2()
        {
            InitializeComponent();
        }

        private void Cancelbtn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Salvarbtn_Click(object sender, EventArgs e)
        {
            
            Alerta_Clicked();
        }

        private void Alerta_Clicked()
        {
            EventArgs args = new EventArgs();

            OnEdit.Invoke(this, args);
        }

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {

            
        }
    }
}
