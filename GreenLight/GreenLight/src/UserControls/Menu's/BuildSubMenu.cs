using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;

namespace GreenLight
{
    public partial class BuildSubMenu : UserControl
    {
        public List<CurvedButtons> BSM_Buttons = new List<CurvedButtons>();
        public CurvedButtons Road_button;
        public CheckBox AutoSave;
        public BuildSubMenu(int Menu_width, Form Form, FontFamily Dosis_font_family)
        {
            this.BackColor = Color.FromArgb(255,255,255);
            this.Size = new Size(UserControls.Config.standardSubMenu["subMenuWidth"], Form.Height); //Size in Width
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
            Dictionary<string, int> menu = UserControls.Config.buildSubMenu;


            CurvedButtons Logo = new CurvedButtons(Form, 1);
            Logo.Location = new Point(UserControls.Config.standardSubMenu["logoX"], UserControls.Config.standardSubMenu["logoY"]);
            this.Controls.Add(Logo);

            CurvedButtons Divider1 = new CurvedButtons();
            Divider1.Location = new Point(UserControls.Config.standardSubMenu["deviderX"], UserControls.Config.standardSubMenu["deviderY"]);
            this.Controls.Add(Divider1);

            MovePanel Drag_pad = new MovePanel(Form);
            this.Controls.Add(Drag_pad);

            PictureBox Elements_header = new PictureBox();
            Elements_header.Size = new Size(menu["elementHeaderSizeX"], menu["elementHeaderSizeY"]); //elementHeaderSizeX , //elementHeaderSizeY
            Elements_header.SizeMode = PictureBoxSizeMode.StretchImage;
            Elements_header.Location = new Point(menu["elementHeaderX"], menu["elementHeaderY"]); //elementHeaderX, //elementHeaderY
            Elements_header.Image = Image.FromFile("../../src/User Interface Recources/Elements_Header.png");
            this.Controls.Add(Elements_header);

            //buttonSizeL //buttonSizeS //devider2 //devider4 //devider3 /simStartSizeX /simStartSizeY / simStartX / simStartY / buttonL / buttonS / buttonHome / buttonSave / buttonRoad / buttonLight / buttonSign / buttonBuilding

            CurvedButtons Home_button = new CurvedButtons(new Size(menu["buttonSizeL"], menu["buttonSizeS"]), new Point(Sub_menu_width / 2 - menu["buttonHome"], menu["buttonL"]), 25, "../../src/User Interface Recources/Custom_Small_Button.png", "Home", Dosis_font_family, Form, this.BackColor);
            Home_button.Click += (object o, EventArgs EA) => { General_Form.Main.SwitchControllers(General_Form.Main.MenuController); };
            this.Controls.Add(Home_button);

            CurvedButtons Save_button = new CurvedButtons(new Size(menu["buttonSizeL"], menu["buttonSizeS"]), new Point(Sub_menu_width / 2 + menu["buttonSave"], menu["buttonL"]), 25, "../../src/User Interface Recources/Custom_Small_Button.png", "Save", Dosis_font_family, Form, this.BackColor);
            Save_button.Click += (object o, EventArgs EA) => { General_Form.Main.Save(); };
            this.Controls.Add(Save_button);

            System.Timers.Timer t = new System.Timers.Timer(TimeSpan.FromMinutes(5).TotalMilliseconds);
            t.AutoReset = true;
            t.Elapsed += (object to, ElapsedEventArgs EEA) => { General_Form.Main.Save();
                Console.WriteLine("AutoSaved");
            };

            Label Auto_save_label = new Label();
            Auto_save_label.Text = "Autosave:";
            Auto_save_label.Font = new Font(Dosis_font_family, 11, FontStyle.Bold);
            Auto_save_label.ForeColor = Color.FromArgb(142, 140, 144);
            Auto_save_label.Location = new Point(menu["autosavelabelX"], menu["autosaveY"]);
            this.Controls.Add(Auto_save_label);

            AutoSave = new CheckBox();
            AutoSave.Checked = false;
            AutoSave.Location = new Point(menu["autosaveboxX"], menu["autosaveY"]+menu["autosavediff"]);
            AutoSave.Size = new Size(25, 25);
            AutoSave.Click += (object o, EventArgs EA) => { 
                if (AutoSave.Checked)
                {
                    General_Form.Main.Save();
                    t.Start();
                }
                else 
                {
                    t.Stop();
                }
            };
            this.Controls.Add(AutoSave);

            Road_button = new CurvedButtons(new Size(menu["buttonSizeS"], menu["buttonSizeS"]), new Point(Sub_menu_width / 2 - menu["buttonRoad"], menu["buttonS"]), 25, "../../src/User Interface Recources/Road_Button.png", this.BackColor);
            Road_button.Click += (object o, EventArgs EA) => { General_Form.Main.UserInterface.Reset_All_Buttons(Road_button, Road_button.Image_path); General_Form.Main.BuildScreen.SwitchSubMenus("Roads");};
            this.Controls.Add(Road_button);
            BSM_Buttons.Add(Road_button);

            CurvedButtons Sign_button = new CurvedButtons(new Size(menu["buttonSizeS"], menu["buttonSizeS"]), new Point(Sub_menu_width / 2 - menu["buttonSign"], menu["buttonS"]), 25, "../../src/User Interface Recources/Speed_Sign_Button.png", this.BackColor);
            Sign_button.Click += (object o, EventArgs EA) => { General_Form.Main.UserInterface.Reset_All_Buttons(Sign_button, Sign_button.Image_path); General_Form.Main.BuildScreen.SwitchSubMenus("Signs"); };
            this.Controls.Add(Sign_button);
            BSM_Buttons.Add(Sign_button);

            CurvedButtons Settings_button = new CurvedButtons(new Size(menu["buttonSizeS"], menu["buttonSizeS"]), new Point(Sub_menu_width / 2 + menu["buttonBuilding"], menu["buttonS"]), 25, "../../src/User Interface Recources/Setting_Button.png", this.BackColor);
            Settings_button.Click += (object o, EventArgs EA) => { General_Form.Main.UserInterface.Reset_All_Buttons(Settings_button, Settings_button.Image_path); General_Form.Main.BuildScreen.SwitchSubMenus("Buildings"); };
            this.Controls.Add(Settings_button);
            BSM_Buttons.Add(Settings_button);

            CurvedButtons Divider2 = new CurvedButtons();
            Divider2.Location = new Point(0, menu["divider2"]); //divider2
            this.Controls.Add(Divider2);

            CurvedButtons Divider4 = new CurvedButtons();
            Divider4.Location = new Point(0, Form.Height - menu["divider4"]); //divider4
            this.Controls.Add(Divider4);     

            CurvedButtons Undo_button = new CurvedButtons(new Size(30, 30), new Point(10, Form.Height - menu["simStartY"]+3), 20, "../../src/User Interface Recources/Reset_Simulation_Button.png", this.BackColor);
            Undo_button.Click += (object o, EventArgs EA) => { General_Form.Main.BuildScreen.builder.roadBuilder.UndoRoad(); };
            this.Controls.Add(Undo_button);

            CurvedButtons Toggle_button = new CurvedButtons(new Size(30, 30), new Point(Sub_menu_width - 40, Form.Height - menu["simStartY"]+3), 20, "../../src/User Interface Recources/Toggle_Button.png", this.BackColor);
            Toggle_button.Click += (object o, EventArgs EA) => {General_Form.Main.BuildScreen.Toggle =  General_Form.Main.BuildScreen.ToggleHitbox(); General_Form.Main.BuildScreen.Screen.Invalidate();};
            this.Controls.Add(Toggle_button);
            BSM_Buttons.Add(Toggle_button);

            CurvedButtons Start_sim_button = new CurvedButtons(new Size(menu["simStartSizeX"], menu["simStartSizeY"]), new Point(Sub_menu_width / 2 - menu["simStartX"], Form.Height - menu["simStartY"]), 25,
                "../../src/User Interface Recources/Custom_Button.png", "Start simulation", Dosis_font_family, Form, this.BackColor);
            Start_sim_button.Click += (object o, EventArgs EA) => { General_Form.Main.MenuController.SwitchToSimulation();  };
            this.Controls.Add(Start_sim_button);
           
            CurvedButtons Divider3 = new CurvedButtons();
            Divider3.Location = new Point(0, menu["divider3"]); //divider3
            this.Controls.Add(Divider3);
        }
    }
}
