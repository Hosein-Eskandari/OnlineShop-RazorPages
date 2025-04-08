using System.ComponentModel.DataAnnotations;
using _0_Framework.Application;

namespace CommentManagement.Application.Contracts.Comment;

public class AddComment
{
    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    [StringLength(100, ErrorMessage = ValidationMessages.OutOfRange)]
    public string Name { get; set; }

    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    [StringLength(500, ErrorMessage = ValidationMessages.OutOfRange)]
    public string Email { get; set; }

    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    [StringLength(500, ErrorMessage = ValidationMessages.OutOfRange)]
    public string Website { get; set; }

    [Required(ErrorMessage = ValidationMessages.IsRequired)]
    [StringLength(1000, ErrorMessage = ValidationMessages.OutOfRange)]
    public string Message { get; set; }

    public long OwnerRecordId { get; set; }

    public int Type { get; set; }

    public long? ParentId { get; set; }
}