using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApiProject.Controllers
{
    public class GeoController : ControllerBase
    {
        [HttpGet]
        public ActionResult GetLatitude()
        {
            return null;
        }
        [HttpGet]
        public ActionResult GetLongitude()
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
