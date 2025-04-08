using System.Collections.Generic;
using _0_Framework.Application;
using ShopManagement.Application.Contracts.Product;
using ShopManagement.Domain.ProductAgg;
using ShopManagement.Domain.ProductCategoryAgg;

namespace ShopManagement.Application;

public class ProductApplication : IProductApplication
{
    private readonly IFileUploader _fileUploader;
    private readonly IProductCategoryRepository _productCategoryRepository;
    private readonly IProductRepository _productRepository;

    public ProductApplication(IProductRepository productRepository, IFileUploader fileUploader,
        IProductCategoryRepository productCategoryRepository)
    {
        _productRepository = productRepository;
        _fileUploader = fileUploader;
        _productCategoryRepository = productCategoryRepository;
    }

    public OperationResult Create(CreateProduct entity)
    {
        var operation = new OperationResult();

        if (_productRepository.Exists(x => x.Name == entity.Name))
            return operation.Failed(ApplicationMessages.DuplicatedRecord);

        var slug = entity.Slug.Slugify();

        var categorySlug = _productCategoryRepository.GetSlugById(entity.CategoryId);

        var path = $"{categorySlug}/{slug}";
        var picturePath = _fileUploader.Upload(entity.Picture, path);

        var product = new Product(entity.Name, entity.Code, entity.ShortDescription,
            entity.Description, picturePath, entity.PictureAlt, entity.PictureTitle,
            entity.CategoryId, slug, entity.Keywords, entity.MetaDescription);

        _productRepository.Create(product);
        _productRepository.SaveChanges();
        return operation.Succeeded();
    }

    public OperationResult Edit(EditProduct command)
    {
        var operation = new OperationResult();
        var product = _productRepository.GetProductWithCategory(command.Id);

        if (product == null) return operation.Failed(ApplicationMessages.RecordNotFound);


        if (_productRepository.Exists(x => x.Name == command.Name && x.Id != command.Id))
            return operation.Failed(ApplicationMessages.IsExisted);

        var slug = command.Slug.Slugify();


        var path = $"{product.Category.Slug}/{slug}";
        var picturePath = _fileUploader.Upload(command.Picture, path);

        product.Edit(command.Name, command.Code, command.ShortDescription,
            command.Description, picturePath, command.PictureAlt, command.PictureTitle,
            command.CategoryId, slug, command.Keywords, command.MetaDescription);

        _productRepository.SaveChanges();
        return operation.Succeeded();
    }

    public EditProduct GetDetails(long id)
    {
        return _productRepository.GetDetails(id);
    }

    public List<ProductViewModel> Search(ProductSearchModel search)
    {
        return _productRepository.Search(search);
    }


    public List<ProductViewModel> GetProducts()
    {
        return _productRepository.GetProducts();
    }
}