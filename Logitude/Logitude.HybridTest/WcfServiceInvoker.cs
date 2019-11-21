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
        public static object InvokeServiceMethod(InvokedProperties serviceProperties,object[] serviceParameters, Type entityType, ref Response response)
        {
            try
            {
                //Import all contracts and endpoints
                WsdlImporter importer = ImportContractsAndEndPoints(serviceProperties, entityType);
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
                ServiceEndpoint serviceEndPoint = endpointsForContracts[serviceProperties.IServiceName].First();
                object serviceClient = GetServiceClient(compilerResults, serviceEndPoint, serviceProperties);

                var wcfService = serviceClient.GetType().GetMethod(serviceProperties.ServiceOperation);

                var innerChannel = (IClientChannel)serviceClient.GetType().GetProperty("InnerChannel").GetValue(serviceClient, null);
                using (new OperationContextScope(innerChannel))
                {
                    System.ServiceModel.Web.WebOperationContext.Current.OutgoingRequest.Headers.Add("Token", TestEnvironmentGlobalParameters.Token);
                    // Now call the service, get back response
                    var serviceresults = wcfService.Invoke(serviceClient, BindingFlags.InvokeMethod, null, serviceParameters, null);
                    //response = (Response)retVal;
                    return serviceresults;
                }
            }
            catch (Exception ex)
            {
                response.Result = ex.Message;
                return null;
            }
        }

        public static WsdlImporter ImportContractsAndEndPoints(InvokedProperties serviceProperties, Type entityType)
        {
            Uri mexAddress = new Uri(TestEnvironmentGlobalParameters.ServerURL + "/WcfApi/" + serviceProperties.ServiceName + "WcfService.svc?wsdl");
            MetadataExchangeClientMode mexMode = MetadataExchangeClientMode.HttpGet;

            // Get Metadata file from service
            MetadataExchangeClient mexClient = new MetadataExchangeClient(mexAddress, mexMode)
            {
                ResolveMetadataReferences = true
            };
            MetadataSet metaSet = mexClient.GetMetadata(mexAddress, mexMode);

            WsdlImporter importer = new WsdlImporter(metaSet);
            XsdDataContractImporter xsd = new XsdDataContractImporter
            {
                Options = new ImportOptions()
            };
            xsd.Options.ImportXmlType = true;
            xsd.Options.GenerateSerializable = true;
            xsd.Options.ReferencedTypes.Add(entityType); 
            xsd.Options.ReferencedTypes.Add(typeof(Response));
            xsd.Options.ReferencedTypes.Add(typeof(ApiSearchFilters));

            importer.State.Add(typeof(XsdDataContractImporter), xsd);
            return importer;
        }

        public static CompilerResults GetCompilerResults(ServiceContractGenerator generator)
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

        public static object GetServiceClient(CompilerResults compilerResults, ServiceEndpoint serviceEndPoint, InvokedProperties parameters)
        {
            Type clientProxyType = compilerResults.CompiledAssembly.GetTypes().FirstOrDefault(
                     t => t.IsClass &&
                         t.GetInterface(parameters.IServiceName) != null &&
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

    }
    public class InvokedProperties
    {
        public string ServiceName { get; set; }
        public string IServiceName { get; set; }
        public string ServiceOperation { get; set; }
    }
}
