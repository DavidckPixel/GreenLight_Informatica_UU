using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Text;

namespace GreenLight
{
    public class YieldSignController : AbstractSignController
    {

        public Label QuestionLabel, errorMess, FlipLabel;
        public CurvedButtons YesButton, NoButton;
        public PictureBox pb1;
        public YieldSign selected;

        public YieldSignController(Form _main, MainSignController _signcontroller)
        {
            this.signController = _signcontroller;
            this.mainScreen = _main;
        }

        public override void initSettingScreen()
        {
            this.settingScreen = new Form();
            this.settingScreen.Size = new Size(300, 300);
            this.settingScreen.BackColor = Color.FromArgb(255, 255, 255);
            this.settingScreen.FormBorderStyle = FormBorderStyle.None;

            pb1 = new PictureBox();
            pb1.Image = Image.FromFile("../../User Interface Recources/Yield_sign.png");
            pb1.Location = new Point(190, 30);
            pb1.Size = new Size(75, 75);
            pb1.SizeMode = PictureBoxSizeMode.Zoom;
            pb1.BringToFront();
            this.settingScreen.Controls.Add(pb1);

            FlipLabel = new Label();
            FlipLabel.Text = "The sign has to be on the right side of the road.";
            FlipLabel.Location = new Point(30, 135);
            FlipLabel.Size = new Size(230, 20);
            FlipLabel.TextAlign = ContentAlignment.MiddleCenter;
            this.settingScreen.Controls.Add(FlipLabel);

            CurvedButtons FlipButton = new CurvedButtons(new Size(250, 40), new Point(30, 170), 25, "../../User Interface Recources/Custom_Button.png", "Flip sign to the other side", DrawData.Dosis_font_family, this.settingScreen, this.settingScreen.BackColor);
            FlipButton.Click += (object o, EventArgs ea) => { };// General_Form.Main.BuildScreen.builder.signController.stopSign.flipSign(); };
            this.settingScreen.Controls.Add(FlipButton);

            QuestionLabel = new Label();
            QuestionLabel.Text = "Do you want to place a yield sign?";
            QuestionLabel.Location = new Point(30, 45);
            QuestionLabel.TextAlign = ContentAlignment.MiddleCenter;
            QuestionLabel.Size = new Size(150, 40);
            this.settingScreen.Controls.Add(QuestionLabel);

            errorMess = new Label();
            errorMess.Location = new Point(20, 60);
            errorMess.Text = "";
            errorMess.ForeColor = Color.Red;
            this.settingScreen.Controls.Add(errorMess);

            CurvedButtons Divider1 = new CurvedButtons();
            Divider1.Location = new Point(20, 220);
            this.settingScreen.Controls.Add(Divider1);

            YesButton = new CurvedButtons(new Size(80, 40), new Point(55, 240), 25, "../../User Interface Recources/Custom_Small_Button.png", "OK", DrawData.Dosis_font_family, this.settingScreen, this.settingScreen.BackColor);
            YesButton.Click += (object o, EventArgs ea) => { General_Form.Main.BuildScreen.builder.signController.yieldSignC.placeSign(); };
            this.settingScreen.Controls.Add(YesButton);

            NoButton = new CurvedButtons(new Size(80, 40), new Point(165, 240), 25, "../../User Interface Recources/Custom_Small_Button.png", "Remove", DrawData.Dosis_font_family, this.settingScreen, this.settingScreen.BackColor);
            NoButton.Click += (object o, EventArgs ea) => { General_Form.Main.BuildScreen.builder.signController.yieldSignC.deleteSign(); };
            this.settingScreen.Controls.Add(NoButton);
        }
        public void placeSign()
        {
            this.settingScreen.Hide();
        }

        public override void openMenu()
        {
            if (selected == null)
            {
                return; // HOORT NOOIT TE GEBEUREN
            }

            if (this.settingScreen.Visible)
            {
                return;
            }

            this.errorMess.Text = "";


            settingScreen.ShowDialog();
            settingScreen.BringToFront();

        }

        public override void onSignClick(AbstractSign _sign)
        {
            selected = (YieldSign)_sign;
            openMenu();
        }

        public override AbstractSign newSign()
        {
            YieldSign _temp = new YieldSign(this);
            this.signController.Signs.Add(_temp);

            onSignClick(_temp);

            return _temp;
        }

        public override void deleteSign()
        {
            this.signController.deleteSign(selected);
            this.settingScreen.Hide();
        }
    }
}
