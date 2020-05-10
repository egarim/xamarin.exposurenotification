using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DevExpress.Xpo;
using Xamarin.ExposureNotifications;

namespace Exposure.ORM
{
    [Persistent("DbTemporaryExposureKey")]  
    public class DbTemporaryExposureKey : CustomBaseObject
    {
        public DbTemporaryExposureKey(Session session) : base(session)
        {

        }

        public DbTemporaryExposureKey()
        {

        }




        //[Persistent(nameof(Base64KeyData)), Size(300)]
        //public string Base64KeyData { get; set; }

        string base64KeyData;
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Base64KeyData
        {
            get => base64KeyData;
            set => SetPropertyValue(nameof(Base64KeyData), ref base64KeyData, value);
        }

        //[Persistent(nameof(TimestampSecondsSinceEpoch))]
        //public long TimestampSecondsSinceEpoch { get; set; }

        long timestampSecondsSinceEpoch;
        public long TimestampSecondsSinceEpoch 
        {
            get => timestampSecondsSinceEpoch;
            set => SetPropertyValue(nameof(TimestampSecondsSinceEpoch), ref timestampSecondsSinceEpoch, value);
        }

        //[Persistent(nameof(RollingStartSecondsSinceEpoch))]
        //public long RollingStartSecondsSinceEpoch { get; set; }

        long rollingStartSecondsSinceEpoch;
        public long RollingStartSecondsSinceEpoch
        {
            get => rollingStartSecondsSinceEpoch;
            set => SetPropertyValue(nameof(RollingStartSecondsSinceEpoch), ref rollingStartSecondsSinceEpoch, value);
        }


        //[Persistent(nameof(RollingDuration))]
        //public int RollingDuration { get; set; }

        int rollingDuration;
        public int RollingDuration
        {
            get => rollingDuration;
            set => SetPropertyValue(nameof(RollingDuration), ref rollingDuration, value);
        }



        //[Persistent(nameof(TransmissionRiskLevel))]
        //public int TransmissionRiskLevel { get; set; }

        int transmissionRiskLevel;

    
        public int TransmissionRiskLevel
        {
            get => transmissionRiskLevel;
            set => SetPropertyValue(nameof(TransmissionRiskLevel), ref transmissionRiskLevel, value);
        }

        public TemporaryExposureKey ToKey()
        {
            return new TemporaryExposureKey(
                Convert.FromBase64String(Base64KeyData),
                DateTimeOffset.FromUnixTimeSeconds(RollingStartSecondsSinceEpoch),
                TimeSpan.FromMinutes(RollingDuration),
                (RiskLevel)TransmissionRiskLevel);
        }

        public static DbTemporaryExposureKey FromKey(TemporaryExposureKey key)
        {
            return new DbTemporaryExposureKey
            {
                Base64KeyData = Convert.ToBase64String(key.KeyData),
                TimestampSecondsSinceEpoch = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                RollingStartSecondsSinceEpoch = key.RollingStart.ToUnixTimeSeconds(),
                RollingDuration = (int)key.RollingDuration.TotalMinutes,
                TransmissionRiskLevel = (int)key.TransmissionRiskLevel
            };
        }
    }
}
