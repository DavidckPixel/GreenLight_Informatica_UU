using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//ABSTRACT CONTROLLER CLASS, this is the most basic of the controller class from which all controllers inherit.
//It only has a abstract Initialize class which all other controllers are forced to have

namespace GreenLight
{
    public abstract class AbstractController
    {
        abstract public void Initialize();

        public AbstractController()
        {
        }
    }
}
