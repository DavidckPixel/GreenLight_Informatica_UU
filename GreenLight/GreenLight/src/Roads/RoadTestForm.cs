using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GreenLight
{
    public partial class RoadTestForm : Form
    {

        //Quick temperary form for testing purposes, a form onwhich the road sketch can be drawn
        //To test different kinds of roads, change what kind of road is made in the Test Variable
        //You can pick from: DiagonalRoad/CurvedRoad/StraightRoad, You can alos set the amount of lanes u require.
        //Make sure that you pick the Direction string accoredable

        AbstractRoad Test; 

        public RoadTestForm()
        {
            Test = new DiagonalRoad(new Point(20, 20), new Point(220, 220), 2, "S"); //TEMP OM TE TESTEN
            this.Paint += Drawing;
        }

        public void Drawing(Object o, PaintEventArgs pea)
        {
            Graphics g = pea.Graphics;
            foreach (DrivingLane _temp in Test.Drivinglanes)
            {
                _temp.Draw(g);
            }

        }

        private void RoadTestForm_Load(object sender, EventArgs e)
        {

        }
    }
}
