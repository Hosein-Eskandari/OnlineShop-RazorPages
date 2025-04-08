namespace _01_LampshadeQuery.Contracts.Product;

public class ProductPictureQueryModel
{
    public string Picture { get; set; }

    public string PictureAlt { get; set; }

    public string PictureTitle { get; set; }

    public bool IsRemoved { get; set; }

    public long ProductId { get; set; }
}