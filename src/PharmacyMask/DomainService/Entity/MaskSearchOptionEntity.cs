using System.Collections.Generic;

namespace PharmacyMask.DomainService.Entity
{
    public class MaskSearchOptionEntity
    {
        public List<int> MaskIdList { get; set; }
        public List<int> MaskDetailIdList { get; set; }
        public string MaskName { get; set; }
    }
}
