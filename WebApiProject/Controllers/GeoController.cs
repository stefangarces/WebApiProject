using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApiProject.Controllers
{
    public class GeoController : ControllerBase
    {
        private readonly object Longitude;

        [HttpGet]
        public ActionResult GetLongitudeLatitude()
        {
            Longitude
        }
        [HttpGet]
        public ActionResult GetComment()
        {
            return null;
        }
        [Authorize]
        [HttpPost]
        public ActionResult PostComment()
        {
            return null;
        }
    }
}
