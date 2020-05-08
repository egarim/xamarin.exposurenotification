using System;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.Metadata;

namespace ExposureNotification.Backend.Functions.Xpo
{
    public static class XpoHelper
    {
        static readonly Type[] entityTypes = new Type[] {
            typeof(DbDiagnosis),
            typeof(DbTemporaryExposureKey)

        };
        public static void InitXpo(string connectionString)
        {
            var dictionary = PrepareDictionary();

            //var DbDiagnosisClassInfo = Session.DefaultSession.GetClassInfo(typeof(DbDiagnosis));
            //var DbTemporaryExposureKeyClassInfo = Session.DefaultSession.GetClassInfo(typeof(DbTemporaryExposureKey));
            
            if (XpoDefault.DataLayer == null)
            {
                using (var updateDataLayer = XpoDefault.GetDataLayer(connectionString, dictionary, AutoCreateOption.DatabaseAndSchema))
                {
                    updateDataLayer.UpdateSchema(false, dictionary.CollectClassInfos(entityTypes));
                    //updateDataLayer.UpdateSchema(false,new XPClassInfo[] { DbDiagnosisClassInfo , DbTemporaryExposureKeyClassInfo });
                }
            }

            var dataStore = XpoDefault.GetConnectionProvider(connectionString, AutoCreateOption.SchemaAlreadyExists);
            XpoDefault.DataLayer = new ThreadSafeDataLayer(dictionary, dataStore);
            XpoDefault.Session = null;


        }
        public static UnitOfWork CreateUnitOfWork()
        {
            return new UnitOfWork();
        }
        static XPDictionary PrepareDictionary()
        {
            var dict = new ReflectionDictionary();
            dict.GetDataStoreSchema(entityTypes);
            return dict;
        }
    }
}
