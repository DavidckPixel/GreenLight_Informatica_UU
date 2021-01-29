using System;
using System.Timers;
using System.Diagnostics;

namespace GreenLight
{
    public class TrafficLight : AbstractSign
    {
        public string color = "Red";
        public int crossRoadnr; //number from 1 to 4 depending on the connected crossroadside
        public int trafficLightnr;

        public TrafficLight(AbstractSignController _controller) : base()
        {
            controller = (TrafficLightController)_controller;
        }



        public void SwitchLights()
        {

            if (color == "Red")
            {
                color = "Green";
                controller.signController.changeColor(color, trafficLightnr);
            }
            else if (color == "Orange")
            {
                color = "Red";
                controller.signController.changeColor(color, trafficLightnr);
            }
            else if (color == "Green")
            {
                color = "Orange";
                controller.signController.changeColor(color, trafficLightnr);
                Timer orangeTimer = new Timer();
                orangeTimer.Interval = 2000;
                orangeTimer.Enabled = true;
                orangeTimer.AutoReset = false;
                orangeTimer.Elapsed += (object source, ElapsedEventArgs e) => { SwitchLights(); };
            }
        }

        public override void Read(BetterAI _ai)
        {
            if (color == "Red")
            {
                _ai.lightIsRed = true;
                _ai.lightIsGreen = false;
            }
            else if (color == "Green")
            {
                _ai.lightIsGreen = true;
                _ai.lightIsRed = false;
            }
        }
    }
}