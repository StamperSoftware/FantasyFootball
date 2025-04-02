using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class HomeController : BaseApiController
{
    [HttpGet]
    public ActionResult IsConnected()
    {
        return Ok(new{connected=true});
    }
}