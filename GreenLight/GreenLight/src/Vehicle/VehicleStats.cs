using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenLight
{
    public class VehicleStats
    {
        //This class allows for storage of all the variables of a vehicletype

        public string Name;
        public int Weight;
        public float Length;
        public int Topspeed;
        public int Motorpwr;
        public float Surface;
        public float Cw;
        public float Occurance;
        public bool canEdit;

        public VehicleStats(string Name, int Weight, float Length, int Topspeed, int Motorpwr, float Cw, float Surface, bool canEdit, float Occurance)
        {
            this.Name = Name;
            this.Weight = Weight;
            this.Length = Length;
            this.Topspeed = Topspeed;
            this.Motorpwr = Motorpwr;
            this.Surface = Surface;
            this.Cw = Cw;
            this.Occurance = Occurance;
            this.canEdit = canEdit;
        }
    }
}