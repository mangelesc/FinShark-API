using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace api.controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentControllers : ControllerBase
    {
        private readonly ICommentRepository _commentRepo; 

        public CommentControllers(ApplicationDBContext context, ICommentRepository commentRepo)
        {
          _commentRepo = commentRepo; 
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
          // ToList() - Deferred execution, its going to return a list as an object, we need to get the SQL the go out the DB and get the info we need. 
          var comments = await _commentRepo.GetAllAsync();
          // Select - kind of a map, return an immutable list of ToStockDto. Return a whole new data structure instead of manipulating the actual one. 
          var commentDto = comments.Select( s => s.ToCommentDto() ); 

          return Ok(commentDto); 
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
          var comment = await _commentRepo.GetByIdAsync(id); 

          if (comment == null)
          {
            return NotFound();
          }

          return Ok(comment.ToCommentDto()); 
        }
    }
}