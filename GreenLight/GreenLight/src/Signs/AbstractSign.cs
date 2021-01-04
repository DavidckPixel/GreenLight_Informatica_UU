using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenLight
{
    public abstract class AbstractSign
    {

        public AbstractSign()
        {
            ID = General_Form.Main.BuildScreen.builder.signController.Signs.Count();
        }

        public abstract void Read(AI _ai);
        private int ID;
    }
}
