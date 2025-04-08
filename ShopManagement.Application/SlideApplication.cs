using System.Collections.Generic;
using _0_Framework.Application;
using ShopManagement.Application.Contracts.Slide;
using ShopManagement.Domain.SlideAgg;

namespace ShopManagement.Application;

public class SlideApplication : ISlideApplication
{
    private readonly IFileUploader _fileUploader;
    private readonly ISlideRepository _slideRepository;

    public SlideApplication(ISlideRepository slideRepository, IFileUploader fileUploader)
    {
        _slideRepository = slideRepository;
        _fileUploader = fileUploader;
    }

    public OperationResult Create(CreateSlide command)
    {
        var operation = new OperationResult();


        var pictureName = _fileUploader.Upload(command.Picture, "Slides");


        var slide = new Slide(pictureName, command.PictureTitle, command.PictureAlt,
            command.Heading, command.Title, command.Text, command.BtnText, command.Link);

        _slideRepository.Create(slide);
        _slideRepository.SaveChanges();
        return operation.Succeeded();
    }

    public OperationResult Edit(EditSlide command)
    {
        var operation = new OperationResult();
        var slide = _slideRepository.Get(command.Id);
        if (slide == null) return operation.Failed(ApplicationMessages.RecordNotFound);

        var pictureName = _fileUploader.Upload(command.Picture, "Slides");


        slide.Edit(pictureName, command.PictureTitle, command.PictureAlt,
            command.Heading, command.Title, command.Text, command.BtnText, command.Link);

        _slideRepository.SaveChanges();
        return operation.Succeeded();
    }

    public EditSlide GetDetails(long id)
    {
        return _slideRepository.GetDetails(id);
    }

    public List<SlideViewModel> GetList()
    {
        return _slideRepository.GetList();
    }

    public OperationResult Remove(long id)
    {
        var operation = new OperationResult();
        var slide = _slideRepository.Get(id);
        if (slide == null) return operation.Failed(ApplicationMessages.RecordNotFound);
        slide.Remove();
        _slideRepository.SaveChanges();
        return operation.Succeeded();
    }

    public OperationResult Restore(long id)
    {
        var operation = new OperationResult();
        var slide = _slideRepository.Get(id);
        if (slide == null) return operation.Failed(ApplicationMessages.RecordNotFound);
        slide.Restore();
        _slideRepository.SaveChanges();
        return operation.Succeeded();
    }
}