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
using System.Drawing.Imaging;


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
            DialogResult result = loginForm.ShowDialog() ;

            if (SharedObjects.userManager.GetActiveUser()==null)
            {
                loginForm.Dispose();
                this.Dispose();
                Application.Exit();
                Environment.Exit(0);
                return ;

            }
                InitializeComponent();
                deselectPerson();
                graf = splitC.Panel1;
                tree = Tree.GetInstance();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //centriraj se i primjeni temu
            graf.AutoScrollPosition= new Point(2500-graf.Width/2, 2500-graf.Height / 2);
            setTheme(Properties.Settings.Default.theme);
        }
        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {
            redrawConnections();
        }
        public void RefreshTree()
        {
            graf.Controls.Clear();
            controls.Clear();
            foreach (Person p in tree.osobe)
            {
                
                PersonControl c = new PersonControl(p, this);
                // ako je osoba dodana iz konzole, koordinate nisu dobro postavljene i iznose 0,0. Bolje rjesenje: promjena strukture da stavlja -1,-1
                if (p.positionX == 0 && p.positionY == 0)
                    c.setLocation(newLocationInGraph());
                else
                    c.setLocation(R2A(new Point(p.positionX, p.positionY)));
                splitC.Panel1.Controls.Add(c);
                controls.Add(p,c);

            }
            graf.Invalidate();
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
                        if (person1.positionX < person2.positionX)
                        {
                            p1 = R2A(c1.getRealRightPoint());
                            p2 = R2A(c2.getRealLeftPoint());
                        }
                        else {
                            p1 = R2A(c1.getRealLeftPoint());
                            p2 = R2A(c2.getRealRightPoint());
                        }

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

            if (connectionCreationInProgress != 0 && currentlySelected != null)
            {
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
            else
            {
                // samo oznacavamo osobu
                if (currentlySelected != null)
                    deselectPerson();
                currentlySelected = c;
                toolStripDeletePerson.Enabled = true;
                c.BackColor = Color.FromArgb(51, 181, 229);     //bojanje kontrole u plavu
                Person p = c.getPerson();

                textBoxIme.Text = p.name;
                textBoxPrezime.Text = p.surname;
                maskedTextBoxDate.Text = dateToString(p.birthDate);

                maskedTextBoxDeath.Text = dateToString(p.deathDate);
                textBoxAddress.Text = p.address;
                textBoxCV.Text = p.CV;
                textBoxTelefon.Text = p.telephone;
                textBoxEmail.Text = p.email;

                if (p.sex == Person.Sex.Male)
                    radioButtonMale.Checked = true;
                else if (p.sex == Person.Sex.Female)
                    radioButtonFemale.Checked = true;
                else
                    radioButtonUnkown.Checked = true;
                //dodavanje supruznika

                IEnumerable<Person> l = tree.GetPartners(p.ID);
                foreach (Person item in l)
                {

                    textBoxPartner.Text += item.name + " " + item.surname;

                    maskedTextBoxWedding.Text = "00000000";

                }
                pictureBoxImage.Image = p.photo;
                if (p.photo == null) pictureBoxImage.Image = Properties.Resources.largerperson;
            }

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
                maskedTextBoxWedding.Text = "";
                maskedTextBoxDeath.Text = "";
                toolStripDeletePerson.Enabled = false;
                pictureBoxImage.Image = Properties.Resources.largerperson;
                textBoxPartner.Text = "";
                radioButtonUnkown.Checked = true;
                


            }
        }
        private void buttonSaveChanges_Click(object sender, EventArgs e)
        {
            if (currentlySelected != null)
            {
                Person p = currentlySelected.getPerson();
                p.name = textBoxIme.Text;
                p.surname = textBoxPrezime.Text;
                p.address = textBoxAddress.Text;
                p.CV = textBoxCV.Text;
                p.email = textBoxEmail.Text;
                p.telephone = textBoxTelefon.Text;
                if (radioButtonUnkown.Checked)
                    p.sex = Person.Sex.Unknown;
                if (radioButtonMale.Checked)
                    p.sex = Person.Sex.Male;
                if (radioButtonFemale.Checked)
                    p.sex = Person.Sex.Female;
                
                p.photo = pictureBoxImage.Image;
                
                p.birthDate = stringToDate(maskedTextBoxDate.Text);
                if (p.deathDate!=null)
                    p.deathDate = stringToDate(maskedTextBoxDeath.Text);
                currentlySelected.updateControlContent();

            }
           // RefreshTree();
        }

        public string dateToString(DateTime dateTime)
        {
            string datetime= dateTime.ToString("ddMMyyyy");
            if (datetime.Equals("01011000")) return "";
            else return datetime;

        }
        public DateTime stringToDate(string str) {
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
        private void spremiKaoJpegToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stream myStream = null;

            SaveFileDialog dialog = new SaveFileDialog();

            dialog.InitialDirectory = Properties.Settings.Default.workdirectory;

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
            dialog.InitialDirectory = Properties.Settings.Default.workdirectory;
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
            dialog.InitialDirectory = Properties.Settings.Default.workdirectory;
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

        private Point newLocationInGraph() {
            Random random = new Random();
            return new Point(graf.Width / 2 + random.Next(-200, 200), graf.Height / 2 + random.Next(-200, 200));
        
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
            Form settingsForm = new SettingsForm(this);
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
            c.setLocation(newLocationInGraph());
            graf.Controls.Add(c);
            controls.Add(p,c);


        }

        private void izlazToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
            Environment.Exit(0);

        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            splitC.VerticalScroll.Enabled = !splitC.VerticalScroll.Enabled;

        }
        private void otvoriKonzoluToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (consoleForm==null)
                consoleForm = new ConsoleForm(this);
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

        private void toolStripDeletePerson_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Želite li obrisati označenu osobu i sve njezine veze?", "Potvrda", MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.Yes))
            {
                tree.DeletePersonWithConnections(currentlySelected.getPerson().ID);
                deselectPerson();
                RefreshTree();
            }
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

        private void novoStabloToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tree.clearTree();
            controls.Clear();
            graf.Controls.Clear();
            redrawConnections();
            graf.Update();
        }

        public void setTheme(int i) {
            Properties.Settings.Default.theme = i;
            Properties.Settings.Default.Save();
            if (i == 1) { 
                menuStrip1.BackColor=MenuStrip.DefaultBackColor;
                toolStrip.BackColor=ToolStrip.DefaultBackColor;
                splitC.Panel2.BackColor = SplitContainer.DefaultBackColor;
            }
            else if (i == 2) {
                menuStrip1.BackColor = Color.Cornsilk;
                toolStrip.BackColor = Color.Cornsilk;
                splitC.Panel2.BackColor = Color.Cornsilk;      
            }
            else if (i == 3)
            {
                menuStrip1.BackColor = Color.LightGreen;
                toolStrip.BackColor = Color.LightGreen;
                splitC.Panel2.BackColor = Color.LightGreen;
            }
            else if (i == 4)
            {
                menuStrip1.BackColor = Color.SkyBlue;
                toolStrip.BackColor = Color.SkyBlue;
                splitC.Panel2.BackColor = Color.SkyBlue;
            }

        }

    }
}
