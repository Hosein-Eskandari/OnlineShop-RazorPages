using System.Collections.Generic;
using System.Linq;
using _0_Framework.Application;
using _0_Framework.Infrastructure;
using CommentManagement.Application.Contracts.Comment;
using CommentManagement.Domain.CommentAgg;

namespace CommentManagement.Infrastructure.EFCore.Repository;

public class CommentRepository : RepositoryBase<long, Comment>, ICommentRepository
{
    private readonly CommentContext _context;

    public CommentRepository(CommentContext context) : base(context)
    {
        _context = context;
    }

    public List<CommentViewModel> Search(CommentSearchModel searchModel)
    {
        var query = _context.Comments
            .Select(x => new CommentViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Email = x.Email,
                Website = x.Website,
                Message = x.Message,
                OwnerRecordId = x.OwnerRecordId,
                Type = x.Type,
                IsCanceled = x.IsCanceled,
                IsConfirmed = x.IsConfirmed,
                CreationDate = x.CreationDate.ToFarsiFull()
            });

        if (!string.IsNullOrWhiteSpace(searchModel.Name)) query = query.Where(x => x.Name.Contains(searchModel.Name));

        if (!string.IsNullOrWhiteSpace(searchModel.Email))
            query = query.Where(x => x.Email.Contains(searchModel.Email));

        //var result = query.OrderByDescending(x => x.Id).ToList();

        var result = query.ToList();

        return result;
    }
}