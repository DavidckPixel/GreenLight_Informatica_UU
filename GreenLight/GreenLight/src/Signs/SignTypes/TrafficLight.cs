using System;
using System.Timers;
using System.Diagnostics;

namespace GreenLight
{
    public class TrafficLight : AbstractSign
    {
        public string color = "Red";
        Stopwatch lightTimer;


        public TrafficLight(AbstractSignController _controller) : base()
        {
            controller = (TrafficLightController)_controller;
        }



        public void SwitchLights()
        {
            if (color == "Red")
            {
                color = "Green";
                controller.changeColor(color);
            }
            else if (color == "Orange")
            {
                color = "Red";
                controller.changeColor(color);
            }
            else if (color == "Green")
            {
                color = "Orange";
                controller.changeColor(color);
                Timer orangeTimer = new Timer();
                orangeTimer.Interval = 1000;
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

        /*public void color()
        {
            controller.changeColor("Red");
        }*/
    }
}
