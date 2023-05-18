using CitiesManager.Core.DTO;
using CitiesManager.Core.Identity;
using CitiesManager.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CitiesManager.WebAPI.Controllers
{
    /// <summary>
    /// Account controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IJwtService _jwtService;
        private readonly ILogger<AccountController> _logger;
        /// <summary>
        /// Login constructor
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="signInManager"></param>
        /// <param name="roleManager"></param>
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager, IJwtService jwtService, ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _jwtService = jwtService;
            _logger = logger;
        }
        /// <summary>
        /// Register user
        /// </summary>
        /// <param name="registerDTO"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<ActionResult<ApplicationUser>> PostRegister(RegisterDTO registerDTO)
        {
            _logger.LogInformation("Reached PostRegister action");
            if (ModelState.IsValid == false)
            {
                string errorMessage = string.Join(" | ", ModelState.Values.SelectMany(error => error.Errors).Select(e => e.ErrorMessage));
                return Problem(errorMessage);
            }
            ApplicationUser applicationUser = new ApplicationUser()
            {
                Email = registerDTO.Email,
                PhoneNumber = registerDTO.PhoneNumber,
                UserName = registerDTO.Email,
                PersonName = registerDTO.PersonName
            };
            //Create new user in identity AspNet.User table in DB
            IdentityResult identityResult = await _userManager.CreateAsync(applicationUser,registerDTO.Password);
            if(identityResult.Succeeded)
            {
                //Sign-in the created user
               await _signInManager.SignInAsync(applicationUser, isPersistent:false);
               AuthenticationResponse authenticationResponse = _jwtService.CreateJWTToken(applicationUser);
                _logger.LogInformation("JWT Token created successfully");

                return Ok(authenticationResponse);
            }
            else
            {
                string errorMessage = string.Join(" | ", identityResult.Errors.Select(e => e.Description));
                _logger.LogInformation("Error occurred while login");

                return Problem(errorMessage);
            }
        }
        /// <summary>
        /// Check email is already used
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> IsEmailAlreadyRegistered(string email)
        {
            ApplicationUser? user = await _userManager.FindByEmailAsync(email);
            if(user == null)
            {
                _logger.LogInformation("Email is not used");
                return Ok(true);
            }
            else
            {
                _logger.LogInformation("Email is in use");
                return Ok(false);
            }
        }
        /// <summary>
        /// Login user
        /// </summary>
        /// <param name="loginDTO"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<ActionResult<ApplicationUser>> PostLogin(LoginDTO loginDTO)
        {
            _logger.LogInformation("Reached PostLogin method");
            if (ModelState.IsValid == false)
            {
                string errorMessage = string.Join(" | ", ModelState.Values.SelectMany(errors => errors.Errors).Select(e => e.ErrorMessage));
                return Problem(errorMessage);
            }
            else
            {
               var result = await _signInManager.PasswordSignInAsync(loginDTO.Email, loginDTO.Password, isPersistent: false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(loginDTO.Email);
                    if(user == null)
                    {
                        return NoContent();
                    }
                    var authenticationResponse = _jwtService.CreateJWTToken(user);
                    _logger.LogInformation("JWT token produced successfully");

                    return Ok(authenticationResponse);
                }
                else
                {
                    _logger.LogInformation("JLogin failed due to incorrect credential");
                    return Problem("Invalid email or password!");
                }
            }
        }
        /// <summary>
        /// Logout 
        /// </summary>
        /// <returns></returns>
        [HttpGet("logout")]
        public async Task<IActionResult> GetLogout()
        {
            _logger.LogInformation("Reached logout method");
            await _signInManager.SignOutAsync();
            return NoContent();
        }
    }
}
