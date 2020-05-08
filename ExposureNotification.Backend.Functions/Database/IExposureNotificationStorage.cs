using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExposureNotification.Backend
{
    public interface IExposureNotificationStorage
    {
        Task AddDiagnosisUidsAsync(IEnumerable<string> diagnosisUids);
        Task<bool> CheckIfDiagnosisUidExistsAsync(string diagnosisUid);
        void DeleteAllKeysAsync();
        Task<ExposureNotificationStorage.KeysResponse> GetKeysAsync(Int64 since, int skip = 0, int take = 1000);
        Task RemoveDiagnosisUidsAsync(IEnumerable<string> diagnosisUids);
        Task SubmitPositiveDiagnosisAsync(ExposureNotificationStorage.SelfDiagnosisSubmissionRequest diagnosis);
    }
}