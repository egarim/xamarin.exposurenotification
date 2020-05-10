using System;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;

namespace Exposure.ORM
{
    [Persistent("DbDiagnosis")]
    public class DbDiagnosis : CustomBaseObject
    {
        public DbDiagnosis()
        {

        }

        public DbDiagnosis(Session session) : base(session)
        {

        }       

        string diagnosisUid;
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string DiagnosisUid
        {
            get => diagnosisUid;
            set => SetPropertyValue(nameof(DiagnosisUid), ref diagnosisUid, value);
        }



    }
}
