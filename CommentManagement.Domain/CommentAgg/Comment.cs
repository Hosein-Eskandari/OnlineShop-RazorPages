﻿using System.Collections.Generic;
using _0_Framework.Domain;

namespace CommentManagement.Domain.CommentAgg;

public class Comment : EntityBase
{
    public Comment(string name, string email, string website, string message,
        long ownerRecordId, int type, long? parentId)
    {
        Name = name;
        Email = email;
        Website = website;
        Message = message;
        OwnerRecordId = ownerRecordId;
        Type = type;
        ParentId = parentId;
    }

    protected Comment()
    {
    }

    public string Name { get; private set; }

    public string Email { get; private set; }

    public string? Website { get; private set; }

    public string Message { get; private set; }

    public bool IsConfirmed { get; private set; }

    public bool IsCanceled { get; private set; }

    public long OwnerRecordId { get; private set; }

    public int Type { get; private set; }


    public long? ParentId { get; private set; }

    public virtual Comment? Parent { get; }


    public virtual ICollection<Comment> Children { get; private set; } = new List<Comment>();


    public void Cancel()
    {
        IsCanceled = true;
    }

    public void Confirm()
    {
        IsConfirmed = true;
    }
}