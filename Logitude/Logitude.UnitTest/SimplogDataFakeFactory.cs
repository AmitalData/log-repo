using FakeItEasy;
using Logitude.Server.Tools;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Logitude.UnitTest
{
    public class SimplogDataFakeFactory
    {
       
        public Action<IObjectTableRepository> IObjectTableRepository_ReturnObjectTable_Id1
        {
            get
            {
                return (fake) =>
                    {
                        //A.CallTo(() => fake.GetObjectTableById(A<string>.Ignored, A<int>.Ignored)).ReturnsLazily((string id, int tenant1) => { return new ObjectTable() { Id="journal" }; });

                        A.CallTo(fake).WithReturnType<ObjectTable>().Returns(new ObjectTable() { Id = "" });
                    };
            }
        }

    }
}
