using Microsoft.AspNetCore.Mvc;
using MISA.QLTS.API.Controllers;
using MISA.QLTS.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Test
{
    public class Tests
    {
        internal class test
        {
            [Test]
            public void PostAsset_ValidInput_ReturnCorectResult()
            {

                //Arange -- chuẩn bị dl đầu vào và kq mong muốn
                var newEmployeeId = Guid.NewGuid();
                var newasset = new Asset
                {
                    FixedAssetId = newEmployeeId,
                    FixedAssetCode = "TS10000",
                    FixedAssetName = "Sarasota Bradenton Intl",
                    DepartmentId = Guid.NewGuid(),
                    FixedAssetCategoryId = Guid.NewGuid(),
                    PurchaseDate = DateTime.Now,
                    Cost = 38000000,
                    Quantity = 5,
                    DepreciationRate = 2,
                    TrackedYear = 2022,
                    LifeTime = 10,
                    ProductionYear = 2022,
                    CreatedBy = "Võ Viên Đức",
                    CreatedDate = DateTime.Now,
                    ModifiedBy = "Đào Việt Duyên",
                    ModifiedDate = DateTime.Now
                };

                string expectResult = "11326c16-72e5-61b3-82c2-579daad24557";
                var fixedAsset = new AssetsController();

                //Act -- Gọi vào hàm cần test
                ObjectResult actualResult = (ObjectResult)fixedAsset.InsertAsset(newasset);

                //Assert -- k tra kq mong muốn và kết quả thực tế
                Assert.AreEqual(expectResult, actualResult.Value);
            }
        }
    }
}