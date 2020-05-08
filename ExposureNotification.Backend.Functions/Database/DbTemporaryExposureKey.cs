using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DevExpress.Xpo;
using Xamarin.ExposureNotifications;

namespace ExposureNotification.Backend
{
	[Persistent("DbTemporaryExposureKey")]
	public class DbTemporaryExposureKey
	{
		[DevExpress.Xpo.Key(true)]
		[System.ComponentModel.DataAnnotations.Key, Column(Order = 0)]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Int64 Id { get; set; }

		//TODO Jm check the real size
		[Persistent(nameof(Base64KeyData)), Size(300)]
		public string Base64KeyData { get; set; }

		[Persistent(nameof(TimestampSecondsSinceEpoch))]
		public long TimestampSecondsSinceEpoch { get; set; }
		[Persistent(nameof(RollingStartSecondsSinceEpoch))]
		public long RollingStartSecondsSinceEpoch { get; set; }
		[Persistent(nameof(RollingDuration))]
		public int RollingDuration { get; set; }
		[Persistent(nameof(TransmissionRiskLevel))]
		public int TransmissionRiskLevel { get; set; }

		public TemporaryExposureKey ToKey()
			=> new TemporaryExposureKey(
				Convert.FromBase64String(Base64KeyData),
				DateTimeOffset.FromUnixTimeSeconds(RollingStartSecondsSinceEpoch),
				TimeSpan.FromMinutes(RollingDuration),
				(RiskLevel)TransmissionRiskLevel);

		public static DbTemporaryExposureKey FromKey(TemporaryExposureKey key)
			=> new DbTemporaryExposureKey
			{
				Base64KeyData = Convert.ToBase64String(key.KeyData),
				TimestampSecondsSinceEpoch = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
				RollingStartSecondsSinceEpoch = key.RollingStart.ToUnixTimeSeconds(),
				RollingDuration = (int)key.RollingDuration.TotalMinutes,
				TransmissionRiskLevel = (int)key.TransmissionRiskLevel
			};
	}
}
