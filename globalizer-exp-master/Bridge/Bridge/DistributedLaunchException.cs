using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bridge
{
    class DistributedLaunchException : Exception
    {
        public DistributedLaunchException(String message) : base(message) { }

        public DistributedLaunchException() {}
    }
}
