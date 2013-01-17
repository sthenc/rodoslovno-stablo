using ApplicationLogic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Rodoslovno_stablo
{
    public partial class ConsoleForm : Form
    {
        TextWriter _writer = null;

        private static ApplicationLogic.QueryProcessor qpro;

        public QueryProcessor MyQueryProcessor
        {
            get
            {
                return qpro;
            }
        }
        public ConsoleForm()
        {
            InitializeComponent();
            textBox1.Select();

            qpro = new ApplicationLogic.QueryProcessor(QueryDisambiguator, GetLine);
        }
        private void ConsoleForm_Load(object sender, EventArgs e)
        {
            _writer = new TextBoxStreamWriter(textBoxOutput);
            Console.SetOut(_writer);
            

        }
       
        static Person QueryDisambiguator(IEnumerable<Person> kandidati, string pitanje = "")
        {
            // TODO resolvanje dvosmislenosti upita
            return kandidati.ElementAt(0);
        }
        static string GetLine()
        {
            return System.Console.ReadLine();
        }

        private void executeText(string request)
        {
            textBox1.Text = "";
            Console.WriteLine("> "+request);
            qpro.ProcessQuery(request);
            


        }

        private void buttonHelp_Click(object sender, EventArgs e)
        {
            executeText("help");
            textBox1.Select();



        }

        private void buttonExecute_Click(object sender, EventArgs e)
        {
            executeText(textBox1.Text);
            textBox1.Select();

        }

       

    }
}
