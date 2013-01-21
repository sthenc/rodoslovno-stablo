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
    public partial class UsersForm : Form
    {
        User activeUser,admin, u1,u2,u3;

        User currentlySelected;
        public UsersForm()
        {
            InitializeComponent();
            activeUser = SharedObjects.userManager.GetActiveUser();
            admin = SharedObjects.userManager.GetUser(1);
            u1 = SharedObjects.userManager.GetUser(2);
            u2 = SharedObjects.userManager.GetUser(3);
            u3 = SharedObjects.userManager.GetUser(4);


            if (!activeUser.isAdmin) {
                buttonAdmin.Enabled = false;
                if (!(activeUser.ID==2))
                    button2.Enabled = false;
                if (!(activeUser.ID == 3))
                    button3.Enabled = false;
                if (!(activeUser.ID == 4))
                    button4.Enabled = false;
            }
        }
        public void personSelected(User p)
        {
            currentlySelected = p;
            if (currentlySelected.isAdmin)
                groupBox1.Enabled = false;
            else
                groupBox1.Enabled = true;

            textBoxIme.Text = p.name;
            textBoxPrezime.Text = p.surname;
            maskedTextBoxDate.Text = dateToString(p.birthDate);
            maskedTextBoxDeath.Text = dateToString(p.deathDate);
            textBoxAddress.Text = p.address;
            textBoxCV.Text = p.CV;
            textBoxTelefon.Text = p.phone;
            textBoxEmail.Text = p.email;

            if (p.sex == Person.Sex.Male)
                radioButtonMale.Checked = true;
            else if (p.sex == Person.Sex.Female)
                radioButtonFemale.Checked = true;
            else
                radioButtonUnkown.Checked = true;
            //dodavanje supruznika
            /*
            IEnumerable<Person> l =.GetPartners(p.ID);
            foreach (Person item in l)
            {

                textBoxPartner.Text += item.name + " " + item.surname;

                maskedTextBoxWedding.Text = "00000000";

            }*/
            pictureBoxImage.Image = p.photo;
            if (p.photo == null) pictureBoxImage.Image = Properties.Resources.largerperson;

            textBoxUsername.Text = p.username;
            if (p.isEnabled) radioButton1.Checked = true;
            else radioButton2.Checked = true;


      
        }
        private void buttonSaveChanges_Click(object sender, EventArgs e)
        {
            if (currentlySelected != null)
            {
                User p = currentlySelected;
                p.name = textBoxIme.Text;
                p.surname = textBoxPrezime.Text;
                p.address = textBoxAddress.Text;
                p.CV = textBoxCV.Text;
                p.email = textBoxEmail.Text;
                p.phone = textBoxTelefon.Text;
                if (radioButtonUnkown.Checked)
                    p.sex = Person.Sex.Unknown;
                if (radioButtonMale.Checked)
                    p.sex = Person.Sex.Male;
                if (radioButtonFemale.Checked)
                    p.sex = Person.Sex.Female;

                p.photo = pictureBoxImage.Image;

                p.birthDate = stringToDate(maskedTextBoxDate.Text);
                p.deathDate = stringToDate(maskedTextBoxDeath.Text);
                p.username = textBoxUsername.Text;
                if (radioButton1.Checked) p.isEnabled = true;
                else p.isEnabled = false;
                SharedObjects.userManager.UpdateUser(currentlySelected);

            }
           
        }
        public string dateToString(DateTime dateTime)
        {
            string datetime = dateTime.ToString("ddMMyyyy");
            if (datetime.Equals("01011000")) return "";
            else return datetime;

        }
        public DateTime stringToDate(string str)
        {
            if (str.Equals("  .  .")) return new DateTime(1000, 01, 01);
            try
            {
                return DateTime.ParseExact(str, "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture);
            }
            catch
            {
                MessageBox.Show("Datum je u neispravnom formatu");
                return DateTime.Now;

            }
        }

        private void buttonAdmin_Click(object sender, EventArgs e)
        {
            
            personSelected(admin);
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            personSelected(u1);

        }

        private void button3_Click(object sender, EventArgs e)
        {
            personSelected(u2);

        }

        private void button4_Click(object sender, EventArgs e)
        {
            personSelected(u3);
        }

        private void buttonChangePassword_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == textBox2.Text) {
                currentlySelected.password = textBox2.Text;
                MessageBox.Show("Lozinka promijenjena.", "Lozinka", MessageBoxButtons.OK, MessageBoxIcon.Information);

               SharedObjects.userManager.UpdateUser(currentlySelected);
            }
            MessageBox.Show("Lozinke se ne podudaraju", "Lozinka", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void pictureBoxImage_Click(object sender, EventArgs e)
        {
            if (currentlySelected != null)
            {
                // Configure open file dialog box 
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.InitialDirectory = Properties.Settings.Default.workdirectory;
                dlg.Filter = "Slikovne datoteke|*.jpeg;*.png;*.jpg;*.gif";

                dlg.DefaultExt = ".jpg"; // Default file extension 

                // Show open file dialog box 
                if (dlg.ShowDialog() == DialogResult.OK)
                    pictureBoxImage.Image = Image.FromFile(dlg.FileName);
            }
        }
    }
}
