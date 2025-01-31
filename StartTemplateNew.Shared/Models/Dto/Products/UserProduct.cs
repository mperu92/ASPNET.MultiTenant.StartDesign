using StartTemplateNew.Shared.Models.Dto.Identity;

namespace StartTemplateNew.Shared.Models.Dto.Products
{
    public class UserProduct
    {
        public UserProduct() { }
        public UserProduct(Guid userId, Guid productId)
        {
            UserId = userId;
            ProductId = productId;
        }

        public Guid UserId { get; set; }
        public User? User { get; set; }
        public Guid ProductId { get; set; }
        public Product? Product { get; set; }
    }
}
