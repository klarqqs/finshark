using api.Dtos.Comment;
using api.Models;

namespace api.Interfaces;

public interface ICommentRepository
{
    Task<List<Comment>> GetAllCommentsAsync();
    Task<Comment?> GetCommentById(Guid id);
    Task<Comment> CreateCommentAsync(Comment comment);
    Task<Comment?> UpdateCommentAsync(Guid id, Comment comment);
    Task<Comment?> DeleteCommentAsync(Guid id);
}