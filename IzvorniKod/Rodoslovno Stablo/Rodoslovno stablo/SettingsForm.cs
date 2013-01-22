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
    public partial class SettingsForm : Form
    {
        MainForm f;
        public SettingsForm(MainForm fa)
        {
            f = fa;
            InitializeComponent();
            if (Properties.Settings.Default.theme == 1)
                radioButton1.Checked = true;
            else if (Properties.Settings.Default.theme == 2)
                radioButton2.Checked = true;
            else if (Properties.Settings.Default.theme == 3)
                radioButton3.Checked = true;
            else
                radioButton4.Checked = true;
            textBox1.Text = Properties.Settings.Default.workdirectory;

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            f.setTheme(1);


        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            f.setTheme(2);

        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            f.setTheme(3);
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            f.setTheme(4);
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.workdirectory = textBox1.Text;
            Properties.Settings.Default.Save();

            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                textBox1.Text = folderBrowserDialog1.SelectedPath;
        }
    }
}
