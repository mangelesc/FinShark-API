using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
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
          var stocks = _context.Stocks.ToList(); 

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

          return Ok(stock); 
        }

    }
}