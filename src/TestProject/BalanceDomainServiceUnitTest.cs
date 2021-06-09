using ExpectedObjects;
using NUnit.Framework;
using PharmacyMask.DomainService.Entity;
using PharmacyMask.Fundation.Dao;
using PharmacyMask.Fundation.Definition.Enum;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PharmacyMask.DomainService
{
    [TestFixture]
    public class BalanceDomainServiceUnitTest
    {
        private BalanceDomainServiceForTest _fakeBalanceDomainServiceForTest;

        [SetUp]
        public void Setup()
        {
            _fakeBalanceDomainServiceForTest = new BalanceDomainServiceForTest(
             userDomainService: null,
             userBalanceRepository: null,
             userBalanceLogRepository: null,
             userTransactionHistoryRepository: null,
             pharmacyBalanceRepository: null,
             pharmacyBalanceLogRepository: null,
             maskService: null,
             pharmacyDomainService: null);
        }


        [Test(Author = "Vince", Description = "test get pharmacy product mask by a given pharmacy")]
        [TestCaseSource(typeof(BalanceParameterDataSetting), nameof(BalanceParameterDataSetting.GetUserTranHistoryData))]
        public void Get_PharmacyProductList_GivenPharmacy(
                List<int> userIdList,
                DateTime tranDateFrom,
                DateTime tranDateTo,
                List<UserTransactionHistoryEntity> expectedResult)
        {
            GivenUserList(GetUserList());
            GivenUserTransactionHistory(GetUserTransactionHistory());
            GivenMaskDetailList(GetMaskDetailList());
            GivenPharmacyInfo(GetPharmacyInfo());

            var actual = _fakeBalanceDomainServiceForTest.GetUserTranHistoryByOption(userIdList, tranDateFrom, tranDateTo);

            actual.ToExpectedObject().ShouldMatch(expectedResult);
        }


        #region arrange
        private UserEntity[] GetUserList()
        {
            return new[]
            {
                new UserEntity
                {
                    Id = 1,
                    Name = "Jo Barton"
                },
                new UserEntity
                {
                    Id = 2,
                    Name = "Mae Hill"
                }
            };
        }

        private UserTransactionHistoryDao[] GetUserTransactionHistory()
        {
            return new[]
            {
                new UserTransactionHistoryDao
                {
                    Id = 2,
                    UserId = 1,
                    PharmacyId = 1,
                    ProductTypeId = PharmacyProductTypeEnum.Mask,
                    ProductDetailId = 1,
                    Remark = "",
                    TransactionAmount = 7,
                    TransactionDate = new DateTime(2021,04,01,21,00,30)
                },
                new UserTransactionHistoryDao
                {
                    Id = 3,
                    UserId = 1,
                    PharmacyId = 1,
                    ProductTypeId = PharmacyProductTypeEnum.Mask,
                    ProductDetailId = 1,
                    Remark = "",
                    TransactionAmount = 7,
                    TransactionDate = new DateTime(2021,06,01,21,00,30)
                },
                new UserTransactionHistoryDao
                {
                    Id = 4,
                    UserId = 2,
                    PharmacyId = 1,
                    ProductTypeId = PharmacyProductTypeEnum.Mask,
                    ProductDetailId = 1,
                    Remark = "",
                    TransactionAmount = 7,
                    TransactionDate = new DateTime(2021,05,01,21,00,30)
                }
            };
        }

        private MaskDetailEntity[] GetMaskDetailList()
        {
            return new[]
            {
                new MaskDetailEntity
                {
                    MaskId = 1,
                    Name = "AniMask",
                    DetailId = 1,
                    ColorId = MaskColorEnum.green,
                    QtyPerPack = 3
                }
            };
        }

        private PharmacyEntity[] GetPharmacyInfo()
        {
            return new[]
            {
                new PharmacyEntity
                {
                    Id = 1,
                    Name = "Cash Saver Pharmacy",
                }
            };
        }


        #region Given
        private void GivenUserList(params UserEntity[] userEntity)
        {
            _fakeBalanceDomainServiceForTest.AddUserInfo(userEntity.ToList());
        }
        private void GivenUserTransactionHistory(params UserTransactionHistoryDao[] userTransactionHistoryDao)
        {
            _fakeBalanceDomainServiceForTest.AddUserTransactionHistory(userTransactionHistoryDao.ToList());
        }
        private void GivenMaskDetailList(params MaskDetailEntity[] maskDetailEntity)
        {
            _fakeBalanceDomainServiceForTest.AddMaskDetail(maskDetailEntity.ToList());
        }
        private void GivenPharmacyInfo(params PharmacyEntity[] pharmacyEntity)
        {
            _fakeBalanceDomainServiceForTest.AddPharmacyInfo(pharmacyEntity.ToList());
        }
        #endregion

        #endregion


        public class BalanceParameterDataSetting
        {
            public static object[] GetUserTranHistoryData
            {
                get
                {
                    var cases = new List<object>
                {
                    new object[]
                    {
                        null,
                        new DateTime(2021,04,01,13,10,30),
                        new DateTime(2021,06,09,18,35,30),
                        new List<UserTransactionHistoryEntity>
                        {
                            new UserTransactionHistoryEntity
                            {
                                TransactionDate = new DateTime(2021,04,01,21,00,30),
                                UserId = 1,
                                UserName = "Jo Barton",
                                PharmacyId = 1,
                                PharmacyName =  "Cash Saver Pharmacy",
                                MaskId = 1,
                                MaskName = "AniMask",
                                TransactionAmount = 7
                            },
                            new UserTransactionHistoryEntity
                            {
                                TransactionDate = new DateTime(2021,06,01,21,00,30),
                                UserId = 1,
                                UserName = "Jo Barton",
                                PharmacyId = 1,
                                PharmacyName =  "Cash Saver Pharmacy",
                                MaskId = 1,
                                MaskName = "AniMask",
                                TransactionAmount = 7
                            },
                            new UserTransactionHistoryEntity
                            {
                                TransactionDate = new DateTime(2021,05,01,21,00,30),
                                UserId = 2,
                                UserName = "Mae Hill",
                                PharmacyId = 1,
                                PharmacyName =  "Cash Saver Pharmacy",
                                MaskId = 1,
                                MaskName = "AniMask",
                                TransactionAmount = 7
                            }
                        }
                    }
                };

                    return cases.ToArray();
                }
            }
        }
    }
}