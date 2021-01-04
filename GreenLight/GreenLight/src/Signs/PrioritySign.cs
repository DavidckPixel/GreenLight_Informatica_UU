using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenLight
{
    public class PrioritySign : AbstractSign
    {
        int prioritylevel;
        public PrioritySign()
        {
            this.prioritylevel = 3;
        }
        public override void Read(AI _ai)
        {
            _ai.prioritylevel = prioritylevel;
        }
    }
    
}
