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
        
        public YieldSign(AbstractSignController _controller) : base()
        {
            this.prioritylevel = 1;
            controller = _controller;
        }
            
        public override void Read(BetterAI _ai)
        {
            _ai.ChangePriority(this.prioritylevel);
        }
    }
}
