using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace GreenLight
{
    public class WorldController : AbstractController
    {
        Form settingScreen;
        TextBox name, brakePwr, density, gravity, slip;
        Label nameLabel, brakePwrLabel, densityLabel, gravityLabel, slipLabel, entityTypeLabel;
        ListBox entityType;

        Label errorText;

        CurvedButtons doneButton, returnButton ,deleteButton, errorButton;
        PictureBox locked;

        World selectedWorld;

        public WorldController()
        {
        }

        public override void Initialize()
        {
            this.settingScreen = new Pop_Up_Form(new Size(400, 600));
            this.settingScreen.Hide();

            this.settingScreen.Location = new Point(100, 100);
            this.settingScreen.BackColor = Color.FromArgb(255,255,255);
            this.settingScreen.FormBorderStyle = FormBorderStyle.None;

            this.locked = new PictureBox();
            this.locked.Size = new Size(50, 50);
            this.locked.Location = new Point(400, 50);
            this.locked.Image = new Bitmap(Image.FromFile("../../Images/Lock.png"), 50,50);
            this.settingScreen.Controls.Add(this.locked);

            Move_panel Move = new Move_panel(this.settingScreen);
            Move.Location = new Point(350, 0);
            Move.Size = new Size(100, 600);
            this.settingScreen.Controls.Add(Move);

            CreateTextBox(new Point(10, 10), new Size(200, 30), ref name, ref nameLabel, "Worldname:", "");
            CreateTextBox(new Point(10, 50), new Size(200, 30), ref brakePwr, ref brakePwrLabel, "Brake Power:", "");
            CreateTextBox(new Point(10, 100), new Size(200, 30), ref density, ref densityLabel, "Density:", "");
            CreateTextBox(new Point(10, 150), new Size(200, 30), ref gravity, ref gravityLabel, "Gravity:", "");
            CreateTextBox(new Point(10, 200), new Size(200, 30), ref slip, ref slipLabel, "Slip:", "");


            name.Leave += CheckName;
            brakePwr.Leave += CheckBrakePwr;
            density.Leave += CheckDensity;
            gravity.Leave += CheckGravity;
            slip.Leave += CheckSlip;

            entityType = new ListBox();
            DriverProfileData.FacesToString().ForEach(x => entityType.Items.Add(x));
            entityType.Size = new Size(90, 80);
            entityType.Location = new Point(210, 250);
            entityType.SelectionMode = SelectionMode.One;
            this.settingScreen.Controls.Add(entityType);

            entityTypeLabel = new Label();
            entityTypeLabel.Size = new Size(150, 30);
            entityTypeLabel.Location =  new Point(10, 300);
            this.settingScreen.Controls.Add(entityTypeLabel);

            this.doneButton = new CurvedButtons(new Size(80, 40), new Point(10, 550), 25, "../../User Interface Recources/Custom_Small_Button.png", "Done", DrawData.Dosis_font_family, this.settingScreen, this.settingScreen.BackColor);
            this.returnButton = new CurvedButtons(new Size(100, 40), new Point(100, 550), 25, "../../User Interface Recources/Custom_Small_Button.png", "Return", DrawData.Dosis_font_family, this.settingScreen, this.settingScreen.BackColor);
            this.deleteButton = new CurvedButtons(new Size(90, 40), new Point(210, 550), 25, "../../User Interface Recources/Custom_Small_Button.png", "Delete", DrawData.Dosis_font_family, this.settingScreen, this.settingScreen.BackColor);

            this.doneButton.Click += DoneClick;
            this.returnButton.Click += ReturnClick;
            this.deleteButton.Click += DeleteClick;

            this.settingScreen.Controls.Add(this.doneButton);
            this.settingScreen.Controls.Add(this.returnButton);
            this.settingScreen.Controls.Add(this.deleteButton);

            errorText = new Label();
            errorText.Text = "";
            errorText.ForeColor = Color.Red;
            errorText.Location = new Point(10, 500);
            errorText.Size = new Size(390, 60);
            errorText.BringToFront();
            errorText.Hide();

            errorButton = new CurvedButtons(new Size(40, 30), new Point(400, 450), 25, "../../User Interface Recources/Custom_Small_Button.png", "Oke!", DrawData.Dosis_font_family, this.settingScreen, this.settingScreen.BackColor);
            errorButton.Click += HideError;
            errorButton.Hide();

            this.settingScreen.Controls.Add(errorText);
            this.settingScreen.Controls.Add(errorButton);
        }

        private void CreateTextBox(Point _location, Size _size, ref TextBox _text, ref Label _label, string _labelText, string _baseValue)
        {
            _label = new Label();
            _label.Location = _location;
            _label.Size = _size;
            _label.Text = _labelText;

            _text = new TextBox();
            _text.Location = new Point(_location.X + _size.Width, _location.Y);
            _text.Size = new Size(50, 30);
            _text.Text = _baseValue;

            this.settingScreen.Controls.Add(_label);
            this.settingScreen.Controls.Add(_text);
        }

        private void Show()
        {
            this.name.Text = this.selectedWorld.name;
            this.brakePwr.Text = this.selectedWorld.Brakepwr.ToString();
            this.density.Text = this.selectedWorld.Density.ToString();
            this.gravity.Text = this.selectedWorld.Gravity.ToString();
            this.slip.Text = this.selectedWorld.slip.ToString();
            this.entityType.SelectedIndex = this.entityType.FindString(this.selectedWorld.entityTypes);

            if (this.selectedWorld.canDelete)
            {
                this.locked.Hide();
            }
            else
            {
                this.locked.Show();
            }

            this.settingScreen.ShowDialog();
            this.settingScreen.BringToFront();
            this.settingScreen.Invalidate();
        }

        private void Hide()
        {
            this.settingScreen.Hide();
        }

        public void CreateNewWorld()
        {
            World _newWorld = new World("", 0, 0, 0, "Normal", 0);
            WorldConfig.physics.Add(_newWorld);

            EditWorld(_newWorld);
        }

        public void EditWorld(World _editWorld)
        {
            if(_editWorld == null)
            {
                return;
            }
            this.selectedWorld = _editWorld;
            this.Show();
        }

        private void DoneClick(object o, EventArgs ea)
        {
            this.doneButton.Focus();

            try
            {
                this.selectedWorld.name = name.Text;
                this.selectedWorld.Brakepwr = Int32.Parse(brakePwr.Text);
                this.selectedWorld.Density = float.Parse(density.Text);
                this.selectedWorld.Gravity = float.Parse(gravity.Text);
                this.selectedWorld.slip = float.Parse(slip.Text);
                this.selectedWorld.entityTypes = (string)entityType.SelectedItem;

                WorldConfig.WriteJson();
            }
            catch (Exception)
            {
                return;
            }

            this.selectedWorld = null;
            this.Hide();
        }

        private void ReturnClick(object o, EventArgs ea)
        {
            this.selectedWorld = null;
            this.Hide();
        }

        private void DeleteClick(object o, EventArgs ea)
        {
            if (!this.selectedWorld.canDelete)
            {
                DisplayError("This WorldConfig cannot not be deleted!");
                return;
            }
            //an ARE U SURE MESSAGE?

            WorldConfig.physics.Remove(this.selectedWorld);
            this.Hide();

            WorldConfig.WriteJson();
        }

        private void CheckName(object o, EventArgs ea)
        {
            if (!this.selectedWorld.canDelete)
            {
                this.name.Text = this.selectedWorld.name;
                DisplayError("Invalid Input - Name already inuse");
                return;
            }

            if (WorldConfig.CheckDuplicateName(this.name.Text))
            {
                this.name.Text = this.selectedWorld.name;
            }
        }

        private void CheckBrakePwr(object o, EventArgs ea)
        {
            if (!this.selectedWorld.canDelete)
            {
                this.brakePwr.Text = this.selectedWorld.Brakepwr.ToString();
                DisplayError("This worldconfig cannot be edited!");
                return;
            }

            int _amount; 
            if(Int32.TryParse(this.brakePwr.Text, out _amount))
            {
                if (_amount > 0 && _amount < 10000) //HARDCODED LIMIT
                {
                    return;
                }

                DisplayError("Invalid Input - Number must be between 0 - 10000!");
            }
            else
            {
                DisplayError("Invalid Input - Must be a number!");
            }

            this.brakePwr.Text = this.selectedWorld.Brakepwr.ToString();
        }

        private void CheckDensity(object o, EventArgs ea)
        {
            if (!this.selectedWorld.canDelete)
            {
                this.density.Text = this.selectedWorld.Density.ToString();
                DisplayError("This worldconfig cannot be edited!");
                return;
            }

            float _amount;
            if (float.TryParse(this.density.Text, out _amount))
            {
                if (_amount > 0 && _amount < 30) //HARDCODED LIMIT
                {
                    return;
                }

                DisplayError("Invalid Input - Number must be between 0 - 30!");
            }
            else
            {
                DisplayError("Invalid Input - Must be a number!");
            }

            this.density.Text = this.selectedWorld.Density.ToString();
        }

        private void CheckGravity(object o, EventArgs ea)
        {
            if (!this.selectedWorld.canDelete)
            {
                this.gravity.Text = this.selectedWorld.Gravity.ToString();
                DisplayError("This worldconfig cannot be edited!");
                return;
            }

            float _amount;
            if (float.TryParse(this.gravity.Text, out _amount))
            {
                if (_amount > 0 && _amount < 1000) //HARDCODED LIMIT
                {
                    return;
                }

                DisplayError("Invalid Input - Number must be between 0 - 1000!");
            }
            else
            {
                DisplayError("Invalid Input - Must be a number!");
            }

            this.gravity.Text = this.selectedWorld.Gravity.ToString();
        }

        private void CheckSlip(object o, EventArgs ea)
        {
            if (!this.selectedWorld.canDelete)
            {
                this.slip.Text = this.selectedWorld.slip.ToString();
                DisplayError("This worldconfig cannot be edited!");
                return;
            }

            float _amount;
            if (float.TryParse(this.slip.Text, out _amount))
            {
                if (_amount > 0 && _amount < 1) //HARDCODED LIMIT
                {
                    return;
                }

                DisplayError("Invalid Input - Number must be between 0 - 1!");
            }
            else
            {
                DisplayError("Invalid Input - Must be a number!");
            }

            this.slip.Text = this.selectedWorld.slip.ToString();
        }

        private void DisplayError(string _text)
        {
            errorText.Text = _text;
            errorText.Show();
            errorButton.Show();

            this.settingScreen.Invalidate();
        }

        private void HideError(object o, EventArgs ea)
        {
            errorText.Text = "";
            errorText.Hide();
            errorButton.Hide();

            this.settingScreen.Invalidate();
        }
    }
}
