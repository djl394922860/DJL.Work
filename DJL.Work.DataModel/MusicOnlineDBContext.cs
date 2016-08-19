using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using DJL.Work.DataModel.Mapping;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace DJL.Work.DataModel
{
    public partial class MusicOnlineDBContext : DbContext
    {
        static MusicOnlineDBContext()
        {
            //Database.SetInitializer<MusicOnlineDBContext>(null);
            //Database.SetInitializer<MusicOnlineDBContext>(new CreateDatabaseIfNotExists<MusicOnlineDBContext>());
            Database.SetInitializer<MusicOnlineDBContext>(new DropCreateDatabaseIfModelChanges<MusicOnlineDBContext>());
            //Database.SetInitializer<MusicOnlineDBContext>(new DropCreateDatabaseAlways<MusicOnlineDBContext>());
            //�Զ������ݳ�ʼ��
            //Database.SetInitializer<MusicOnlineDBContext>(new MusicOnlineDBInitializer());
        }

        public MusicOnlineDBContext()
            : base("Name=MusicOnlineDBContext")
        {
            //this.Configuration.AutoDetectChangesEnabled
            //this.Configuration.LazyLoadingEnabled
            //this.Configuration.ProxyCreationEnabled
            //this.Configuration.UseDatabaseNullSemantics
            //this.Configuration.ValidateOnSaveEnabled
            //this.ChangeTracker
        }

        public DbSet<Member> Members { get; set; }
        public DbSet<MemberCollector> MemberCollectors { get; set; }
        public DbSet<MemberInfo> MemberInfoes { get; set; }
        public DbSet<Music> Musics { get; set; }
        public DbSet<MusicPlayer> MusicPlayers { get; set; }
        public DbSet<MusicType> MusicTypes { get; set; }

        // test
        public DbSet<Category> Categorys { get; set; }

        // for manager
        public DbSet<AdminInfo> AdminInfos { get; set; }
        public DbSet<RoleInfo> RoleInfos { get; set; }
        public DbSet<ActionInfo> ActionInfos { get; set; }
        public DbSet<CarouselCanfig> CarouselCanfigs { get; set; }

        //for log
        public DbSet<WebLog> WebLogs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // �ֶ��������configurations
            modelBuilder.Configurations.Add(new MemberMap());
            modelBuilder.Configurations.Add(new MemberInfoMap());
            modelBuilder.Configurations.Add(new MusicMap());
            modelBuilder.Configurations.Add(new MusicPlayerMap());
            modelBuilder.Configurations.Add(new MusicTypeMap());
            modelBuilder.Configurations.Add(new MemberCollectorMap());
            modelBuilder.Configurations.Add(new CategoryMap());
            modelBuilder.Configurations.Add(new AdminInfoMap());
            modelBuilder.Configurations.Add(new RoleInfoMap());
            modelBuilder.Configurations.Add(new ActionInfoMap());
            modelBuilder.Configurations.Add(new WebLogMap());
            modelBuilder.Configurations.Add(new CarouselCanfigMap());

            // ����ѭ����������AddFromAssembly
            // modelBuilder.Configurations.AddFromAssembly
            // ��������conventions
            // ���ö�Զ༶��ɾ��modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            // ����һ�Զ༶��ɾ��
            //modelBuilder.Conventions.Remove<>(OneToManyCascadeDeleteConvention);
            // ����Ĭ�ϱ���������ʽ
            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
