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
        public PersonControl() 
        {
            InitializeComponent();
            
        }

        public void setLocation(Point location){
            //pocetno podesavanje lokacije kontrole, bitno nam je azurirati osobu prilikom azuriranja kontrole
            this.Location = location;
            p.positionX = f.A2R(this.Location).X;
            p.positionY = f.A2R(this.Location).Y;
        }
        public void updateControlContent() {
            labelName.Text = p.name + " " + p.surname;
            if (p.photo!= null)
                pictureBoxUser.Image = p.photo;
            //labelDateofBirth.Text = p.birthDate.ToShortDateString();


        }
        private void SelectionClick() {
            f.personSelected(this);
        }

        private Point _Offset = Point.Empty;
        private int movingState = 0;
        private void PersonControl_MouseDown(object sender, MouseEventArgs e)
        {
            
            if (e.Button == MouseButtons.Left)
            {
                _Offset = new Point(e.X, e.Y);
                movingState = 1;
            }
        }

        private void PersonControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (_Offset != Point.Empty)
            {
                if (movingState == 1)
                {
                    f.personSelected(this);
                    movingState = 2;
                }
                f.moveRefresh();
                
                Point newlocation = this.Location;
                newlocation.X += e.X - _Offset.X;
                newlocation.Y += e.Y - _Offset.Y;
                
                Point realLocation = f.A2R(newlocation);
                //da ne bjezi izvan podrucja za crtanje
                if (realLocation.X > 4900) realLocation.X = 4900;
                if (realLocation.Y > 4900) realLocation.Y = 4900;
                if (realLocation.X < 0) realLocation.X = 0;
                if (realLocation.Y < 0) realLocation.Y = 0;
                // snap to grid
                if (realLocation.Y % 100 < 15 || realLocation.Y % 100 > 85) realLocation.Y = Convert.ToInt32(Math.Round(realLocation.Y / 100.0)) * 100;

                this.Location =f.R2A( realLocation);
                realLocation = f.A2R(this.Location);
                p.positionX = realLocation.X;
                p.positionY = realLocation.Y;

            }
        }

        private void PersonControl_MouseUp(object sender, MouseEventArgs e)
        {
            _Offset = Point.Empty;
            movingState = 0;
        }

        private void PersonControl_Click(object sender, EventArgs e)
        {
            SelectionClick();
        }

        public Person getPerson(){
            return p;

        }
        private void setPerson(Person per) {
            p = per;

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
        public Point getRealLeftPoint(){
            Point p = new Point(this.Location.X, this.Location.Y + this.Height/2);
            return f.A2R(p);
            
        }
        public Point getRealRightPoint() {
            Point p = new Point(this.Location.X + this.Width, this.Location.Y+this.Height/2);
            return f.A2R(p);
            
        }

        private void labelName_Click(object sender, EventArgs e)
        {
            SelectionClick();
        }

        private void pictureBoxUser_Click(object sender, EventArgs e)
        {
            SelectionClick();
        }
    }
}
