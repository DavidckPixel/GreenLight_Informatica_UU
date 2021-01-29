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
    /* This is the Simulation sub vehicle menu class. This class has a method AdjustSize to fit the size of the users window.
       In the initialize void the controls are added to the submenu.
       This user control is shown when the user is in the simulation screen and has selected the vehicle menu.
       The user can change properties of the vehicles using sliders and a selectionbox.
       Switching to this user control and closing the other user controls happens in the UserInterfaceController class. */
    public partial class SimulationSubVehicleMenu : UserControl
    {
        public SelectionBox selectionBox;
        public CurvedButtons saveButton;
        public Slider Surface, maxSpeed, Cw, Length, horsePower, Weight, Occurunce;

        public SimulationSubVehicleMenu(int _menuwidth, Form _form, FontFamily _dosisfontfamily)
        {
            this.BackColor = Color.FromArgb(255, 255, 255);
            this.Size = new Size(_menuwidth, _form.Height - UserControls.Config.simElementsMenu["menuY"] - UserControls.Config.simElementsMenu["menuSizeY"]); 
            this.Location = new Point(_form.Width - _menuwidth, UserControls.Config.simElementsMenu["menuY"]); 
            this.AutoScroll = true;
            Initialize(_form, _menuwidth, _dosisfontfamily);
        }
        public void AdjustSize(Form _form, int _submenuwidth, FontFamily _dosisfontfamily)
        {
            this.Size = new Size(_submenuwidth, _form.Height - UserControls.Config.simElementsMenu["menuY"] - UserControls.Config.simElementsMenu["menuSizeY"]);
            this.Location = new Point(_form.Width - _submenuwidth, UserControls.Config.simElementsMenu["menuY"]);
            this.Controls.Clear();
            Initialize(_form, _submenuwidth, _dosisfontfamily);
        }

        private void Initialize(Form _form, int _submenuwidth, FontFamily _dosisfontfamily)
        {
            Dictionary<string, int> menu = UserControls.Config.simElementsMenu;
            int _sliderX = menu["sliderX"];
            int _startY = menu["sliderStartVehicleY"];
            int _diffY = menu["sliderDiffY"];
            int _textX = menu["sliderTextX"];
            int _textY = menu["startTextY"];

            List<string> _temp = VehicleController.getStringVehicleStats();

            selectionBox = new SelectionBox(_form, _dosisfontfamily, _temp, new Action(this.SetValues), new Action(this.AddVehicle), new Action(this.DeleteVehicle));
            if (_form.WindowState == FormWindowState.Maximized) selectionBox.Location = new Point(UserControls.Config.standardSubMenu["selectionBoxMaxX"], UserControls.Config.standardSubMenu["selectionBoxMaxY"]);
            else selectionBox.Location = new Point(UserControls.Config.standardSubMenu["selectionBoxX"], UserControls.Config.standardSubMenu["selectionBoxY"]);
            this.Controls.Add(selectionBox);

            Dictionary<string, int> vehiclemenu = UserControls.Config.simVehicle;

            CurvedButtons Vehicles_header = new CurvedButtons(new Size(menu["headerSizeX"], menu["headerSizeY"]), new Point(menu["headerX"], menu["headerY"]), "../../src/User Interface Recources/Edit_Vehicle_Header.png");
            this.Controls.Add(Vehicles_header);

            saveButton = new CurvedButtons(new Size(80, 40), new Point(_sliderX, _startY + 3 * _diffY), 25, "../../src/User Interface Recources/Custom_Small_Button.png", "Save", DrawData.Dosis_font_family, null, this.BackColor);
            saveButton.Click += (object o, EventArgs ea) => { VehicleTypeConfig.SaveJson(); };
            this.Controls.Add(saveButton);

            /*     Sliders    */

            Cw = new Slider(new Point(_sliderX, _startY + 2 * _diffY), vehiclemenu["surfaceMin"], vehiclemenu["surfaceMax"]);
            this.Controls.Add(Cw);
            SliderText Cw_label = new SliderText(_dosisfontfamily, new Point(_sliderX, _textY + 2 * _diffY), "Drag Co:");
            this.Controls.Add(Cw_label);
            SliderText Cw_Value = new SliderText(_dosisfontfamily, new Point(_textX, _textY + 2 * _diffY), (Cw.Value / 10).ToString() + " ");
            this.Controls.Add(Cw_Value);
            Cw.ValueChanged += (object o, EventArgs EA) => 
            {
                General_Form.Main.SimulationScreen.Simulator.vehicleController.ChangeCw(Cw.Value, (Slider)o);
                Cw_Value.Text = ((double)Cw.Value/10).ToString() + " ";
            };

            Surface = new Slider(new Point(_sliderX, _startY + _diffY), vehiclemenu["surfaceMin"], vehiclemenu["surfaceMax"]);
            this.Controls.Add(Surface);
            SliderText Surface_label = new SliderText(_dosisfontfamily, new Point(_sliderX, _textY + 1 * _diffY), "Frontal Surface:");
            this.Controls.Add(Surface_label);
            SliderText Surface_Value = new SliderText(_dosisfontfamily, new Point(_textX, _textY + 1* _diffY), (Surface.Value / 10).ToString() + " m^2");
            this.Controls.Add(Surface_Value);
            Surface.ValueChanged += (object o, EventArgs EA) => 
            {
                General_Form.Main.SimulationScreen.Simulator.vehicleController.ChangeSurface(Surface.Value, (Slider)o);
                Surface_Value.Text = (((double)(Surface.Value)) / 10).ToString() + " m^2";
            };

            maxSpeed = new Slider(new Point(_sliderX, _startY), vehiclemenu["topSpeedMin"], vehiclemenu["topSpeedMax"]);
            this.Controls.Add(maxSpeed);
            SliderText Max_speed_label = new SliderText(_dosisfontfamily, new Point(_sliderX, _textY), "Topspeed:");
            this.Controls.Add(Max_speed_label);
            SliderText Max_speed_Value = new SliderText(_dosisfontfamily, new Point(_textX, _textY), (maxSpeed.Value*3.6).ToString() + " km/h");
            this.Controls.Add(Max_speed_Value);
            maxSpeed.ValueChanged += (object o, EventArgs EA) => 
            {
                General_Form.Main.SimulationScreen.Simulator.vehicleController.ChangeTopspeed(maxSpeed.Value, (Slider)o);
                Max_speed_Value.Text = (maxSpeed.Value*3.6).ToString() + " km/h"; 
            };

            Length = new Slider(new Point(_sliderX, _startY - _diffY), vehiclemenu["lengthMin"], vehiclemenu["lengthMax"]);
            this.Controls.Add(Length);
            SliderText Length_label = new SliderText(_dosisfontfamily, new Point(_sliderX, _textY - 1 * _diffY), "Length:");
            this.Controls.Add(Length_label);
            SliderText Length_Value = new SliderText(_dosisfontfamily, new Point(_textX, _textY - 1 * _diffY), (Length.Value / 10).ToString() + " m");
            this.Controls.Add(Length_Value);
            Length.ValueChanged += (object o, EventArgs EA) =>
            {
                General_Form.Main.SimulationScreen.Simulator.vehicleController.ChangeLength(Length.Value, (Slider)o);
                Length_Value.Text = ((double) Length.Value / 10).ToString() + " m";
            };

            horsePower = new Slider(new Point(_sliderX, _startY - 2* _diffY), vehiclemenu["horsepwrMin"], vehiclemenu["horsepwrMax"]);
            this.Controls.Add(horsePower);
            SliderText HorsePower_label = new SliderText(_dosisfontfamily, new Point(_sliderX, _textY - 2 * _diffY), "Horsepower:");
            this.Controls.Add(HorsePower_label);
            SliderText HorsePower_Value = new SliderText(_dosisfontfamily, new Point(_textX, _textY - 2 * _diffY), (horsePower.Value/746).ToString() + " hp");
            this.Controls.Add(HorsePower_Value);
            horsePower.ValueChanged += (object o, EventArgs EA) => 
            {
                General_Form.Main.SimulationScreen.Simulator.vehicleController.ChangeMotorpwr(horsePower.Value, (Slider)o);
                HorsePower_Value.Text = (horsePower.Value/746).ToString() + " hp";
            };

            Weight = new Slider(new Point(_sliderX, _startY - 3 * _diffY), vehiclemenu["weightMin"], vehiclemenu["weightMax"]);
            this.Controls.Add(Weight);
            SliderText Weight_label = new SliderText(_dosisfontfamily, new Point(_sliderX, _textY - 3 * _diffY), "Weight:");
            this.Controls.Add(Weight_label);
            SliderText Weight_Value = new SliderText(_dosisfontfamily, new Point(_textX, _textY - 3 * _diffY), Weight.Value.ToString() + " kg");
            this.Controls.Add(Weight_Value);
            Weight.ValueChanged += (object o, EventArgs EA) => 
            {
                General_Form.Main.SimulationScreen.Simulator.vehicleController.ChangeWeight(Weight.Value, (Slider)o);
                Weight_Value.Text = Weight.Value.ToString() + " kg"; 
            };

            Occurunce = new Slider(new Point(_sliderX, _startY - 4 * _diffY), vehiclemenu["occurenceMin"], vehiclemenu["occurenceMax"]);
            this.Controls.Add(Occurunce);
            SliderText Occurunce_label = new SliderText(_dosisfontfamily, new Point(_sliderX, _textY - 4 * _diffY), "Occurunce:");
            this.Controls.Add(Occurunce_label);
            SliderText Occurunce_Value = new SliderText(_dosisfontfamily, new Point(_textX, _textY - 4 * _diffY), Occurunce.Value.ToString() + " %");
            this.Controls.Add(Occurunce_Value);
            Occurunce.ValueChanged += (object o, EventArgs EA) => 
            {
                General_Form.Main.SimulationScreen.Simulator.vehicleController.ChangeOccurance(Occurunce.Value, (Slider)o);
                Occurunce_Value.Text = Occurunce.Value.ToString() + " %"; 
            };
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



            Cw.Value = (int)(controller.selectedVehicle.Cw * 10);
            Surface.Value = (int)(controller.selectedVehicle.Surface * 10);
            Length.Value = (int)(controller.selectedVehicle.Length * 10);
            maxSpeed.Value = controller.selectedVehicle.Topspeed;
            horsePower.Value = controller.selectedVehicle.Motorpwr;
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



            VehicleController.addVehicleStats(name, 5000, 5, 100, 100, 5, 1, 50); 
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

            if (_stats.canEdit == true)
            {
                return;
            }

            controller.DeleteVehicle(_stats);
            selectionBox.RemoveElement(_stats.Name);
        }

        private VehicleStats FindVehicle()
        {
            int index = selectionBox.selectedIndex;

            if (selectionBox.selectedLeftBool)
            {
                if (index < selectionBox.elementsSelected.Count)
                {
                    return VehicleController.getVehicleStat(selectionBox.elementsSelected[selectionBox.selectedIndex]);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                if (index < selectionBox.elementsAvailable.Count)
                {
                    return VehicleController.getVehicleStat(selectionBox.elementsAvailable[selectionBox.selectedIndex]);
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
