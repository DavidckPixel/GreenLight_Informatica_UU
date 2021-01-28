using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenLight
{
    public class World
    {
        //This is the World class.
        //It stores variables that all cars/roads take into their calculations

        public string name;
        public int Brakepwr;
        public double Density;
        public double Gravity;
        public string entityTypes;
        public float slip;
        public bool canDelete;

        public World(string _name ,int _Brakepwr, double _Density, double _Gravity, string _entityTypes, float _slip, bool _canDelete = true)
        {
            this.Brakepwr = _Brakepwr;
            this.Density = _Density;
            this.Gravity = _Gravity;
            this.entityTypes = _entityTypes;
            this.slip = _slip; //Temporarily 0.012f
            this.name = _name;
            this.canDelete = _canDelete;
        }

        public override string ToString()
        {
            return this.name;
        }
    }
}