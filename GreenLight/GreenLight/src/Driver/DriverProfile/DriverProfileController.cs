using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace GreenLight
{

    //This class is the Controller for the DriverProfile popup window, when the user clicks on a car when the 
    //Simulation is paused, this window shows it, Its mainly a bunch of labels that are set to a certain
    //value

    public class DriverProfileController : AbstractController
    {
        PictureBox screen;

        PopUpForm settingScreen;
        PictureBox settingScreenImage, backgroundImage;
        Label fuelUsed, brakeTime, stopTime, mood, braking, wantsToSwitch, Priority, TargetSpeed;
        CurvedButtons doneButton, deleteButton;

        public bool simulationPaused;
        BetterVehicle selectedVehicle;
        Bitmap displayImage;

        public DriverProfileController(PictureBox _screen)
        {
            this.screen = _screen;
        }

        public override void Initialize()
        {
            this.settingScreen = new PopUpForm(new Size(500, 500));
            this.settingScreen.Hide();

            this.settingScreen.Location = new Point(100, 100);
            this.settingScreen.BackColor = Color.White;
            this.settingScreen.FormBorderStyle = FormBorderStyle.None;

            this.settingScreenImage = new PictureBox();
            this.settingScreenImage.Size = new Size(120, 120);
            this.settingScreenImage.Location = new Point(330, 50);
            this.settingScreenImage.BackColor = Color.Black;
            this.settingScreenImage.Paint += DrawImage;
            this.settingScreen.Controls.Add(this.settingScreenImage);

            this.fuelUsed = new Label();
            this.fuelUsed.Location = new Point(220, 210);
            this.fuelUsed.Size = new Size(200, 30);
            this.settingScreen.Controls.Add(this.fuelUsed);

            this.braking = new Label();
            this.braking.Location = new Point(220, 240);
            this.braking.Size = new Size(200, 30);
            this.settingScreen.Controls.Add(this.braking);

            this.brakeTime = new Label();
            this.brakeTime.Location = new Point(220, 271);
            this.brakeTime.Size = new Size(200, 30);
            this.settingScreen.Controls.Add(this.brakeTime);

            this.stopTime = new Label();
            this.stopTime.Location = new Point(220, 301);
            this.stopTime.Size = new Size(200, 30);
            this.settingScreen.Controls.Add(this.stopTime);

            this.TargetSpeed = new Label();
            this.TargetSpeed.Location = new Point(220, 332);
            this.TargetSpeed.Size = new Size(200, 30);
            this.settingScreen.Controls.Add(this.TargetSpeed);

            this.wantsToSwitch = new Label();
            this.wantsToSwitch.Location = new Point(220, 362);
            this.wantsToSwitch.Size = new Size(200, 30);
            this.settingScreen.Controls.Add(this.wantsToSwitch);

            this.Priority = new Label();
            this.Priority.Location = new Point(220, 392);
            this.Priority.Size = new Size(200, 30);
            this.settingScreen.Controls.Add(this.Priority);

            this.mood = new Label();
            this.mood.Location = new Point(220, 422);
            this.mood.Size = new Size(200, 30);
            this.settingScreen.Controls.Add(this.mood);

            this.doneButton = new CurvedButtons(new Size(100, 35), new Point(125, 450), 25, "../../src/User Interface Recources/Custom_Small_Button.png", "Done", DrawData.Dosis_font_family, this.settingScreen, this.settingScreen.BackColor);
            this.doneButton.Click += HideScreen;
            this.settingScreen.Controls.Add(this.doneButton);

            this.deleteButton = new CurvedButtons(new Size(100, 35), new Point(275, 450), 25, "../../src/User Interface Recources/Custom_Small_Button.png", "Delete Car", DrawData.Dosis_font_family, this.settingScreen, this.settingScreen.BackColor);
            this.deleteButton.Click += deleteCar;
            this.settingScreen.Controls.Add(this.deleteButton);

            this.backgroundImage = new PictureBox();
            this.backgroundImage.Size = new Size(500, 500);
            this.backgroundImage.Location = new Point(0, 0);
            this.backgroundImage.Image = Image.FromFile("../../src/User Interface Recources/Driver_Profile_Card.png");
            this.settingScreen.Controls.Add(this.backgroundImage);
        }

        private void deleteCar(object sender, EventArgs e)
        {
            if (this.selectedVehicle.vehicleAI.currentCrossRoadSide != null)
            {
                if (this.selectedVehicle.vehicleAI.startedCrossing)
                {
                    Microsoft.VisualBasic.Interaction.MsgBox("You cannot delete this vehicle now");
                    return;
                }

                this.selectedVehicle.vehicleAI.currentCrossRoadSide.aiOnSide.Remove(this.selectedVehicle.vehicleAI);
            }

            General_Form.Main.DataScreen.dataController.collector.RemoveVehicle(this.selectedVehicle, false);
            General_Form.Main.SimulationScreen.Simulator.vehicleController.vehicleList.Remove(this.selectedVehicle);
            this.screen.Invalidate();

            HideScreen(sender, e);
        }

        private void DrawImage(object o, PaintEventArgs pea)
        {
            Graphics g = pea.Graphics;
            g.DrawImage(this.displayImage, 0, 0 , this.settingScreenImage.Width, this.settingScreenImage.Height);
        }

        public void PauseSimulation(List<BetterVehicle> _vehicleList)
        {
            simulationPaused = true;
            _vehicleList.ForEach(x => x.CreateHitbox());
        }

        public void UnPauseSimulation()
        {
            simulationPaused = false;
            this.settingScreen.Hide();
            this.selectedVehicle = null;
        }

        public void OnClick(Point _location, List<BetterVehicle> _vehicleList)
        {
            if (!simulationPaused || this.settingScreen.Visible)
            {
                Console.WriteLine("Simulation Not paused || Screen not visable");
                return;
            }

            this.selectedVehicle = _vehicleList.Find(x => x.hitbox.Contains(_location));

            if(this.selectedVehicle == null)
            {
                Console.WriteLine("No hitbox found");
                return;
            }

            DisplayScreen();
        }

        private void DisplayScreen()
        {
            BetterAI _ai = this.selectedVehicle.vehicleAI;
            DriverProfile _profile = _ai.profile;

            this.fuelUsed.Text = _profile.fuelUsed.ToString();
            this.brakeTime.Text = _profile.ticksOnBrake.ToString();
            this.stopTime.Text = _profile.ticksStationary.ToString();
            this.mood.Text = _profile.mood;
            this.braking.Text = _ai.isBraking.ToString();
            this.displayImage = _profile.imgFace;
            this.wantsToSwitch.Text = _ai.wantsToSwitch.ToString();
            this.Priority.Text = _ai.priority.ToString();
            this.TargetSpeed.Text = _ai.targetspeed.ToString();

            this.settingScreen.ShowDialog();
            this.settingScreen.BringToFront();
            this.settingScreen.Invalidate();
        }

        private void HideScreen(object o, EventArgs ea)
        {
            this.settingScreen.Hide();
        }

    }
}
