using ApplicationLogic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;


namespace Rodoslovno_stablo
{
    public partial class MainForm : Form
    {
        private ConsoleForm consoleForm;
        private Tree tree;
        private QueryProcessor qerp;
        private Panel graf;
        private System.Drawing.Bitmap myBitmap;
        private PersonControl currentlySelected = null;


        public MainForm()
        {
            LoginForm loginForm = new LoginForm();
            loginForm.ShowDialog();

            InitializeComponent();

            graf = splitC.Panel1;

            consoleForm = new ConsoleForm();
            qerp = consoleForm.MyQueryProcessor;
            tree = qerp.Drvo;
            //tree.osobe.Add(new Person(new System.Guid(), "Ime", "Prezime"));
            qerp.AddPerson(new string[] { "Ime", "Prezime" });
            graf.Refresh();

        }

        private void izlazToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            splitC.VerticalScroll.Enabled = !splitC.VerticalScroll.Enabled;

        }

        private void otvoriKonzoluToolStripMenuItem_Click(object sender, EventArgs e)
        {
            consoleForm.Show();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //centriraj se
            splitC.Panel1.VerticalScroll.Value = (2500 - splitC.Panel1.Height / 2);
            splitC.Panel1.HorizontalScroll.Value = (2500 - splitC.Panel1.Width / 2);
            Graphics graphicsObj;
            myBitmap = new Bitmap(5000, 5000,
            System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            RefreshTree();

            graphicsObj = Graphics.FromImage(myBitmap);
            graphicsObj.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            Pen myPen = new Pen(System.Drawing.Color.Black, 4);

            graphicsObj.DrawLine(myPen, new Point(2000, 2000), new Point(2500, 2500));
            graphicsObj.Dispose();
        }

        private void SaveToJpeg(string path)
        {
            Panel myPanel = splitC.Panel1;

            Bitmap image = new Bitmap(myPanel.Width, myPanel.Height);

            myPanel.DrawToBitmap(image, new Rectangle(0, 0, image.Width, image.Height));
            image.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);
        }

        private void SaveToJpeg(Stream file)
        {
            Panel myPanel = splitC.Panel1;

            Bitmap image = new Bitmap(myPanel.Width, myPanel.Height);

            myPanel.DrawToBitmap(image, new Rectangle(0, 0, image.Width, image.Height));
            image.Save(file, System.Drawing.Imaging.ImageFormat.Jpeg);
        }

        private void RefreshTree()
        {
            /*List<PersonControl> persons = new List<PersonControl>();
            Person p = new Person(new System.Guid(), "Netko", "Netkic");
            Person p1 = new Person(new System.Guid(), "Bla", "Netkic");

            PersonControl c = new PersonControl(p, this);
            PersonControl c1 = new PersonControl(p1,this);
            persons.Add(c);
            persons.Add(c1);

            splitContainer1.Panel1.Controls.Add(c);

            splitContainer1.Panel1.Controls.Add(c1);*/

            foreach (Person p in tree.osobe)
            {
                PersonControl c = new PersonControl(p, this);
                c.Location = new Point(p.positionX, p.positionY);
                

                splitC.Panel1.Controls.Add(c);
            }
        }


        public void personSelected(PersonControl c)
        {
            if (currentlySelected != null)
            {
                deselectPerson();

            }
            currentlySelected = c;
            toolStripDeletePerson.Enabled = true;

            c.BackColor = Color.FromArgb(51, 181, 229);
          

            Person p = c.getPerson();

            textBoxIme.Text = p.name;
            textBoxPrezime.Text = p.surname;
            maskedTextBoxDate.Text = dateToString(p.birthDate);
            textBoxAddress.Text = p.address;
            textBoxCV.Text = p.CV;
            if (p.sex == Person.Sex.Male)
                radioButtonMale.Checked = true;
            else if (p.sex == Person.Sex.Female)
                radioButtonFemale.Checked = true;
            else
                radioButtonUnkown.Checked = true;



        }
        public string dateToString(DateTime dateTime)
        {
            return dateTime.ToString("ddMMyyyy");

        }
        private void spremiKaoJpegToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stream myStream = null;

            SaveFileDialog dialog = new SaveFileDialog();

            dialog.AddExtension = true;
            dialog.DefaultExt = "jpeg";
            dialog.Filter = "JPEG slika (*.jpg)|*.jpg";
            dialog.FilterIndex = 0;
            //dialog.FileOk += dialog_FileOk;

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                myStream = dialog.OpenFile();

                if (myStream != null)
                {
                    SaveToJpeg(myStream);
                    myStream.Close();
                }
            }
        }



        private void SaveXMLClick()
        {
            Stream myStream = null;

            SaveFileDialog dialog = new SaveFileDialog();

            dialog.AddExtension = true;
            dialog.DefaultExt = "xml";
            dialog.Filter = "xml files (*.xml)|*.xml";
            dialog.FilterIndex = 0;

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                myStream = dialog.OpenFile();

                if (myStream != null)
                {
                    SaveToXML(myStream);
                    myStream.Close();
                }

            }
        }
        private void SaveToXML(Stream file)
        {
            tree.Save(file);
        }
        private void OpenXMLClick()
        {
            string filename = null;

            OpenFileDialog dialog = new OpenFileDialog();

            dialog.AddExtension = true;
            dialog.DefaultExt = "xml";
            dialog.Filter = "Rodoslovno stablo XML datoteke (*.xml)|*.xml";
            dialog.FilterIndex = 0;

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                filename = dialog.FileName;

                if (filename != null)
                {
                    OpenFromXML(filename);

                }

            }

        }
        private void OpenFromXML(string file)
        {
            resetEverything();
            tree = Tree.Load(file);
            RefreshTree();
            
        }
        private void resetEverything() {

            graf.Controls.Clear();
            myBitmap.Dispose();
            myBitmap = new Bitmap(5000, 5000,
            System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            graf.Refresh();
            


        }
        private void saveToXML_Click(object sender, EventArgs e)
        {
            SaveXMLClick();
        }

        private void spremiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveXMLClick();
        }

        private void postavkeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form settingsForm = new SettingsForm();
            settingsForm.Show();

        }

        private void upravljanjeKorisnicimaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UsersForm usersForm = new UsersForm();
            usersForm.Show();

        }

        private void oProgramuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox aboutBox = new AboutBox();
            aboutBox.Show();

        }

        private void toolStripButton1_Click_2(object sender, EventArgs e)
        {
            SaveXMLClick();
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics graphicsObj = e.Graphics;
            // TODO paziti kad stizemo na rub ekrana
            int upperLeftX = splitC.Panel1.HorizontalScroll.Value;
            int upperLeftY = splitC.Panel1.VerticalScroll.Value;

            // if ( upperLeftX > 5000-
            Rectangle rect = new Rectangle(upperLeftX, upperLeftY, splitC.Panel1.Width - 0, splitC.Panel1.Height - 0);
            Bitmap cropped = myBitmap.Clone(rect, myBitmap.PixelFormat);

            graphicsObj.DrawImage(cropped, 0, 0);
            cropped.Dispose();
            graphicsObj.Dispose();
        }

        private void otvoriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenXMLClick();
        }

        private void splitC_Panel1_Click(object sender, EventArgs e)
        {
            deselectPerson();
        }
        private void deselectPerson()
        {
            if (currentlySelected != null)
            {
                currentlySelected.BackColor = PersonControl.DefaultBackColor;
                currentlySelected = null;
                textBoxAddress.Text = "";
                textBoxCV.Text = "";
                textBoxEmail.Text = "";
                textBoxIme.Text = "";
                textBoxPrezime.Text = "";
                textBoxTelefon.Text = "";
                maskedTextBoxDate.Text = "";
                toolStripDeletePerson.Enabled = false;

            }
        }

        private void toolStripAddPerson_Click(object sender, EventArgs e)
        {
            Guid novaOsobaGuid = tree.AddPerson("Nova", "Osoba");
            Person p = tree.GetPersonByID(novaOsobaGuid);
            PersonControl c = new PersonControl(p, this);
            graf.Controls.Add(c);


        }

        private void buttonSaveChanges_Click(object sender, EventArgs e)
        {
            if (currentlySelected != null) {
                Person p = currentlySelected.getPerson();
                p.name = textBoxIme.Text;
                p.surname = textBoxPrezime.Text;
                p.address = textBoxAddress.Text;
                p.CV = textBoxCV.Text;
                p.telephone = textBoxTelefon.Text;
                currentlySelected.updateControlContent();

                // todo p.birthDate
            
            }
        }

        private void collapseEditingPanel(object sender, EventArgs e)
        {
            splitC.Panel2Collapsed = !splitC.Panel2Collapsed;

        }


    }
}
