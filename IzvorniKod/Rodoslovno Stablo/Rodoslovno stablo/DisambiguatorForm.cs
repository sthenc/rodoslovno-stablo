using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Rodoslovno_stablo
{
    public partial class DisambiguatorForm : Form
    {
       
        public int  chosen;
        public ListBox listBox;


        public DisambiguatorForm()
        {
            

            InitializeComponent();
            listBox = listBoxChoices;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            chosen = listBoxChoices.SelectedIndex;
  
        }

        private void DisambiguatorForm_Load(object sender, EventArgs e)
        {

        }
    }
}
