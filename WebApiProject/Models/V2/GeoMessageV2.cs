using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiProject.Models.V2
{
    public class GeoMessageV2
    {
        public int ID { get; set; }
        public string Message { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}
