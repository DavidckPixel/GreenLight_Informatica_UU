using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace GreenLight
{
    public class DriverProfile
    {
        public int ticksOnBrake = 0;
        public double fuelUsed = 0;
        public int ticksStationary = 0;

        public bool isBraking;
        public string imageFile;
        public Tuple<int, int> imageIndex;

        public string mood;
        private DriverProfileFace faceType;
        public Bitmap imgFace;

        public DriverProfile(World _physics)
        {
            Random ran = new Random();

            string _type = _physics.entityTypes;

            faceType = DriverProfileData.faces.Find(x => x.name == _type); 

            if(faceType == null)
            {
                faceType = DriverProfileData.faces.First();
            }

            this.mood = faceType.speech[1][ran.Next(0, faceType.speech[1].Count)];

            int _indexX = ran.Next(1, faceType.index.Item1);
            int _indexY = ran.Next(1, faceType.index.Item2);

            this.imageIndex = new Tuple<int, int>(_indexX, _indexY);

            Bitmap _original = new Bitmap(faceType.fileName);
            Rectangle _src = new Rectangle(faceType.imgSize.Item1 * (_indexX - 1), faceType.imgSize.Item2 * (_indexX - 1), faceType.imgSize.Item1, faceType.imgSize.Item2);
            this.imgFace = (Bitmap)_original.Clone(_src, _original.PixelFormat);
        }

        public void AddBreakTick()
        {
            this.ticksOnBrake++;
        }

        public void CalculateFuel(double _speed)
        {
            ticksStationary = _speed <= 0 ? ticksStationary++ : ticksStationary;
            fuelUsed += _speed * 0.005;
        }

    }
}
