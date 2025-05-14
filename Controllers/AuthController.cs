using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using kitabhChauta.DbContext;
using KitabhChautari.Dto;
using kitabhChauta.Models;
using KitabhChautari.Services;
using Microsoft.EntityFrameworkCore;
using kitabhChauta.Dto;
using kitabhChauta.Services;

namespace kitabhChauta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtService _jwtService;
        private readonly KitabhChautariDbContext _context;
        private readonly ILogger<AuthController> _logger;
        private readonly IEmailService _emailService;

        public AuthController(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IJwtService jwtService,
            KitabhChautariDbContext context,
            ILogger<AuthController> logger,
            IEmailService emailService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtService = jwtService;
            _context = context;
            _logger = logger;
            _emailService = emailService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] MemberDto model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state during registration: {Errors}", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return BadRequest(ModelState);
            }

            var existingUser = await _userManager.FindByNameAsync(model.ContactNo.Trim());
            if (existingUser != null)
            {
                return BadRequest(new { Message = "A user with this contact number already exists" });
            }

            var existingMember = await _context.Members.FirstOrDefaultAsync(m => m.ContactNo == model.ContactNo.Trim());
            if (existingMember != null)
            {
                return BadRequest(new { Message = "A member with this contact number already exists" });
            }

            if (model.Email != null)
            {
                var emailExists = await _context.Members.FirstOrDefaultAsync(m => m.Email == model.Email.Trim().ToLower());
                if (emailExists != null)
                {
                    return BadRequest(new { Message = "A member with this email already exists" });
                }
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var user = new IdentityUser
                {
                    UserName = model.ContactNo.Trim(),
                    Email = model.Email?.Trim().ToLower()
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (!result.Succeeded)
                {
                    _logger.LogWarning("User creation failed: {Errors}", result.Errors);
                    return BadRequest(new { Errors = result.Errors });
                }

                const string userRole = "User";
                if (!await _roleManager.RoleExistsAsync(userRole))
                {
                    await _roleManager.CreateAsync(new IdentityRole(userRole));
                }

                await _userManager.AddToRoleAsync(user, userRole);

                var member = new Member
                {
                    UserId = user.Id,
                    FirstName = model.FirstName.Trim(),
                    LastName = model.LastName.Trim(),
                    ContactNo = model.ContactNo.Trim(),
                    Email = model.Email?.Trim().ToLower(),
                    DateOfBirth = model.DateOfBirth,
                    RegistrationDate = DateTime.UtcNow
                };

                _context.Members.Add(member);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new { Message = "Member registered successfully" });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Registration failed");
                return StatusCode(500, new { Message = "Registration failed", Error = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state during login: {Errors}", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(model.Email?.Trim().ToLower());
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                _logger.LogWarning("Invalid login attempt for email: {Email}", model.Email);
                return Unauthorized(new { Message = "Invalid credentials" });
            }

            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtService.GenerateJwtToken(user, roles.FirstOrDefault());

            return Ok(new AuthResponseDto
            {
                Token = token,
                Role = roles.FirstOrDefault() ?? "User"
            });
        }

        [HttpPost("register-staff")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RegisterStaff([FromBody] StaffDto model)
        {
            _logger.LogInformation("RegisterStaff endpoint called");
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model state invalid: {Errors}", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return BadRequest(ModelState);
            }

            IdentityUser? user = null;
            string password = null;
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                _logger.LogInformation("Generating secure password");
                password = GenerateSecurePassword();

                _logger.LogInformation("Creating user account");
                user = new IdentityUser
                {
                    UserName = model.Email, // Use Email as UserName
                    Email = model.Email,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user, password);
                if (!result.Succeeded)
                {
                    _logger.LogWarning("User creation failed: {Errors}", result.Errors);
                    return BadRequest(result.Errors);
                }

                _logger.LogInformation("Assigning staff role");
                const string staffRole = "Staff";
                if (!await _roleManager.RoleExistsAsync(staffRole))
                {
                    await _roleManager.CreateAsync(new IdentityRole(staffRole));
                }

                await _userManager.AddToRoleAsync(user, staffRole);

                _logger.LogInformation("Creating member profile");
                _context.Members.Add(new Member
                {
                    UserId = user.Id,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    ContactNo = model.ContactNo,
                    Email = model.Email,
                    IsStaff = true,
                    DateOfBirth = null,
                    RegistrationDate = DateTime.UtcNow
                });

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                _logger.LogInformation("Sending email credentials");
                await _emailService.SendStaffCredentialsAsync(model.Email, password);

                return Ok(new { Message = "Staff registered successfully" });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                if (user != null) await _userManager.DeleteAsync(user);
                _logger.LogError(ex, "Staff registration failed");
                return StatusCode(500, "Registration failed");
            }
        }
        private string GenerateSecurePassword()
        {
            var options = _userManager.Options.Password;
            const string lower = "abcdefghijklmnopqrstuvwxyz";
            const string upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string digits = "0123456789";
            const string special = "!@#$%^&*()_+-=[]{}|;:,.<>?";
            var random = new Random();

            var password = new System.Text.StringBuilder();
            if (options.RequireLowercase) password.Append(lower[random.Next(lower.Length)]);
            if (options.RequireUppercase) password.Append(upper[random.Next(upper.Length)]);
            if (options.RequireDigit) password.Append(digits[random.Next(digits.Length)]);
            if (options.RequireNonAlphanumeric) password.Append(special[random.Next(special.Length)]);

            var allChars = lower + upper + digits + special;
            for (int i = password.Length; i < options.RequiredLength; i++)
            {
                password.Append(allChars[random.Next(allChars.Length)]);
            }

            var array = password.ToString().ToCharArray();
            for (int i = array.Length - 1; i > 0; i--)
            {
                int j = random.Next(0, i + 1);
                (array[i], array[j]) = (array[j], array[i]);
            }

            return new string(array);
        }
    }
}