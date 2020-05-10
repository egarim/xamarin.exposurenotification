using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Data.Filtering;
using DevExpress.Xpo;

namespace ExposureNotification.Backend.Functions.Xpo
{
    public class XpoExposureNotificationStorage : IExposureNotificationStorage
    {

        public XpoExposureNotificationStorage(string ConnectionString)
        {
            XpoHelper.InitXpo(ConnectionString);
        }
        public async Task AddDiagnosisUidsAsync(IEnumerable<string> diagnosisUids)
        {
            try
            {
                var UoW = XpoHelper.CreateUnitOfWork();
                foreach (var d in diagnosisUids)
                {
                    if (!(await UoW.Query<DbDiagnosis>().AnyAsync(r => r.DiagnosisUid == d)))
                    {
                        UoW.Save(new DbDiagnosis(d));

                    }

                }
                if (UoW.InTransaction)
                    UoW.CommitChanges();

            }
            catch (Exception ex)
            {
                var message = ex.Message;
                throw ex;
            }
           
         
        }

        public Task<bool> CheckIfDiagnosisUidExistsAsync(string diagnosisUid)
        {
            using (var UoW = XpoHelper.CreateUnitOfWork())
                return Task.FromResult(UoW.Query<DbDiagnosis>().Any(d => d.DiagnosisUid.Equals(diagnosisUid)));
        }

        public void DeleteAllKeysAsync()
        {
            using (var UoW = XpoHelper.CreateUnitOfWork())
            {
                var ToDelete=UoW.Query<DbDiagnosis>().ToList();
                foreach (var item in ToDelete)
                {
                    UoW.Delete(item);
                }
                
            }
        }

        public async Task<KeysResponse> GetKeysAsync(Int64 since, int skip = 0, int take = 1000)
        {
            using(var UoW = XpoHelper.CreateUnitOfWork())
            {
                var oldest = DateTimeOffset.UtcNow.AddDays(-14).ToUnixTimeSeconds();

                var results = await UoW.Query<DbTemporaryExposureKey>()
                    .Where(dtk => dtk.Id > since
                        && dtk.TimestampSecondsSinceEpoch >= oldest)
                    .OrderBy(dtk => dtk.Id)
                    .Skip(skip)
                    .Take(take)
                    .ToListAsync().ConfigureAwait(false);

                var newestIndex = results
                    .LastOrDefault()?.Id;

                var keys = results.Select(dtk => dtk.ToKey());

                return new KeysResponse
                {
                    Latest = newestIndex ?? 0,
                    Keys = keys
                };
            }
        }

        public async Task RemoveDiagnosisUidsAsync(IEnumerable<string> diagnosisUids)
        {
            using (var UoW = XpoHelper.CreateUnitOfWork())
            {
                var toRemove = new List<DbDiagnosis>();

                foreach (var d in diagnosisUids)
                {
                    var existingUid = await UoW.FindObjectAsync<DbDiagnosis>(new BinaryOperator(nameof(DbDiagnosis.DiagnosisUid), d));
                    //var existingUid = await UoW.Query<DbDiagnosis>().FirstAsync(x=>x.DiagnosisUid==d);
                    if (existingUid != null)
                        UoW.Delete(existingUid);
                }

                //ctx.Diagnoses.RemoveRange(toRemove);
                //await ctx.SaveChangesAsync();
                if (UoW.InTransaction)
                    await UoW.CommitChangesAsync();
            }
        }

        public async Task SubmitPositiveDiagnosisAsync(SelfDiagnosisSubmissionRequest diagnosis)
        {
            using (var UoW = XpoHelper.CreateUnitOfWork())
            {
                // Ensure the database contains the diagnosis uid
                if (!UoW.Query<DbDiagnosis>().Any(d => d.DiagnosisUid == diagnosis.DiagnosisUid))
                    throw new InvalidOperationException();

                var dbKeys = diagnosis.Keys.Select(k => DbTemporaryExposureKey.FromKey(k)).ToList();

                foreach (var dbk in dbKeys)
                    UoW.Save(dbk);
                
                if(UoW.InTransaction)
                    await UoW.CommitChangesAsync();
            }
        }
    }
}
