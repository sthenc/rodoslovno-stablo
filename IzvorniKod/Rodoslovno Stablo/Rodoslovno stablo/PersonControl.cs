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
            pictureBoxUser.Image = p.photo;

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
                this.Location = newlocation;
            }
        }

        private void PersonControl_MouseUp(object sender, MouseEventArgs e)
        {
            _Offset = Point.Empty;
        }

        private void PersonControl_Click(object sender, EventArgs e)
        {

            f.personSelected(p);

            
        }

  
      
    }
}
