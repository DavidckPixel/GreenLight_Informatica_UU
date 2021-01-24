using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

//The most basic sign class from which all the signs inherit
//An important thing to note is that every sign has its unique identifier stored in ID

namespace GreenLight
{
    public abstract class AbstractSign
    {
        public int speed;
        public AbstractSign()
        {
            ID = General_Form.Main.BuildScreen.builder.signController.SignCount;
        }

        public abstract void Read(AI _ai);
        private int ID { get; }
        public AbstractSignController controller;
    }
}
