using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublishingData
{
    public interface IPiStatusService
    {
        public PiStatus GetPiStatus();
    }
}
