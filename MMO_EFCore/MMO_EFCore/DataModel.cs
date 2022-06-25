using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MMO_EFCore {
    //테이블이름 : Property 생성 시 Property이름,
    //미생성 시 클래스 이름,
    //Attribute추가 시 Attribute parameter string

    //DB모델링
    //1. 1:1 관계
    //2. 1:n 관계
    //3. n:m 관계

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
     * Foreign Key      <PrincipalKeyName>
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

    #region Entity Class
    [Table("Item")]
    public class Item {
        public bool SoftDeleted { get; set; }
        public int ItemID { get; set; } //DB에서의 ID
        public int TemplateID { get; set; } //DataSheet에서의 ID
        public DateTime CreateDate { get; set; }


        //FK
        //Nullable을 하고싶다면 테이블에 남지않은 Navigational Property가 아닌 FK에 직접 Nullable을 적용해야한다.
        //[ForeignKey("OwnerID")] -> 따로 FK를 선언해주지 않고 자동으로 만들어주고 싶다면 Attribute를 사용한다. 기본적으로 Nullable이다.
        public int? OwnerID { get; set; }

        // Navigational Property
        // DB 테이블에는 존재하지 않는다.
        // 위의 OwnerID를 Convention을 통해서 FK를 만들어주지 않아도 다른 Entity Class이기 때문에 자동으로 FK용 Column을 생성해준다.
        public Player Owner { get; set; }

        public int CreatorID { get; set; }
        public Player Creator { get; set; }
    }

    [Table("Player")]
    public class Player {
        public int PlayerID { get; set; }   //DB에서 PK가 된다.

        [Required] [MaxLength(20)]
        public string Name { get; set; }

        public Item Item { get; set; }
        public Guild? Guild { get; set; }
        public ICollection<Item> CreatedItems { get; set; }
    }

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
