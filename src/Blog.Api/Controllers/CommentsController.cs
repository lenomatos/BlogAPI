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
    [Route("comment")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;

        public CommentsController(ICommentRepository commentRepository,
            IUserRepository userRepository,
            IMapper mapper,
            UserManager<IdentityUser> userManager)
        {
            _commentRepository = commentRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IEnumerable<CommentViewModel>> GetComments()
        {
            return _mapper.Map<IEnumerable<CommentViewModel>>(await _commentRepository.GetAll());
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<Comment>> GetComment(Guid id)
        {
            var comment = await _commentRepository.Get(id);

            if (comment == null)
            {
                return NotFound();
            }

            return comment;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutComment(Guid id, CommentViewModel commentViewModel)
        {
            if (id != commentViewModel.Id)
            {
                return BadRequest();
            }

            try
            {
                var comment = _mapper.Map<Comment>(commentViewModel);
                comment.UserId = FindUserId();
                await _commentRepository.Update(comment);
            }
            catch (DbUpdateConcurrencyException)
            {
                var comment = await _commentRepository.Get(id);
                if (comment == null)
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
        public async Task<ActionResult<Comment>> PostComment(CommentViewModel commentViewModel)
        {
            var comment = _mapper.Map<Comment>(commentViewModel);
            comment.UserId = FindUserId();
            await _commentRepository.Add(comment);
            return CreatedAtAction("GetComment", new { id = comment.Id }, comment);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Comment>> DeleteComment(Guid id)
        {
            var comment = await _commentRepository.Get(id);
            if (comment == null)
            {
                return NotFound();
            }

            await _commentRepository.Remove(id);

            return comment;
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
