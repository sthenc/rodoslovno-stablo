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
            f.personSelected(this);
            if (e.Button == MouseButtons.Left)
            {
                _Offset = new Point(e.X, e.Y);
            }
        }

        private void PersonControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (_Offset != Point.Empty)
            {
                f.moveRefresh();
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
                Point realLocation = f.A2R(this.Location);
                p.positionX = realLocation.X;
                p.positionY = realLocation.Y;

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
        public Point getRealTopPoint() {
            Point p = new Point(this.Location.X + this.Width / 2, this.Location.Y);
            return f.A2R(p);

        
        }
        public Point getRealBottomPoint()
        {
            
            Point p = new Point(this.Location.X+this.Width/2, this.Location.Y + this.Height);
            return f.A2R(p);


        }
    }
}
