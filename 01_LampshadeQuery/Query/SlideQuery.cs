using System.Collections.Generic;
using System.Linq;
using _01_LampshadeQuery.Contracts.Slide;
using ShopManagement.Infrastructure.EFCore;

namespace _01_LampshadeQuery.Query;

public class SlideQuery : ISlideQuery
{
    private readonly ShopContext _context;

    public SlideQuery(ShopContext context)
    {
        _context = context;
    }

    public List<SlideQueryModel> GetSlides()
    {
        var query = _context.Slides
            .Where(x => x.IsRemoved == false)
            .Select(x => new SlideQueryModel
            {
                Picture = x.Picture,
                PictureAlt = x.PictureAlt,
                PictureTitle = x.PictureTitle,
                Heading = x.Heading,
                Title = x.Title,
                Text = x.Text,
                BtnText = x.BtnText,
                IsRemoved = x.IsRemoved,
                Link = x.Link
            });

        return query.ToList();
    }
}