using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TheTask.Data.Consts;
using TheTask.Data.Seeds;
using TheTask.DTOS;
using TheTask.Models;

namespace TheTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;
        private readonly ILogger<AccountController> _logger;
        public AccountController(UserManager<User> userManager, IConfiguration configuration, AppDbContext context, ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _configuration = configuration;
            _context = context;
            _logger = logger;
        }
        [HttpGet("AllUsers")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ViewAllUsers()
        {
            _logger.LogInformation("Get users endpoint called");
            var users = await _userManager.Users.Select(u => new UserDTO
            {
                UserName = u.UserName,
                Deposit = u.Deposit,
                Role = u.Role
            }).ToListAsync();
            _logger.LogInformation("Users viewed");
            return Ok(users);
        }
        [HttpGet("User/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUser(string id)
        {
            _logger.LogInformation("Get user endpoint called");
            var User = await _userManager.FindByIdAsync(id);
            if (User is null)
            {
                _logger.LogInformation("User entered an invalid id");
                return BadRequest($"Can't find user with this id: {id}");
            }
            _logger.LogInformation("User viewed");
            return Ok(User);
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterDTO user)
        {
            _logger.LogInformation("Register user endpoint called");
            if (ModelState.IsValid)
            {
                var User = new User()
                {
                    UserName = user.UserName,
                    Deposit = user.Deposit,
                    Role = user.Role
                };
                var result = await _userManager.CreateAsync(User, user.Password);
                if (result.Succeeded)
                {
                    if (User.Role == ConstsNames.Admin)
                        await _userManager.AddToRoleAsync(User, ConstsNames.Admin);
                    else if (User.Role == ConstsNames.Seller)
                        await _userManager.AddToRoleAsync(User, ConstsNames.Seller);
                    else if (User.Role == ConstsNames.Buyer)
                        await _userManager.AddToRoleAsync(User, ConstsNames.Buyer);
                    else
                    {
                        _logger.LogInformation("The role is invalid");
                        return BadRequest("Invalid role!");
                    }
                    _logger.LogInformation("User has been created");
                    return Ok("User has been created");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("Custom", item.Description);
                    }
                }
            }
            return BadRequest();
        }
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDTO user)
        {
            _logger.LogInformation("Login user endpoint called");
            if (ModelState.IsValid)
            {
                var User = await _userManager.FindByNameAsync(user.UserName);
                if (User is not null)
                {
                    if (await _userManager.CheckPasswordAsync(User, user.Password))
                    {
                        var claims = new List<Claim>();
                        claims.Add(new Claim(ClaimTypes.Name, User.UserName));
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, User.Id));
                        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                        var roles = await _userManager.GetRolesAsync(User);
                        foreach (var role in roles)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
                        }

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));
                        var sc = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var token = new JwtSecurityToken(
                            claims: claims,
                            issuer: _configuration["JWT:Issuer"],
                            audience: _configuration["JWT:Audience"],
                            expires: DateTime.Now.AddHours(1),
                            signingCredentials: sc
                            );
                        var _token = new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo,
                        };
                        _logger.LogInformation("User Logged in");
                        return Ok(_token);
                    }
                    _logger.LogInformation("Invalid password");
                    return BadRequest("Invalid username or password");
                }
                _logger.LogInformation("Invalid username");
                return BadRequest("Invalid username or password");
            }
            return BadRequest();
        }
        [HttpPut("UpdateUser/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser([FromRoute] string id, [FromBody] UpdatedUserDTO UpdatedUser)
        {
            _logger.LogInformation("Update user endpoint called");
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
            {
                _logger.LogInformation("User entered an invalid id");
                return BadRequest($"Can't find user with this id: {id}");
            }
            user.UserName = UpdatedUser.UserName;
            user.PasswordHash = UpdatedUser.Password;
            user.Role = UpdatedUser.Role;
            user.Deposit = UpdatedUser.Deposit;
            _logger.LogInformation("User updated");
            return Ok("User updated");
        }
        [HttpDelete("DeleteUser/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser([FromRoute] string id)
        {
            _logger.LogInformation("Delete user endpoint called");
            var User = await _userManager.FindByIdAsync(id);
            if (User is null)
            {
                _logger.LogInformation("User entered an invalid id");
                return BadRequest($"Can't find user with this id: {id}");
            }
            await _userManager.DeleteAsync(User);
            _logger.LogInformation("User deleted");
            return Ok("User deleted");
        }
        [HttpPost("Deposit/{id}")]
        [Authorize(Roles = "Buyer")]
        public async Task<IActionResult> Deposit([FromRoute] string id, double amount)
        {
            _logger.LogInformation("Deposit endpoint called");
            var User = await _userManager.FindByIdAsync(id);
            if (User is null)
            {
                _logger.LogInformation("User entered an invalid id");
                return BadRequest($"Can't find user with this id: {id}");
            }
            if (amount != 5 && amount != 10 && amount != 20 && amount != 50 && amount != 100)
            {
                _logger.LogInformation("Invalid amount");
                return BadRequest("Invalid amount only accepted coins is 5,10,20,50 and 100 cents coins");
            }
            User.Deposit += amount;
            _context.SaveChanges();
            _logger.LogInformation("Deposit has been added");
            return Ok("Deposit has been added");
        }
        [HttpPost("Reset/{id}")]
        [Authorize(Roles = "Buyer")]
        public async Task<IActionResult> Reset([FromRoute] string id)
        {
            _logger.LogInformation("Reset endpoint called");
            var User = await _userManager.FindByIdAsync(id);
            if (User is null)
            {
                _logger.LogInformation("User entered an invalid id");
                return BadRequest($"Can't find user with this id: {id}");
            }
            User.Deposit = 0;
            _context.SaveChanges();
            _logger.LogInformation("Deposit has been reset");
            return Ok("Deposit has been reset");
        }
        [HttpPost("Buy/{id}")]
        [Authorize(Roles = "Buyer")]
        public async Task<IActionResult> Buy([FromRoute] string id, int productId, int amount)
        {
            _logger.LogInformation("Buy endpoint called");
            var User = await _userManager.FindByIdAsync(id);
            if (User is null)
            {
                _logger.LogInformation("User entered an invalid user id");
                return BadRequest($"Can't find user with this id: {id}");
            }
            var product = _context.Products.Find(productId);
            if (product is null)
            {
                _logger.LogInformation("User entered an invalid product id");
                return BadRequest($"Can't find product with this id: {productId}");
            }
            if (product.AmountAvailable < amount)
            {
                _logger.LogInformation("The amount is unavailable");
                return BadRequest("The product amount is unavailable");
            }
            var totalCost = amount * product.Cost;
            if (totalCost > User.Deposit)
            {
                _logger.LogInformation("Deposit is insufficient");
                return BadRequest("Your deposit is insufficient. Please deposit more");
            }
            product.AmountAvailable -= amount;
            User.Deposit -= totalCost;
            _context.SaveChanges();
            var change = User.Deposit;
            Dictionary<int, int> changeCoins = new Dictionary<int, int>();
            int[] coinDenominations = new int[] { 100, 50, 20, 10, 5 };
            foreach (int coin in coinDenominations)
            {
                int count = (int)(change / coin);
                if (count > 0)
                {
                    changeCoins.Add(coin, count);
                    change %= coin;
                }
            }
            var response = new
            {
                TotalSpent = totalCost,
                ProductsPurchased = amount,
                Change = changeCoins
            };
            _logger.LogInformation("Process done");
            return Ok(response);
        }
    }
}