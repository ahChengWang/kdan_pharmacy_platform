using PharmacyMask.Fundation.Definition.Enum;

namespace PharmacyMask.BackOffice.Model
{
    public class PharmacyMaskSearchModel
    {
        /// <summary>
        /// 1. 後端出 pharmay option 列出所有藥局及對應 id, 多選 id逗號串聯 
        /// 2. 利用字串 模糊查詢 (V)
        /// </summary>
        public OrderTermEnum SearchTermId { get; set; }
        public string PharmacyName { get; set; }
        public string MaskName { get; set; }
        public decimal? PriceFrom { get; set; }
        public decimal? PriceTo { get; set; }
        public OrderTermEnum OrderTermId { get; set; }
    }
}
