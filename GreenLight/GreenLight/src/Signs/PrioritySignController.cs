﻿using System;
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
        public Label QuestionLabel, errorMess;
        public CurvedButtons YesButton, NoButton;

        public MainSignController signController;

        public PrioritySign selected;

        PrivateFontCollection Font_collection = new PrivateFontCollection();

        public PrioritySignController(Form _main, MainSignController _signcontroller)
        {
            this.signController = _signcontroller;
            this.mainScreen = _main;
        }

        public override void initSettingScreen()
        {
            //Waarschijnlijk beter om mee te geven aan initSettingScreen zoals in Build_sub_menu.Initialize
            Font_collection.AddFontFile("../../Fonts/Dosis-bold.ttf");
            FontFamily Dosis_font_family = Font_collection.Families[0];


            this.settingScreen = new Form();

            this.settingScreen.Size = new Size(400, 400);
            this.settingScreen.BackColor = Color.FromArgb(255, 255, 255);
            this.settingScreen.FormBorderStyle = FormBorderStyle.None;

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
            YesButton.Click += (object o, EventArgs ea) => { General_Form.Main.BuildScreen.builder.signController.prioritySignC.placeSign(); };
            this.settingScreen.Controls.Add(YesButton);

            NoButton = new CurvedButtons(new Size(80, 40), new Point(110, 150), 25, "../../User Interface Recources/Custom_Button_Small.png", "Don't Place", Dosis_font_family, this.settingScreen, this.settingScreen.BackColor);
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

            this.settingScreen.Show();
            this.settingScreen.BringToFront();

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
