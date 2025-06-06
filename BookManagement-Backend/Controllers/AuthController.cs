using AuthDTOs;
using AuthDTOs.Models;
using BookManagement_Backend.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Shared;
using Services;

namespace Task1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<BookController> _logger;
        private readonly JwtService _jwtService;
        public AuthController(UserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManager,
                                 RoleManager<IdentityRole> roleManager,
                                  ILogger<BookController> logger,
                                  JwtService jwtService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = logger;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] RegisterDTO model)
        {
            IActionResult response = BadRequest();
            _logger.LogInformation("Entered the Register Method of AuthController with data as "+ model.Password);

            try
            {
                var userToAdd = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName
                };

                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    response = Ok(new Response
                    {
                        StatusCode = 1,
                        Message = "User is alredy registered with this email.",
                        Data = string.Empty,
                    });
                    return response;
                }
                var result = await _userManager.CreateAsync(userToAdd, model.Password);

                if (!await _roleManager.RoleExistsAsync("Admin"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                }
                if (!result.Succeeded)
                {
                    response = Ok(new Response
                    {
                        StatusCode = 1,
                        Message = "User Is Not registered Something went wrong",
                        Data = result.Errors.First().Description
                    });
                    return response;
                }
                result = await _userManager.AddToRoleAsync(userToAdd, "Admin");

                if (result.Succeeded)
                {

                    response = Ok(new Response
                    {
                        StatusCode = 0,
                        Message = "User registered successfully",
                        Data = string.Empty
                    });
                }
                else
                {
                    response = Ok(new Response
                    {
                        StatusCode = 1,
                        Message = "User is not asigned the role",
                        Data = result.Errors.ToString(),
                    });
                }

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error: " + ex.Message);
                response = Ok(new Response
                {
                    StatusCode = 1,
                    Message = "Unknown error ocured.",
                    Data = string.Empty,
                });
                return response;
            }

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] LoginDTO model)
        {
            IActionResult response = BadRequest();
            _logger.LogInformation("Entered the Login Method of AuthController");

            try
            {
                var user = await _userManager.FindByNameAsync(model.Username);
                if (user == null)
                {
                    response = Ok(new Response
                    {
                        StatusCode = 1,
                        Message = "User with this username does not exists",
                        Data = string.Empty,
                    });
                    return response;
                }

                var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
                if (!result.Succeeded)
                {
                    response = Ok(new Response
                    {
                        StatusCode = 1,
                        Message = "Password is Incorrect.",
                        Data = String.Empty
                    });
                    return response;
                }
                var userRoles = await _userManager.GetRolesAsync(user);
                var tokensDto = await CreateApplicationUserDto(user, string.Join(", ", userRoles));

                user.RefreshToken = tokensDto.RefreshToken;
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(2); 
                await _userManager.UpdateAsync(user);

                response = Ok(new Response
                {
                    StatusCode = 0,
                    Message = "You have successfully loged In.",
                    Data = tokensDto
                }); 

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error: " + ex.Message);
                response = Ok(new Response
                {
                    StatusCode = 1,
                    Message = "Unknown error ocured.",
                    Data = string.Empty,
                });
                return response;
            }
        }
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDTO tokenDto)
        {
            IActionResult response = BadRequest();

            try
            {
                var user = await _userManager.FindByNameAsync(tokenDto.username);
                if (user == null || user.RefreshToken != tokenDto.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                {
                    return Ok(new Response
                    {
                        StatusCode = 1,
                        Message = "Invalid refresh token or session expired",
                        Data = string.Empty
                    });
                }

                var newAccessToken = await _jwtService.GenerateToken(user);
                var newRefreshToken = _jwtService.GenerateRefreshToken();

                user.RefreshToken = newRefreshToken;
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
                await _userManager.UpdateAsync(user);

                return Ok(new Response
                {
                    StatusCode = 0,
                    Message = "Token refreshed successfully",
                    Data = new
                    {
                        accessToken = newAccessToken,
                        refreshToken = newRefreshToken
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError("Error refreshing token: " + ex.Message);
                return Ok(new Response
                {
                    StatusCode = 1,
                    Message = "An error occurred while refreshing the token.",
                    Data = string.Empty
                });
            }
        }


















        private async Task<TokensDto> CreateApplicationUserDto(ApplicationUser user, string userroles)
        {
            return new TokensDto
            {
                AccessToken = await _jwtService.GenerateToken(user),
                RefreshToken = _jwtService.GenerateRefreshToken()
            };
        }

    }
}
