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
    public partial class QueriesForm : Form
    {
        public QueriesForm()
        {
            InitializeComponent();
        }

        private void QueriesForm_Load(object sender, EventArgs e)
        {
            IEnumerable<Query> queries = SharedObjects.userManager.GetQueries();
            List<string[]> list = new List<string[]>();
	   

	    

            foreach (Query item in queries){
                User u = SharedObjects.userManager.GetUser(item.UserID);
                list.Add(new string[] { item.ID.ToString(), u.name, u.surname, item.command });   
            }
            DataTable table = ConvertListToDataTable(list);
            dataGridView.AutoSize = true;
            dataGridView.DataSource = table;
            


        }
        static DataTable ConvertListToDataTable(List<string[]> list)
        {
            // New table.
            DataTable table = new DataTable();
            
            // Get max columns.
            int columns = 0;
            foreach (var array in list)
            {
                if (array.Length > columns)
                {
                    columns = array.Length;
                }
            }

            
            table.Columns.Add("Broj", typeof(string) );
            table.Columns.Add("Ime", typeof(string));
            table.Columns.Add("Prezime", typeof(string));
            table.Columns.Add("Upit", typeof(string));
            
            // Add rows.
            foreach (var array in list)
            {
                table.Rows.Add(array);
            }
            
            return table;
        }

        private void buttonCloseQueries_Click(object sender, EventArgs e)
        {
            this.Dispose();

        }
    }
}
