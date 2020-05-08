using DevExpress.Xpo.DB;
using ExposureNotification.Backend.Functions.Xpo;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

[assembly: FunctionsStartup(typeof(ExposureNotification.Backend.Functions.Startup))]

namespace ExposureNotification.Backend.Functions
{
	public class Startup : FunctionsStartup
	{
		internal static IExposureNotificationStorage Database;

		public override void Configure(IFunctionsHostBuilder builder)
		{
			//Database = new ExposureNotificationStorage(
			//	builder => builder.UseInMemoryDatabase("ChangeInProduction"),
			//	initialize => initialize.Database.EnsureCreated());



			Database = new XpoExposureNotificationStorage(InMemoryDataStore.GetConnectionString("Data.xml"));
		}
	}
}
