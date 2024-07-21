using api.Data;
using api.Dtos.Comment;
using api.Extensions;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers

{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController(ICommentRepository commentRepository, IStockRepository stockRepository, UserManager<AppUser> userManager) : ControllerBase
    {
        // Get all comments
        [HttpGet]
        [Route("GetAllComments")]
        [Authorize]
        public async Task<IActionResult> GetAllComments()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
           
            var allComments = await commentRepository.GetAllCommentsAsync();
            var allCommentsDto = allComments.Select(s => s.ToCommentDto());
           
            return Ok(allCommentsDto);
        }
        
        // Get comment by id
        [HttpGet]
        [Route("GetCommentById/{id:guid}")]
        [Authorize]
        public async Task<IActionResult> GetCommentById([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
           
            var stock = await commentRepository.GetCommentById(id);
           
            if (stock is null)
                return NotFound("Comment not found");
            
            return Ok(stock.ToCommentDto());
        }
        
        // Create a comment
        [HttpPost]
        [Route("CreateComment/{stockId:guid}")]
        [Authorize]
        public async Task<IActionResult> CreateComment([FromRoute] Guid stockId , [FromBody] CommentCreateDto createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            if (!await stockRepository.StockExists(stockId))
                return BadRequest("Stock does not exist");

            var username = User.GetUsername();
            var appUser = await userManager.FindByNameAsync(username);
           
            var comment = createDto.ToCreateCommentDto(stockId);
            comment.AppUserId = appUser.Id;
            await commentRepository.CreateCommentAsync(comment);
            
            return CreatedAtAction(nameof(GetCommentById), new { id = comment.Id }, comment.ToCommentDto());
        }
        
        // Update a comment
        [HttpPut]
        [Route("UpdateComment/{id:guid}")]
        [Authorize]
        public async Task<IActionResult> UpdateComment([FromRoute] Guid id, [FromBody] CommentUpdateDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
           
            var comment = await commentRepository.UpdateCommentAsync(id, updateDto.ToUpdateCommentDto());
            
            if (comment == null)
                return NotFound("Comment not found");
            
            return Ok(comment.ToCommentDto());
        }
        
        // Delete a comment
        [HttpDelete]
        [Route("DeleteComment/{id:guid}")]
        [Authorize]
        public async Task<IActionResult> DeleteComment([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var comment = await commentRepository.DeleteCommentAsync(id);
            
            if (comment is null) 
                return NotFound("Comment not found");
            
            return NoContent();
        }
    }
}
