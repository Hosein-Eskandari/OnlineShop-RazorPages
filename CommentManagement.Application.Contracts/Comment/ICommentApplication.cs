﻿using System.Collections.Generic;
using _0_Framework.Application;

namespace CommentManagement.Application.Contracts.Comment;

public interface ICommentApplication
{
    OperationResult Add(AddComment command);

    OperationResult Confirm(long id);

    OperationResult Cancel(long id);

    List<CommentViewModel> Search(CommentSearchModel searchModel);
}