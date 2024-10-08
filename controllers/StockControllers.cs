using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockControllers : ControllerBase
    {
        // To prevent it to be mutable
        private readonly IStockRepository _stockRepo; 


        // We use ApplicationDBContext to bring in our DB
        public StockControllers (ApplicationDBContext context, IStockRepository stockRepo)
        {
          _stockRepo = stockRepo; 
        }


        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] QueryObject query)
        {
          if(!ModelState.IsValid)
          {
            return BadRequest(ModelState);
          }
          
          // ToList() - Deferred execution, its going to return a list as an object, we need to get the SQL the go out the DB and get the info we need. 
          var stocks = await _stockRepo.GetAllAsync(query);
          // Select - kind of a map, return an immutable list of ToStockDto
          var stockDto = stocks.Select( s => s.ToStockDto() ); 

          return Ok(stockDto); 
        }


        // {id} - .net uses Model Binding to extract this string out, turn it into an int and 
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
          if(!ModelState.IsValid)
          {
            return BadRequest(ModelState);
          }
          
          var stock = await _stockRepo.GetByIdAsync(id); 

          if (stock == null)
          {
            return NotFound();
          }

          return Ok(stock.ToStockDto()); 
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto)
        {
          if(!ModelState.IsValid)
          {
            return BadRequest(ModelState);
          }
          
          var stockModel = stockDto.ToStockFromCreateDto(); 
          await _stockRepo.CreateAsync(stockModel);

          return CreatedAtAction(nameof(GetById), new { id = stockModel.Id}, stockModel.ToStockDto()); 
        }


        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto) {
          if(!ModelState.IsValid)
          {
            return BadRequest(ModelState);
          }
          
          var stockModel = await _stockRepo.UpdateAsync(id, updateDto);

          if (stockModel == null)
            {
              return NotFound();
            }


          return Ok(stockModel.ToStockDto());

        }



        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id) {
          if(!ModelState.IsValid)
          {
            return BadRequest(ModelState);
          }
          
          var stockModel = await _stockRepo.DeleteAsync(id);

          if (stockModel == null)
            {
              return NotFound();
            }

          return NoContent();

        }
    }
}