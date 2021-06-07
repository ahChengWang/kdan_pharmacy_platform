namespace PharmacyMask.DomainService.Entity
{
    public class ProductSearchEntity
    {
        public string ProductName { get; set; }
        public decimal? PriceFrom { get; set; }
        public decimal? PriceTo { get; set; }
    }
}
