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
    public class StopSignController : AbstractSignController
    {
        public Label QuestionLabel, errorMess, BeginLabel, EndLabel;
        public CurvedButtons BeginButton, EndButton, CancelButton;
        
        public MainSignController signController;

        public StopSign selected;

        public Point point1;
        public Point point2;

        PrivateFontCollection Font_collection = new PrivateFontCollection();

        public StopSignController(Form _main, MainSignController _signcontroller)
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
            QuestionLabel.Text = "Location for Sign?";
            QuestionLabel.Location = new Point(20, 10);
            this.settingScreen.Controls.Add(QuestionLabel);

            errorMess = new Label();
            errorMess.Location = new Point(20, 60);
            errorMess.Text = "";
            errorMess.ForeColor = Color.Red;
            this.settingScreen.Controls.Add(errorMess);

            BeginLabel = new Label();
            BeginLabel.Location = new Point(20, 100);
            BeginLabel.Text = "should be cords";
            this.settingScreen.Controls.Add(BeginLabel);

            EndLabel = new Label();
            EndLabel.Location = new Point(120, 100);
            EndLabel.Text = "should be cords";
            this.settingScreen.Controls.Add(EndLabel);

            BeginButton = new CurvedButtons(new Size(80, 40), new Point(10, 150), 25, "../../User Interface Recources/Custom_Button_Small.png", "First Point", Dosis_font_family, this.settingScreen, this.settingScreen.BackColor);
            this.settingScreen.Controls.Add(BeginButton);

            EndButton = new CurvedButtons(new Size(80, 40), new Point(110, 150), 25, "../../User Interface Recources/Custom_Button_Small.png", "Second Point", Dosis_font_family, this.settingScreen, this.settingScreen.BackColor);
            this.settingScreen.Controls.Add(EndButton);

            CancelButton = new CurvedButtons(new Size(80, 40), new Point(60 , 300), 25, "../../User Interface Recources/Custom_Button_Small.png", "Cancel", Dosis_font_family, this.settingScreen, this.settingScreen.BackColor);
            this.settingScreen.Controls.Add(CancelButton);


        }
        public void firstButton()
        {
            Point _correctpoint = point1;

            if (_correctpoint != null)
            {
                selected.editLocation(_correctpoint);
                this.settingScreen.Hide();
            }
            else
            {
                errorMessage("Input too high");
                return;
            }
        }
        public void secondButton()
        {
            Point _correctpoint = point2;

            if (_correctpoint != null)
            {
                selected.editLocation(_correctpoint);
                this.settingScreen.Hide();
            }
            else
            {
                errorMessage("Input not correct");
                return;
            }
        }

        private void errorMessage(string _error)
        {
            this.errorMess.Text = _error;            
            this.settingScreen.Invalidate();
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
            this.BeginLabel.Text = point1.ToString();
            this.EndLabel.Text = point2.ToString();

            Console.WriteLine(this.settingScreen.Visible.ToString());

            this.settingScreen.Show();
            this.settingScreen.BringToFront();

        }

        public void onSignClick(AbstractSign _sign)
        {
            selected = (StopSign)_sign;
            openMenu();
        }

        public void newSign(Point _start, Point _end)
        {
            StopSign _temp = new StopSign(_end.X, _end.Y);
            this.signController.Signs.Add(_temp);
            this.point1 = _start;
            this.point2 = _end;

            onSignClick(_temp);
        }

        public void deleteSign()
        {

            this.signController.Signs.Remove(selected);
            this.settingScreen.Hide();
        }



    }
}
