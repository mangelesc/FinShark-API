using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Comment;
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
        private readonly IStockRepository _stockRepo; 


        public CommentControllers(ICommentRepository commentRepo, IStockRepository stockRepo)
        {
          _commentRepo = commentRepo; 
          _stockRepo = stockRepo; 
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
          
          // ModelState - To perform validations implemented in ours DTOs
          // ModelState is's inheriting from ControllerBase
          if(!ModelState.IsValid)
          {
            return BadRequest(ModelState);
          }

          // ToList() - Deferred execution, its going to return a list as an object, we need to get the SQL the go out the DB and get the info we need. 
          var comments = await _commentRepo.GetAllAsync();
          // Select - kind of a map, return an immutable list of ToStockDto. Return a whole new data structure instead of manipulating the actual one. 
          var commentDto = comments.Select( s => s.ToCommentDto() ); 

          return Ok(commentDto); 
        }


        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
          if(!ModelState.IsValid)
          {
            return BadRequest(ModelState);
          }

          var comment = await _commentRepo.GetByIdAsync(id); 

          if (comment == null)
          {
            return NotFound();
          }

          return Ok(comment.ToCommentDto()); 
        }


        [HttpPost("{stockId:int}")]
        public async Task<IActionResult> Create([FromRoute] int stockId, CreateCommentRequestDto commentDto)
        {
          if(!ModelState.IsValid)
          {
            return BadRequest(ModelState);
          }

          if (!await _stockRepo.StockExists(stockId))
          {
            return BadRequest("Stock does not exist");
          }

          var commentModel = commentDto.ToCommentFromCreateDto(stockId); 

          await _commentRepo.CreateAsync(commentModel);

          // nameof(GetById) 
          // new { id = commentModel} -> find the actual comment
          // commentModel.ToCommentDto() -> so it returns it into the form that we want
          return CreatedAtAction(nameof(GetById), new { id = commentModel.Id}, commentModel.ToCommentDto()); 
        }


        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentRequestDto updateDto) {
          if(!ModelState.IsValid)
          {
            return BadRequest(ModelState);
          }
          
          var commentModel = await _commentRepo.UpdateAsync(id, updateDto);

          if (commentModel == null)
            {
              return NotFound();
            }

          return Ok(commentModel.ToCommentDto());
        }


        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id) {
          if(!ModelState.IsValid)
          {
            return BadRequest(ModelState);
          }

          var commentModel = await _commentRepo.DeleteAsync(id);

          if (commentModel == null)
            {
              return NotFound();
            }

          return NoContent();
        }
    }
}