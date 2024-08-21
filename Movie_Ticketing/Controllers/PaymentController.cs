using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movie_Ticketing.Models;
using Microsoft.AspNetCore.Cors;


namespace Movie_Ticketing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors()]
    public class PaymentController : Controller
    {
        private readonly IConfiguration _config;
        private readonly UserContext _context;
        public PaymentController(IConfiguration config, UserContext context)
        {
            _config = config;
            _context = context;
        }
        [HttpPost("CreatePayment")]
        public IActionResult Create(Payment payment)
        {
            if (_context.Payments.Where(u => u.PaymentID == payment.PaymentID).FirstOrDefault() != null)
            {
                return Ok("AlreadyExist");
            }
            payment.PaymentDate = DateTime.Now;
            _context.Payments.Add(payment);
            _context.SaveChanges();
            return Ok("Success");
        }
       

        //[HttpGet("{id}")]
        //public async Task<ActionResult<Movie>> GetMovie(int id)
        //{
        //    if (_context.Movies == null)
        //    {
        //        return NotFound();
        //    }
        //    var movie = await _context.Movies.FindAsync(id);
        //
        //    if (movie == null)
        //    {
        //        return NotFound();
        //    }
        //
        //    return movie;
        //}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayment(int id)
        {
            if (_context.Payments == null)
            {
                return NotFound();
            }
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null)
            {
                return NotFound();
            }
        
            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();
        
            return NoContent();
        }
        
        //private bool MovieExist(int id)
        //{
        //    return (_context.Movies?.Any(e => e.MovieID == id)).GetValueOrDefault();
        //}
    }
}
