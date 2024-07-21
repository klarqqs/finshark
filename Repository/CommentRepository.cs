using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository;

public class CommentRepository(ApplicationDBContext context) : ICommentRepository
{
    public async Task<List<Comment>> GetAllCommentsAsync()
    {
        return await context.Comments.Include(c => c.AppUser).ToListAsync();
    }

    public async Task<Comment?> GetCommentById(Guid id)
    {
        return await context.Comments.Include(c => c.AppUser).FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<Comment> CreateCommentAsync(Comment comment)
    {
        await context.Comments.AddAsync(comment);
        await context.SaveChangesAsync();
        return comment;
    }

    public async Task<Comment?> UpdateCommentAsync(Guid id, Comment comment)
    {
        var existingComment = await context.Comments.FindAsync(id);
        if (existingComment is null)
            return null;
        existingComment.Title = comment.Title;
        existingComment.Content = comment.Content;
        await context.SaveChangesAsync();
        return existingComment;

    }

    public async Task<Comment?> DeleteCommentAsync(Guid id)
    {
        var comment = await context.Comments.FirstOrDefaultAsync(x => x.Id == id);
        if (comment == null)
            return null;
        context.Comments.Remove(comment);
        await context.SaveChangesAsync();
        return comment;
    }
}