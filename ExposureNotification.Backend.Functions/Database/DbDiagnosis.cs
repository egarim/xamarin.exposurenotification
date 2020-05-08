using System.ComponentModel.DataAnnotations;
using DevExpress.Xpo;

namespace ExposureNotification.Backend
{
	[Persistent("DbDiagnosis")]
	public class DbDiagnosis
	{
        public DbDiagnosis()
        {
        }

        public DbDiagnosis(string diagnosisUid)
			=> DiagnosisUid = diagnosisUid;

		[System.ComponentModel.DataAnnotations.Key]
		[DevExpress.Xpo.Key(false)]
		public string DiagnosisUid { get; set; }
	}
}
