using System;
using CS321_W5D2_BlogAPI.ApiModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CS321_W5D2_BlogAPI.Core.Services;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CS321_W5D2_BlogAPI.Controllers
{

    [Route("api/[controller]")]
    public class PostsController : Controller
    {

        private readonly IPostService _postService;

        public PostsController(IPostService postService)
        {
            _postService = postService;
        }

        // GET /api/blogs/{blogId}/posts
        [HttpGet("/api/blogs/{blogId}/posts")]
        public IActionResult Get(int blogId)
        {
           try
            {
                var allBlogs = _postService
                    .GetBlogPosts(blogId)
                    .ToApiModels();
                return Ok(allBlogs);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("GetPosts", ex.Message);
                return BadRequest(ModelState);
            }
        }

        //get post by id
        //allow anyone to get, even if not logged in
        // GET api/blogs/{blogId}/posts/{postId}
        [HttpGet("/api/blogs/{blogId}/posts/{postId}")]
        public IActionResult Get(int blogId, int postId)
        {
            try
            {
                var singlePost = _postService
                    .GetBlogPosts(blogId)
                    .ToApiModels().FirstOrDefault(p=> p.Id == postId);
                return Ok(singlePost);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("GetPost", ex.Message);
                return BadRequest(ModelState);
            }

        }

        // add a new post to blog
        // POST /api/blogs/{blogId}/post
        [HttpPost("/api/blogs/{blogId}/posts")]
        public IActionResult Post(int blogId, [FromBody]PostModel postModel)
        {
            try
            {
                _postService.Add(postModel.ToDomainModel());
                return Ok(postModel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("AddPost", "Fix Me! Implement POST /api/blogs{blogId}/posts");
                return BadRequest(ModelState);
            }

        }

        // PUT /api/blogs/{blogId}/posts/{postId}
        [HttpPut("/api/blogs/{blogId}/posts/{postId}")]
        public IActionResult Put(int blogId, int postId, [FromBody]PostModel postModel)
        {
            try
            {
                var updatedPost = _postService.Update(postModel.ToDomainModel());
                return Ok(updatedPost);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("UpdatePost", ex.Message);
                return BadRequest(ModelState);
            }
        }


        // DELETE /api/blogs/{blogId}/posts/{postId}
        [HttpDelete("/api/blogs/{blogId}/posts/{postId}")]
        public IActionResult Delete(int blogId, int postId)
        {
            try
            {
                _postService.Remove(postId);
                return Ok(_postService.Get(blogId));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("DeletePost", "Fix Me! Implement DELETE /api/blogs{blogId}/posts/{postId}");
                return BadRequest(ModelState);
            }
          
        }
    }
}
