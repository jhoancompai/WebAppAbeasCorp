using Microsoft.AspNetCore.Mvc;
using Trabajo_Final___AbeasCorp;

namespace WebAppAbeasCorp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequesController : Controller
    {
        private static List<Request> _requests = new List<Request>();
        private static RequestSchedulerTest _scheduler = new RequestSchedulerTest();

        
        [HttpPost("NuevaSolicitud")]
        public IActionResult NuevaSolicitud([FromBody] List<Request> requests)
        {
            if (requests == null || requests.Count == 0)
            {
                return BadRequest("No se recibieron solicitudes.");
            }

            foreach (var request in requests) 
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Datos de solicitud no validos.");
                }
                if (!ModelStateStartime(request.StartTime)) 
                {
                    return BadRequest($"La hora de inicio debe estar entre  0 y 23. {request.StartTime} no es valido.");                    
                }
                if (!ModelStateDuration(request.Duration))
                {
                    return BadRequest($"La duracion de debe estar entre 0 y 15. {request.Duration} no es valido.");
                }
                _requests.Add(request);
            }
            return Ok("Solicitudes agregadas correctamente.");
        }

        private bool ModelStateDuration(int startTime)
        {
            return startTime >= 0 && startTime <= 23; 
        }

        private bool ModelStateStartime(int duration)
        {
            return duration >= 0 && duration <= 15;
        }
        [HttpPost("MaximoBeneficio")]
        public IActionResult MaximoBeneficio()
        {
            if (_requests.Count == 0)
            {
                return NotFound("No se recibieron solicitudes.");
            }

            var maxGain = _scheduler.GetMaxGain(_requests);                  
            return Ok($"El maximo beneficio es: {maxGain}");
        }
    }
}
