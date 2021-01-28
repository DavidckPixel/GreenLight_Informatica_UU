using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenLight
{
    // This is the AbstractController, this is the most basic of the controller class from which all controllers inherit.
    // It only has an abstract Initialize method which all other controllers are forced to have

    public abstract class AbstractController
    {
        abstract public void Initialize();

        public AbstractController()
        {
        }
    }
}
