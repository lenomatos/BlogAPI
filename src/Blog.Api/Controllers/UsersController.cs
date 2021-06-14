using AutoMapper;
using Blog.Api.ViewModels;
using Blog.Business.Interfaces;
using Blog.Business.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;

        public UsersController(IUserRepository userRepository,
            IMapper mapper,
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            IConfiguration configuration)
        {
            _userRepository = userRepository;
            _mapper = mapper;

            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
        }


        [HttpGet]
        public async Task<IEnumerable<UserViewModel>> GetUsers()
        {
            return _mapper.Map<IEnumerable<UserViewModel>>(await _userRepository.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(Guid id)
        {
            var user = await _userRepository.Get(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(Guid id, UserViewModel userViewModel)
        {
            if (id != userViewModel.Id)
            {
                return BadRequest();
            }

            try
            {
                await _userRepository.Update(_mapper.Map<User>(userViewModel));
            }
            catch (DbUpdateConcurrencyException)
            {
                var userValid = await _userRepository.Get(id);

                if (userValid == null)
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
        public async Task<ActionResult<User>> PostUser(UserViewModel userViewModel)
        {
            var user = _mapper.Map<User>(userViewModel);
            await _userRepository.Add(user);
            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(Guid id)
        {
            var user = await _userRepository.Get(id);

            if (user == null)
            {
                return NotFound();
            }

            await _userRepository.Remove(id);

            return user;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterUserViewModel registerUser)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = new IdentityUser
            {
                UserName = registerUser.Email,
                Email = registerUser.Email,
                EmailConfirmed = true
            };

            UserViewModel userViewModel = new UserViewModel();
            userViewModel.Name = registerUser.Name;
            userViewModel.Email = registerUser.Email;

            await _userRepository.Add(_mapper.Map<User>(userViewModel));

            var result = await _userManager.CreateAsync(user, registerUser.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return Ok(BuildToken(user.Email));
            }

            return BadRequest();
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginUserViewModel loginUser)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _signInManager.PasswordSignInAsync(loginUser.Email, loginUser.Password, false, true);

            if (result.Succeeded)
            {
                return Ok(BuildToken(loginUser.Email));
            }

            return BadRequest();
        }

        private UserTokenViewModel BuildToken(string email)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // tempo de expiração do token: 30 min
            // parte de token ainda não implementado fica necessário a adição de parte da lógica
            var expiration = TimeZoneInfo.ConvertTime
                (DateTime.Now,
                TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")
                ).AddMinutes(30);//  30 MINUTOS

            JwtSecurityToken token = new JwtSecurityToken(
               issuer: null,
               audience: null,
               claims: claims,
               expires: expiration,
               signingCredentials: creds);

            //--------------------------------------------

            return 
                new UserTokenViewModel()
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Expiration = expiration,
                    Id = GetCurrentUserId(email),
                    Email = email
                }
            ;
        }

        private Guid GetCurrentUserId(string email)
        {
            return _userRepository.GetWhere(u => u.Email == email).Result.First().Id;
        }

    }
}
