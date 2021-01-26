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
        public PrioritySign(AbstractSignController _controller)
        {
            this.prioritylevel = 3;
            controller = _controller;
        }
        public override void Read(BetterAI _ai)
        {
            _ai.ChangePriority(this.prioritylevel);
        }
    }
    
}
