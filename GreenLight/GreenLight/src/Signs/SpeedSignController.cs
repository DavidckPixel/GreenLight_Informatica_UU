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
    public class SpeedSignController : AbstractSignController
    {
        public Label label1;
        public TextBox Textbox1;
        public CurvedButtons Button1;
        public CurvedButtons Button2;
        public Label errorMess;

        public MainSignController signController;

        public SpeedSign selected;

        PrivateFontCollection Font_collection = new PrivateFontCollection();

        public SpeedSignController(Form _main, MainSignController _signcontroller)
        {
            this.signController = _signcontroller;
            this.mainScreen = _main;
        }

        public override void initSettingScreen()
        {
            this.settingScreen = new Form();
            //is.settingScreen.MdiParent = this.mainScreen;

            this.settingScreen.Size = new Size(300, 600);
            this.settingScreen.BackColor = Color.FromArgb(255,255,255);
            this.settingScreen.FormBorderStyle = FormBorderStyle.None;

            label1 = new Label();
            label1.Text = "Speed?";
            label1.Location = new Point(50, 50);

            this.settingScreen.Controls.Add(label1);

            errorMess = new Label();
            errorMess.Location = new Point(80, 100);
            errorMess.Text = "";
            errorMess.ForeColor = Color.Red;

            this.settingScreen.Controls.Add(errorMess);

            Textbox1 = new TextBox();
            Textbox1.Text = "50";
            Textbox1.Location = new Point(40, 150);
            this.settingScreen.Controls.Add(Textbox1);

            //Waarschijnlijk beter om mee te geven aan initSettingScreen zoals in Build_sub_menu.Initialize            
            Font_collection.AddFontFile("../../Fonts/Dosis-bold.ttf");
            FontFamily Dosis_font_family = Font_collection.Families[0];

            Button1 = new CurvedButtons(new Size(80, 40), new Point(10, 400), 25, "../../User Interface Recources/Custom_Button_Small.png", "Done", Dosis_font_family, this.settingScreen, this.settingScreen.BackColor);
            Button1.Click += (object o, EventArgs ea) => { General_Form.Main.BuildScreen.builder.signController.speedSign.saveButton(); };
            this.settingScreen.Controls.Add(Button1); 

            Button2 = new CurvedButtons(new Size(80, 40), new Point(120, 400), 25, "../../User Interface Recources/Custom_Button_Small.png", "Delete", Dosis_font_family, this.settingScreen, this.settingScreen.BackColor);
            Button2.Click += (object o, EventArgs ea) => { General_Form.Main.BuildScreen.builder.signController.speedSign.deleteSign(); };
            this.settingScreen.Controls.Add(Button2);
        }

        public void saveButton()
        {
            string _valueS;
            int _value;

            _valueS = Textbox1.Text;
            
            try
            {
                _value = Int32.Parse(_valueS);
            }
            catch (Exception)
            {
                errorMessage("Invalid Input");
                return;
            }

            if(_value > 0 && _value <= 300)
            {
                selected.editSpeed(_value);
                this.settingScreen.Hide();
            }
            else
            {
                errorMessage("Input too high");
                return;
            }
            
        }

        private void errorMessage(string _error)
        {
            this.errorMess.Text = _error;
            this.Textbox1.Text = selected.getSpeed().ToString();
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
            this.Textbox1.Text = selected.getSpeed().ToString();

            Console.WriteLine(this.settingScreen.Visible.ToString());

            this.settingScreen.Show();
            this.settingScreen.BringToFront();

        }

        public override void onSignClick(AbstractSign _sign)
        {
            selected = (SpeedSign)_sign;
            openMenu();
        }

        public override AbstractSign newSign()
        {
            SpeedSign _temp = new SpeedSign(this);
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
