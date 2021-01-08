using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GreenLight
{
    public abstract class AbstractSign
    {
        public AbstractSign()
        {
            ID = General_Form.Main.BuildScreen.builder.signController.SignCount;
        }

        public abstract void Read(AI _ai);
        private int ID;
        public AbstractSignController controller;
    }
}
