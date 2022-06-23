using System;
using System.Collections.Generic;
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

    #region Entity Class
    [Table("Item")]
    public class Item {
        public int ItemID { get; set; } //DB에서의 ID
        public int TemplateID { get; set; } //DataSheet에서의 ID
        public DateTime CreateDate { get; set; }


        public int OwnerID { get; set; }
        // FK ( Navigational Property )
        // DB 테이블에는 존재하지 않는다.
        // 위의 OwnerID를 Convention을 통해서 FK를 만들어주지 않아도 다른 Entity Class이기 때문에 자동으로 FK용 Column을 생성해준다.
        public Player Owner { get; set; }
    }

    [Table("Player")]
    public class Player {
        public int PlayerID { get; set; }   //DB에서 PK가 된다.
        public string Name { get; set; }

        public Item Item { get; set; }
        public Guild Guild { get; set; }
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
