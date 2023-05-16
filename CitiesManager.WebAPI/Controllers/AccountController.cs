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
        /// <summary>
        /// Login constructor
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="signInManager"></param>
        /// <param name="roleManager"></param>
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager, IJwtService jwtService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _jwtService = jwtService;
        }
        /// <summary>
        /// Register user
        /// </summary>
        /// <param name="registerDTO"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<ActionResult<ApplicationUser>> PostRegister(RegisterDTO registerDTO)
        {
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
                return Ok(authenticationResponse);
            }
            else
            {
                string errorMessage = string.Join(" | ", identityResult.Errors.Select(e => e.Description));
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
                return Ok(true);
            }
            else
            {
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
                    return Ok(authenticationResponse);
                }
                else
                {
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
            await _signInManager.SignOutAsync();
            return NoContent();
        }
    }
}
