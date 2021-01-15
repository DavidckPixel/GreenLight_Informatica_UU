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
        public CurvedButtons SaveButton, CancelButton;

        public StopSign selected;

        public Point point1;
        public Point point2;

        public StopSignController(Form _main, MainSignController _signcontroller)
        {
            this.signController = _signcontroller;
            this.mainScreen = _main;
        }

        public override void initSettingScreen()
        {
            this.settingScreen = new Form();

            this.settingScreen.Size = new Size(400, 400);
            this.settingScreen.BackColor = Color.FromArgb(255, 255, 255);
            this.settingScreen.FormBorderStyle = FormBorderStyle.None;

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

            SaveButton = new CurvedButtons(new Size(80, 40), new Point(10, 300), 25, "../../User Interface Recources/Custom_Small_Button.png", "Done", DrawData.Dosis_font_family, this.settingScreen, this.settingScreen.BackColor);
            SaveButton.Click += (object o, EventArgs ea) => { General_Form.Main.BuildScreen.builder.signController.stopSign.placeSign(); };
            this.settingScreen.Controls.Add(SaveButton);

            CancelButton = new CurvedButtons(new Size(80, 40), new Point(60 , 300), 25, "../../User Interface Recources/Custom_Small_Button.png", "Delete", DrawData.Dosis_font_family, this.settingScreen, this.settingScreen.BackColor);
            CancelButton.Click += (object o, EventArgs ea) => { General_Form.Main.BuildScreen.builder.signController.stopSign.deleteSign(); };
            this.settingScreen.Controls.Add(CancelButton);


        }
        
        public void placeSign()
        {
            this.settingScreen.Hide();
        }

        public void errorMessage(string _error)
        {
            this.errorMess.Text = _error;            
            this.settingScreen.Invalidate();
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
            this.BeginLabel.Text = point1.ToString();
            this.EndLabel.Text = point2.ToString();

            Console.WriteLine(this.settingScreen.Visible.ToString());

            this.settingScreen.Show();
            this.settingScreen.BringToFront();

        }

        public override void onSignClick(AbstractSign _sign)
        {
            selected = (StopSign)_sign;
            openMenu();
        }

        public override AbstractSign newSign()
        {
            StopSign _temp = new StopSign(this);
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
