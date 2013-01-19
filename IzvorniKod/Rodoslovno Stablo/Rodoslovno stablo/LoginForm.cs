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
    public partial class LoginForm : Form
    {
        
        public LoginForm()
        {
            
            InitializeComponent();
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            bool result;
            result = SharedObjects.userManager.Login(textBoxUsername.Text, textBoxPassword.Text);

                //form.userManager.Login(textBoxUsername.Text, textBoxPassword.Text);
            if (result == true)
            {
                this.Dispose();

            }
            else
                MessageBox.Show("Pogrešno korisničko ime ili lozinka. Pokušajte ponovno.", "Prijava neuspješna", MessageBoxButtons.OK, MessageBoxIcon.Error);



        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();

        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();

        }
    }
}
