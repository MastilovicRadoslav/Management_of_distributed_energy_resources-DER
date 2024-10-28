using Common.Models;
using System.ServiceModel;

namespace Common.Interfaces
{
    [ServiceContract]
    public interface IDERService
    {
        [OperationContract]
        void RegisterResource(DERResource resource); // Registruje novi distribuirani resurs (DER) na serveru.

        [OperationContract]
        ResourceSchedule GetSchedule(int resourceId); // Dohvata plan rada (raspored) za određeni resurs na osnovu ID-ja.

        [OperationContract]
        void LogProduction(int resourceId, double producedEnergy); // Beleži količinu proizvedene energije za dati resurs.
                                                                   
        [OperationContract] 
        void SetSchedule(ResourceSchedule schedule); // Nova metoda za postavljanje rasporeda
    }
}
