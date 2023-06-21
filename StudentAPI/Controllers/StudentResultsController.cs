using Microsoft.AspNetCore.Mvc;

namespace StudentAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentResultsController : ControllerBase
    {
        private static readonly string[] Names = new[]
        {
        "Joe", "Watson", "Billy", "Mike", "Sam", "Toby", "Leo", "Claudia", "Donna", "Margo"
    };

        private readonly ILogger<StudentResultsController> _logger;

        public StudentResultsController(ILogger<StudentResultsController> logger)
        {
            _logger = logger;
        }

        [HttpGet("GetStudentResults")]
        public IEnumerable<StudentResult> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new StudentResult
            {
                Marks = Random.Shared.Next(300, 500),
                StudentName = Names[Random.Shared.Next(Names.Length)]
            })
            .ToArray();
        }

        [HttpGet("HealthCheck")]
        public ActionResult<string> HealthCheck()
        {
            return Ok("Healthy");
        }
    }
}