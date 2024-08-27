using GMap.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Helpers
{
    public  class MarkerData
    {
        public int UnitId {  get; set; }
        public PointLatLng Position { get; set; }
        public string ExtraData { get; set; }
    }
}
