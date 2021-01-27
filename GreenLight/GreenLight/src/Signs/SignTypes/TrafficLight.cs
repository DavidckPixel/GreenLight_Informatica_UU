using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenLight
{
    public class TrafficLight : AbstractSign
    {
        TrafficLightController tcontroller;
        public TrafficLight(AbstractSignController _controller) : base()
        {
            controller = (TrafficLightController)_controller;
            tcontroller = (TrafficLightController)_controller; 
        }

        public override void Read(BetterAI _ai)
        {
        }

        public void color()
        {
            tcontroller.changeColor("Red");
        }

    }
}
