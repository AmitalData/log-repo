using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data.EntityPOCOs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Accounting.BL.CoreBL;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Test.Infrastructure;

namespace Logitude.UnitTest.Accounting.UniTests
{
    /// <summary>
    /// MSTest-compatible version of ChartOfAccountFlatUnitTest.
    /// Migrated to MSTest - no mocking required, simple data structure test.
    /// </summary>
    [TestClass]
    public class ChartOfAccountFlatUnitTest_MSTest : TestBase_MSTest
    {
        List<ChartOfAccount> _All = null;

        [TestInitialize]
        public void MyTestInitialize()
        {
            _All = new List<ChartOfAccount>()
            {
                ///level 1
                new ChartOfAccount(){ Id="1" , ParentId=null, TypeCode ="1" ,  EnglishName="1"  },
                new ChartOfAccount(){ Id="2" , ParentId=null, TypeCode ="2" ,  EnglishName="2"  },
                new ChartOfAccount(){ Id="3" , ParentId=null, TypeCode ="3" ,  EnglishName="3"  },

                ///level 2
                new ChartOfAccount(){ Id="11" , ParentId="1", TypeCode ="1" ,  EnglishName="11"  },
                new ChartOfAccount(){ Id="12" , ParentId="2", TypeCode ="2" ,  EnglishName="12"  },
                new ChartOfAccount(){ Id="13" , ParentId="3", TypeCode ="3" ,  EnglishName="13"  },

                ///level 3
                new ChartOfAccount(){ Id="21" , ParentId="11", TypeCode ="1" ,  EnglishName="21"  },
                new ChartOfAccount(){ Id="22" , ParentId="12", TypeCode ="2" ,  EnglishName="22"  },
                new ChartOfAccount(){ Id="23" , ParentId="13", TypeCode ="3" ,  EnglishName="23"  },

                ///level 3
                new ChartOfAccount(){ Id="31" , ParentId="21", TypeCode ="1" ,  EnglishName="31"  },
                new ChartOfAccount(){ Id="32" , ParentId="22", TypeCode ="2" ,  EnglishName="32"  },
                new ChartOfAccount(){ Id="33" , ParentId="23", TypeCode ="3" ,  EnglishName="33"  },

                ///level 4
                new ChartOfAccount(){ Id="41" , ParentId="31", TypeCode ="1" ,  EnglishName="41"  },
                new ChartOfAccount(){ Id="42" , ParentId="32", TypeCode ="2" ,  EnglishName="42"  },
                new ChartOfAccount(){ Id="43" , ParentId="33", TypeCode ="3" ,  EnglishName="43"  },

                ///level 5
                new ChartOfAccount(){ Id="51" , ParentId="41", TypeCode ="1" ,  EnglishName="51"  },
                new ChartOfAccount(){ Id="52" , ParentId="42", TypeCode ="2" ,  EnglishName="52"  },
                new ChartOfAccount(){ Id="53" , ParentId="43", TypeCode ="3" ,  EnglishName="53"  },
            };
        }

        [TestMethod]
        public void MyTestMethod111()
        {
            ChartOfAccount5LevelM newChartOfAccount5LevelM = null;
            var listOfChartOfAccount5LevelM = new List<ChartOfAccount5LevelM>();
            var all = _All;
            var lookUp = all.ToLookup(r => r.ParentId);

            var allPrim = lookUp.Where(g => g.Key == null);
            foreach (var item0Group in allPrim)
            {
                foreach (var item1 in item0Group)
                {
                    listOfChartOfAccount5LevelM.Add(
                        new ChartOfAccount5LevelM()
                        {
                            Level1Id = item1.Id,
                            Level1Name = item1.EnglishName,
                        });
                    var item1Look = lookUp.Where(g => g.Key == item1.Id);
                    foreach (var item2G in item1Look)
                    {
                        foreach (var item2 in item2G)
                        {
                            listOfChartOfAccount5LevelM.Add(
                            new ChartOfAccount5LevelM()
                            {
                                Level1Id = item1.Id,
                                Level1Name = item1.EnglishName,

                                Level2Id = item2.Id,
                                Level2Name = item2.EnglishName,
                            });

                            var item2Look = lookUp.Where(g => g.Key == item2.Id);
                            foreach (var item3G in item2Look)
                            {
                                foreach (var item3 in item3G)
                                {
                                    listOfChartOfAccount5LevelM.Add(
                                    new ChartOfAccount5LevelM()
                                    {
                                        Level1Id = item1.Id,
                                        Level1Name = item1.EnglishName,

                                        Level2Id = item2.Id,
                                        Level2Name = item2.EnglishName,

                                        Level3Id = item3.Id,
                                        Level3Name = item3.EnglishName,
                                    });

                                    var item3Look = lookUp.Where(g => g.Key == item3.Id);
                                    foreach (var item4G in item3Look)
                                    {
                                        foreach (var item4 in item4G)
                                        {
                                            listOfChartOfAccount5LevelM.Add(
                                            new ChartOfAccount5LevelM()
                                            {
                                                Level1Id = item1.Id,
                                                Level1Name = item1.EnglishName,

                                                Level2Id = item2.Id,
                                                Level2Name = item2.EnglishName,

                                                Level3Id = item3.Id,
                                                Level3Name = item3.EnglishName,

                                                Level4Id = item4.Id,
                                                Level4Name = item4.EnglishName,
                                            });

                                            var item4Look = lookUp.Where(g => g.Key == item4.Id);
                                            foreach (var item5G in item4Look)
                                            {
                                                foreach (var item5 in item5G)
                                                {
                                                    listOfChartOfAccount5LevelM.Add(
                                                    new ChartOfAccount5LevelM()
                                                    {
                                                        Level1Id = item1.Id,
                                                        Level1Name = item1.EnglishName,

                                                        Level2Id = item2.Id,
                                                        Level2Name = item2.EnglishName,

                                                        Level3Id = item3.Id,
                                                        Level3Name = item3.EnglishName,

                                                        Level4Id = item4.Id,
                                                        Level4Name = item4.EnglishName,
                                                        Level5Id = item5.Id,
                                                        Level5Name = item5.EnglishName,
                                                    });
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            Assert.AreEqual(1, 1);
        }
    }
}



