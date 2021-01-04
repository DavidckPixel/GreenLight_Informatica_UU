using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenLight
{
    public class SpeedSign : AbstractSign
    {
        int speed;

        public SpeedSign()
        {
            this.speed = 50; //BaseSPEED
        }

        public void editSpeed(int _speed)
        {
            this.speed = _speed;
        }

        public override void Read(AI _ai)
        {
            _ai.maxSpeed = speed;
        }

        public int getSpeed()
        {
            return speed;
        }
    }
}
