using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiProject.Models.V2
{
    public class GeoMessageV2_DTO
    {
        public Messages Message { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
