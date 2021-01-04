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

        public MainSignController signController;

        public YieldSign selected;

        PrivateFontCollection Font_collection = new PrivateFontCollection();

        public YieldSignController(Form _main, MainSignController _signcontroller)
        {
            this.signController = _signcontroller;
            this.mainScreen = _main;
        }

        public void initSettingScreen()
        {
            //Waarschijnlijk beter om mee te geven aan initSettingScreen zoals in Build_sub_menu.Initialize
            Font_collection.AddFontFile("../../Fonts/Dosis-bold.ttf");
            FontFamily Dosis_font_family = Font_collection.Families[0];


            this.settingScreen = new Form();

            this.settingScreen.Size = new Size(400, 400);
            this.settingScreen.BackColor = Color.FromArgb(255, 255, 255);

            QuestionLabel = new Label();
            QuestionLabel.Text = "Place Priority Sign?";
            QuestionLabel.Location = new Point(20, 10);
            this.settingScreen.Controls.Add(QuestionLabel);

            errorMess = new Label();
            errorMess.Location = new Point(20, 60);
            errorMess.Text = "";
            errorMess.ForeColor = Color.Red;
            this.settingScreen.Controls.Add(errorMess);


            YesButton = new CurvedButtons(new Size(80, 40), new Point(10, 150), 25, "../../User Interface Recources/Custom_Button_Small.png", "Place Sign", Dosis_font_family, this.settingScreen, this.settingScreen.BackColor);
            this.settingScreen.Controls.Add(YesButton);

            NoButton = new CurvedButtons(new Size(80, 40), new Point(110, 150), 25, "../../User Interface Recources/Custom_Button_Small.png", "Don't Place", Dosis_font_family, this.settingScreen, this.settingScreen.BackColor);
            this.settingScreen.Controls.Add(NoButton);
        }
        public void placeSign()
        {
            this.settingScreen.Hide();
        }

        public void openMenu()
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

            this.settingScreen.Show();
            this.settingScreen.BringToFront();

        }

        public void onSignClick(AbstractSign _sign)
        {
            selected = (YieldSign)_sign;
            openMenu();
        }

        public void newSign()
        {
            YieldSign _temp = new YieldSign();
            this.signController.Signs.Add(_temp);

            onSignClick(_temp);
        }

        public void deleteSign()
        {
            this.signController.Signs.Remove(selected);
            this.settingScreen.Hide();
        }
    }
}
