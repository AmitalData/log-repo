
using Logitude.Server.Tools;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.HybridTest
{
    class WcfServiceInvoker
    {
        public static ServiceOutcome InvokeServiceMethod(InvokedProperties serviceProperties,object[] serviceParameters)
        {
            ServiceOutcome serviceOutcome = new ServiceOutcome();
            try
            {
                //Import all contracts and endpoints
                object serviceClient = ResolveServiceClient(serviceProperties);
                var wcfService = serviceClient.GetType().GetMethod(serviceProperties.ServiceOperation);
                var innerChannel = (IClientChannel)serviceClient.GetType().GetProperty("InnerChannel").GetValue(serviceClient, null);
                using (new OperationContextScope(innerChannel))
                {
                    SetHeader(serviceProperties);
                    // Now call the service, get back response
                    serviceOutcome.Result = wcfService.Invoke(serviceClient, BindingFlags.InvokeMethod, null, serviceParameters, null);
                    if (serviceOutcome.Result is Response)
                        serviceOutcome.Response = (Response)serviceOutcome.Result;
                    else if(serviceProperties.ServiceResponseIndex > 0)
                        serviceOutcome.Response = (Response)serviceParameters[serviceProperties.ServiceResponseIndex];
                    return serviceOutcome;
                }
            }
            catch (Exception ex)
            {
                serviceOutcome.Response.HasError = true;
                serviceOutcome.Response.ErrorMessage = ex.Message;
                return serviceOutcome;
            }
        }
        
        private static object ResolveServiceClient(InvokedProperties serviceProperties)
        {
            WsdlImporter importer = ImportContractsAndEndPoints(serviceProperties);
            var contracts = importer.ImportAllContracts();
            ServiceEndpointCollection allEndpoints = importer.ImportAllEndpoints();

            //Generate type information for each contract
            ServiceContractGenerator generator = new ServiceContractGenerator();
            var endpointsForContracts = new Dictionary<string, IEnumerable<ServiceEndpoint>>();

            foreach (ContractDescription contract in contracts)
            {
                generator.GenerateServiceContractType(contract);
                endpointsForContracts[contract.Name] = allEndpoints.Where(
                    see => see.Contract.Name == contract.Name).ToList();
            }

            CompilerResults compilerResults = GetCompilerResults(generator);
            string IServiceName = GetIserviceName(serviceProperties);

            ServiceEndpoint serviceEndPoint = endpointsForContracts[IServiceName].First();
            object serviceClient = GetServiceClient(compilerResults, serviceEndPoint, serviceProperties);
            return serviceClient;
        }

        private static WsdlImporter ImportContractsAndEndPoints(InvokedProperties serviceProperties)
        {
            string uri = GetURI(serviceProperties);
            Uri mexAddress = new Uri(uri);

            MetadataExchangeClientMode mexMode = MetadataExchangeClientMode.HttpGet;
            WSHttpBinding binding = new WSHttpBinding(SecurityMode.None)
            {
                MaxReceivedMessageSize = 50000000
            };
            MetadataExchangeClient mexClient = new MetadataExchangeClient(binding)
            {
                MaximumResolvedReferences = 50000000,
                ResolveMetadataReferences = true
            };

            MetadataSet metaSet = mexClient.GetMetadata(mexAddress, mexMode);
            WsdlImporter importer = new WsdlImporter(metaSet);
            XsdDataContractImporter xsd = GetXSDDataContractImporter(serviceProperties);

            importer.State.Add(typeof(XsdDataContractImporter), xsd);
            return importer;
        }

        private static XsdDataContractImporter GetXSDDataContractImporter(InvokedProperties serviceProperties)
        {
            XsdDataContractImporter xsd = new XsdDataContractImporter
            {
                Options = new ImportOptions()
            };
            xsd.Options.ImportXmlType = true;
            xsd.Options.GenerateSerializable = true;
            xsd.Options.ReferencedTypes.Add(typeof(Response));
            if (serviceProperties.ServiceType != null)
                xsd.Options.ReferencedTypes.Add(serviceProperties.ServiceType);
            if (serviceProperties.ServiceFilterType != null)
                xsd.Options.ReferencedTypes.Add(serviceProperties.ServiceFilterType);
            return xsd;
        }

        private static string GetURI(InvokedProperties serviceProperties)
        {
            string uri = EnvironmentGlobalParams.ServerURL + "/WcfApi/" + serviceProperties.ServiceName;
            if (serviceProperties.ServiceName == "ContactPassword")
                uri += "Service.svc?wsdl";
            else if (serviceProperties.ServiceName == "Vessel")
                uri += "WcfServcie.svc?wsdl";
            else
                uri += "WcfService.svc?wsdl";
            return uri;
        }

        private static CompilerResults GetCompilerResults(ServiceContractGenerator generator)
        {
            // Generate a code file for the contracts 
            CodeDomProvider codeDomProvider = CodeDomProvider.CreateProvider("C#");

            // Compile the code file to an in-memory assembly
            // Don't forget to add all WCF-related assemblies as references
            CompilerParameters compilerParameters = new CompilerParameters(
                new string[] {
                "System.dll", "System.ServiceModel.dll",
                "System.Runtime.Serialization.dll" ,
                })
            {
                GenerateInMemory = true
            };

            CompilerResults compilerResults = codeDomProvider.CompileAssemblyFromDom(compilerParameters, generator.TargetCompileUnit);
            return compilerResults;
        }

        private static object GetServiceClient(CompilerResults compilerResults, ServiceEndpoint serviceEndPoint, InvokedProperties serviceProperties)
        {
            string IServiceName = GetIserviceName(serviceProperties);

            Type clientProxyType = compilerResults.CompiledAssembly.GetTypes().FirstOrDefault(
                     t => t.IsClass &&
                         t.GetInterface(IServiceName) != null &&
                         t.GetInterface(typeof(System.ServiceModel.ICommunicationObject).Name) != null);
            
            object serviceClient = compilerResults.CompiledAssembly.CreateInstance(
                    clientProxyType.Name,
                    false,
                    System.Reflection.BindingFlags.CreateInstance,
                    null,
                    new object[] { serviceEndPoint.Binding, serviceEndPoint.Address },
                    CultureInfo.CurrentCulture, null);

            return serviceClient;
        }

        private static string GetIserviceName(InvokedProperties serviceProperties)
        {
            string IServiceName = "I" + serviceProperties.ServiceName;
            if (serviceProperties.ServiceName == "ContactPassword")
                IServiceName += "Service";
            else if (serviceProperties.ServiceName == "Vessel")
                IServiceName += "WcfServcie";
            else
                IServiceName += "WcfService";
            return IServiceName;
        }

        private static void SetHeader(InvokedProperties serviceProperties)
        {
            if (serviceProperties.SecondaryToken == null)
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", EnvironmentGlobalParams.MainTenantToken);
            else
                System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", EnvironmentGlobalParams.SecondaryTenantToken);
        }

    }
    public class InvokedProperties
    {
        public string ServiceName { get; set; }
        public string ServiceOperation { get; set; }
        public int ServiceResponseIndex { get; set; }
        public Type ServiceType { get; set; }
        public Type ServiceFilterType { get; set; }
        public string SecondaryToken { get; set; }
    }

    public class ServiceOutcome
    {
        public object Result { get; set; }
        public Response Response { get; set; }
    }
}
