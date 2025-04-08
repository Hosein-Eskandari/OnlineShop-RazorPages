using System.Collections.Generic;

namespace _01_LampshadeQuery.Contracts.Comment;

public class CommentQueryModel
{
    public long Id { get; set; }

    public long? ParentId { get; set; }

    public string ParentName { get; set; }

    public string Name { get; set; }

    public string Message { get; set; }

    public string Website { get; set; }

    public string CreationDate { get; set; }

    public List<CommentQueryModel> Children { get; set; }
}