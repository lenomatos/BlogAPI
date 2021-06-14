using AutoMapper;
using Blog.Api.ViewModels;
using Blog.Business.Interfaces;
using Blog.Business.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("album")]
    [ApiController]
    public class AlbumsController : ControllerBase
    {
        private readonly IAlbumRepository _albumRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;

        public AlbumsController(IAlbumRepository albumRepository,
            IUserRepository userRepository,
            IMapper mapper,
            UserManager<IdentityUser> userManager)
        { 
            _albumRepository = albumRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IEnumerable<AlbumViewModel>> GetAlbums()
        {
            return _mapper.Map<IEnumerable<AlbumViewModel>>(await _albumRepository.GetAll());
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<Album>> GetAlbum(Guid id)
        {
            var album = await _albumRepository.Get(id);

            if (album == null)
            {
                return NotFound();
            }

            return album;
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutAlbum(Guid id, AlbumViewModel albumViewModel)
        {
            if (id != albumViewModel.Id)
            {
                return BadRequest();
            }

            try
            {
                var album = _mapper.Map<Album>(albumViewModel);
                album.UserId = FindUserId();
                await _albumRepository.Update(album);
            }
            catch (DbUpdateConcurrencyException)
            {
                var album = await _albumRepository.Get(id);
                if (album == null)
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
        public async Task<ActionResult<Album>> PostAlbum(AlbumViewModel albumViewModel)
        {
            var album = _mapper.Map<Album>(albumViewModel);
            album.UserId = FindUserId();
            await _albumRepository.Add(album);
            return CreatedAtAction("GetAlbum", new { id = album.Id }, album);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Album>> DeleteAlbum(Guid id)
        {
            var album = await _albumRepository.Get(id);
            if (album == null)
            {
                return NotFound();
            }

            await _albumRepository.Remove(id);

            return album;
        }

        private async Task<string> GetCurrentUserEmail()
        {
            IdentityUser applicationUser = await _userManager.GetUserAsync(User);
            return applicationUser?.Email;
        }

        private Guid GetCurrentUserId(string email)
        {
            return _userRepository.GetWhere(u => u.Email == email).Result.First().Id;
        }
        private Guid FindUserId()
        {
            return GetCurrentUserId(GetCurrentUserEmail().Result);
        }

    }
}
