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

//This controller is arguably the most important and base controller. This is the controller that is at the top and handles everything
//It keeps track of which ScreenController is currently selected and draws/ updates is accordingly
//It hold 3 different screencontrollers -> Buildscreen (for the road builder)
// -> SimulationScreen (for the simulation)
// -> Menuscreen (for the main menu) which is also the start value of active
//It also holds a very important function that deals with switching between thse 3 screencontrollers

namespace GreenLight
{
    public class MainScreenController : ScreenController
    {
        //public SettingScreenController SettingScreen;
        public BuildScreenController BuildScreen;
        public SimulationScreenController SimulationScreen;
        public MenuController MenuController;
        public ScreenController Active;
        public DataScreen DataScreen;
        public InterfaceController UserInterface;
        
        public static MainScreenController_Config Config;

        // StreamWriter recentProjects = new StreamWriter("../../Recent_projects/Recent_projects.txt");

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

            Console.WriteLine(this.Active.GetType().ToString() + "----" + _controller.GetType().ToString());

            if (this.Active.GetType().ToString() == "GreenLight.BuildScreenController" && _controller.GetType().ToString() == "GreenLight.MenuController")
            {
                Save();
                UserInterface.BSM.AutoSave.Checked = false;
                pathName = null;
                fileName = null;
                imagePath = null;
                Clear_all_lists();
            }
            this.Active.DeActivate();
            this.Active = _controller;
            this.Active.Activate();
            this.form.Invalidate();

            Console.WriteLine("Switched and invalidated!");
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
            Console.WriteLine(this.Active);

            if (this.Active.Screen != null)
            {
                this.Active.Screen.Show();
                this.Active.Screen.Invalidate();
            }
        }
        
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
                        //writer.WriteLine(x.ToString());
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
                            //text[t] = text[t].Replace(fileName + " " + pathName + " " + imagePath, null);
                            _remove = true;
                            Console.WriteLine(text[t].Length);
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

        public void Save()
        {
            if (this.BuildScreen.builder.roadBuilder.roads.Count != 0) //&& drivers.count != 0
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
                        //writer.WriteLine(x.ToString());
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

                    for (int t = 0; t < text.Count(); t++)
                    {
                        if (text[t] == fileName + " " + pathName + " " + imagePath)
                        {
                            text[t] = text[t].Remove(0, text[t].Length);
                            //text[t] = text[t].Replace(fileName + " " + pathName + " " + imagePath, null);
                            Console.WriteLine(text[t].Length);
                        }
                    }

                    string[] test = text.Where(s => s.Trim() != string.Empty).ToArray();
                    File.WriteAllLines(recent_project, test);
                    File.AppendAllText(recent_project, recent);

                    UserInterface.Size_adjust_SSRPM();
                }
            }
            else
            {
                Console.WriteLine("Nothing to save");
            }
        }

        public void LoadDialog()
        {
            OpenFileDialog _fileDialog = new OpenFileDialog();
            _fileDialog.Title = "Open Text File";
            _fileDialog.Filter = "TXT files|*.txt";
            //theDialog.InitialDirectory = @"C:\";
            if (_fileDialog.ShowDialog() == DialogResult.OK)
            {
                Load(_fileDialog.FileName);
            }
        }

        public void Load(string file)
        {
            StreamReader myStream;
            try
            {
                this.pathName = file;
                this.fileName = Path.GetFileNameWithoutExtension(file);
                Console.WriteLine(fileName);
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

        public void Clear_all_lists()
        {
            try
            {
                if (BuildScreen.builder.roadBuilder.roads != null) BuildScreen.builder.roadBuilder.roads.Clear();
                if (BuildScreen.builder.roadBuilder.crossRoadController.roads != null) BuildScreen.builder.roadBuilder.crossRoadController.roads.Clear();

                //if (BuildScreen.builder.roadBuilder.OPC.OriginPoints != null) BuildScreen.builder.roadBuilder.OPC.OriginPoints.Clear();
                //if(BuildScreen.builder.roadBuilder.OPC.converted != null) BuildScreen.builder.roadBuilder.OPC.converted.Clear();

                if (BuildScreen.builder.signController.Signs != null) BuildScreen.builder.signController.Signs.Clear();
            }
            catch (Exception e) { }
        }
    }
}
