using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.ContainerTracking
{
    public class ContainerTrackingHelper
    {
        private PortRepository portRepository;
        private int tenant;

        public ContainerTrackingHelper(int tenant)
        {
            this.tenant = tenant;
            this.portRepository = new PortRepository(tenant);
        }

        public bool IsSameLocation(string entityPortId, string responsePortCode)
        {
            Port responsePort = GetPortByCode(responsePortCode, tenant);
            Port responsePort_zero = GetPortByCode(responsePortCode, 0);
            Port entityPort = GetPortById(entityPortId, tenant);
            Port entityPort_Zero = GetPortByCode(entityPort?.CombinedCode, 0);

            if (responsePort == null) return false;

            if (string.IsNullOrEmpty(entityPortId))
                return true;

            else if (entityPortId == responsePort.Id)
                return true;

            else if (responsePort_zero?.PortGroupId != null && entityPort_Zero?.PortGroupId != null && responsePort_zero?.PortGroupId == entityPort_Zero?.PortGroupId)
                return true;

            return false;
        }

        private Port GetPortByCode(string portCode, int tenant)
        {
            return portRepository.GetOceanPortByCombinedCode(portCode, tenant);
        }
        private Port GetPortById(string portId, int tenant)
        {
            return portRepository.GetSinglePort(portId, tenant);
        }
    }
}