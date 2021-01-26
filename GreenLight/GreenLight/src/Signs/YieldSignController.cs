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

        public Label QuestionLabel, errorMess;
        public CurvedButtons YesButton, NoButton;

        public YieldSign selected;

        public YieldSignController(Form _main, MainSignController _signcontroller)
        {
            this.signController = _signcontroller;
            this.mainScreen = _main;
        }

        public override void initSettingScreen()
        {
            this.settingScreen = new Pop_Up_Form(new Size(400, 105));

            this.settingScreen.BackColor = Color.FromArgb(255, 255, 255);
            this.settingScreen.FormBorderStyle = FormBorderStyle.None;

            QuestionLabel = new Label();
            QuestionLabel.Text = "Place Yield Sign?";
            QuestionLabel.Location = new Point(20, 10);
            QuestionLabel.Width = 130;
            this.settingScreen.Controls.Add(QuestionLabel);

            errorMess = new Label();
            errorMess.Location = new Point(140, 10);
            errorMess.Text = "";
            errorMess.ForeColor = Color.Red;
            this.settingScreen.Controls.Add(errorMess);

            Move_panel Move = new Move_panel(this.settingScreen);
            Move.Location = new Point(210, 40);
            Move.Size = new Size(190, 65);
            this.settingScreen.Controls.Add(Move);

            YesButton = new CurvedButtons(new Size(80, 40), new Point(10, 50), 25, "../../User Interface Recources/Custom_Small_Button.png", "Place", DrawData.Dosis_font_family, this.settingScreen, this.settingScreen.BackColor);
            YesButton.Click += (object o, EventArgs ea) => { General_Form.Main.BuildScreen.builder.signController.yieldSignC.placeSign(); };
            this.settingScreen.Controls.Add(YesButton);

            NoButton = new CurvedButtons(new Size(90, 40), new Point(110, 50), 25, "../../User Interface Recources/Custom_Small_Button.png", "Delete", DrawData.Dosis_font_family, this.settingScreen, this.settingScreen.BackColor);
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

            this.settingScreen.ShowDialog();
            this.settingScreen.BringToFront();

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
