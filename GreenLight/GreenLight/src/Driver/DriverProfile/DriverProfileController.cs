using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace GreenLight
{
    public class DriverProfileController : AbstractController
    {
        PictureBox screen;

        Form settingScreen;
        PictureBox settingScreenImage;
        Label fuelUsed, brakeTime, stopTime, mood, braking;
        CurvedButtons doneButton;

        public bool simulationPaused;
        BetterVehicle selectedVehicle;
        Bitmap displayImage;

        public DriverProfileController(PictureBox _screen)
        {
            this.screen = _screen;
        }

        public override void Initialize()
        {
            this.settingScreen = new Form();
            this.settingScreen.Hide();

            this.settingScreen.Size = new Size(500, 500);
            this.settingScreen.Location = new Point(100, 100);
            this.settingScreen.BackColor = Color.FromArgb(100, 100, 100);
            this.settingScreen.FormBorderStyle = FormBorderStyle.None;

            this.settingScreenImage = new PictureBox();
            this.settingScreenImage.Size = new Size(100, 100);
            this.settingScreenImage.Location = new Point(10, 10);
            this.settingScreenImage.BackColor = Color.Black;
            this.settingScreenImage.Paint += DrawImage;
            this.settingScreen.Controls.Add(this.settingScreenImage);

            this.fuelUsed = new Label();
            this.fuelUsed.Location = new Point(10, 200);
            this.fuelUsed.Size = new Size(200, 30);
            this.settingScreen.Controls.Add(this.fuelUsed);

            this.brakeTime = new Label();
            this.brakeTime.Location = new Point(10, 250);
            this.brakeTime.Size = new Size(200, 30);
            this.settingScreen.Controls.Add(this.brakeTime);

            this.stopTime = new Label();
            this.stopTime.Location = new Point(10, 300);
            this.stopTime.Size = new Size(200, 30);
            this.settingScreen.Controls.Add(this.stopTime);

            this.mood = new Label();
            this.mood.Location = new Point(10, 400);
            this.mood.Size = new Size(200, 30);
            this.settingScreen.Controls.Add(this.mood);

            this.braking = new Label();
            this.braking.Location = new Point(300, 200);
            this.braking.Size = new Size(200, 30);
            this.settingScreen.Controls.Add(this.braking);

            this.doneButton = new CurvedButtons(new Size(100, 30), new Point(10, 450), 25, "../../User Interface Recources/Custom_Small_Button.png", "Done", DrawData.Dosis_font_family, this.settingScreen, this.settingScreen.BackColor);
            this.doneButton.Click += HideScreen;
            this.settingScreen.Controls.Add(this.doneButton);
        }

        private void DrawImage(object o, PaintEventArgs pea)
        {
            Graphics g = pea.Graphics;
            g.DrawImage(this.displayImage, 0, 0 , this.settingScreenImage.Width, this.settingScreenImage.Height);
        }

        public void PauseSimulation()
        {
            simulationPaused = true;
            BetterVehicleTest.vehiclelist.ForEach(x => x.CreateHitbox());
        }

        public void UnPauseSimulation()
        {
            simulationPaused = false;
            this.settingScreen.Hide();
            this.selectedVehicle = null;
        }

        public void OnClick(Point _location)
        {
            if (!simulationPaused || this.settingScreen.Visible)
            {
                return;
            }

            this.selectedVehicle = BetterVehicleTest.vehiclelist.Find(x => x.hitbox.Contains(_location));

            if(this.selectedVehicle == null)
            {
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

            this.settingScreen.Show();
            this.settingScreen.BringToFront();
            this.settingScreen.Invalidate();
        }

        private void HideScreen(object o, EventArgs ea)
        {
            this.settingScreen.Hide();
        }

    }
}
