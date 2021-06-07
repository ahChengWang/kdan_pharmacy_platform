using System;

namespace PharmacyMask.DomainService.Entity
{
    public class PharmacyOpenTimeEntity
    {
        public int PharmacyId { get; set; }
        public string PharmacyName { get; set; }
        public DayOfWeek DayOfWeekId { get; set; }
        public string DayOfWeek { get; set; }
        public string OpenTime { get; set; }
        public string CloseTime { get; set; }
    }
}
