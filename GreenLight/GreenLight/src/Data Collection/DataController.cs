using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using System.Windows.Forms.DataVisualization.Charting;

namespace GreenLight.src.Data_Collection
{
    class DataController : AbstractController
    {
        public DataCollector collector;
        Chart brakeChart, averageSpeed;
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
            this.brakeChart.Location = new Point(500, 600);

            averageSpeed = new Chart();
            this.averageSpeed.Palette = ChartColorPalette.EarthTones;
            this.averageSpeed.ChartAreas.Add(new ChartArea("AverageSpeed"));
            this.averageSpeed.Location = new Point(200, 600);

            // Set title
            this.brakeChart.Titles.Add("Animals");

            this.screen.Controls.Add(this.averageSpeed);
            this.screen.Controls.Add(this.brakeChart);
        }

        public void UpdateBrakeChart()
        {
            this.brakeChart.Series.Clear();
            List<int> _series;
            List<int> _points;

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
                Console.WriteLine("UPDATED!");
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
            //SAVE DATA HERE

            collector.data = _dataSet;
        }

        public void DrawCharts(Graphics g)
        {
            Console.WriteLine("Drawing Charts!");
        }
    }
}
