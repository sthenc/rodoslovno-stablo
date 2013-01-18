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
            qerp.AddPerson(new string[] {"Ime", "Prezime"});
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
            Graphics graphicsObj;
            myBitmap = new Bitmap(5000, 5000,
            System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            RefreshTree();
            
            graphicsObj = Graphics.FromImage(myBitmap);
            graphicsObj.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            Pen myPen = new Pen(System.Drawing.Color.Plum, 3);
            Rectangle rectangleObj = new Rectangle(10, 10, 200, 200);
            graphicsObj.DrawEllipse(myPen, rectangleObj);
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

            splitC.Panel1.Controls.Clear();

            foreach (Person p in tree.osobe)
            {
                PersonControl c = new PersonControl(p, this);
                splitC.Panel1.Controls.Add(c);
            }
        }


        public void personSelected(Person p) {
            textBoxIme.Text = p.name;
            textBoxPrezime.Text = p.surname;
            maskedTextBoxDate.Text=dateToString(p.birthDate);
            textBoxAddress.Text = p.address;
            textBoxCV.Text = p.CV;
            if (p.sex == Person.Sex.Male)
                radioButtonMale.Checked = true;
            else if (p.sex == Person.Sex.Female)
                radioButtonFemale.Checked = true;
            else
                radioButtonUnkown.Checked = true;

           
           }
        public string dateToString(DateTime dateTime) {
            return dateTime.ToString("ddMMyyyy");

        }
        private void spremiKaoJpegToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stream myStream = null;

            SaveFileDialog dialog = new SaveFileDialog();

            dialog.AddExtension = true;
            dialog.DefaultExt = "jpeg";
            dialog.Filter = "jpeg files (*.jpeg)|*.jpeg";
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

        private void SaveToXML(Stream file)
        {
            tree.Save(file);
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
            
            Rectangle rect = new Rectangle(splitC.Panel1.HorizontalScroll.Value, splitC.Panel1.VerticalScroll.Value, splitC.Panel1.Width, splitC.Panel1.Height);
            Bitmap cropped = myBitmap.Clone(rect, myBitmap.PixelFormat);
            graphicsObj.DrawImage(cropped,0,0);

            graphicsObj.Dispose();
        }
       
   

    }
}
