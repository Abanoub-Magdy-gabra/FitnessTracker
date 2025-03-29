using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Request.Performance_tracking
{
    [Route("api/[controller]")]
    [ApiController]
    public class PerformanceMiddleware : ControllerBase
    {
    }
}
