using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace GreenLight
{
    public partial class SimulationSubVehicleMenu : UserControl
    {

        public SelectionBox Selection_box;
        Slider Surface, Max_speed, Cw, Length, HorsePower, Weight, Occurunce;

        public SimulationSubVehicleMenu(int Menu_width, Form Form, FontFamily Dosis_font_family)
        {
            this.BackColor = Color.FromArgb(255, 255, 255);
            this.Size = new Size(Menu_width, Form.Height - UserControls.Config.simElementsMenu["menuY"] - UserControls.Config.simElementsMenu["menuSizeY"]); //menuSizeY
            this.Location = new Point(Form.Width - Menu_width, UserControls.Config.simElementsMenu["menuY"]);  //menuY
            this.AutoScroll = true;
            Initialize(Form, Menu_width, Dosis_font_family);
        }
        public void Size_adjust(Form Form, int Sub_menu_width, FontFamily Dosis_font_family)
        {
            this.Size = new Size(Sub_menu_width, Form.Height - UserControls.Config.simElementsMenu["menuY"] - UserControls.Config.simElementsMenu["menuSizeY"]);
            this.Location = new Point(Form.Width - Sub_menu_width, UserControls.Config.simElementsMenu["menuY"]);
            this.Controls.Clear();
            Initialize(Form, Sub_menu_width, Dosis_font_family);
        }

        private void Initialize(Form Form, int Sub_menu_width, FontFamily Dosis_font_family)
        {
            Dictionary<string, int> menu = UserControls.Config.simElementsMenu;
            int _sliderX = menu["sliderX"];
            int _startY = menu["sliderStartVehicleY"];
            int _diffY = menu["sliderDiffY"];
            int _textX = menu["sliderTextX"];
            int _textY = menu["startTextY"];

            List<string> _temp = VehicleController.getStringVehicleStats();

            //VehicleController controller = General_Form.Main.SimulationScreen.Simulator.vehicleController;
            

            Selection_box = new SelectionBox(Form, Dosis_font_family, _temp, new Action(this.SetValues), new Action(this.AddVehicle), new Action(this.DeleteVehicle));
            if (Form.WindowState == FormWindowState.Maximized) Selection_box.Location = new Point(UserControls.Config.standardSubMenu["selectionBoxMaxX"], UserControls.Config.standardSubMenu["selectionBoxMaxY"]);
            else Selection_box.Location = new Point(UserControls.Config.standardSubMenu["selectionBoxX"], UserControls.Config.standardSubMenu["selectionBoxY"]);

            this.Controls.Add(Selection_box);

            Dictionary<string, int> vehiclemenu = UserControls.Config.simVehicle;
            /*SliderText name_label = new SliderText(Dosis_font_family, new Point(_sliderX, _startY + 3 * _diffY), "Vehicle Name: ");
            this.Controls.Add(name_label);

            TextBox name = new TextBox();
            name.Location = new Point(_sliderX, _startY + 3 * _diffY);
            this.Controls.Add(name);*/

            CurvedButtons saveButton = new CurvedButtons(new Size(80, 40), new Point(_sliderX, _startY + 3 * _diffY), 25, "../../src/User Interface Recources/Custom_Small_Button.png", "Save", DrawData.Dosis_font_family, null, this.BackColor);
            saveButton.Click += (object o, EventArgs ea) => { VehicleTypeConfig.SaveJson(); };
            this.Controls.Add(saveButton);

            Cw = new Slider(new Point(_sliderX, _startY + 2 * _diffY), vehiclemenu["surfaceMin"], vehiclemenu["surfaceMax"]);
            this.Controls.Add(Cw);
            SliderText Cw_label = new SliderText(Dosis_font_family, new Point(_sliderX, _textY + 2 * _diffY), "Drag Co:");
            this.Controls.Add(Cw_label);
            SliderText Cw_Value = new SliderText(Dosis_font_family, new Point(_textX, _textY + 2 * _diffY), (Cw.Value / 10 ).ToString() + " ");
            this.Controls.Add(Cw_Value);
            Cw.ValueChanged += (object o, EventArgs EA) => 
            {
                General_Form.Main.SimulationScreen.Simulator.vehicleController.ChangeCw(Cw.Value, (Slider)o);
                Cw_Value.Text = (((double)(Cw.Value)) / 10).ToString() + " ";
            };

            Surface = new Slider(new Point(_sliderX, _startY + _diffY), vehiclemenu["surfaceMin"], vehiclemenu["surfaceMax"]);
            this.Controls.Add(Surface);
            SliderText Surface_label = new SliderText(Dosis_font_family, new Point(_sliderX, _textY + 1 * _diffY), "Frontal Surface:");
            this.Controls.Add(Surface_label);
            SliderText Surface_Value = new SliderText(Dosis_font_family, new Point(_textX, _textY + 1* _diffY), (Surface.Value / 10).ToString() + " m^2");
            this.Controls.Add(Surface_Value);
            Surface.ValueChanged += (object o, EventArgs EA) => 
            {
                General_Form.Main.SimulationScreen.Simulator.vehicleController.ChangeSurface(Surface.Value, (Slider)o);
                Surface_Value.Text = (((double)(Surface.Value)) / 10).ToString() + " m^2";
            };

            Max_speed = new Slider(new Point(_sliderX, _startY), vehiclemenu["topSpeedMin"], vehiclemenu["topSpeedMax"]);
            this.Controls.Add(Max_speed);
            SliderText Max_speed_label = new SliderText(Dosis_font_family, new Point(_sliderX, _textY), "Topspeed:");
            this.Controls.Add(Max_speed_label);
            SliderText Max_speed_Value = new SliderText(Dosis_font_family, new Point(_textX, _textY), Max_speed.Value.ToString() + " km/h");
            this.Controls.Add(Max_speed_Value);
            Max_speed.ValueChanged += (object o, EventArgs EA) => 
            {
                General_Form.Main.SimulationScreen.Simulator.vehicleController.ChangeTopspeed(Max_speed.Value, (Slider)o);
                Max_speed_Value.Text = Max_speed.Value.ToString() + " km/h"; 
            };

            Length = new Slider(new Point(_sliderX, _startY - _diffY), vehiclemenu["lengthMin"], vehiclemenu["lengthMax"]);
            this.Controls.Add(Length);
            SliderText Length_label = new SliderText(Dosis_font_family, new Point(_sliderX, _textY - 1 * _diffY), "Length:");
            this.Controls.Add(Length_label);
            SliderText Length_Value = new SliderText(Dosis_font_family, new Point(_textX, _textY - 1 * _diffY), (Length.Value / 10).ToString() + " m");
            this.Controls.Add(Length_Value);
            Length.ValueChanged += (object o, EventArgs EA) =>
            {
                General_Form.Main.SimulationScreen.Simulator.vehicleController.ChangeLength(Length.Value, (Slider)o);
                Length_Value.Text = (((double)(Length.Value)) / 10).ToString() + " m";
            };

            HorsePower = new Slider(new Point(_sliderX, _startY - 2* _diffY), vehiclemenu["horsepwrMin"], vehiclemenu["horsepwrMax"]);
            this.Controls.Add(HorsePower);
            SliderText HorsePower_label = new SliderText(Dosis_font_family, new Point(_sliderX, _textY - 2 * _diffY), "Horsepower:");
            this.Controls.Add(HorsePower_label);
            SliderText HorsePower_Value = new SliderText(Dosis_font_family, new Point(_textX, _textY - 2 * _diffY), HorsePower.Value.ToString() + " hp");
            this.Controls.Add(HorsePower_Value);
            HorsePower.ValueChanged += (object o, EventArgs EA) => 
            {
                General_Form.Main.SimulationScreen.Simulator.vehicleController.ChangeMotorpwr(HorsePower.Value, (Slider)o);
                HorsePower_Value.Text = HorsePower.Value.ToString() + " hp";
            };

            Weight = new Slider(new Point(_sliderX, _startY - 3 * _diffY), vehiclemenu["weightMin"], vehiclemenu["weightMax"]);
            this.Controls.Add(Weight);
            SliderText Weight_label = new SliderText(Dosis_font_family, new Point(_sliderX, _textY - 3 * _diffY), "Weight:");
            this.Controls.Add(Weight_label);
            SliderText Weight_Value = new SliderText(Dosis_font_family, new Point(_textX, _textY - 3 * _diffY), Weight.Value.ToString() + " kg");
            this.Controls.Add(Weight_Value);
            Weight.ValueChanged += (object o, EventArgs EA) => 
            {
                General_Form.Main.SimulationScreen.Simulator.vehicleController.ChangeWeight(Weight.Value, (Slider)o);
                Weight_Value.Text = Weight.Value.ToString() + " kg"; 
            };

            Occurunce = new Slider(new Point(_sliderX, _startY - 4 * _diffY), vehiclemenu["occurenceMin"], vehiclemenu["occurenceMax"]);
            this.Controls.Add(Occurunce);
            SliderText Occurunce_label = new SliderText(Dosis_font_family, new Point(_sliderX, _textY - 4 * _diffY), "Occurunce:");
            this.Controls.Add(Occurunce_label);
            SliderText Occurunce_Value = new SliderText(Dosis_font_family, new Point(_textX, _textY - 4 * _diffY), Occurunce.Value.ToString() + " %");
            this.Controls.Add(Occurunce_Value);
            Occurunce.ValueChanged += (object o, EventArgs EA) => 
            {
                General_Form.Main.SimulationScreen.Simulator.vehicleController.ChangeOccurance(Occurunce.Value, (Slider)o);
                Occurunce_Value.Text = Occurunce.Value.ToString() + " %"; 
            };



            CurvedButtons Vehicles_header = new CurvedButtons(new Size(menu["headerSizeX"], menu["headerSizeY"]),
               new Point(menu["headerX"], menu["headerY"]), "../../src/User Interface Recources/Edit_Vehicle_Header.png");
            this.Controls.Add(Vehicles_header);



            /* Slider temp1 = new Slider(new Point(_sliderX, _start + _diff * 4), 0, 100); //sliderDiffY //sliderStart:100 / sliderX:25 //headerSizeX //headerSizeY //headerX //headerY
            this.Controls.Add(temp1);

            Slider temp2 = new Slider(new Point(_sliderX, _start + _diff * 3), 0, 100);
            this.Controls.Add(temp2);

            Slider temp3 = new Slider(new Point(_sliderX, _start + _diff * 2), 0, 100);
            this.Controls.Add(temp3);

            Slider temp4 = new Slider(new Point(_sliderX, _start + _diff), 0, 100);
            this.Controls.Add(temp4);

            Slider temp5 = new Slider(new Point(_sliderX, _start), 0, 100);
            this.Controls.Add(temp5);

            CurvedButtons Vehicles_header = new CurvedButtons(new Size(menu["headerSizeX"], menu["headerSizeY"]),
               new Point(menu["headerX"], menu["headerY"]), "../../src/User Interface Recources/Edit_Vehicle_Header.png");
            this.Controls.Add(Vehicles_header);*/

        }

        private void SetValues()
        {
            VehicleController controller = General_Form.Main.SimulationScreen.Simulator.vehicleController;

            VehicleStats _stats = FindVehicle();
            if(_stats == null)
            {
                return;
            }

            controller.SelectVehicle(_stats);

            Console.WriteLine("Je bent erdoor!!");

            Cw.Value = (int)(controller.selectedVehicle.Cw * 10);
            Surface.Value = (int)(controller.selectedVehicle.Surface * 10);
            Length.Value = (int)(controller.selectedVehicle.Length * 10);
            Max_speed.Value = controller.selectedVehicle.Topspeed;
            HorsePower.Value = controller.selectedVehicle.Motorpwr;
            Weight.Value = controller.selectedVehicle.Weight;
            Occurunce.Value = (int)(controller.selectedVehicle.Occurance);
        }

        private void AddVehicle()
        {
            string name = Interaction.InputBox("Enter Name: ", "Vehicle", "no name", 100, 100);

            if(name == "no name")
            {
                return;
            }
            if(name == "")
            {
                return;
            }

            Console.WriteLine("Vehicle Created With name : {0}", name);

            VehicleController.addVehicleStats(name, 5000, 5, 100, 100, 5, 1, 50); //WEIGHT (1000,40000) //LENGTH 3,12 // TOPSPEED 30,300 // HORSEPWR 40,1500 // SURFACE 1,20//CW = 1 //OCCU = 0-100
            SetValues();
        }

        private void DeleteVehicle()
        {
            VehicleController controller = General_Form.Main.SimulationScreen.Simulator.vehicleController;

            VehicleStats _stats = FindVehicle();
            if (_stats == null)
            {
                return;
            }

            if (_stats.canEdit == true) //if cna edit = true it cannot be edited nor deleted, so is returned
            {
                return;
            }

            controller.DeleteVehicle(_stats);
            Selection_box.Remove_Element(_stats.Name);
        }

        private VehicleStats FindVehicle()
        {
            int index = Selection_box.Selected_index;

            if (Selection_box.Selected_left_bool)
            {
                if (index < Selection_box.Elements_selected.Count)
                {
                    return VehicleController.getVehicleStat(Selection_box.Elements_selected[Selection_box.Selected_index]);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                if (index < Selection_box.Elements_available.Count)
                {
                    return VehicleController.getVehicleStat(Selection_box.Elements_available[Selection_box.Selected_index]);
                }
                else
                {
                    return null;
                }
            }
        }
        /*
         "topSpeedMin": 30,
        "topSpeedMax": 300,
        "occurenceMin": 0,
        "occurenceMax": 100,
        "horsepwrMin": 40,
        "horsepwrMax": 1500,
        "lengthMin": 30,
        "lengthMax": 120,
        "weightMin": 1000,
        "weightMax": 40000,
        "surfaceMin": 10,
        "surfaceMax": 200,
        "cwMin": 0,
        "cwMax": 10
        */
    }
}
