using api.Dtos.Stock;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController(IStockRepository stockRepository) : ControllerBase
    {
        // Get all stocks
        [HttpGet]
        [Route("GetAllStocks")]
        [Authorize]
        public async Task<IActionResult> GetAllStocks([FromQuery] QueryObject query)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var allStocks = await stockRepository.GetAllStocksAsync(query); 
            var allStocksDto = allStocks.Select(s => s.ToStockDto()).ToList();
           
            return Ok(allStocksDto);
        }
        
        // Get stock by id
        [HttpGet]
        [Route("GetStockById/{id:guid}")]
        [Authorize]
        public async Task<IActionResult> GetStockById([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var stock = await stockRepository.GetStockByIdAsync(id);
          
            if (stock is null)
                return NotFound("Stock not found");
           
            return Ok(stock.ToStockDto());
        }
        
        // Create a stock
        [HttpPost]
        [Route("CreateStock")]
        [Authorize]
        public async Task<IActionResult> CreateStock([FromBody] StockCreateDto createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var stock = createDto.ToCreateStockDto();
          
            await stockRepository.CreateStockAsync(stock);
         
            return CreatedAtAction(nameof(GetStockById), new {id = stock.Id}, stock.ToStockDto());
        }
        
        // Update a stock
        [HttpPut]
        [Route("UpdateStock/{id:guid}")]
        [Authorize]
        public async Task<IActionResult>  UpdateStock([FromRoute] Guid id, StockUpdateDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var stock = await stockRepository.UpdateStockAsync(id, updateDto);
           
            if (stock is null)
                return NotFound("Stock not found");
            
            return Ok(stock.ToStockDto());
        }
        
        // Delete a stock
        [HttpDelete]
        [Route("DeleteStock/{id:guid}")]
        [Authorize]
        public async Task<IActionResult> DeleteStock([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var stock = await stockRepository.DeleteStockAsync(id);
           
            if (stock is null)
                return NotFound("Stock not found");
           
            return NoContent();
        }
    }
}
