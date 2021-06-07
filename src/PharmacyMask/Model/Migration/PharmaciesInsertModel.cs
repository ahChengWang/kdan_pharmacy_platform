using System.Collections.Generic;

namespace PharmacyMask.Model
{
    public class PharmaciesMigraModel
    {
        public string Name { get; set; }
        public decimal CashBalance { get; set; }
        public string OpeningHours { get; set; }
        public List<MasksModel> Masks { get; set; }
    }
}
