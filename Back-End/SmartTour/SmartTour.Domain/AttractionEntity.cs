using System;
using System.Collections.Generic;
using System.Text;

namespace SmartTour.Domain
{
    public class AttractionEntity : PlaceEntity
    {
        public List<Dictionary<string, string>> Subtype { get; set; }

    }
}
