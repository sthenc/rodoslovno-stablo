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
        private Panel graf;
        
        private PersonControl currentlySelected = null;
        private int connectionCreationInProgress = 0; // 5 - roditelj, 10 brak
        
        Dictionary<Person, PersonControl> controls = new Dictionary<Person, PersonControl>();


        public MainForm()
        {  
            LoginForm loginForm = new LoginForm();
            loginForm.ShowDialog();
            InitializeComponent();
            graf = splitC.Panel1;
            tree = Tree.GetInstance();
            //tree.osobe.Add(new Person(new System.Guid(), "Ime", "Prezime"));
            
  
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //centriraj se
            splitC.Panel1.VerticalScroll.Value = (2500 - splitC.Panel1.Height / 2);
            splitC.Panel1.HorizontalScroll.Value = (2500 - splitC.Panel1.Width / 2);
         
 
        }
        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {
            redrawConnections();
        }
        private void RefreshTree()
        {
            controls.Clear();
           foreach (Person p in tree.osobe)
            {
                PersonControl c = new PersonControl(p, this);
                c.Location = R2A(new Point(p.positionX, p.positionY));
                splitC.Panel1.Controls.Add(c);
                controls.Add(p,c);

            }
            redrawConnections();
        }

        public void redrawConnections()
        {
            foreach (Connection item in tree.veze)
            {
                Person person1 = tree.GetPersonByID(item.personID1);
                Person person2 = tree.GetPersonByID(item.personID2);
                PersonControl c1, c2;
                if (controls.TryGetValue(person1, out c1) && controls.TryGetValue(person2, out c2))
                {
                    Graphics graphicsObj = graf.CreateGraphics();
                    graphicsObj.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    Pen myPen; SolidBrush myBrush;
                    Point p1, p2;
                    if (item.type == "parent")
                    {
                        myPen = new Pen(System.Drawing.Color.Black, 4);
                        myBrush = new SolidBrush(Color.Black);

                        p1 = R2A(c1.getRealBottomPoint());
                        p2 = R2A(c2.getRealTopPoint());
                    }
                    else if (item.type == "partner")
                    {
                        myPen = new Pen(System.Drawing.Color.IndianRed, 4);
                        myBrush = new SolidBrush(Color.IndianRed);

                        p1 = R2A(c1.getRealRightPoint());
                        p2 = R2A(c2.getRealLeftPoint());

                    }
                    else continue;

                    graphicsObj.DrawLine(myPen, p1, p2);
                    int radius = 6;
                    p1.X -= radius; p1.Y -= radius;
                    p2.X -= radius; p2.Y -= radius;
                    Rectangle rect = new Rectangle(p1, new Size(2 * radius, 2 * radius));
                    graphicsObj.FillEllipse(myBrush, rect);
                    rect = new Rectangle(p2, new Size(2 * radius, 2 * radius));
                    graphicsObj.FillEllipse(myBrush, rect);

                    graphicsObj.Dispose();

                }

            }
        }
        private void resetEverything()
        {

            graf.Controls.Clear();
            graf.Refresh();



        }
  

        


        // Ostale bitne stvari 
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

    
        public void personSelected(PersonControl c)
        {
            if (connectionCreationInProgress != 0 && currentlySelected!= null) { 
                // kreiramo novu vezu 
                Person person1 = currentlySelected.getPerson();
                Person person2 = c.getPerson();
                if (connectionCreationInProgress == 5) // roditelj
                    tree.AddParent(person1.ID, person2.ID);
                else if (connectionCreationInProgress == 10)  // brak
                    tree.AddPartner(person1.ID, person2.ID);
                else if (connectionCreationInProgress == 6) // dijete
                    tree.AddChild(person1.ID, person2.ID);

                connectionCreationInProgress = 0;
                restoreInferfaceAfterConnection();
               
                redrawConnections();

            }
           

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

       

        // Konverzija 

        public Point R2A(Point p1) {
            return new Point(p1.X + graf.AutoScrollPosition.X, p1.Y + graf.AutoScrollPosition.Y);
        }
        public Point A2R(Point p1) {
            return new Point(p1.X - graf.AutoScrollPosition.X, p1.Y - graf.AutoScrollPosition.Y);
        }


        // Toolstripovi


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
        private void collapseEditingPanel(object sender, EventArgs e)
        {
            splitC.Panel2Collapsed = !splitC.Panel2Collapsed;

        }

        private void preglednikUpitaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            QueriesForm qf = new QueriesForm();
            qf.Show();

        }
        private void toolStripAddPerson_Click(object sender, EventArgs e)
        {
            Guid novaOsobaGuid = tree.AddPerson("Nova", "Osoba");
            Person p = tree.GetPersonByID(novaOsobaGuid);
            PersonControl c = new PersonControl(p, this);
            c.Location= new Point ( graf.Width / 2, graf.Height / 2);
            graf.Controls.Add(c);
            controls.Add(p,c);


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
            consoleForm = new ConsoleForm();
            consoleForm.Show();

        }

        private void splitC_Panel1_Scroll(object sender, ScrollEventArgs e)
        {
            graf.Refresh();
        }
        public void moveRefresh(){
            graf.Refresh();
        }

        private void toolStripButtonCreateParent_Click(object sender, EventArgs e)
        {
            connectionCreationInProgress = 5; // roditelj
            prepareInterfaceForConnection();
        }

        private void toolStripButtonCreateMarriage_Click(object sender, EventArgs e)
        {
            connectionCreationInProgress = 10; // brak
            prepareInterfaceForConnection();
        }

        private void toolStripButtonChild_Click(object sender, EventArgs e)
        {
            connectionCreationInProgress = 6; // dijete
            prepareInterfaceForConnection();
        }
        private void prepareInterfaceForConnection() {
            toolStripAddPerson.Enabled = false;
            toolStripButtonChild.Enabled = false;
            toolStripButtonCreateMarriage.Enabled = false;
            toolStripButtonCreateParent.Enabled = false;
            
            
            toolStripDeletePerson.Enabled = false;

            graf.Cursor = Cursors.Cross;
            toolStripButtonCancel.Visible = true;


        }
        private void restoreInferfaceAfterConnection() {
            toolStripAddPerson.Enabled = true;
            toolStripButtonChild.Enabled = true;
            toolStripButtonCreateMarriage.Enabled = true;
            toolStripButtonCreateParent.Enabled = true;
            toolStripDeletePerson.Enabled = true;
            graf.Cursor = Cursors.Default;
            toolStripButtonCancel.Visible = false;
            
        
        }

        private void toolStripButtonCancel_Click(object sender, EventArgs e)
        {
            connectionCreationInProgress = 0;
            restoreInferfaceAfterConnection();

            redrawConnections();
        }



    }
}
