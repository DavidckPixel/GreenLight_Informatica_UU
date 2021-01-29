using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;

namespace GreenLight
{
    //this is the data Class for the DriverProfileFaces, instances of which are loaded from the faces.json
    //it contains the faces, a name, speech and also a background image;

    public class DriverProfileFace
    {
        public string name {get;}
        public string fileName { get; }
        Tuple<int, int> fullSize { get; }
        public Tuple<int, int> index { get; }
        public Tuple<int,int> imgSize { get; }
        public List<string>[] speech { get; }
        public Bitmap backImg { get; }

        public DriverProfileFace(string name, string fileName, int sizeX, int sizeY,int indexX,int indexY, List<string>[] speech, string backImg)
        {
            this.name = name;
            this.fileName = fileName;
            this.fullSize = new Tuple<int, int>(sizeX, sizeY);
            this.index = new Tuple<int, int>(indexX, indexY);
            this.speech = speech;

            try
            {
                this.imgSize = new Tuple<int, int>(sizeX / indexX, sizeY / indexY);
            }
            catch (Exception)
            {
                this.imgSize = new Tuple<int, int>(sizeX, sizeY);
            }

            this.backImg = new Bitmap(backImg);
        }
    }
}
