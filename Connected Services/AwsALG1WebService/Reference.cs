//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SIT323AssessmentTask2.AwsALG1WebService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="AwsALG1WebService.IService")]
    public interface IService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/GetAllocations", ReplyAction="http://tempuri.org/IService/GetAllocationsResponse")]
        string[] GetAllocations(ConfigurationDataLibrary.ConfigurationData configData);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IService/GetAllocations", ReplyAction="http://tempuri.org/IService/GetAllocationsResponse")]
        System.Threading.Tasks.Task<string[]> GetAllocationsAsync(ConfigurationDataLibrary.ConfigurationData configData);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IServiceChannel : SIT323AssessmentTask2.AwsALG1WebService.IService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ServiceClient : System.ServiceModel.ClientBase<SIT323AssessmentTask2.AwsALG1WebService.IService>, SIT323Assignment1.AwsALG1WebService.IService {
        
        public ServiceClient() {
        }
        
        public ServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string[] GetAllocations(ConfigurationDataLibrary.ConfigurationData configData) {
            return base.Channel.GetAllocations(configData);
        }
        
        public System.Threading.Tasks.Task<string[]> GetAllocationsAsync(ConfigurationDataLibrary.ConfigurationData configData) {
            return base.Channel.GetAllocationsAsync(configData);
        }
    }
}
