using PharmacyMask.Fundation.Definition.Enum;

namespace PharmacyMask.Model
{
    public class PurchaseDetailModel
    {
        public int PharmacyProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Amount { get; set; }
    }
}
