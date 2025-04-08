using System.Collections.Generic;
using System.Linq;
using _0_Framework.Application;
using _0_Framework.Infrastructure;
using ShopManagement.Application.Contracts.Slide;
using ShopManagement.Domain.SlideAgg;

namespace ShopManagement.Infrastructure.EFCore.Repository;

public class SlideRepository : RepositoryBase<long, Slide>, ISlideRepository
{
    private readonly ShopContext _context;

    public SlideRepository(ShopContext context) : base(context)
    {
        _context = context;
    }

    public EditSlide GetDetails(long id)
    {
        return _context.Slides.Select(x => new EditSlide
        {
            Id = x.Id,
            PictureAlt = x.PictureAlt,
            PictureTitle = x.PictureTitle,
            Heading = x.Heading,
            Title = x.Title,
            Text = x.Text,
            BtnText = x.BtnText,
            Link = x.Link
        }).FirstOrDefault(x => x.Id == id);
    }

    public List<SlideViewModel> GetList()
    {
        var query = _context.Slides.Select(x => new SlideViewModel
        {
            Id = x.Id,
            Picture = x.Picture,
            Heading = x.Heading,
            Title = x.Title,
            IsRemoved = x.IsRemoved,
            CreationDate = x.CreationDate.ToFarsiFull()
        });

        return query.OrderByDescending(x => x.Id).ToList();
    }
}