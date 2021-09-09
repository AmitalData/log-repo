using Logitude.FullAccounting.Test.Models;
using Logitude.TimeManagementTests.Models.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Logitude.FullAccounting.Test.Services
{
    public class JournalService
    {
        internal List<JournalLinePM> CreateLines(Table table)
        {
            var lines = new List<JournalLinePM>();
            var linsSet = table.CreateDynamicSet();
            foreach (var line in linsSet)
            {
                lines.Add(CreateLine(line));
            }
            return lines;
        }

        private JournalLinePM CreateLine(dynamic line)
        {
            return new JournalLinePMBuilder().WithDefualtValues()
                .Line()
        }
    }
}
