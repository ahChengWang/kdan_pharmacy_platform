using System;

namespace PharmacyMask.Fundation.Dao
{
    public class PharmacyDetailDao
    {
        public int Id { get; set; }
        public int PharmacyId { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan OpenTime { get; set; }
        public TimeSpan CloseTime { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateTime { get; set; }
        public string UpdateUser { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
