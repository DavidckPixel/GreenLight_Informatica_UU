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
    /* This is the Simulation sub driver menu class. This class has a method AdjustSize to fit the size of the users window.
       In the initialize void the controls are added to the submenu.
       This user control is shown when the user is in the simulation screen and has selected the driver menu.
       The user can change variables of the driver AI, using sliders and a selectionbox.
       Switching to this user control and closing the other user controls happens in the UserInterfaceController class. */
    public partial class SimulationSubDriverMenu : UserControl
    {
        public SelectionBox Selection_box;
        public Slider reactionTime, followInterval, Speeding, Rulebreaking, Occurance;
        public CurvedButtons editDriverHeader, saveButton;

        public SimulationSubDriverMenu(int _menuwidth, Form _form, FontFamily _dosisfontfamily)
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
            int _start = menu["sliderStartDriver"];
            int _diff = menu["sliderDiffY"];
            int _text = menu["textX"];
            int _textstart = menu["textStartDriver"];

            List<string> _temp = AIController.getStringDriverStats();
            Dictionary<string, int> DriverMenu = UserControls.Config.simDriver;

            Selection_box = new SelectionBox(_form, _dosisfontfamily, _temp, new Action(this.SetValues), new Action(this.AddAI), new Action(this.DeleteAI));
            if (_form.WindowState == FormWindowState.Maximized) Selection_box.Location = new Point(UserControls.Config.standardSubMenu["selectionBoxMaxX"], UserControls.Config.standardSubMenu["selectionBoxMaxY"]);
            else Selection_box.Location = new Point(UserControls.Config.standardSubMenu["selectionBoxX"], UserControls.Config.standardSubMenu["selectionBoxY"]);
            this.Controls.Add(Selection_box);

            /*     Sliders    */

            reactionTime = new Slider(new Point(_sliderX, _start + _diff * 4), DriverMenu["reactionTimeMin"], DriverMenu["reactionTimeMax"]);
            this.Controls.Add(reactionTime);
            SliderText Reaction_time_label = new SliderText(_dosisfontfamily, new Point(_sliderX, _textstart + _diff * 4), "Reaction time:");
            this.Controls.Add(Reaction_time_label);
            SliderText Reaction_time_Value = new SliderText(_dosisfontfamily, new Point(_text, _textstart + _diff * 4), (reactionTime.Value / 10).ToString() + " s");
            this.Controls.Add(Reaction_time_Value);
            reactionTime.ValueChanged += (object o, EventArgs EA) => 
            {
                General_Form.Main.SimulationScreen.Simulator.aiController.ChangeReactionTime((Slider)o);
                Reaction_time_Value.Text = (((reactionTime.Value)) / 10).ToString() + " s"; 
            };

            followInterval = new Slider(new Point(_sliderX, _start + _diff * 3), DriverMenu["followIntervalMin"], DriverMenu["followIntervalMax"]);
            this.Controls.Add(followInterval);
            SliderText Follow_interval_label = new SliderText(_dosisfontfamily, new Point(_sliderX, _textstart + _diff * 3), "Follow interval:");
            this.Controls.Add(Follow_interval_label);
            SliderText Follow_interval_Value = new SliderText(_dosisfontfamily, new Point(_text, _textstart + _diff * 3), (followInterval.Value / 10).ToString() + " s");
            this.Controls.Add(Follow_interval_Value);
            followInterval.ValueChanged += (object o, EventArgs EA) => 
            {
                General_Form.Main.SimulationScreen.Simulator.aiController.ChangeFollowInterval((Slider)o);
                Follow_interval_Value.Text = (((double)(followInterval.Value)) / 10).ToString() + " s"; 
            };

            Speeding = new Slider(new Point(_sliderX, _start + _diff * 2), DriverMenu["speedingMin"], DriverMenu["speedingMax"]);
            this.Controls.Add(Speeding);
            SliderText Speeding_label = new SliderText(_dosisfontfamily, new Point(_sliderX, _textstart + _diff * 2), "Speeding:");
            this.Controls.Add(Speeding_label);
            SliderText Speeding_Value = new SliderText(_dosisfontfamily, new Point(_text, _textstart + _diff * 2), Speeding.Value.ToString() + " km/h");
            this.Controls.Add(Speeding_Value);
            Speeding.ValueChanged += (object o, EventArgs EA) => 
            {
                General_Form.Main.SimulationScreen.Simulator.aiController.ChangeSpeedRelativeToLimit((Slider)o);
                Speeding_Value.Text = Speeding.Value.ToString() + " km/h"; 
            };

            Rulebreaking = new Slider(new Point(_sliderX, _start + _diff), DriverMenu["ruleBreakingMin"], DriverMenu["ruleBreakingMax"]);
            this.Controls.Add(Rulebreaking);
            SliderText Rulebreaking_label = new SliderText(_dosisfontfamily, new Point(_sliderX, _textstart + _diff), "Rulebreaking:");
            this.Controls.Add(Rulebreaking_label);
            SliderText Rulebreaking_Value = new SliderText(_dosisfontfamily, new Point(_text, _textstart + _diff), Rulebreaking.Value.ToString() + " %");
            this.Controls.Add(Rulebreaking_Value);
            Rulebreaking.ValueChanged += (object o, EventArgs EA) => 
            {
                General_Form.Main.SimulationScreen.Simulator.aiController.ChangeRuleBreakingChance((Slider)o);
                Rulebreaking_Value.Text = Rulebreaking.Value.ToString() + " %"; 
            };

            Occurance = new Slider(new Point(_sliderX, _start), DriverMenu["occurenceMin"], DriverMenu["occurenceMax"]);
            this.Controls.Add(Occurance);
            SliderText Occurunce_label = new SliderText(_dosisfontfamily, new Point(_sliderX, _textstart), "Occurunce:");
            this.Controls.Add(Occurunce_label);
            SliderText Occurunce_Value = new SliderText(_dosisfontfamily, new Point(_text, _textstart), Occurance.Value.ToString() + " %");
            this.Controls.Add(Occurunce_Value);
            Occurance.ValueChanged += (object o, EventArgs EA) => 
            {
                General_Form.Main.SimulationScreen.Simulator.aiController.ChangeOccurance((Slider)o);
                Occurunce_Value.Text = Occurance.Value.ToString() + " %"; 
            };

            /*     Buttons & Dividers    */

            editDriverHeader = new CurvedButtons(new Size(menu["headerSizeX"], menu["headerSizeY"]),
               new Point(menu["headerX"], menu["headerY"]), "../../src/User Interface Recources/Edit_Driver_Header.png");
            this.Controls.Add(editDriverHeader);

            saveButton = new CurvedButtons(new Size(80, 40), new Point(_sliderX, _start + _diff * 5), 25, "../../src/User Interface Recources/Custom_Small_Button.png", "Save", DrawData.Dosis_font_family, null, this.BackColor);
            saveButton.Click += (object o, EventArgs ea) => { AITypeConfig.SaveJson(); };
            this.Controls.Add(saveButton);
        }


        private void SetValues()
        {
            AIController controller = General_Form.Main.SimulationScreen.Simulator.aiController;

            DriverStats _stats = FindAI();
            if (_stats == null)
            {
                return;
            }

            controller.SelectAI(_stats);



            reactionTime.Value = (int)(controller.selectedAI.ReactionTime);
            followInterval.Value = (int)(controller.selectedAI.FollowInterval );
            Speeding.Value = controller.selectedAI.SpeedRelativeToLimit;
            Rulebreaking.Value = (int)(controller.selectedAI.RuleBreakingChance);
            Occurance.Value = controller.selectedAI.Occurance;
        }

        private void AddAI()
        {
            string _name = Interaction.InputBox("Enter Name: ", "Driver", "no name", 100, 100);

            if (_name == "no name")
            {
                return;
            }



            AIController.addDriverStats(_name, 2, 2, 0, 0, 50, false);
            SetValues();
            
        }

        private void DeleteAI()
        {
            AIController controller = General_Form.Main.SimulationScreen.Simulator.aiController;

            DriverStats _stats = FindAI();
            if (_stats == null)
            {
                return;
            }

            if (_stats.Locked == true)
            {
                return;
            }

            controller.DeleteAI(_stats);
            Selection_box.RemoveElement(_stats.Name);
        }

        private DriverStats FindAI()
        {
            int _index = Selection_box.selectedIndex;

            if (Selection_box.selectedLeftBool)
            {
                if (_index < Selection_box.elementsSelected.Count)
                {
                    return AIController.getDriverStat(Selection_box.elementsSelected[Selection_box.selectedIndex]);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                if (_index < Selection_box.elementsAvailable.Count)
                {
                    return AIController.getDriverStat(Selection_box.elementsAvailable[Selection_box.selectedIndex]);
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
