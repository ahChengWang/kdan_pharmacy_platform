using Microsoft.AspNetCore.Mvc;
using PharmacyMask.BackOffice.Model;
using PharmacyMask.DomainService;
using PharmacyMask.DomainService.Entity;
using PharmacyMask.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PharmacyMask.Controller
{
    [ApiController]
    [Route("[controller]/[action]")]

    public class MigrationController : ControllerBase
    {
        private readonly PharmacyDomainService _pharmacyDomainService;
        private readonly MaskService _maskDomainService;
        private readonly UserDomainService _userDomainService;

        public MigrationController(PharmacyDomainService pharmacyDomainService,
            MaskService maskDomainService,
            UserDomainService userDomainService
            )
        {
            _pharmacyDomainService = pharmacyDomainService;
            _maskDomainService = maskDomainService;
            _userDomainService = userDomainService;
        }

        [HttpPost]
        public string InsertPharmaciesMaskData(List<PharmaciesMigraModel> pharmaciesModel)
        {
            try
            {
                var pharmacyMigraResult = _pharmacyDomainService.MigrationPharmacy(
                    pharmaciesModel.Select(s =>
                    {
                        return new PharmacyMigrationEntity
                        {
                            PharmacyName = s.Name,
                            CashBalance = s.CashBalance,
                            OpeningHours = s.OpeningHours
                        };
                    }).ToList());

                var maskMigraResult = _maskDomainService.MigrationMask(
                    pharmaciesModel.SelectMany(sm =>
                        sm.Masks.Select(s =>
                        {
                            return new MaskMigrationEntity
                            {
                                PharmacyName = sm.Name,
                                MaskDesc = s.Name,
                                Price = s.Price
                            };
                        })).ToList());

                if (pharmacyMigraResult && maskMigraResult)
                    return "insert data success";
                else
                    return "insert data faile";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public string InsertUserData(List<UserMigraModel> userModel)
        {
            try
            {
                var inserUserDataResult = _userDomainService.MigrationUser(
                    userModel.Select(s =>
                    {
                        return new UserMigrationEntity
                        {
                            Name = s.Name,
                            CashBalance = s.CashBalance,
                            PurchaseHistories = s.PurchaseHistories.Select(se => new PurchaseHistoryEntity
                            {
                                PharmacyName = se.PharmacyName,
                                Remark = se.MaskName,
                                TransactionAmount = se.TransactionAmount,
                                TransactionDate = DateTime.Parse(se.TransactionDate)
                            }).ToList()
                        };
                    }).ToList());

                if (inserUserDataResult)
                    return "insert user data success.";
                else
                    return "insert user data faile.";
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
    }
}
