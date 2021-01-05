using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenLight
{
    public class YieldSign : AbstractSign
    {
        int prioritylevel; 
        
        public YieldSign(AbstractSignController _controller)
        {
            this.prioritylevel = 1;
            controller = _controller;
        }
            
        public override void Read(AI _ai)
        {
            _ai.prioritylevel = prioritylevel;
        }
    }
}
