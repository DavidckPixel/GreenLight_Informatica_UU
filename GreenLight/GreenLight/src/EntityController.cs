using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenLight
{
    public abstract class EntityController : AbstractController
    {
        public OriginPointController OPC = new OriginPointController();
    }
}
