using System.Collections.Generic;

namespace PharmacyMask.Model
{
    public class PurchaseCreateModel
    {
        public int PharmacyId { get; set; }
        public int UserId { get; set; }
        public List<PurchaseDetailModel> DetailList { get; set; }
    }
}
