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
        
        public YieldSign()
        {
            this.prioritylevel = 1;
        }
            
        
        public override void Read(AI _ai)
        {
            _ai.prioritylevel = prioritylevel;
        }
    }
}
