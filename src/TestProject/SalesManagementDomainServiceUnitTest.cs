using ExpectedObjects;
using NUnit.Framework;
using PharmacyMask.DomainService.Entity;
using PharmacyMask.Fundation.Definition.Enum;
using System.Collections.Generic;
using System.Linq;

namespace PharmacyMask.DomainService
{
    [TestFixture]
    public class SalesManagementDomainServiceUnitTest
    {
        private SalesManagementDomainServiceForTest _fakeSalesManagementDomainServiceForTest;

        [SetUp]
        public void Setup()
        {

            _fakeSalesManagementDomainServiceForTest = new SalesManagementDomainServiceForTest(
            maskRepository: null,
            maskInfoRepository: null,
            pharmacyProductRepository: null,
            pharmacyRepository: null,
            maskDomainService: null,
            productDomainService: null
            );
        }


        [Test(Author = "Vince", Description = "test get pharmacy product mask by a given pharmacy")]
        [TestCaseSource(typeof(PharmacyAndMaskDataSetting), nameof(PharmacyAndMaskDataSetting.GetPharmacyProductMaskListByGivenPharmacyName))]
        public void Get_PharmacyProductList_GivenPharmacy(
                ProductSearchEntity searchEntity,
                List<PharmacyEntity> pharmacyListEntity,
                List<PharmacyProductMaskEntity> expectedResult)
        {
            GivenMaskDetail(GetMaskDetail());
            GivenPharmacyProduct(GetPharmacyProductByGivenPharmacy());

            var actual = _fakeSalesManagementDomainServiceForTest.GetPharmacyProductMaskList(searchEntity, pharmacyListEntity);

            actual.ToExpectedObject().ShouldMatch(expectedResult);
        }


        [Test(Author = "Vince", Description = "test get pharmacy product mask by a given mask name")]
        [TestCaseSource(typeof(PharmacyAndMaskDataSetting), nameof(PharmacyAndMaskDataSetting.GetPharmacyProductMaskList))]
        public void Get_PharmacyProductList_GivenMaskName(
                ProductSearchEntity searchEntity,
                List<PharmacyEntity> pharmacyListEntity,
                List<PharmacyProductMaskEntity> expectedResult)
        {
            GivenMaskDetail(GetMaskDetail());
            GivenPharmacyProduct(GetPharmacyProduct());

            var actual = _fakeSalesManagementDomainServiceForTest.GetPharmacyProductMaskList(searchEntity, pharmacyListEntity);

            actual.ToExpectedObject().ShouldMatch(expectedResult);
        }


        [Test(Author = "Vince", Description = "test get pharmacy product mask summary")]
        [TestCaseSource(typeof(PharmacyAndMaskDataSetting), nameof(PharmacyAndMaskDataSetting.GetPharmacyProductMaskList))]
        public void Get_PharmacyProductList_MaskSummary(
                ProductSearchEntity searchEntity,
                List<PharmacyEntity> pharmacyListEntity,
                List<PharmacyProductMaskEntity> expectedResult)
        {
            GivenMaskDetail(GetMaskDetail());
            GivenPharmacyProduct(GetPharmacyProduct());

            var actual = _fakeSalesManagementDomainServiceForTest.GetPharmacyProductMaskList(searchEntity, pharmacyListEntity);

            actual.ToExpectedObject().ShouldMatch(expectedResult);
        }


        #region arrange
        private MaskDetailEntity[] GetMaskDetail()
        {
            return new[]
            {
                new MaskDetailEntity
                {
                    MaskId = 1,
                    Name = "Free to Roam",
                    DetailId = 1,
                    ColorId = MaskColorEnum.blue,
                    QtyPerPack = 10
                },
                new MaskDetailEntity
                {
                    MaskId = 2,
                    Name = "Second Smile",
                    DetailId = 2,
                    ColorId = MaskColorEnum.black,
                    QtyPerPack = 6
                },
                new MaskDetailEntity
                {
                    MaskId = 2,
                    Name = "Second Smile",
                    DetailId = 3,
                    ColorId = MaskColorEnum.green,
                    QtyPerPack = 9
                }
            };
        }

        private PharmacyProductEntity[] GetPharmacyProductByGivenPharmacy()
        {
            return new[]
            {
                new PharmacyProductEntity
                {
                    Id = 1,
                    PharmacyId = 1,
                    ProductTypeId = PharmacyProductTypeEnum.Mask,
                    ProductDetailId = 1,
                    ProductName = "Free to Roam",
                    Price = 3,
                },
                new PharmacyProductEntity
                {
                    Id = 2,
                    PharmacyId = 1,
                    ProductTypeId = PharmacyProductTypeEnum.Mask,
                    ProductDetailId = 2,
                    ProductName = "Second Smile",
                    Price = 7,
                }
            };
        }

        private PharmacyProductEntity[] GetPharmacyProduct()
        {
            return new[]
            {
                new PharmacyProductEntity
                {
                    Id = 1,
                    PharmacyId = 1,
                    ProductTypeId = PharmacyProductTypeEnum.Mask,
                    ProductDetailId = 2,
                    ProductName = "Second Smile",
                    Price = 3,
                },
                new PharmacyProductEntity
                {
                    Id = 2,
                    PharmacyId = 2,
                    ProductTypeId = PharmacyProductTypeEnum.Mask,
                    ProductDetailId = 2,
                    ProductName = "Second Smile",
                    Price = 7,
                },
                new PharmacyProductEntity
                {
                    Id = 3,
                    PharmacyId = 3,
                    ProductTypeId = PharmacyProductTypeEnum.Mask,
                    ProductDetailId = 3,
                    ProductName = "Second Smile",
                    Price = 11,
                }
            };
        }


        #region Given
        private void GivenMaskDetail(params MaskDetailEntity[] maskDetailEntity)
        {
            _fakeSalesManagementDomainServiceForTest.AddMsakDetailList(maskDetailEntity.ToList());
        }
        private void GivenPharmacyProduct(params PharmacyProductEntity[] pharmacyProductEntity)
        {
            _fakeSalesManagementDomainServiceForTest.AddPharmacyProduct(pharmacyProductEntity.ToList());
        }
        #endregion

        #endregion


        public class PharmacyAndMaskDataSetting
        {
            public static object[] GetPharmacyProductMaskListByGivenPharmacyName
            {
                get
                {
                    var cases = new List<object>
                {
                    new object[]
                    {
                        new ProductSearchEntity
                        {
                            ProductName = null,
                            PriceFrom = null,
                            PriceTo = null
                        },
                        new List<PharmacyEntity>
                        {
                            new PharmacyEntity
                            {
                                Id = 1,
                                Name = "Better You"
                            }
                        },
                        new List<PharmacyProductMaskEntity>
                        {
                            new PharmacyProductMaskEntity
                            {
                                Id = 1,
                                PharmacyId = 1,
                                PharmacyName = "Better You",
                                MaskId = 1,
                                MaskName = "Free to Roam",
                                ColorId = MaskColorEnum.blue,
                                Color = MaskColorEnum.blue.ToString(),
                                Price = 3,
                                QtyPerPack = 10
                            },
                            new PharmacyProductMaskEntity
                            {
                                Id = 2,
                                PharmacyId = 1,
                                PharmacyName = "Better You",
                                MaskId = 2,
                                MaskName = "Second Smile",
                                ColorId = MaskColorEnum.black,
                                Color = MaskColorEnum.black.ToString(),
                                Price = 7,
                                QtyPerPack = 6
                            }
                        }
                    }
                };

                    return cases.ToArray();
                }
            }

            public static object[] GetPharmacyProductMaskList
            {
                get
                {
                    var cases = new List<object>
                {
                    new object[]
                    {
                        new ProductSearchEntity
                        {
                            ProductName = "Second Smile",
                            PriceFrom = null,
                            PriceTo = null
                        },
                        new List<PharmacyEntity>
                        {
                            new PharmacyEntity
                            {
                                Id = 1,
                                Name = "PrecisionMed"
                            },
                            new PharmacyEntity
                            {
                                Id = 2,
                                Name = "Pill Pack"
                            },
                            new PharmacyEntity
                            {
                                Id = 3,
                                Name = "Neighbors"
                            }
                        },
                        new List<PharmacyProductMaskEntity>
                        {
                            new PharmacyProductMaskEntity
                            {
                                Id = 1,
                                PharmacyId = 1,
                                PharmacyName = "PrecisionMed",
                                MaskId = 2,
                                MaskName = "Second Smile",
                                ColorId = MaskColorEnum.black,
                                Color = MaskColorEnum.black.ToString(),
                                Price = 3,
                                QtyPerPack = 6
                            },
                            new PharmacyProductMaskEntity
                            {
                                Id = 2,
                                PharmacyId = 2,
                                PharmacyName = "Pill Pack",
                                MaskId = 2,
                                MaskName = "Second Smile",
                                ColorId = MaskColorEnum.black,
                                Color = MaskColorEnum.black.ToString(),
                                Price = 7,
                                QtyPerPack = 6
                            },
                            new PharmacyProductMaskEntity
                            {
                                Id = 3,
                                PharmacyId = 3,
                                PharmacyName = "Neighbors",
                                MaskId = 2,
                                MaskName = "Second Smile",
                                ColorId = MaskColorEnum.green,
                                Color = MaskColorEnum.green.ToString(),
                                Price = 11,
                                QtyPerPack = 9
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