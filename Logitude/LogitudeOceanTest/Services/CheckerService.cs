using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using Logitude.OceanTest.Models;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;

namespace Logitude.OceanTest.Services
{
    public class CheckerService
    {
        public void CheckShipmentContainerSimulatorIfHasError(ShipmentContainerSimulator shipmentContainerSimulator)
        {
            shipmentContainerSimulator.Errors.Should().BeNullOrEmpty(string.Join(", ", shipmentContainerSimulator.Errors));
        }

        public void CheckContainerStatuses(ShipmentContainerSimulator shipmentContainerSimulator)
        {
            CheckShipmentContainerSimulatorIfHasError(shipmentContainerSimulator);
            var tryEvreySecound = 4;
            var tineLifeInSecound = 60 * 5;
            var isDone = Waiter.RunAndWait(tryEvreySecound, tineLifeInSecound, () => CheckIfCommunicationLogsAddSuccessfullyForContener());
            isDone.Should().BeTrue();
        }
        public void CheckShipmentContainersStatuses(ShipmentContainerSimulator shipmentContainerSimulator)
        {
            CheckShipmentContainerSimulatorIfHasError(shipmentContainerSimulator);
            var tryEvreySecound = 4;
            var tineLifeInSecound = 60 * 3;
            var isDone = Waiter.RunAndWait(tryEvreySecound, tineLifeInSecound, () => CheckIfCommunicationLogsAddSuccessfullyForShipment());
            isDone.Should().BeTrue();

        }

        public bool ValidateCommunicationLogs(List<CommunicationLogList> communicationLogs)
        {
            if (communicationLogs == null || communicationLogs.Count <= 0)
                return false;

            var lastCommunicationLog = communicationLogs.First();
            if (lastCommunicationLog.CommunicationStatusTypeCode != "D")
                return false;

            if (lastCommunicationLog.From != "Amital")
                return false;

            return true;
        }
    }
}
