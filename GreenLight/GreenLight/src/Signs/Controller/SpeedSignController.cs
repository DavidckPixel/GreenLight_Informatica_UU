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
        public Label label1, label2, FlipLabel;
        public ComboBox Combobox1;
        public PictureBox pb1;
        public CurvedButtons Button1;
        public CurvedButtons Button2;
        public Label errorMess;
        public SpeedSign selected;
        public Speedsign ss;
        public AbstractSign thisSign;

        public SpeedSignController(Form _main, MainSignController _signcontroller)
        {
            this.signController = _signcontroller;
            this.mainScreen = _main;
        }

        public override void initSettingScreen()
        {
            this.settingScreen = new Pop_Up_Form(new Size(300,300));
            //is.settingScreen.MdiParent = this.mainScreen;
            this.settingScreen.BackColor = Color.FromArgb(255, 255, 255);
            this.settingScreen.FormBorderStyle = FormBorderStyle.None;

            this.settingScreen.Controls.Add(label2);

            FlipLabel = new Label();
            FlipLabel.Text = "The sign has to be on the right side of the road.";
            FlipLabel.Location = new Point(30, 140);
            FlipLabel.Size = new Size(230, 20);
            FlipLabel.TextAlign = ContentAlignment.MiddleCenter;
            this.settingScreen.Controls.Add(FlipLabel);

            CurvedButtons FlipButton = new CurvedButtons(new Size(100, 40), new Point(100, 170), 25, "../../src/User Interface Recources/Custom_Button.png", "Flip sign", DrawData.Dosis_font_family, this.settingScreen, this.settingScreen.BackColor);
            FlipButton.Click += (object o, EventArgs ea) => {  General_Form.Main.BuildScreen.builder.signController.flipSing(thisSign); };
            this.settingScreen.Controls.Add(FlipButton);

            label1 = new Label();
            label1.Text = "Change the speedlimit on this road to: ";
            label1.Location = new Point(30, 45);
            label1.Size = new Size(150, 40);
            label1.TextAlign = ContentAlignment.MiddleCenter;

            Move_panel Move = new Move_panel(this.settingScreen);
            Move.Location = new Point(0, 0);
            Move.Size = new Size(300, 35);
            Move.BackColor = Color.FromArgb(142, 140, 144);
            this.settingScreen.Controls.Add(Move);


            this.settingScreen.Controls.Add(label1);

            errorMess = new Label();
            errorMess.Location = new Point(80, 85);
            errorMess.Text = "";
            errorMess.ForeColor = Color.Red;

            this.settingScreen.Controls.Add(errorMess);

            Combobox1 = new ComboBox();
            Combobox1.Items.AddRange(new object[] { "30", "40", "50", "60", "70", "80", "90", "100", "110", "120", "130" });
            Combobox1.DropDownWidth = 75;
            Combobox1.DropDownStyle = ComboBoxStyle.DropDownList;
            Combobox1.Text = "50";
            Combobox1.Location = new Point(100, 115);

            this.settingScreen.Controls.Add(Combobox1);

            ss = new Speedsign(new Size(75, 75), new Point(200, 40));
            ss.speed = 0;
            this.settingScreen.Controls.Add(ss);

            CurvedButtons Divider1 = new CurvedButtons();
            Divider1.Location = new Point(20, 220);
            this.settingScreen.Controls.Add(Divider1);

            Button1 = new CurvedButtons(new Size(80, 40), new Point(45, 240), 25, "../../src/User Interface Recources/Custom_Small_Button.png", "Done", DrawData.Dosis_font_family, this.settingScreen, this.settingScreen.BackColor);
            Button1.Click += (object o, EventArgs ea) => { General_Form.Main.BuildScreen.builder.signController.speedSign.saveButton(); };
            this.settingScreen.Controls.Add(Button1);

            Button2 = new CurvedButtons(new Size(90, 40), new Point(155, 240), 25, "../../src/User Interface Recources/Custom_Small_Button.png", "Delete", DrawData.Dosis_font_family, this.settingScreen, this.settingScreen.BackColor);

            Button2.Click += (object o, EventArgs ea) => { General_Form.Main.BuildScreen.builder.signController.speedSign.deleteSign(); };
            this.settingScreen.Controls.Add(Button2);
        }

        public void saveButton()
        {
            string _valueS;
            int _value;
            _valueS = Combobox1.Text;

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
            this.mainScreen.Invalidate();
        }

        private void errorMessage(string _error)
        {
            this.errorMess.Text = _error;
            this.Combobox1.Text = selected.getSpeed().ToString();
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
            this.Combobox1.Text = selected.getSpeed().ToString();

            Console.WriteLine(this.settingScreen.Visible.ToString());

            this.ss.Invalidate();
            settingScreen.ShowDialog();
            settingScreen.BringToFront();

        }

        public override void onSignClick(AbstractSign _sign)
        {
            Console.WriteLine("SIGN HAS BEEN SELECTED!!!");
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
