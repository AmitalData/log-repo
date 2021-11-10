using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CommunicationWorkerRole
{
    public class UpdatePortTimeZoneWR : WorkerEntryPoint
    {
        DbQueueService queueservice;
        string queueName = "UpdatePortTimeZone";
        private string portId;
        private int tenant;
        private QueueResponse response;
        private Port tenantZeroPort;
        private ICommonDataContext commonDataContext;
        private PortRepository portRepository;
        public override void Run()
        {
            while (IsRunning)
            {
                this.RunWorkerRole();                
            }
        }

        public override bool OnStart()
        {
            ConnectClient();
            ServicePointManager.DefaultConnectionLimit = 12;
            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "UpdatePortTimeZoneWR";
            return base.OnStart();
        }

        public void ConnectClient()
        {
            try
            {
                queueservice = new DbQueueService(queueName, 0);
            }

            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 1, null, "Connect client method", null, null);
            }
        }
        private void RunWorkerRole()
        {
            if (!General.IsUpdating())
            {
                this.Start();                
            }
            else
            {
                Thread.Sleep(60000);
            }
        }
        private void Start()
        {
            try
            {
                queueservice = new DbQueueService(queueName, 0);
                response = queueservice.Receive(new TimeSpan(0, 0, 0, 10));

                if (response.MessageId != null)
                {
                    this.HandleQueueResponse();                    
                }
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
            }
        }
        private void HandleQueueResponse()
        {
            this.ReadParametersFromResponse();
            this.Initialize();
            this.StartUpdatingProcess();
            queueservice.Complete();
        }
        private void ReadParametersFromResponse()
        {
            this.tenant = int.Parse(response.MessageValues["Tenant"].ToString());
            this.portId = response.MessageValues["PortId"].ToString();
        }
        private void Initialize()
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
            portRepository = new PortRepository(commonDataContext);
            tenantZeroPort = this.GetPortById();
        }
        private Port GetPortById()
        {
            return portRepository.GetSinglePort(portId, tenant);
        }
        private void StartUpdatingProcess()
        {
            List<Port> similarPorts = this.GetSimilarPortsFromOtherTenants();

            foreach (Port port in similarPorts)
            {
                this.UpdatePortTimeZone(port);                
            }

            this.Save();
            this.RefreshPortsCache(similarPorts);
        }

        private List<Port> GetSimilarPortsFromOtherTenants()
        {
            return (from port in commonDataContext.Ports where port.CombinedCode == tenantZeroPort.CombinedCode && port.Tenant != 0 select port).ToList();
        }
        private void UpdatePortTimeZone(Port port)
        {
            port.PortTimeZoneCode = tenantZeroPort.PortTimeZoneCode;
            portRepository.Update(port);
        }
        private void Save()
        {
            portRepository.SubmitChanges();
        }
        private void RefreshPortsCache(List<Port> similarPorts)
        {
            foreach (Port port in similarPorts)
            {
                TableLastUpdateClass.UpdateTableHistory(port.Tenant, "Port");
            }
        }
        private void HandleException(Exception ex)
        {
            queueservice.CompleteAsFailed();
            ConnectClient();
            ExceptionHandler.HandleException(ex, DateTime.Now, 1, null, "CToolLookups worker role start", null, null);
            Thread.Sleep(10000);
        }
    }
}
