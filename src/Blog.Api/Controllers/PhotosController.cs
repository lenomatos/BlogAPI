using AutoMapper;
using Blog.Api.ViewModels;
using Blog.Business.Interfaces;
using Blog.Business.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("[controller]")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private readonly IPhotoRepository _photoRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;
        public PhotosController(IPhotoRepository photoRepository,
            IUserRepository userRepository,
            IMapper mapper,
            UserManager<IdentityUser> userManager)
        {
            _photoRepository = photoRepository;
            _userRepository = userRepository;
            _userManager = userManager;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IEnumerable<PhotoViewModel>> GetPhotos()
        {
            return _mapper.Map<IEnumerable<PhotoViewModel>>(await _photoRepository.GetAll());
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<PhotoViewModel>> GetPhoto(Guid id)
        {
            var photoViewModel = _mapper.Map<PhotoViewModel>(await _photoRepository.Get(id));
            var pathFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\uploads", photoViewModel.Imagem);
            Byte[] bytes = System.IO.File.ReadAllBytes(pathFile);
            photoViewModel.ImagemUpload = Convert.ToBase64String(bytes);
            if (photoViewModel == null)
            {
                return NotFound();
            }

            return photoViewModel;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPhoto(Guid id, PhotoViewModel photoViewModel)
        {
            if (id != photoViewModel.Id)
            {
                return BadRequest();
            }

            try
            {

                var imgName = Guid.NewGuid() + "_" + photoViewModel.Imagem;
                if (!UploadFile(photoViewModel.ImagemUpload, imgName))
                {
                    return BadRequest();
                }

                photoViewModel.Imagem = imgName;
                await _photoRepository.Update(_mapper.Map<Photo>(photoViewModel));
            }
            catch (DbUpdateConcurrencyException)
            {
                var photo = await _photoRepository.Get(photoViewModel.Id);

                if (photo == null)
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
        public async Task<ActionResult<PhotoViewModel>> PostPhoto(PhotoViewModel photoViewModel)
        {
            var imgName = Guid.NewGuid() + "_" + photoViewModel.Imagem;
            if (!UploadFile(photoViewModel.ImagemUpload, imgName))
            {
                return BadRequest();
            }

            photoViewModel.Imagem = imgName;
            var photo = _mapper.Map<Photo>(photoViewModel);

            await _photoRepository.Add(photo);
            return CreatedAtAction("GetPhoto", new { id = photo.Id }, photo);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Photo>> DeletePhoto(Guid id)
        {
            var photo = await _photoRepository.Get(id);

            if (photo == null)
            {
                return NotFound();
            }

            await _photoRepository.Remove(id);

            return photo;
        }

        private bool UploadFile(string file, string imgName)
        {
            var arrayDataByte64 = Convert.FromBase64String(file);

            if (string.IsNullOrEmpty(file)) return false;

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", imgName);

            if (System.IO.File.Exists(path))
            {
                return false;
            }

            System.IO.File.WriteAllBytes(path, arrayDataByte64);

            return true;
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
