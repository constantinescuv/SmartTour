using System;
using System.Collections.Generic;
using System.Text;

namespace SmartTour.Domain
{
    public class SubtypeDataEntity
    {
        public Dictionary<string, string> icons;
        public Dictionary<string, int> time;
        public Dictionary<string, bool> is_outdoor;
        public Dictionary<string, string> season; //cold-only, hot-only or both
    }
}
