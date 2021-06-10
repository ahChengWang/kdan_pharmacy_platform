using PharmacyMask.Fundation.Definition.Enum;

namespace PharmacyMask.BackOffice.Model
{
    public class PharmacyMaskSearchModel
    {
        /// <summary>
        /// SearchTermId (查詢項目) 1:PharmacyName, 2:MaskName
        /// </summary>
        public OrderTermEnum SearchTermId { get; set; }
        public string PharmacyName { get; set; }
        public string MaskName { get; set; }
        public decimal? PriceFrom { get; set; }
        public decimal? PriceTo { get; set; }
        /// <summary>
        /// OrderTermId (排序項目) 1:PharmacyName, 2:MaskName, 3:MaskPrice
        /// </summary>
        public OrderTermEnum OrderTermId { get; set; }
    }
}
