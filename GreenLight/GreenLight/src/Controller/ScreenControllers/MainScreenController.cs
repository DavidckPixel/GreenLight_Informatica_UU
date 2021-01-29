using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using GreenLight.src.Data_Collection;
using System.IO;
using System.Drawing.Imaging;

// This is the MainScreenController and with that the most important controller. This is the controller that is at the top and handles all controllers.
// It keeps track of which ScreenController is currently selected and draws/ updates is accordingly
// It hold 3 different screencontrollers -> Buildscreen (for the road builder)
// -> SimulationScreen (for the simulation)
// -> Menuscreen (for the main menu) which is also the start value of active
// It also holds a very important function that deals with switching between thse 3 screencontrollers
// Lastly this class takes care of Saving and Loading the users projects.

namespace GreenLight
{
    public class MainScreenController : ScreenController
    {
        public BuildScreenController BuildScreen;
        public SimulationScreenController SimulationScreen;
        public MenuController MenuController;
        public ScreenController Active;
        public DataScreen DataScreen;
        public InterfaceController UserInterface;


        public PopUpForm ChoosePresetForm;
        
        public static MainScreenControllerConfig Config;
        
        string fileName = null;
        string pathName = null;
        string imagePath = null;
        public string recent_project = "../../Recent_projects/Recent_projects.txt";

        public MainScreenController(Form _tempform) 
        {
            this.form = _tempform;

            this.form.Paint += DrawMain;

            form.Controls.Add(this.Screen);
            this.form.Invalidate();

            Log.Write("Created the Main Controller");
        }
        
        public void Invalidate()
        {
            this.form.Invalidate();
        }

        public override void Initialize()
        {
            Log.Write("Initializing the MainController..");

            UserInterface = new InterfaceController(this.form);

            UserInterface.Initialize();

            BuildScreen = new BuildScreenController(this.form);
            SimulationScreen = new SimulationScreenController(this.form);
            MenuController = new MenuController(this.Screen);
            DataScreen = new DataScreen(this.form);

            ChoosePresetForm = new PopUpForm(new Size(700, 600));

            MovePanel DragPad = new MovePanel(ChoosePresetForm);
            DragPad.Location = new Point(0, 0);
            DragPad.Size = new Size(ChoosePresetForm.Width,35);
            DragPad.BackColor = Color.FromArgb(142, 140, 144);

            Label Presets = new Label();
            Presets.Text = "Presets";
            Presets.Font = new Font(DrawData.Dosis_font_family, 40, FontStyle.Bold);
            Presets.ForeColor = Color.FromArgb(142, 140, 144);
            Presets.Size = new Size(250, 80);
            Presets.Location = new Point((int)(ChoosePresetForm.Width/2) - 125, 35);

            Dictionary<string, int> startmenu = UserControls.Config.startSubMenu;

            string[] _recentProjects;
            _recentProjects = File.ReadAllLines("../../Recent_projects/Presets/PresetsDir.txt");

            int _counterX = 0;
            int _counterY = 0;
            if (_recentProjects != null)
            {

                for (int t = _recentProjects.Length - 1; t >= _recentProjects.Length -6; t--)
                {
                    try
                    {
                        string[] _temp = _recentProjects[t].Split(' ');
                        if (File.Exists(_temp[1]))
                        {
                            try
                            {
                                CurvedButtons Project = new CurvedButtons(new Size(300,200), new Point(25+325*_counterX,150 + 225*_counterY), startmenu["projectButtonCurve"], _temp[2], _temp[0], DrawData.Dosis_font_family, form, Color.White, 1);
                                Project.Click += (object o, EventArgs ea) => { General_Form.Main.MenuController.SwitchToBuild(); General_Form.Main.Load(_temp[1]); ChoosePresetForm.Close(); };
                                this.ChoosePresetForm.Controls.Add(Project);
                                _counterX++;
                                if (_counterX == 2)
                                {
                                    _counterY++;
                                    _counterX = 0;
                                }
                            }
                            catch (Exception e) { }
                        }
                    }
                    catch { };
                }
            }

            ChoosePresetForm.Controls.Add(Presets);
            ChoosePresetForm.Controls.Add(DragPad);

            BuildScreen.Initialize();
            SimulationScreen.Initialize();
            MenuController.Initialize();
            DataScreen.Initialize();

            Log.Write("Setting Active Controller to MenuController");

            this.Active = this.MenuController;
            this.Active.Activate();

            Log.Write("Completed the Initialization of MainController");
        }

        public void SwitchControllers(ScreenController _controller)
        {
            Log.Write("Switched Active Controller from " + this.Active.GetType().ToString() + "to " + _controller.GetType().ToString());

            if (this.Active.GetType().ToString() == "GreenLight.BuildScreenController" && _controller.GetType().ToString() == "GreenLight.MenuController")
            {
                Save();
                UserInterface.BSM.autoSave.Checked = false;
                pathName = null;
                fileName = null;
                imagePath = null;
                Clear_all_lists();
            }
            this.Active.DeActivate();
            this.Active = _controller;
            this.Active.Activate();
            this.form.Invalidate();
        }

        public override void Activate()
        {
            
        }

        public override void DeActivate()
        {
            throw new NotImplementedException();
        }

        public void DrawMain(Object o, PaintEventArgs pea)
        {
            if (this.Active.Screen != null)
            {
                this.Active.Screen.Show();
                this.Active.Screen.Invalidate();
            }
        }
        
        // Saves a textfile containing all roads and signs in the preffered fodler
        public void SaveAs()
        {
            SaveFileDialog save = new SaveFileDialog();

            save.FileName = "DefaultOutputName.txt";

            save.Filter = "Text File | *.txt";

            try
            {
                if (save.ShowDialog() == DialogResult.OK)
                {
                    pathName = save.FileName;
                    fileName = Path.GetFileNameWithoutExtension(save.FileName);

                    string[] save_text = new string[this.BuildScreen.builder.roadBuilder.roads.Count() + this.BuildScreen.builder.signController.Signs.Count()];
                    int _count = 0;
                    foreach (AbstractRoad x in this.BuildScreen.builder.roadBuilder.roads)
                    {
                        save_text[_count] = x.ToString();
                        _count++;
                        foreach (PlacedSign y in x.Signs)
                        {
                            save_text[_count] = y.ToString();
                            _count++;
                        }
                    }
                    File.WriteAllLines(pathName, save_text);

                    imagePath = "../../Recent_projects/Images/" + fileName + ".png";
                    Bitmap pic = new Bitmap(BuildScreen.Screen.ClientSize.Width, BuildScreen.Screen.ClientSize.Height);

                    if (File.Exists(imagePath))
                    {
                        UserInterface.SSRPM.Controls.Clear();
                        File.Delete(imagePath);
                    }

                    using (Graphics g = Graphics.FromImage(pic))
                    {
                        Color c = Color.FromArgb(142, 140, 144);
                        Brush b = new SolidBrush(c);
                        g.FillRectangle(b, 0, 0, pic.Width, pic.Height);
                        BuildScreen._DrawPictureBox(g);
                        pic.Save(imagePath, ImageFormat.Png);
                    }

                    string recent = fileName + " " + pathName + " " + imagePath + Environment.NewLine;
                    string[] text = File.ReadAllLines(recent_project);
                    bool _remove = false;

                    for (int t = 0; t < text.Count(); t++)
                    {
                        if (text[t] == fileName + " " + pathName + " " + imagePath)
                        {
                            text[t] = text[t].Remove(0, text[t].Length);
                            _remove = true;

                        }
                    }
                    if (!_remove)
                        File.AppendAllText(recent_project, recent);
                    else
                    {
                        string[] test = text.Where(s => s.Trim() != string.Empty).ToArray();
                        File.WriteAllLines(recent_project, test);
                        File.AppendAllText(recent_project, recent);
                    }
                    UserInterface.Size_adjust_SSRPM();
                }
            }
            catch (Exception e) { };
        }

        // Saves an existing project or redirects to SaveAs
        public void Save()
        {
            if (this.BuildScreen.builder.roadBuilder.roads.Count != 0)
            {
                if (fileName == null)
                {
                    SaveAs();
                }
                else
                {
                    string[] save_text = new string[this.BuildScreen.builder.roadBuilder.roads.Count() + this.BuildScreen.builder.signController.Signs.Count()];
                    int _count = 0;
                    foreach (AbstractRoad x in this.BuildScreen.builder.roadBuilder.roads)
                    {
                        save_text[_count] = x.ToString();
                        _count++;
                        foreach (PlacedSign y in x.Signs)
                        {
                            save_text[_count] = y.ToString();
                            _count++;
                        }
                    }
                    try
                    {
                        File.WriteAllLines(pathName, save_text);
                    }
                    catch (Exception e) { }

                    imagePath = "../../Recent_projects/Images/" + fileName + ".png";
                    Bitmap pic = new Bitmap(BuildScreen.Screen.ClientSize.Width, BuildScreen.Screen.ClientSize.Height);

                    if (File.Exists(imagePath))
                    {
                        UserInterface.SSRPM.Controls.Clear();
                        File.Delete(imagePath);
                    }

                    Graphics g = Graphics.FromImage(pic);

                        Color c = Color.FromArgb(142, 140, 144);
                        Brush b = new SolidBrush(c);
                        g.FillRectangle(b, 0, 0, pic.Width, pic.Height);
                        BuildScreen._DrawPictureBox(g);
                        pic.Save(imagePath, ImageFormat.Png);

                    string recent = fileName + " " + pathName + " " + imagePath + Environment.NewLine;
                    string[] text = File.ReadAllLines(recent_project);

                    for (int t = 0; t < text.Count(); t++)
                    {
                        if (text[t] == fileName + " " + pathName + " " + imagePath)
                        {
                            text[t] = text[t].Remove(0, text[t].Length);
                        }
                    }

                    string[] test = text.Where(s => s.Trim() != string.Empty).ToArray();
                    try
                    {
                        File.WriteAllLines(recent_project, test);
                    }
                    catch (Exception e) { }

                    File.AppendAllText(recent_project, recent);

                    UserInterface.Size_adjust_SSRPM();
                }
            }
        }

        public void LoadDialog()
        {
            OpenFileDialog _fileDialog = new OpenFileDialog();
            _fileDialog.Title = "Open Text File";
            _fileDialog.Filter = "TXT files|*.txt";
            if (_fileDialog.ShowDialog() == DialogResult.OK)
            {
                Load(_fileDialog.FileName);
            }
        }

        public void LoadPresets() 
        {
            ChoosePresetForm.ShowDialog();
        }

        // Rebuilds a project from a text file
        public void Load(string file)
        {
            StreamReader myStream;
            try
            {
                this.pathName = file;
                this.fileName = Path.GetFileNameWithoutExtension(file);

                myStream = new StreamReader(file);
                {
                    using (myStream)
                    {
                        string line = " ";
                        while ((line = myStream.ReadLine()) != null)
                        {
                            string[] _words = line.Split(' ');

                            switch (_words[0])
                            {
                                case "Road":
                                    this.BuildScreen.builder.roadBuilder.loadRoads(_words);
                                    break;
                                case "Sign":
                                    this.BuildScreen.builder.signController.loadSigns(_words, BuildScreen.builder.roadBuilder.roads[BuildScreen.builder.roadBuilder.roads.Count - 1]);
                                    break;

                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
            }
        }

        // This method is called when the user goes back to the Main menu, it resets everything in the BuildScreen
        public void Clear_all_lists()
        {
            try
            {
                if (BuildScreen.builder.roadBuilder.roads != null)
                {
                    int count = BuildScreen.builder.roadBuilder.roads.Count;
                    for (int t = 0; t < count; t++)
                    {
                        BuildScreen.builder.roadBuilder.DeleteRoad(BuildScreen.builder.roadBuilder.roads.First());
                    }
                }
                
                if (BuildScreen.builder.roadBuilder.crossRoadController.roads != null) BuildScreen.builder.roadBuilder.crossRoadController.roads.Clear();
                if (BuildScreen.builder.signController.Signs != null) BuildScreen.builder.signController.Signs.Clear();
                if (BuildScreen.builder.roadBuilder.AllCrossArrows != null) BuildScreen.builder.roadBuilder.AllCrossArrows.Clear();
            }
            catch (Exception e) { }
        }
    }
}
