using PharmacyMask.Fundation.Definition.Enum;

namespace PharmacyMask.Model
{
    public class ResponseModel<T>
    {
        public ResponseCodeEnum ResponseCode { get; set; }

        public T Data { get; set; }
    }
}
