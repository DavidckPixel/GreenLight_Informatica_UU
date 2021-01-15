using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GreenLight
{
    public partial class Build_sub_menu : UserControl
    {
        public Build_sub_menu(int Menu_width, Form Form, FontFamily Dosis_font_family)
        {
            this.BackColor = Color.FromArgb(255,255,255);
            this.Size = new Size(User_Controls.Config.standardSubMenu["subMenuWidth"], Form.Height); //Size in Width
            this.Location = new Point(Form.Width - Menu_width, 0);

            Initialize(Form, Menu_width, Dosis_font_family);
        }
        public void Size_adjust(Form Form, int Sub_menu_width, FontFamily Dosis_font_family)
        {
            this.Size = new Size(Sub_menu_width, Form.Height);
            this.Location = new Point(Form.Width - Sub_menu_width, 0);
            this.Controls.Clear();
            Initialize(Form,Sub_menu_width, Dosis_font_family);
        }

        //Cleaner maar General_form moet form zijn
        /*
        public Build_sub_menu(int Sub_menu_width, General_form General_form, FontFamily Dosis_font_family)
        {
            this.BackColor = Color.FromArgb(255,255,255);
            this.Size = new Size(250, General_form.Height);
            this.Location = new Point(General_form.Width - Sub_menu_width, 0);
            General_form.SizeChanged += (object o, EventArgs EA) => {
                this.Size = new Size(Sub_menu_width, General_form.Height);
                this.Location = new Point(General_form.Width - Sub_menu_width, 0);
                this.Controls.Clear();
                Initialize(General_form, Sub_menu_width, Dosis_font_family);
            };
            Initialize(General_form, Sub_menu_width, Dosis_font_family);
        }*/

        private void Initialize(Form Form, int Sub_menu_width, FontFamily Dosis_font_family)
        {
            Dictionary<string, int> menu = User_Controls.Config.buildSubMenu;

            

            CurvedButtons Logo = new CurvedButtons(Form, 1);
            Logo.Location = new Point(User_Controls.Config.standardSubMenu["logoX"], User_Controls.Config.standardSubMenu["logoY"]);
            this.Controls.Add(Logo);

            CurvedButtons Divider1 = new CurvedButtons();
            Divider1.Location = new Point(User_Controls.Config.standardSubMenu["deviderX"], User_Controls.Config.standardSubMenu["deviderY"]);
            this.Controls.Add(Divider1);

            Move_panel Drag_pad = new Move_panel(Form);
            this.Controls.Add(Drag_pad);

            PictureBox Elements_header = new PictureBox();
            Elements_header.Size = new Size(menu["elementHeaderSizeX"], menu["elementHeaderSizeY"]); //elementHeaderSizeX , //elementHeaderSizeY
            Elements_header.SizeMode = PictureBoxSizeMode.StretchImage;
            Elements_header.Location = new Point(menu["elementHeaderX"], menu["elementHeaderY"]); //elementHeaderX, //elementHeaderY
            Elements_header.Image = Image.FromFile("../../User Interface Recources/Elements_Header.png");
            this.Controls.Add(Elements_header);

            //buttonSizeL //buttonSizeS //devider2 //devider4 //devider3 /simStartSizeX /simStartSizeY / simStartX / simStartY / buttonL / buttonS / buttonHome / buttonSave / buttonRoad / buttonLight / buttonSign / buttonBuilding

            CurvedButtons Home_button = new CurvedButtons(new Size(menu["buttonSizeL"], menu["buttonSizeS"]), new Point(Sub_menu_width / 2 - menu["buttonHome"], menu["buttonL"]), 25, "../../User Interface Recources/Custom_Button_Small.png", "Home", Dosis_font_family, Form, this.BackColor);
            Home_button.Click += (object o, EventArgs EA) => { General_Form.Main.SwitchControllers(General_Form.Main.MenuController); };
            this.Controls.Add(Home_button);

            CurvedButtons Save_button = new CurvedButtons(new Size(menu["buttonSizeL"], menu["buttonSizeS"]), new Point(Sub_menu_width / 2 + menu["buttonSave"], menu["buttonL"]), 25, "../../User Interface Recources/Custom_Button_Small.png", "Save", Dosis_font_family, Form, this.BackColor);
            Save_button.Click += (object o, EventArgs EA) => { };
            this.Controls.Add(Save_button);

            CurvedButtons Road_button = new CurvedButtons(new Size(menu["buttonSizeS"], menu["buttonSizeS"]), new Point(Sub_menu_width / 2 - menu["buttonRoad"], menu["buttonS"]), 25, "../../User Interface Recources/Road_Button.png", this.BackColor);
            Road_button.Click += (object o, EventArgs EA) => { General_Form.Main.BuildScreen.SwitchSubMenus("Roads");};
            this.Controls.Add(Road_button);

            CurvedButtons Light_button = new CurvedButtons(new Size(menu["buttonSizeS"], menu["buttonSizeS"]), new Point(Sub_menu_width / 2 - menu["buttonLight"], menu["buttonS"]), 25, "../../User Interface Recources/Traffic_Light_Button.png", this.BackColor);
            Light_button.Click += (object o, EventArgs EA) => { General_Form.Main.BuildScreen.SwitchSubMenus("Lights"); };
            this.Controls.Add(Light_button);

            CurvedButtons Sign_button = new CurvedButtons(new Size(menu["buttonSizeS"], menu["buttonSizeS"]), new Point(Sub_menu_width / 2 + menu["buttonSign"], menu["buttonS"]), 25, "../../User Interface Recources/Speed_Sign_Button.png", this.BackColor);
            Sign_button.Click += (object o, EventArgs EA) =>  { General_Form.Main.BuildScreen.SwitchSubMenus("Signs"); };
            this.Controls.Add(Sign_button);

            CurvedButtons Building_button = new CurvedButtons(new Size(menu["buttonSizeS"], menu["buttonSizeS"]), new Point(Sub_menu_width / 2 + menu["buttonBuilding"], menu["buttonS"]), 25, "../../User Interface Recources/Building_Button.png", this.BackColor);
            Building_button.Click += (object o, EventArgs EA) => { General_Form.Main.BuildScreen.SwitchSubMenus("Buildings"); };
            this.Controls.Add(Building_button);

            CurvedButtons Divider2 = new CurvedButtons();
            Divider2.Location = new Point(0, menu["devider2"]); //devider2
            this.Controls.Add(Divider2);

            CurvedButtons Divider4 = new CurvedButtons();
            Divider4.Location = new Point(0, Form.Height - menu["devider4"]); //devider4
            this.Controls.Add(Divider4);     

            CurvedButtons Undo_button = new CurvedButtons(new Size(30, 30), new Point(10, Form.Height - menu["simStartY"]), 25, "../../User Interface Recources/Reset_Simulation_Button.png", this.BackColor);
            Undo_button.Click += (object o, EventArgs EA) => { General_Form.Main.BuildScreen.builder.roadBuilder.UndoRoad(); };
            this.Controls.Add(Undo_button);


            CurvedButtons Start_sim_button = new CurvedButtons(new Size(menu["simStartSizeX"], menu["simStartSizeY"]), new Point(Sub_menu_width / 2 - menu["simStartX"], Form.Height - menu["simStartY"]), 25,
                "../../User Interface Recources/Custom_Button.png", "Start simulation", Dosis_font_family, Form, this.BackColor);
            Start_sim_button.Click += (object o, EventArgs EA) => { General_Form.Main.MenuController.SwitchToSimulation();  };
            this.Controls.Add(Start_sim_button);
           

            CurvedButtons Divider3 = new CurvedButtons();
            Divider3.Location = new Point(0, menu["devider3"]); //devider3
            this.Controls.Add(Divider3);
        }
    }
}
