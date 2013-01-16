using ApplicationLogic;
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
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void izlazToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            splitContainer1.VerticalScroll.Enabled = !splitContainer1.VerticalScroll.Enabled;

        }

        private void printToolStripButton_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {

        }

        private void otvoriKonzoluToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConsoleForm consoleForm = new ConsoleForm();
            consoleForm.Show();

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            RefreshTree();


        }

        private void RefreshTree()
        {
            List<PersonControl> persons = new List<PersonControl>();
            Person p = new Person(new System.Guid(), "Netko", "Netkic");
            Person p1 = new Person(new System.Guid(), "Bla", "Netkic");

            PersonControl c = new PersonControl(p, this);
            PersonControl c1 = new PersonControl(p1,this);
            persons.Add(c);
            persons.Add(c1);


            splitContainer1.Panel1.Controls.Add(c);

            splitContainer1.Panel1.Controls.Add(c1);
        }


        public void personSelected(Person p) {
            textBoxIme.Text = p.name;
            textBoxPrezime.Text = p.surname;


        }




    }
}
