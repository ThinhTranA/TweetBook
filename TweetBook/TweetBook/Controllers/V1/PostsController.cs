using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using TweetBook.Contracts.V1;
using TweetBook.Contracts.V1.Requests;
using TweetBook.Contracts.V1.Responses;
using TweetBook.Domain;
using TweetBook.Services;

namespace TweetBook.Controllers.V1
{
    public class PostsController : Controller
    {
        private readonly IPostService _postService;
      
        public PostsController(IPostService postService)
        {
            _postService = postService; 
        }
         
        [HttpGet(ApiRoutes.Posts.GetAll)]
        public IActionResult GetAll()
        {
            return Ok(_postService.GetPosts());
        }

        [HttpGet(ApiRoutes.Posts.Get)]
        public IActionResult Get([FromRoute] Guid postId)
        {
            var post = _postService.GetPostById(postId);

            if (post == null)
                return NotFound();

            return Ok(post);
        }

        [HttpPost(ApiRoutes.Posts.Create)]
        public IActionResult Create([FromBody] CreatePostRequest postRequest) //use Domain object for now
        {
            //This is to avoid mixed up Contract with Domain object, to help with backward compability in versioning
            var post = new Post { Id = postRequest.Id };

            if (post.Id != Guid.Empty)
                post.Id = Guid.NewGuid();

            _postService.GetPosts().Add(post);

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUri = baseUrl + "/" + ApiRoutes.Posts.Get.Replace("{postId}", post.Id.ToString());

            var response = new PostResponse { Id = post.Id };
            //REST: return "post" object is nice to have.
            //Return "locationUri is should have, so user can retrieve it. 
            return Created(locationUri, response);
        }
    }
}
