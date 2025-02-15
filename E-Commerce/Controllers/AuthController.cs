using E_Commerce.Contract.AuthContract;
using E_Commerce.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using NLog;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace E_Commerce.Controllers
{
    [RoutePrefix("api/auth")]
    public class AuthController : ApiController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger _logger;
        private readonly IJwtProvider _jwtProvider;

        public AuthController(UserManager<ApplicationUser> userManager, ILogger logger, IJwtProvider jwtProvider)
        {
            _userManager = userManager;
            _logger = logger;
            _jwtProvider = jwtProvider;
        }

        [HttpPost]
        [Route("signup")]
        public async Task<IHttpActionResult> SignUp([FromBody] SignUpRequest request)
        {
            if (!ModelState.IsValid)
            {
                _logger.Warn($"Invalid Sign-Up request received for email: {request.Email}");
                return BadRequest(ModelState);
            }

            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                _logger.Warn($"Sign-Up failed: User with email {request.Email} already exists.");
                return BadRequest("User already exists.");
            }

            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Address = request.Address,
                PhoneNumber = request.Phone
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                _logger.Error($"Sign-Up failed for {request.Email}. Errors: {string.Join(", ", result.Errors)}");
                return BadRequest(result.Errors.ToString());
            }

            await _userManager.AddToRoleAsync(user.Id, "Customer");
            _logger.Info($"New user created successfully: {request.Email}");

            return Ok("Account created successfully!");
        }

        [HttpPost]
        [Route("login")]
        public async Task<IHttpActionResult> Signin([FromBody] LoginRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.Warn($"Invalid Login request received for email: {request.Email}");
                    return BadRequest(ModelState);
                }

                _logger.Info($"Login attempt for {request.Email}");

                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user == null)
                {
                    _logger.Warn($"Login failed: No user found with email {request.Email}");
                    return BadRequest("Invalid Email/Password");
                }

                var isValidPassword = await _userManager.CheckPasswordAsync(user, request.Password);
                if (!isValidPassword)
                {
                    _logger.Warn($"Login failed: Incorrect password for {request.Email}");
                    return BadRequest("Invalid Email/Password");
                }

                var jwt = await _jwtProvider.GenerateTokenAsync(user);
                _logger.Info($"Login successful for {request.Email}");

                return Ok(new { token = jwt.Token, expiresIn = jwt.ExpiresIn });
            }
            catch (Exception ex)
            {
                _logger.Error($"Unexpected error during login for {request.Email}: {ex.Message}");
                return InternalServerError(new Exception("Something went wrong while logging in."));
            }
        }
    }
}
