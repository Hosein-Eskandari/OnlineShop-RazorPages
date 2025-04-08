using System.Collections.Generic;
using _0_Framework.Application;
using CommentManagement.Application.Contracts.Comment;
using CommentManagement.Domain.CommentAgg;

namespace CommentManagement.Application;

public class CommentApplication : ICommentApplication
{
    private readonly ICommentRepository _commentRepository;

    public CommentApplication(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    public OperationResult Add(AddComment command)
    {
        var operation = new OperationResult();

        var comment = new Comment(command.Name, command.Email, command.Website, command.Message,
            command.OwnerRecordId, command.Type, command.ParentId);

        if (_commentRepository.Exists(x => x.Message == command.Message && x.Name == command.Name &&
                                           x.ParentId == command.ParentId))
            return operation.Failed(ApplicationMessages.DuplicatedRecord);

        _commentRepository.Create(comment);
        _commentRepository.SaveChanges();
        return operation.Succeeded();
    }

    public OperationResult Confirm(long id)
    {
        var operation = new OperationResult();

        var comment = _commentRepository.Get(id);
        if (comment == null) return operation.Failed(ApplicationMessages.RecordNotFound);

        comment.Confirm();
        _commentRepository.SaveChanges();
        return operation.Succeeded();
    }

    public OperationResult Cancel(long id)
    {
        var operation = new OperationResult();

        var comment = _commentRepository.Get(id);
        if (comment == null) return operation.Failed(ApplicationMessages.RecordNotFound);

        comment.Cancel();
        _commentRepository.SaveChanges();
        return operation.Succeeded();
    }

    public List<CommentViewModel> Search(CommentSearchModel searchModel)
    {
        return _commentRepository.Search(searchModel);
    }
}