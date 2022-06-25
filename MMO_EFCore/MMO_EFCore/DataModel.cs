using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MMO_EFCore {
    #region NotePad
    //테이블이름 : Property 생성 시 Property이름,
    //미생성 시 클래스 이름,
    //Attribute추가 시 Attribute parameter string

    //DB모델링
    //1. 1:1 관계
    //2. 1:n 관계
    //3. n:m 관계

    //FK와 Nullable
    // 1. Required Relationship ( Not-Null )
    //   삭제할 때 OnDelete 인자를 Cascade모드로 호출 -> Principal 삭제하면 Dependent도 삭제
    // 2. Optional Relationship ( Nullable )
    //   -> Principal 삭제할 때 Dependent Tracking하고있으면 FK를 null세팅
    //   -> Principal 삭제할 때 Dependent Tracking하고있지 않으면 Exception 발생
    //
    //ForeignKey를 Data Annotation으로 Relationship 설정
    //1. [ForeignKey("")]
    //2. [InverseProperty] -> 다수의 Navigational Property가 같은 클래스를 참조할 때
    //
    //ForeignKey를 FluentAPI로 Relationship 설정
    // .HasOne().HasMany()          -> 1:1 hasone 1:n hasMany
    // .WithOne().WithMany()        -> 1:1 withone n:m withMany
    // .HasForeignKey() / .IsRequired()      / .OnDelete()
    // .HasConstraint() / .HasPrincipalKey() 

    #region Configuration ( Data Modeling 상세 설정 방법 )

    /* Configurations
     * A) Convention ( 관례 )
     * - 각종 형식과 이름 등을 정해진 규칙에 맞게 만들면 EFCore에서 자동적으로 처리한다.
     *  특징: 쉽고 빠르지만 모든 경우를 처리할 순 없다.
     * 
     * B) Data Annotation ( 데이터 주석 )
     * - Class/Property 등에 Attribute를 사용해서 추가 정보를 기입한다.
     *  특징: 
     *  
     * C) Fluent API ( 직접 정의 )
     * - OnModelCreating단계에서 직접 설정을 정의해서 만드는 방식
     *  특징: 활용범위가 제일 넓지만 직접 설정해야하기 때문에 매우 귀찮다.
     * 
     * 
     * 3개가 겹쳐있을 경우 실행 순서는 A -> B -> C 이지만 A < B < C 우선순위이기 떄문에
     * Convention으로 처리했더라도 Fluent API에 의해 덮어질 수 있다.
     */

    /* <----------------------Convention---------------------->
    *  1) Entity Class관련
    *   - public 접근제어자 + Non-Static
    *   - Property 중에서 Public Getter를 찾으면서 테이블을 구성한다.
    *   - Property 이름 = Table Column 이름
    *  2) 이름, 형식, 크기 관련
    *   - .NET 형식 <-> SQL 형식 (int, bool)
    *   - .NET 기본 Nullable형식 -> SQL Nullable형식을 따라간다. (string은 기본 nullable, int는 non-nullable)
    *  3) PK 관련
    *   - ID 또는 <ClassName>ID로 정의된 Property는 PK로 인정된다. ( 후자 권장 )
    *   - 복합키 ( Composite Key )의 경우 Convention으로 처리 불가능.
    *   <----------------------------------------------------->
    */

    /* Commands, Conventions, Data Annotations, Fluent APIs
     * [Commands]       [Convention]                [Data Annotation]            [FluentAPI]
     * Nullable:        type?                       [Required]                   .IsRequired()
     * 문자열 길이                                   [MaxLength(20)]              .HasMaxLength(20)
     * 문자 형식                                                                  .IsUnicode(true)
     * PK                                           [Key][Column(Order=0)]       .HasKey(x => new {x.Prop1, x.Prop2}
     * Index                                                                     .HasIndex(p => p.Prop1)
     * Composite Index                                                           .HasIndex(p => new {p.Prop1, p.Prop2})
     * Named Index                                                               .HasName("Index_MyProp")
     * Unique Index                                                              .IsUnique()
     * 테이블 이름       DbSet<T> prop이름           [Table("<name>")]            .ToTable("<name>")
     *                 or Entity class 이름
     * Column 이름       Property 이름               [Column("<name>")]           .HasColumnName("<name>")                
     * Column Exception                             [NotMapped]                  .Ignore()
     * Condition Filter                                                          .HasQueryFilter(i => i.<Column> == <Condition>)
     * Foreign Key      <PrincipalKeyName>          [ForeignKey("<name>")]
     *                  <Class><PrincipalKeyName>   
     *                  <Nav.Prop><Prin.KeyName>
     */

    /* 언제 사용할까?
     *  1) Convention이 가장 무난
     *  2) Validation과 관련된 부분들은 Data Annotation (직관적, SaveChanges 호출?)
     *  3) 그 외에는 Fluent API
     * 
     */

    #endregion

    #region 기본 용어
    // 1) Principal Entity
    // 2) Dependent Entity
    // 3) Navigational Property
    // 4) Primary Key ( PK )
    // 5) Foreign Key ( FK )
    // 6) Principal Key = PK or Unique Alternate Key (ex. Player의 Name이 Unique할 경우 Alternate Key로 인정할 수 있다.)
    // 7) Required Relationship ( NOT-NULL )
    // 8) Optional Relationship ( Nullable )
    #endregion

    #region Shadow Property
    /*Class 모델링에는 있지만 DB에는 없기를 원할 때 -> [NotMapped] or .Ignore()
     * 반대로 DB에는 있지만 Class에는 없기를 원할 때 -> Shadow Property
     * 왜 클래스에 오히려 없는게 필요한가?
     *      일반적으로 데이터에 대한 참조를 할 때 주로 사용되지 않는 Column들을 클래스에서 가려준다.
     * 생성 -> .Property<DateTime>("<name>")
     * Read/Write -> .Property("<name>").CurrentValue 를 통해서 Get;Set; 가능
     */
    #endregion

    #region Backing Field
    /* Private Field를 DB에 매핑하고 public getter로 가공해서 사용한다
     * ex) DB에는 Json으로 저장하고 Getter에서 추출할 때는 Json을 가공해서 사용.
     * ---------------------------이것도 DTO 쓸려나?
     * Fluent API만 사용 가능
     */
    #endregion

    #region Entity Class와 DBTable을 연동하는 방법들
    /* EntityClass하나를 통으로 Read/Write -> 부담이 된다. 우회로 (Select Loading, DTO)
     * 1. Owned Type
     * 2. Table Per Hierarchy ( TPH )
     * 3. Table Splitting
     */

    #region OwnedType
    //  - 일반 class를 Entity Class에 추가하는 개념
    //  - Navigational Property와는 다르다.
    //  - 설정 방법 :
    //     1. 동일한 테이블에 추가하기
    //         [FluentAPI} .OwnsOne()
    //  - 일반적인 Relationship의 경우 Navigational Property는 Include를 통해서 데이터를 추가적으로 가져와야 읽을 수 있지만
    //    Ownership의 경우 테이블 내에 저장하고 있기 때문에 Include를 하지 않아도 데이터를 읽을 수 있다.
    //     2. 다른 테이블에 저장하기
    //         [FluentAPI] .OwnsOne().ToTable()
    //         TODO: 다른 테이블에 생성하게 되면 해당 테이블의 ID는 FK까지 겸하게 된다. 이건 그냥 NavP가 아닌가? 그럼 Include 써야될건데?
    #endregion
    #region Table Per Hierarchy ( TPH )
    /* 상속 관계의 여러 class들을 하나의 테이블에 매핑
     *  - 설정 방법:
     *      1. Convention
     *          일단 Class를 상속받아 만들고 DbSet으로 추가하면 됨
     *           -> 같은 테이블에 저장은 되지만, 하위 엔티티에 접근하려면 DbSet으로 만든 EntityClass에 따로 접근해야한다. 아무리 Discriminator가 있더라도...
     *      2. Fluent API
     *          .HasDiscriminator().HasValue();
     *          -> 테이블에서 Discriminator Column대신 HasDiscriminator()에 지정한 Column이 Discriminator가 된다.
     *          -> HasDiscriminator(): Discriminator가 될 Column 기입 HasValue() => 각 Class마다 설정해줘야 하며, 각 Class마다 Discriminator Type을 기입.
     */
    #endregion
    #region Table Splitting
    //다수의 Entity Class를 하나의 테이블에 매핑
    //Navigational Property랑 똑같다. 그냥 테이블이 한 군데에 들어있을 뿐이다.
    //읽어오려면 Include를 사용해야 한다.
    #endregion

    #endregion

    #region Backing Field + Relationship
    /* Backing Field -> private field를 DB에 매핑
     * Navigational Property에서도 사용 가능
     * 
     */
    #endregion

    #endregion

    #region Normal Classes
    public struct ItemOption {
        public int str;
        public int dex;
        public int hp;
    }

    public enum PlayerType {
        NormalPlayer,
        EventPlayer,
    }

    public class Transform {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
    }

    public class ItemDetail {
        public int ItemDetailID { get; set; }
        public string Description { get; set; }
    }

    public class ItemReview {
        public int ItemReviewID { get; set; }
        public int Score { get; set; }
    }
    #endregion

    #region Entity Class
    [Table("Item")]
    public class Item {
        public bool SoftDeleted { get; set; }
        public int ItemID { get; set; } //DB에서의 ID
        public int TemplateID { get; set; } //DataSheet에서의 ID
        public DateTime CreateDate { get; set; }

        #region BackingField
        private string? _jsonData;
        public string? JsonData { 
            get { return _jsonData; } 
        }
        public void SetOption(ItemOption option) { _jsonData = JsonConvert.SerializeObject(option); }
        public ItemOption GetOption() { return JsonConvert.DeserializeObject<ItemOption>(_jsonData); }
        #endregion

        //FK
        //Nullable을 하고싶다면 테이블에 남지않은 Navigational Property가 아닌 FK에 직접 Nullable을 적용해야한다.
        //[ForeignKey("OwnerID")] -> 따로 FK를 선언해주지 않고 자동으로 만들어주고 싶다면 Attribute를 사용한다. 기본적으로 Nullable이다.
        public int? OwnerID { get; set; }

        // Navigational Property
        // DB 테이블에는 존재하지 않는다.
        // 위의 OwnerID를 Convention을 통해서 FK를 만들어주지 않아도 다른 Entity Class이기 때문에 자동으로 FK용 Column을 생성해준다.
        //[InverseProperty("OwnedItem")] // Dependent FK 설정
        public Player Owner { get; set; }

        public int? CreatorID { get; set; }
        public Player Creator { get; set; }

        #region Table Splitting
        public ItemDetail? Detail { get; set; }
        #endregion
        #region BackingField + Relationship
        public double? AverageScore { get; set; }
        private readonly List<ItemReview> _reviews = new List<ItemReview>();
        public IEnumerable<ItemReview>? Reviews { get { return _reviews.ToList(); } }
        public void AddReview(ItemReview itemReview) { 
            _reviews.Add(itemReview);
            AverageScore = _reviews.Average(r => r.Score);
        }
        public void RemoveReview(ItemReview itemReview) {
            _reviews.Remove(itemReview);
            AverageScore = _reviews.Any() ? _reviews.Average(r => r.Score) : (double?)null;
        }
        #endregion
    }

    [Table("Player")]
    public class Player {
        public int PlayerID { get; set; }   //DB에서 PK가 된다.
        public PlayerType Type { get; set; }

        [Required] [MaxLength(20)]
        public string Name { get; set; }

        [InverseProperty("Owner")] //Principal FK 설정
        public Item OwnedItem { get; set; }
        [InverseProperty("Creator")]
        public ICollection<Item>? CreatedItems { get; set; }
        public Guild? Guild { get; set; }

        #region OwnedType
        public Transform? Transform { get; set; }
        #endregion
    }

    #region TPH Class
    public class EventPlayer : Player { 
        public DateTime DestroyDate { get; set; }
    }
    #endregion

    [Table("Guild")]
    public class Guild {
        public int GuildID { get; set; }
        public string GuildName { get; set; }
        public ICollection<Player> Members { get; set; }
    }
    #endregion

    #region Data Transfer Object
    //컨텐츠에 넘기기 전 데이터를 재가공하는 용도로 사용
    public class GuildDto { 
        public int GuildID { get; set; }
        public string Name { get; set; }
        public int MemberCount { get; set; }
    }
    #endregion
}
