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
        void RegisterNewResource(DERResource resource); // UserClient za dodavanje novog resursa

        [OperationContract]
        List<ResourceInfo> GetResourceStatus(); // Vraća sve informacije o resursima

        [OperationContract]
        ResourceSchedule GetSchedule(int resourceId); // Dohvata plan rada (raspored) za određeni resurs na osnovu ID-ja.

        [OperationContract]

        void ClearAllResources();

    }
}
