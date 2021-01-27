using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenLight
{
    public class SpeedSign : AbstractSign
    {
        public SpeedSign(AbstractSignController _controller) : base()
        {
            this.speed = 50; //BaseSPEED
            SpeedSignController _test = (SpeedSignController)_controller;
            Console.WriteLine("CONTROLLLER: " + _test.ToString());
            controller = _controller;
        }

        public void editSpeed(int _speed)
        {
            this.speed = _speed;
        }

        public override void Read(BetterAI _ai)
        {
            Console.WriteLine("CHANGING THE SPEED TO: " + this.speed / 10);
            _ai.ChangeTargetSpeed(this.speed);
        }

        public int getSpeed()
        {
            return speed;
        }
    }
}
