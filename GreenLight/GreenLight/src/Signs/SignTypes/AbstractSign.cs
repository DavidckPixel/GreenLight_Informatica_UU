using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

//The most basic sign class from which all the signs inherit
//An important thing to note is that every sign has its unique identifier stored in ID
//With the read method a car reads the sign and its value.

namespace GreenLight
{
    public abstract class AbstractSign
    {
        public int speed;
        public AbstractSign thisSign;
        public AbstractSign()
        {
            ID = General_Form.Main.BuildScreen.builder.signController.SignCount;
            thisSign = this;
        }
        public abstract void Read(BetterAI _ai);
        private int ID { get; }
        public AbstractSignController controller;
    }
}
