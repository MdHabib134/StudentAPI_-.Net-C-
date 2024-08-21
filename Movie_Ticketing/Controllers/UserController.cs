using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Movie_Ticketing.Models;


namespace Movie_Ticketing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors()]

    public class UserController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly UserContext _context;
        public UserController(IConfiguration config, UserContext context)
        {
            _config = config;
            _context = context;
        }
        [AllowAnonymous]
        [HttpPost("CreateUser")]
        public IActionResult Create(User user)
        {
            if (_context.Users.Where (u => u.Email == user.Email).FirstOrDefault() != null)
            {
                return Ok("AlreadyExist");
            }
            user.MemberSince = DateTime.Now;
            _context.Users.Add(user);
            _context.SaveChanges();
            return Ok("Success");
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }
        [HttpGet("GetUserByEmail/{email}")]
        public async Task<ActionResult<User>> GetUserInfo(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                return NotFound(); // User not found
            }

            return Ok(user); // Return the user information as JSON response
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.UserID)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        [HttpPut("{email}")]
        public async Task<IActionResult> UpdateUser(string email, User user)
        {
            if (email != user.Email)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists2(email))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'UserContext.Users'  is null.");
            }
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.UserID }, user);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        [AllowAnonymous]
        [HttpPost("LoginUser")]
        public IActionResult Login(Login user)
        {
            var userAvailable = _context.Users.Where(u => u.Email == user.Email && u.Password == user.Password).FirstOrDefault();
            if (userAvailable != null)
            {
                return Ok(new JwtService(_config).GenerateTokane(
                    userAvailable.UserID.ToString(),
                    userAvailable.FirstName,
                    userAvailable.LastName,
                    userAvailable.Email,
                    userAvailable.Mobile
                    ));
            }
            return Ok("Failure");
        }


        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] UserUpdateModel userProfileUpdate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Find the existing user by their email
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == userProfileUpdate.Email);

                if (existingUser == null)
                {
                    return NotFound("User not found");
                }

                // Update user data with new values
                existingUser.FirstName = userProfileUpdate.FirstName;
                existingUser.LastName = userProfileUpdate.LastName;
                existingUser.Mobile = userProfileUpdate.Mobile;
                existingUser.Password = userProfileUpdate.Password;

                // Update other properties as needed

                // Save changes to the database
                await _context.SaveChangesAsync();

                return NoContent(); // Indicates successful update
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        private bool UserExists(int id)
        {
            return (_context.Users?.Any(e => e.UserID == id)).GetValueOrDefault();
        }

        private bool UserExists2(string email)
        {
            return (_context.Users?.Any(e => e.Email == email)).GetValueOrDefault();
        }


    }
}

//https://localhost:7231/
