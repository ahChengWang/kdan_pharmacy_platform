using PharmacyMask.Fundation.Definition.Enum;
using System;

namespace PharmacyMask.Fundation.Dao
{
    public class MaskDetailDao
    {
        public int Id { get; set; }
        public int MaskId { get; set; }
        public MaskColorEnum ColorId { get; set; }
        public int QtyPerPack { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateTime { get; set; }
        public string UpdateUser { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
