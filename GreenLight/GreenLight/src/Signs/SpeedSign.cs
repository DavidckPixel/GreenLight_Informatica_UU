using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenLight
{
    public class SpeedSign : AbstractSign
    {
        public SpeedSign(AbstractSignController _controller)
        {
            this.speed = 50; //BaseSPEED
            controller = _controller;
        }

        public void editSpeed(int _speed)
        {
            this.speed = _speed;
        }

        public override void Read(BetterAI _ai)
        {
            _ai.ChangeTargetSpeed(this.speed / 10);
        }

        public int getSpeed()
        {
            return speed;
        }
    }
}
