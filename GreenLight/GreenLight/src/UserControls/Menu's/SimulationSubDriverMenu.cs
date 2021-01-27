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
    public partial class SimulationSubDriverMenu : UserControl
    {
        public SelectionBox Selection_box;
        Slider Reaction_time, Follow_interval, Speeding, Rulebreaking, Occurance;

        public SimulationSubDriverMenu(int Menu_width, Form Form, FontFamily Dosis_font_family)
        {
            this.BackColor = Color.FromArgb(255, 255, 255);
            this.Size = new Size(Menu_width, Form.Height - UserControls.Config.simElementsMenu["menuY"] - UserControls.Config.simElementsMenu["menuSizeY"]);
            this.Location = new Point(Form.Width - Menu_width, UserControls.Config.simElementsMenu["menuY"]);
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
            int _start = menu["sliderStartDriver"];
            int _diff = menu["sliderDiffY"];
            int _text = menu["textX"];
            int _textstart = menu["textStartDriver"];

            

            List<string> _temp = AIController.getStringDriverStats();
            Dictionary<string, int> DriverMenu = UserControls.Config.simDriver;

            Selection_box = new SelectionBox(Form, Dosis_font_family, _temp, new Action(this.SetValues), new Action(this.AddAI), new Action(this.DeleteAI));
            if (Form.WindowState == FormWindowState.Maximized) Selection_box.Location = new Point(UserControls.Config.standardSubMenu["selectionBoxMaxX"], UserControls.Config.standardSubMenu["selectionBoxMaxY"]);
            else Selection_box.Location = new Point(UserControls.Config.standardSubMenu["selectionBoxX"], UserControls.Config.standardSubMenu["selectionBoxY"]);
            this.Controls.Add(Selection_box);

            CurvedButtons saveButton = new CurvedButtons(new Size(80, 40), new Point(_sliderX, _start + _diff * 5), 25, "../../src/User Interface Recources/Custom_Small_Button.png", "Save", DrawData.Dosis_font_family, null, this.BackColor);
            saveButton.Click += (object o, EventArgs ea) => { AITypeConfig.SaveJson(); };
            this.Controls.Add(saveButton);

            Reaction_time = new Slider(new Point(_sliderX, _start + _diff * 4), DriverMenu["reactionTimeMin"], DriverMenu["reactionTimeMax"]);
            this.Controls.Add(Reaction_time);
            SliderText Reaction_time_label = new SliderText(Dosis_font_family, new Point(_sliderX, _textstart + _diff * 4), "Reaction time:");
            this.Controls.Add(Reaction_time_label);
            SliderText Reaction_time_Value = new SliderText(Dosis_font_family, new Point(_text, _textstart + _diff * 4), (Reaction_time.Value / 10).ToString() + " s");
            this.Controls.Add(Reaction_time_Value);
            Reaction_time.ValueChanged += (object o, EventArgs EA) => 
            {
                General_Form.Main.SimulationScreen.Simulator.aiController.ChangeReactionTime((Slider)o);
                Reaction_time_Value.Text = (((Reaction_time.Value)) / 10).ToString() + " s"; 
            };

            Follow_interval = new Slider(new Point(_sliderX, _start + _diff * 3), DriverMenu["followIntervalMin"], DriverMenu["followIntervalMax"]);
            this.Controls.Add(Follow_interval);
            SliderText Follow_interval_label = new SliderText(Dosis_font_family, new Point(_sliderX, _textstart + _diff * 3), "Follow interval:");
            this.Controls.Add(Follow_interval_label);
            SliderText Follow_interval_Value = new SliderText(Dosis_font_family, new Point(_text, _textstart + _diff * 3), (Follow_interval.Value / 10).ToString() + " s");
            this.Controls.Add(Follow_interval_Value);
            Follow_interval.ValueChanged += (object o, EventArgs EA) => 
            {
                General_Form.Main.SimulationScreen.Simulator.aiController.ChangeFollowInterval((Slider)o);
                Follow_interval_Value.Text = (((double)(Follow_interval.Value)) / 10).ToString() + " s"; 
            };

            Speeding = new Slider(new Point(_sliderX, _start + _diff * 2), DriverMenu["speedingMin"], DriverMenu["speedingMax"]);
            this.Controls.Add(Speeding);
            SliderText Speeding_label = new SliderText(Dosis_font_family, new Point(_sliderX, _textstart + _diff * 2), "Speeding:");
            this.Controls.Add(Speeding_label);
            SliderText Speeding_Value = new SliderText(Dosis_font_family, new Point(_text, _textstart + _diff * 2), Speeding.Value.ToString() + " km/h");
            this.Controls.Add(Speeding_Value);
            Speeding.ValueChanged += (object o, EventArgs EA) => 
            {
                General_Form.Main.SimulationScreen.Simulator.aiController.ChangeSpeedRelativeToLimit((Slider)o);
                Speeding_Value.Text = Speeding.Value.ToString() + " km/h"; 
            };

            Rulebreaking = new Slider(new Point(_sliderX, _start + _diff), DriverMenu["ruleBreakingMin"], DriverMenu["ruleBreakingMax"]);
            this.Controls.Add(Rulebreaking);
            SliderText Rulebreaking_label = new SliderText(Dosis_font_family, new Point(_sliderX, _textstart + _diff), "Rulebreaking:");
            this.Controls.Add(Rulebreaking_label);
            SliderText Rulebreaking_Value = new SliderText(Dosis_font_family, new Point(_text, _textstart + _diff), Rulebreaking.Value.ToString() + " %");
            this.Controls.Add(Rulebreaking_Value);
            Rulebreaking.ValueChanged += (object o, EventArgs EA) => 
            {
                General_Form.Main.SimulationScreen.Simulator.aiController.ChangeRuleBreakingChance((Slider)o);
                Rulebreaking_Value.Text = Rulebreaking.Value.ToString() + " %"; 
            };

            Occurance = new Slider(new Point(_sliderX, _start), DriverMenu["occurenceMin"], DriverMenu["occurenceMax"]);
            this.Controls.Add(Occurance);
            SliderText Occurunce_label = new SliderText(Dosis_font_family, new Point(_sliderX, _textstart), "Occurunce:");
            this.Controls.Add(Occurunce_label);
            SliderText Occurunce_Value = new SliderText(Dosis_font_family, new Point(_text, _textstart), Occurance.Value.ToString() + " %");
            this.Controls.Add(Occurunce_Value);
            Occurance.ValueChanged += (object o, EventArgs EA) => 
            {
                General_Form.Main.SimulationScreen.Simulator.aiController.ChangeOccurance((Slider)o);
                Occurunce_Value.Text = Occurance.Value.ToString() + " %"; 
            };

            //----------------------------------------------

            CurvedButtons Edit_Driver_Header = new CurvedButtons(new Size(menu["headerSizeX"], menu["headerSizeY"]),
               new Point(menu["headerX"], menu["headerY"]), "../../src/User Interface Recources/Edit_Driver_Header.png");
            this.Controls.Add(Edit_Driver_Header);
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

            Console.WriteLine("Je bent erdoor!!");

            Reaction_time.Value = (int)(controller.selectedAI.ReactionTime);
            Follow_interval.Value = (int)(controller.selectedAI.FollowInterval );
            Speeding.Value = controller.selectedAI.SpeedRelativeToLimit;
            Rulebreaking.Value = (int)(controller.selectedAI.RuleBreakingChance);
            Occurance.Value = controller.selectedAI.Occurance;
        }

        private void AddAI()
        {
            string name = Interaction.InputBox("Enter Name: ", "Driver", "no name", 100, 100);

            if (name == "no name")
            {
                return;
            }

            Console.WriteLine("Vehicle Created With name : {0}", name);

            AIController.addDriverStats(name, 2, 2, 0, 0, 50, false); //NAME // Reactiontime(0,30) // followinterval (0, 100) // SpeedRelative (-50, 100) // RuleBreaking (0,100) // Occurence (0,100)
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

            if (_stats.Locked == true) //if cna edit = true it cannot be edited nor deleted, so is returned
            {
                return;
            }

            controller.DeleteAI(_stats);
            Selection_box.Remove_Element(_stats.Name);
        }

        private DriverStats FindAI()
        {
            int index = Selection_box.Selected_index;

            if (Selection_box.Selected_left_bool)
            {
                if (index < Selection_box.Elements_selected.Count)
                {
                    return AIController.getDriverStat(Selection_box.Elements_selected[Selection_box.Selected_index]);
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
                    return AIController.getDriverStat(Selection_box.Elements_available[Selection_box.Selected_index]);
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
