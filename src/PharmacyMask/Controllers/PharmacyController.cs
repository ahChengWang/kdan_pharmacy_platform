using Mapster;
using Microsoft.AspNetCore.Mvc;
using PharmacyMask.DomainService;
using PharmacyMask.Fundation.Definition.Enum;
using PharmacyMask.Fundation.Helper;
using PharmacyMask.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PharmacyMask.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class PharmacyController : ControllerBase
    {
        private readonly PharmacyDomainService _pharmacyDomainService;

        public PharmacyController(PharmacyDomainService pharmacyDomainService)
        {
            _pharmacyDomainService = pharmacyDomainService;
        }

        /// <summary>
        /// 1. List all pharmacies that are open at a certain datetime
        /// 2. List all pharmacies that are open on a day of the week, at a certain time
        /// </summary>
        /// <param name="dayOfWeek">
        /// DayOfWeek eunm Sunday = 0,Monday = 1,Tuesday = 2,Wednesday = 3,Thursday = 4,Friday = 5,Saturday = 6 <br/>
        /// 不代參數, return 1. List all pharmacies that are open at a certain datetime <br/>
        /// 代入指定星期 id (多日查詢 id 逗號串聯[0,1,..]), return 2. List all pharmacies that are open on a day of the week, at a certain time
        /// </param>
        /// <response code="100">success</response>
        /// <response code="101">fail</response>
        [HttpGet]
        public ResponseModel<List<PharmacyOpenModel>> GetOpenTime([FromQuery] string dayOfWeek)
        {
            try
            {
                return new ResponseModel<List<PharmacyOpenModel>>
                {
                    ResponseCode = ResponseCodeEnum.Success,
                    Data = _pharmacyDomainService
                        .GetOpenDayTime(dayOfWeek.ToListByCutString<DayOfWeek>())
                        .Select(s => s.Adapt<PharmacyOpenModel>()).ToList()
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Edit pharmacy name
        /// </summary>
        /// <param name="pharmacyEditModel"></param>
        /// <response code="100">success</response>
        /// <response code="101">fail</response>
        [HttpPut]
        public ResponseModel<string> UpdatePharmacy(PharmacyEditModel pharmacyEditModel)
        {
            try
            {
                var updResult = _pharmacyDomainService.UpdatePharmacyName(pharmacyEditModel.PharmacyId, pharmacyEditModel.PharmacyName);

                if (updResult)
                    return new ResponseModel<string>
                    {
                        ResponseCode = ResponseCodeEnum.Success,
                        Data = "update pharmacy success."
                    };
                else
                    return new ResponseModel<string>
                    {
                        ResponseCode = ResponseCodeEnum.Fail,
                        Data = "update pharmacy fail."
                    };
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
