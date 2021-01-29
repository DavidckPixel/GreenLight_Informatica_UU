using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;

namespace GreenLight.src.Data_Collection
{
    //The DataController class is the controller that controls the data collection System
    //It is also the place that holds the stores the charts and that updates the charts

    public class DataController : AbstractController
    {
        public DataCollector collector;
        public Chart brakeChart, averageSpeed;
        PictureBox screen;

        public delegate void UpdateData();

        public DataController(PictureBox _screen)
        {
            this.screen = _screen;
            this.collector = new DataCollector(this);
        }

        public override void Initialize()
        {
            brakeChart = new Chart();
            this.brakeChart.Palette = ChartColorPalette.EarthTones;
            this.brakeChart.ChartAreas.Add(new ChartArea("Test"));
            this.brakeChart.Location = new Point(100, 100);

            averageSpeed = new Chart();
            this.averageSpeed.Palette = ChartColorPalette.EarthTones;
            this.averageSpeed.ChartAreas.Add(new ChartArea("AverageSpeed"));
            this.averageSpeed.Location = new Point(500, 100);

            // Set title
            this.brakeChart.Titles.Add("Brake Ticks");
            this.averageSpeed.Titles.Add("Average Speed per Second");

            this.screen.Controls.Add(this.averageSpeed);
            this.screen.Controls.Add(this.brakeChart);

            this.brakeChart.Hide();
            this.averageSpeed.Hide();

            this.screen.Invalidate();
        }

        //There are 2 types of updates for every chart type, one is the Big updatedate, this will completely recalibrate the chart
        //removing and adding all the collected data points. This however takes more processing power. So we also added smallupdates, 
        //these updates occur everytime new data is collected, and instead of completely recalibrating the chart, it only adds the data point to it
        //This then allows for smooth live updating of charts without it taking to much processing power.

        public void UpdateBrakeChart()
        {
            this.brakeChart.Series.Clear();
            List<int> _series;
            List<int> _points;

            //Console.WriteLine("Updating the big chart!");

            collector.data.GetBrakeData(out _series,out _points);

            for (int i = 0; i < _series.Count(); i++)
            {
                int _num = _series[i];
                Series series = new Series(_num.ToString());
                series.Points.Add(_points[i]);
                series.Name = _num.ToString();
                series.ChartArea = "Test";
                this.brakeChart.Series.Add(series);
            }
        }

        //when the simulation is reset, the data collector also needs to be reset, that happens here

        public void DataControllerReset()
        {
            this.collector = new DataCollector(this);
            this.UpdateBrakeChart();
            this.UpdateBrakePerTickChart();
        }

        public void SmallUpdateBrakeChart(int _serie) //DO NOT USE
        {
            Series serie = this.brakeChart.Series.FindByName(_serie.ToString());

            if (serie != null)
            {
                Console.WriteLine("ADDED!");
                double _value = serie.Points.First().YValues.First();
                serie.Points.Clear();
                serie.Points.Add(_value + 1);
            }
            else
            {
                UpdateBrakeChart();
            }
        }

        public void SmallUpdateSpeedPerTickChart(int _tick, double _amount) //DO NOT USE;
        {
            if (this.averageSpeed.Series.Any())
            {
                this.averageSpeed.Series.First().Points.AddXY(_tick.ToString(), _amount);
            }
            else
            {
                UpdateBrakePerTickChart();
            }
            
        }

        public void UpdateBrakePerTickChart()
        {
            this.averageSpeed.Series.Clear();
            List<Tuple<int,double>> _data = collector.data.GetAverageSpeedPerTick();

            Console.WriteLine("Updating the big chart!");

            Series series = new Series("Average speed for all cars");
            series.ChartType = SeriesChartType.Spline;

            try
            {
                foreach (Tuple<int, double> _point in _data)
                {
                    series.Points.AddXY(_point.Item1.ToString(), _point.Item2);
                }

                this.averageSpeed.Series.Add(series);
            }catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void ChangeDataSet(Data _dataSet)
        {
            collector.data = _dataSet;
        }

        public void DrawCharts(Graphics g)
        {
            //Nothing of interest happens here as of yet
        }

        //This is the function that exports the data, It will take a name give the user as its name
        //To prevent the system from crashig to do duplicate names, it will first check if there is a duplicate name
        //and if this is the case it will add a Number behind it, it will repeat this process untill there is no longer
        // a duplicate name.
        //It then creates the files in a temp folder, it saves the chart images and a txt file containing the raw data
        //thnis temp folder is then zipped and saved with the name given by the user

        public void ExportData(string BeginName)
        {
            string _folderLocation = ".../.../DataDump/";
            string Name = BeginName;

            int x = 1;
            while(File.Exists(_folderLocation + Name + ".zip"))
            {
                Name = BeginName + "(" + x.ToString() + ")";
                x++;
            }


            Data _data = collector.data;
            Bitmap _brakeChart = new Bitmap(500,500);
            Bitmap _averageSpeedChart = new Bitmap(500, 500);
            string _dataDump = _data.ToString();

            System.IO.Directory.CreateDirectory(_folderLocation + "temp");

            this.brakeChart.DrawToBitmap(_brakeChart, new Rectangle(0, 0, 500, 500));
            this.averageSpeed.DrawToBitmap(_averageSpeedChart, new Rectangle(0, 0, 500, 500));

            this.brakeChart.SaveImage(".../.../DataDump/temp/BrakeChart.png", ImageFormat.Png);
            this.averageSpeed.SaveImage(".../.../DataDump/temp/AverageSpeedChart.png", ImageFormat.Png);

            using (StreamWriter sr = new StreamWriter(_folderLocation + "temp/DataDump.txt"))
            {
                sr.Write(_dataDump);
            }

            try
            {
                ZipFile.CreateFromDirectory(".../.../DataDump/temp", ".../.../DataDump/" + Name + ".zip");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
