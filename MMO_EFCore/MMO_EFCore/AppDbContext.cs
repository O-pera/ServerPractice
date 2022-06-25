using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MMO_EFCore {
    //DbSet : Generic이름의 테이블 참조. 해당 테이블에 사용자가 접근하려면 Items를 사용한다.
    //EF Core 작동 스텝
    //1. AppDbContext를 만들 떄
    //2. DbSet<T>을 찾는다.
    //3. 모델링을 담당하는 class를 분석해서 Column을 찾는다.
    //4. 모델링 class에서 참조하는 다른 class가 있을 경우 해당 class도 같이 분석한다.
    //5. OnModelCreating함수 호출 (추가 설정이 필요할 경우 override를 사용해서 정의해준다)
    //6. DB 전체 모델링 구조를 내부 메모리에 들고있는다.

    public class AppDbContext : DbContext{

        //DbSet<Item> -> EF Core한테 Item이라는 테이블을 알려준다. 이렇게 프로퍼티로 생성할 경우 프로퍼티의 이름으로, 프로퍼티를 생성하지 않은 경우 클래스 이름으로 설정된다.
        //Item이라는 DB 테이블이 있는데세부적인 칼럼/키는 Item클래스를 참고한다.
        public DbSet<Item> Items { get; set; }

        //Items에서 Owner를 참조할 때는 Players가 아무리 DbSet이 되어있더라도 Include를 사용해야 읽을 수 있다.
        public DbSet<Player> Players { get; set; }

        #region TPH Convention
        //public DbSet<EventPlayer> EventPlayers { get; set; }
        #endregion

        public DbSet<Guild> Guilds { get; set; }
        
        //ConnectionString
        //DB를 연결하는 방법이 작성된 아주 긴 문자열. 각종 설정과 Authorization 등이 포함되어 있으며, 이 데이터는 민감한 정보이기 때문에 보통 config.json 파일을 따로 생성해서 관리한다.
        public const string ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=PracticeDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        //DB와 연동하는 부분
        protected override void OnConfiguring(DbContextOptionsBuilder options) {
            options.UseSqlServer(ConnectionString);
        }

        //Entity에 대한 추가 설정 가능한 곳
        protected override void OnModelCreating(ModelBuilder builder) {
            //앞으로 Item Entity에 접근할 때 항상 사용되는 모델 레벨의 필터링
            //필터를 무시하고 싶으면 AppDbContext객체에서 Entity를 접근할 때 IgnoreQueryFilter 옵션을 추가하면 된다.
            builder.Entity<Item>().HasQueryFilter(i => i.SoftDeleted == false);

            #region Unique Index 생성
            //builder.Entity<Player>()
            //                .HasIndex(p => p.Name)
            //                .IsUnique()
            //                .HasName("Index_PersonName");
            #endregion
            #region 1:n 관계 FK
            //builder.Entity<Player>()
            //            .HasMany(p => p.CreatedItems)
            //            .WithOne(i => i.Creator)
            //            .HasForeignKey(i => i.CreatorID);
            #endregion
            #region 1:1 관계 FK
            //1:1 관계에선 어디에 FK를 설정할지 EFCore가 모르기 때문에 HasForeignKey에 Generic으로 FK를 가질 Entity Class를 기입한다.
            //builder.Entity<Player>()
            //    .HasOne(p => p.OwnedItem)
            //    .WithOne(i => i.Owner)
            //    .HasForeignKey<Item>(i => i.OwnerID);
            #endregion
            #region Shadow Property
            builder.Entity<Item>().Property<DateTime>("RecoveredDate");
            #endregion
            #region Backing Field
            builder.Entity<Item>()
                .Property(i => i.JsonData)
                .HasField("_jsonData");
            #endregion

            #region Ownership
            builder.Entity<Player>()
                .OwnsOne(i => i.Transform);
            #endregion
            #region TPH FluentAPI
            builder.Entity<Player>()
                .HasDiscriminator(p => p.Type)
                .HasValue<Player>(PlayerType.NormalPlayer)
                .HasValue<EventPlayer>(PlayerType.EventPlayer);
            #endregion
            #region Table Splitting
            builder.Entity<Item>()
                .HasOne(i => i.Detail)
                .WithOne()
                .HasForeignKey<ItemDetail>(d => d.ItemDetailID);

            builder.Entity<Item>().ToTable("Items");
            builder.Entity<ItemDetail>().ToTable("Items");
            #endregion
            
            #region BackingField + Relationship
            builder.Entity<Item>()
                .Metadata
                .FindNavigation("Reviews")
                .SetPropertyAccessMode(PropertyAccessMode.Field);
            #endregion
        }
    }
}
