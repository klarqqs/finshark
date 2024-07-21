using api.Dtos.Comment;
using api.Models;

namespace api.Mappers;

public static class CommentMappers
{
    public static CommentDto ToCommentDto(this Comment commentModel)
    {
        return new CommentDto
        {
            Id = commentModel.Id,
            Title = commentModel.Title,
            Content = commentModel.Content,
            CreatedOn = commentModel.CreatedOn,
            CreatedBy = commentModel.AppUser.UserName,
            StockId = commentModel.StockId
        };
    }
    
    public static Comment ToCreateCommentDto(this CommentCreateDto commentCreateDto, Guid stockId )
    {
        return new Comment
        {
            Title = commentCreateDto.Title,
            Content = commentCreateDto.Content,
            StockId = stockId
        };
    }
    
    public static Comment ToUpdateCommentDto(this CommentUpdateDto commentUpdateDto )
    {
        return new Comment
        {
            Title = commentUpdateDto.Title,
            Content = commentUpdateDto.Content,
        };
    }
}

