using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ApplicationLogic;

namespace Rodoslovno_stablo
{
    public partial class PersonControl : UserControl
    {
        MainForm f;
        Person p;
        public PersonControl(Person person, MainForm form)
        {
            InitializeComponent();
            p = person;
            f = form;
            updateControlContent();

        }
        public void updateControlContent() {
            labelName.Text = p.name + " " + p.surname;
            if (p.photo!= null)
                pictureBoxUser.Image = p.photo;
            labelDateofBirth.Text = p.birthDate.ToShortDateString();


        }


        private Point _Offset = Point.Empty;

        private void PersonControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _Offset = new Point(e.X, e.Y);
            }
        }

        private void PersonControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (_Offset != Point.Empty)
            {
                Point newlocation = this.Location;
                newlocation.X += e.X - _Offset.X;
                newlocation.Y += e.Y - _Offset.Y;
                //da ne bjezi izvan podrucja za crtanje
                if (newlocation.X > 4900) newlocation.X = 4900;
                if (newlocation.Y > 4900) newlocation.Y = 4900;
                if (newlocation.X < 0) newlocation.X = 0;
                if (newlocation.Y < 0) newlocation.Y = 0;
                // snap to grid
                if (newlocation.Y % 100 < 15 || newlocation.Y%100 > 85) newlocation.Y = Convert.ToInt32(Math.Round(newlocation.Y/100.0)) * 100;
                
                this.Location = newlocation;
                p.positionX = this.Location.X;
                p.positionY = this.Location.Y;

            }
        }

        private void PersonControl_MouseUp(object sender, MouseEventArgs e)
        {
            _Offset = Point.Empty;
        }

        private void PersonControl_Click(object sender, EventArgs e)
        {

            f.personSelected(this);

            
        }

        public Person getPerson(){
            return p;

        }
       
      
    }
}
