using Logitude.Accounting.Def.EntityPMs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Logitude.Test.Accounting
{
    [TestClass]
    public class JournalLinePMExtTests
    {
        [TestMethod]
        public void ActionTypeCodeEnumSetter_WithDebit_SetsCodeAndName()
        {
            var line = new JournalLinePM();

            line.ActionTypeCodeEnum_Setter(JournalActionTypeEnum.Debit);

            Assert.AreEqual("2", line.ActionTypeCode);
            Assert.AreEqual("Debit", line.ActionName);
        }

        [TestMethod]
        public void ActionTypeCodeEnumSetter_WithNotValid_ClearsFields()
        {
            var line = new JournalLinePM
            {
                ActionTypeCode = "1",
                ActionName = "Credit"
            };

            line.ActionTypeCodeEnum_Setter(JournalActionTypeEnum.NotValid);

            Assert.AreEqual(string.Empty, line.ActionTypeCode);
            Assert.AreEqual(string.Empty, line.ActionName);
        }

        [TestMethod]
        public void ActionTypeCodeEnumGetter_WhenCodeExists_ReturnsEnum()
        {
            var line = new JournalLinePM
            {
                ActionTypeCode = "1"
            };

            var result = line.ActionTypeCodeEnum_Getter();

            Assert.AreEqual(JournalActionTypeEnum.Credit, result);
        }
    }
}

