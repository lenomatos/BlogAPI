using AutoMapper;
using Blog.Api.ViewModels;
using Blog.Business.Interfaces;
using Blog.Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Blog.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("post")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;

        public PostsController(IPostRepository postRepository,
            IUserRepository userRepository,
            IMapper mapper,
            UserManager<IdentityUser> userManager)
        {
            _postRepository = postRepository;
            _userRepository = userRepository;
            _userManager = userManager;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IEnumerable<PostViewModel>> GetPosts()
        {
            return _mapper.Map<IEnumerable<PostViewModel>>(await _postRepository.GetAll());
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> GetPost(Guid id)
        {
            var post = await _postRepository.Get(id);

            if (post == null)
            {
                return NotFound();
            }

            return post;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPost(Guid id, PostViewModel postViewModel)
        {
            if (id != postViewModel.Id)
            {
                return BadRequest();
            }

            try
            {
                var post = _mapper.Map<Post>(postViewModel);
                post.UserId = FindUserId();
                await _postRepository.Update(post);
            }
            catch (DbUpdateConcurrencyException)
            {
                var post = await _postRepository.Get(id);
                if (post == null)
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
        public async Task<ActionResult<Post>> PostPost(PostViewModel postViewModel)
        {
            var post = _mapper.Map<Post>(postViewModel);
            post.UserId = FindUserId();
            await _postRepository.Add(post);
            return CreatedAtAction("GetPost", new { id = post.Id }, post);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Post>> DeletePost(Guid id)
        {
            var post = await _postRepository.Get(id);
            if (post == null)
            {
                return NotFound();
            }

            await _postRepository.Remove(id);

            return post;
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
