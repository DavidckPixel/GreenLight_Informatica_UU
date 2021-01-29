using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenLight
{
    // A ConnectionLink saves two ConnectionPoints that are connected to create a Lane
    public struct ConnectionLink
    {
        public ConnectionLink(ConnectionPoint _begin, ConnectionPoint _end)
        {
            this.begin = _begin;
            this.end = _end;

            _end.end = true;
        }

        public ConnectionPoint begin;
        public ConnectionPoint end;
    }
}
