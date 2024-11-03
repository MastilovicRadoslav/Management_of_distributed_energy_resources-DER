using Common.Models;
using System.Collections.Generic;
using System.ServiceModel;

namespace Common.Interfaces
{
    [ServiceContract]
    public interface IDERService
    {
        [OperationContract]
        string RegisterResource(int resourceId); // Aktivira resurs i vraća informacije za ispis

        [OperationContract]
        string UnregisterResource(int resourceId); // Deaktivira resurs i vraća informacije za ispis

        [OperationContract]
        DERResource RegisterNewResource(DERResource resource);

        [OperationContract]
        List<ResourceInfo> GetResourceStatus(); // Vraća sve informacije o resursima

        [OperationContract]

        void ClearAllResources();
        [OperationContract]

        Statistics GetStatistics();


    }
}
