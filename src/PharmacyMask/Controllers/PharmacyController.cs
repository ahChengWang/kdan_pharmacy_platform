using Mapster;
using Microsoft.AspNetCore.Mvc;
using PharmacyMask.DomainService;
using PharmacyMask.Fundation.Helper;
using PharmacyMask.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PharmacyMask.Controller
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
        /// 不代參數, return 1.
        /// </param>
        /// <returns></returns>
        [HttpGet]
        public List<PharmacyOpenModel> GetOpenTime([FromQuery] string dayOfWeek)
        {
            try
            {
                return _pharmacyDomainService
                        .GetOpenDayTime(dayOfWeek.ToListByCutString<DayOfWeek>())
                        .Select(s => s.Adapt<PharmacyOpenModel>()).ToList();
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
        [HttpPut]
        public string UpdatePharmacy(PharmacyEditModel pharmacyEditModel)
        {
            try
            {
                var updResult = _pharmacyDomainService.UpdatePharmacyName(pharmacyEditModel.PharmacyId, pharmacyEditModel.PharmacyName);

                if (updResult)
                    return "update pharmacy success.";
                else
                    return "update pharmacy fail.";
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
