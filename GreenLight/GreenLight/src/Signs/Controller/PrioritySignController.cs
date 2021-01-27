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
    public class PrioritySignController : AbstractSignController
    {
        public Label QuestionLabel, errorMess, FlipLabel;
        public CurvedButtons YesButton, NoButton;
        public PictureBox pb1;
        public PrioritySign selected;
        public AbstractSign thisSign;
        public AbstractRoad selectedRoad;

        public PrioritySignController(Form _main, MainSignController _signcontroller)
        {
            this.signController = _signcontroller;
            this.mainScreen = _main;
        }

        public override void initSettingScreen()
        {
            this.settingScreen = new PopUpForm(new Size(300,300));
            this.settingScreen.BackColor = Color.FromArgb(255, 255, 255);
            this.settingScreen.FormBorderStyle = FormBorderStyle.None;

            pb1 = new PictureBox();
            pb1.Image = Image.FromFile("../../src/User Interface Recources/Priority_sign.png");
            pb1.Location = new Point(190, 35);
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

            CurvedButtons FlipButton = new CurvedButtons(new Size(100, 40), new Point(100, 170), 25, "../../src/User Interface Recources/Custom_Button.png", "Flip sign", DrawData.Dosis_font_family, this.settingScreen, this.settingScreen.BackColor);

            FlipButton.Click += (object o, EventArgs ea) => { this.signController.flipSign(); this.settingScreen.Hide() };
            this.settingScreen.Controls.Add(FlipButton);

            QuestionLabel = new Label();
            QuestionLabel.Text = "Do you want to place a priority sign?";
            QuestionLabel.Location = new Point(30, 45);
            QuestionLabel.Size = new Size(150, 40);
            QuestionLabel.TextAlign = ContentAlignment.MiddleCenter;
            this.settingScreen.Controls.Add(QuestionLabel);

            errorMess = new Label();
            errorMess.Location = new Point(50, 100);
            errorMess.Text = "";
            errorMess.ForeColor = Color.Red;
            this.settingScreen.Controls.Add(errorMess);

            MovePanel Move = new MovePanel(this.settingScreen);
            Move.Location = new Point(0, 0);
            Move.Size = new Size(300, 35);
            Move.BackColor = Color.FromArgb(142, 140, 144);
            this.settingScreen.Controls.Add(Move);


            CurvedButtons Divider1 = new CurvedButtons();
            Divider1.Location = new Point(20, 220);
            this.settingScreen.Controls.Add(Divider1);

            YesButton = new CurvedButtons(new Size(80, 40), new Point(45, 240), 25, "../../src/User Interface Recources/Custom_Small_Button.png", "OK", DrawData.Dosis_font_family, this.settingScreen, this.settingScreen.BackColor);
            YesButton.Click += (object o, EventArgs ea) => { General_Form.Main.BuildScreen.builder.signController.prioritySignC.placeSign(); };
            this.settingScreen.Controls.Add(YesButton);

            NoButton = new CurvedButtons(new Size(100, 40), new Point(155, 240), 25, "../../src/User Interface Recources/Custom_Small_Button.png", "Remove", DrawData.Dosis_font_family, this.settingScreen, this.settingScreen.BackColor);
            NoButton.Click += (object o, EventArgs ea) => { General_Form.Main.BuildScreen.builder.signController.prioritySignC.deleteSign(); };
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
            selected = (PrioritySign)_sign;
            openMenu();
        }
        public override AbstractSign newSign()
        {
            PrioritySign _temp = new PrioritySign(this);
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
