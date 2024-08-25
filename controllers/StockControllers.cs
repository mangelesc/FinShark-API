using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace api.controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockControllers : ControllerBase
    {
        // To prevent it to be mutable
        private readonly ApplicationDBContext _context;

        // We use ApplicationDBContext to bring in our DB
        public StockControllers (ApplicationDBContext context)
        {
          _context = context; 
        }

        [HttpGet]
        public IActionResult GetAll()
        {
          // ToList() - Deferred execution, its going to return a list as an object, we need to get the SQL the go out the DB and get the info we need. 
          var stocks = _context.Stocks.ToList()
          // Select - kind of a map, return an immutable list of ToStockDto
          .Select( s => s.ToStockDto() ); 

          return Ok(stocks); 
        }

        // {id} - .net uses Model Binding to extract this string out, turn it into an int and 
        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
          var stock = _context.Stocks.Find(id); 

          if (stock == null)
          {
            return NotFound();
          }

          return Ok(stock.ToStockDto()); 
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateStockRequestDto stockDto)
        {
          var stockModel = stockDto.ToStockFromCreateDto(); 
          _context.Stocks.Add(stockModel); 
          _context.SaveChanges(); 
          return CreatedAtAction(nameof(GetById), new { id = stockModel.Id}, stockModel.ToStockDto()); 
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto) {
          var stockModel = _context.Stocks.FirstOrDefault (x => x.Id == id); 

          if (stockModel == null)
            {
              return NotFound();
            }

          stockModel.Symbol= updateDto.Symbol; 
          stockModel.CompanyName= updateDto.CompanyName; 
          stockModel.Purchase= updateDto.Purchase; 
          stockModel.LastDiv= updateDto.LastDiv; 
          stockModel.Industry= updateDto.Industry; 
          stockModel.MarketCap= updateDto.MarketCap; 

          _context.SaveChanges();

          return Ok(stockModel.ToStockDto());

        }


        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete([FromRoute] int id) {
          var stockModel = _context.Stocks.FirstOrDefault (x => x.Id == id); 

          if (stockModel == null)
            {
              return NotFound();
            }

          _context.Stocks.Remove(stockModel); 
          _context.SaveChanges();

          return NoContent();

        }
    }
}