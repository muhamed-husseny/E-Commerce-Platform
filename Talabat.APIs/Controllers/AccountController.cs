using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Service.Contract;

namespace Talabat.APIs.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public AccountController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IMapper mapper,
            IEmailService emailService,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _emailService = emailService;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrEmpty(email))
            {
                var authHeader = Request.Headers["Authorization"].FirstOrDefault();
                if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
                {
                    var tokenCandidate = authHeader.Substring("Bearer ".Length).Trim();
                    if (tokenCandidate.Contains("@")) email = tokenCandidate;
                }
            }

            var user = !string.IsNullOrEmpty(email) 
                ? await _userManager.FindByEmailAsync(email) 
                : await _userManager.Users.FirstOrDefaultAsync();

            if (user is null)
                return Unauthorized(new ApiResponse(401));

            return Ok(new UserDto
            {
                DisplayName = user.DisplayName,
                Email = user.Email!,
                Token = user.Email!
            });
        }

        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>> CheckEmailExists([FromQuery] string email)
        {
            return await _userManager.FindByEmailAsync(email) is not null;
        }

        [HttpGet("address")]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = !string.IsNullOrEmpty(email)
                ? await _userManager.Users.Include(u => u.Address).FirstOrDefaultAsync(u => u.Email == email)
                : await _userManager.Users.Include(u => u.Address).FirstOrDefaultAsync();

            if (user?.Address is null)
                return Ok(new AddressDto { FirstName = "", LastName = "", Street = "", City = "", Country = "" });

            return Ok(_mapper.Map<AddressDto>(user.Address));
        }

        [HttpPut("address")]
        public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto newAddress)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = !string.IsNullOrEmpty(email)
                ? await _userManager.Users.Include(u => u.Address).FirstOrDefaultAsync(u => u.Email == email)
                : await _userManager.Users.Include(u => u.Address).FirstOrDefaultAsync();

            if (user is null)
                return Unauthorized(new ApiResponse(401));

            var address = _mapper.Map<Talabat.Core.Entities.Identity.Address>(newAddress);
            address.AppUserId = user.Id;
            user.Address = address;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return BadRequest(new ApiResponse(400));

            return Ok(newAddress);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user is null)
               return Unauthorized(new ApiResponse(401));

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

            if (result.Succeeded is false)
                return Unauthorized(new ApiResponse(401));

            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email!,
                Token = user.Email!
            });
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {
            if (await _userManager.FindByEmailAsync(model.Email) is not null)
            {
                return BadRequest(new ApiValidationError 
                { 
                    Errors = new[] { "Email address is already in use" } 
                });
            }

            var user = new AppUser()
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                UserName = model.Email.Split("@")[0],
                PhoneNumber = model.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return BadRequest(new ApiValidationError { Errors = errors });
            }

            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email!,
                Token = user.Email!
            });
        }

        [HttpPost("forgotPassword")]
        [HttpPost("forgot-password")]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null)
                return BadRequest(new ApiResponse(400, "Email does not exist"));

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var baseUrl = _configuration["ApiBaseUrl"] ?? $"{Request.Scheme}://{Request.Host}";
            var resetLink = $"{baseUrl}/api/account/reset-password?email={Uri.EscapeDataString(user.Email!)}&token={Uri.EscapeDataString(token)}";

            var emailBody = $"<h3>Password Reset Request</h3>" +
                            $"<p>To reset your password, please use the token below:</p>" +
                            $"<p><b>Token:</b> {token}</p>" +
                            $"<p><a href='{resetLink}'>Click here to reset your password</a></p>";

            await _emailService.SendEmailAsync(user.Email!, "Reset Password Request", emailBody);

            return Ok(new
            {
                message = "Password reset token generated and sent successfully",
                email = user.Email,
                token = token,
                resetUrl = resetLink
            });
        }

        [HttpPost("resetPassword")]
        [HttpPost("reset-password")]
        public async Task<ActionResult> ResetPassword(ResetPasswordDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null)
                return BadRequest(new ApiResponse(400, "Email does not exist"));

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return BadRequest(new ApiValidationError { Errors = errors });
            }

            return Ok(new { message = "Password has been reset successfully" });
        }

        [HttpPost("changePassword")]
        [HttpPost("change-password")]
        public async Task<ActionResult> ChangePassword(ChangePasswordDto model)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(email))
            {
                var authHeader = Request.Headers["Authorization"].FirstOrDefault();
                if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
                {
                    var tokenCandidate = authHeader.Substring("Bearer ".Length).Trim();
                    if (tokenCandidate.Contains("@")) email = tokenCandidate;
                }
            }
            if (string.IsNullOrEmpty(email)) email = model.Email;

            if (string.IsNullOrEmpty(email))
                return Unauthorized(new ApiResponse(401, "User email not provided"));

            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                return BadRequest(new ApiResponse(400, "User not found"));

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return BadRequest(new ApiValidationError { Errors = errors });
            }

            return Ok(new { message = "Password has been changed successfully" });
        }
    }
}
