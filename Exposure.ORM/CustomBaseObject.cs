using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;

namespace Exposure.ORM
{
    [NonPersistent]
    public abstract class CustomBaseObject : XPCustomObject
    {
        public CustomBaseObject(Session session)
            : base(session)
        {
        }

        public CustomBaseObject()
        {
        }

        [MemberDesignTimeVisibility(false)]
        [Persistent("Oid")]       
        [Key(true)]
        private Guid oid = Guid.Empty;
             
        [PersistentAlias("oid")]
       
        public Guid Oid
        {
            get
            {
                return oid;
            }
        }


        [Persistent("DateOfCreation")]
        [ValueConverter(typeof(UtcDateTimeConverter))]
        DateTime dateOfCreation;
        [PersistentAlias("_dateOfCreation")]
        [Browsable(false)]
        public DateTime DateOfCreation
        {
            get => dateOfCreation;           
        }

        [Persistent("LastModifiedDate")]
        [ValueConverter(typeof(UtcDateTimeConverter))]
        DateTime lastModifiedDate;

        [PersistentAlias("_lastModifiedDate")]
        [Browsable(false)]
        public DateTime LastModifiedDate
        {
            get => lastModifiedDate;
        }
     

        long epochDate;
        [Browsable(false)]
        public long EpochDate
        {
            get => epochDate;
            set => SetPropertyValue(nameof(EpochDate), ref epochDate, value);
        }


        public static DateTime FromUnixTime(long unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixTime);
        }

        public static long ToUnixTime(DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((date - epoch).TotalSeconds);
        }
		public override string ToString()
        {
            return base.ToString();
        }


        public override void AfterConstruction()
        {
            base.AfterConstruction();

            oid = XpoDefault.NewGuid();         

            dateOfCreation = DateTime.UtcNow;

        }

        protected override void OnSaving()
        {
            base.OnSaving();

            if (!(base.Session is NestedUnitOfWork) && base.Session.IsNewObject(this) && oid.Equals(Guid.Empty))
            {
                oid = XpoDefault.NewGuid();
            }

            this.EpochDate = ToUnixTime(DateTime.UtcNow);
            lastModifiedDate = DateTime.UtcNow;

        }


    }
}
