using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EICProjectBackend.Setup
{
    public class SetupRunner
    {
        public static void Run(ISetup setup)
        {
            if (setup == null)
                throw new ArgumentNullException(nameof(setup));

            setup.Run();
        }
    }
}
