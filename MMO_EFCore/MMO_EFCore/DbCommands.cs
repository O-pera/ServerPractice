using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;

namespace MMO_EFCore {
    #region State(상태)

    /// Entity State의 종류 /// 
    //1. Detached       -> No Tracking 상태. SaveChanges로 적용되지 않음
    //2. Unchanged      -> DB에 원본이 저장되어있고,              메모리의 데이터와 차이가 없다.
    //3. Deleted        -> DB에 아직 존재하지만 삭제되어야 한다.   메모리에서 사라져있다.
    //4. Modified       -> DB에 원본이 저장되어있고,              메모리의 데이터와 다르다.
    //5. Added          -> DB에 존재하지 않지만,                  메모리에 존재한다.

    /// SaveChanges를 호출하면 일어나는 일들 ///
    //1. 추가된 객체들의 상태가 Unchanged로 변경된다.
    //2. SQL Identity를 이용해서 PK 관리.
    //  2.1 데이터 추가(SaveChanges) 후 ID를 받아와서 객체의 ID Property를 채워준다.
    //3. Relationship을 참고해서 FK 세팅 및 객체 참조 연결

    #endregion

    public class DbCommands {
        public static void InitializeDB(bool forceReset = false) {
            using(AppDbContext db = new AppDbContext()) {
                if(!forceReset && ( db.GetService<IDatabaseCreator>() as RelationalDatabaseCreator ).Exists())
                    return;

                db.Database.EnsureDeleted();//삭제됨 보장
                db.Database.EnsureCreated();//생성 보장

                CreateTestData(db);
                Console.WriteLine("DB Initialized");
            }
        }

        #region CRUD: Create
        public static void CreateTestData(AppDbContext db) {
            Player opera = new Player(){ Name = "O-pera" };
            Player faker = new Player(){ Name = "Faker" };
            Player daft = new Player(){ Name = "Daft" };

            // TODO: 1. Detached -> 메모리에만 존재하고 DbSet 프로퍼티에 추가되지 않았기 때문에 Detached상태이다.
            Console.WriteLine($"{db.Entry(opera).State}");

            List<Item> items = new List<Item>{
                new Item(){
                    TemplateID = 101,
                    CreateDate = DateTime.Now,
                    Owner = opera
                },
                new Item(){
                    TemplateID = 102,
                    CreateDate = DateTime.Now,
                    Owner = faker
                },
                new Item(){
                    TemplateID = 103,
                    CreateDate = DateTime.Now,
                    Owner = daft
                }
            };

            Guild guild = new Guild(){
                GuildName = "TestGuild",
                Members = new List<Player>(){opera, faker, daft}
            };

            db.Items.AddRange(items);
            db.Guilds.Add(guild);

            //// TODO: 2. Added
            //Console.WriteLine($"{db.Entry(opera).State}");

            db.SaveChanges();

            //// TODO: 3. Unchanged
            //Console.WriteLine($"{db.Entry(opera).State}");

            //{//이미 존재하는 사용자를 FK로 연동하려면?
            //    //1. Tracked Instance를 얻어와서
            //    var owner = db.Players.Where(p => p.Name == "Faker").First();

            //    //2. 데이터 연결
            //    Item item = new Item(){
            //        TemplateID = 300,
            //        CreateDate = DateTime.Now,
            //        Owner = owner,
            //    };
            //    db.Items.Add(item);

            //    //3. SaveChanges
            //    db.SaveChanges();
            //}
        }


        #endregion

        #region CRUD: Read
        public static void ReadAll() {
            using(AppDbContext db = new AppDbContext()) {
                //AsNoTracking이 수정을 가하지 않는다는 뜻. ReadOnly용도
                //Tracking Snapshot을 통해 하지도 않은 데이터 변경이 감지되지 않도록 가져온다.
                //Include : Eager Loading (즉시 로딩)
                foreach(Item item in db.Items.AsNoTracking().Include(i => i.Owner).IgnoreQueryFilters().ToList()) {
                    if(item.SoftDeleted)
                        Console.Write("DELETED - ");
                    if(item.Owner == null)
                        Console.WriteLine($"ItemID: {item.ItemID} TemplateID: {item.TemplateID} Owner: null");
                    else
                        Console.WriteLine($"ItemID: {item.ItemID} TemplateID: {item.TemplateID} Owner: {item.Owner.Name}");
                }
            }
        }
        public static void ShowItems() {
            Console.WriteLine("Input PlayerName");
            Console.Write(">>> ");
            string name = Console.ReadLine();

            using(AppDbContext db = new AppDbContext()) {
                foreach(Player player in db.Players.AsNoTracking().Where(p => p.Name == name).Include(p => p.Item)) {
                    Console.WriteLine($"TemplateID: {player.Item.TemplateID}");
                }
            }
        }
        public static void ShowGuilds() {
            using(AppDbContext db = new AppDbContext()) {
                foreach(GuildDto guild in db.Guilds.MapGuildToDto()) {
                    Console.WriteLine($"GuildID: ({guild.GuildID}) GuildName: ({guild.Name}) MemberCount: ({guild.MemberCount})");
                }
            }
        }

        #region Eager Loading
        /// <summary>
        /// Guild에 연관된 모든 Player들을 다 읽는다
        /// 장점 : 단 한번의 접근으로 전부 로딩
        /// 단점 : 필요없는 데이터까지 메모리에 적재된다.
        /// </summary>
        public static void Eager() {
            Console.WriteLine("Input GuildName");
            Console.Write(">>> ");
            string name = Console.ReadLine();

            using(AppDbContext db = new AppDbContext()) {
                Guild guild = db.Guilds.AsNoTracking()
                                       .Where(g => g.GuildName == name)
                                       .Include(g => g.Members)
                                            .ThenInclude(p => p.Item)
                                       .First();
                //Include에서 가져온 player에서 한번 더 Include를 하기 위해선 ThenInclude를 쓴다.
                //한번 더 ThenInclude를 쓰면 Item으로 들어갈 수 있다.

                foreach(Player player in guild.Members) {
                    Console.WriteLine($"TemplateID: {player.Item.TemplateID} Owner: {player.Name}");
                }
            }
        }
        #endregion

        #region Explicit Loading
        /// <summary>      
        /// 명시한 데이터만 읽어온다.
        /// 장점 : 필요한 시점에 필요한 데이터만 로딩이 가능하다.
        /// 단점 : DB 접근 비용이 많이 든다.
        /// </summary>
        public static void Explicit() {
            Console.WriteLine("Input GuildName");
            Console.Write(">>> ");
            string name = Console.ReadLine();

            using(AppDbContext db = new AppDbContext()) {
                Guild guild = db.Guilds.Where(g => g.GuildName == name) 
                                       .First();

                db.Entry(guild).Collection(g => g.Members).Load();
                //Guild내의 Members에 들어있는 Player데이터를 guild에 가져온다.
                foreach(Player player in guild.Members) {
                    //db.Entry(player).Collection(p => p.Items).Load();-> Item이 1:n일 경우 전부 가져오는 Collection을 쓴다.
                    db.Entry(player).Reference(p => p.Item).Load(); //
                }
            }
        }
        #endregion

        #region Select Loading
        /// <summary>
        /// 장점 : Query처럼 필요한 정보만 추출할 수 있다.
        /// 단점 : Select내에 필요한 정보를 직접 작성해야한다.
        /// </summary>
        public static void Select() {
            Console.WriteLine("Input GuildName");
            Console.Write(">>> ");
            string name = Console.ReadLine();

            using(AppDbContext db = new AppDbContext()) {
                var info = db.Guilds.Where(g => g.GuildName == name)
                    .MapGuildToDto()
                    .First();

                Console.WriteLine($"GuildName: {info.Name} MemberCount: {info.MemberCount}");
            }
        }
        #endregion

        #endregion

        #region CRUD: Update

        #region ----------Notepad----------
        /// Update 3단계 ///
        //1. Tracked Entity를 얻어온다.
        //2. Entity Class의 property 변경 (Set)
        //3. SaveChanges

        //궁금한 점?
        // Update를 할 때 전체 수정을 하는걸까? 아니면 수정된 것만 찾아서 할까?
        //      -> SaveChanges함수를 호출할 때 내부적으로 DetectChanges를 호출하게 된다.
        //         이는 최초 Snapshot과 현재 Snapshot를 비교해서 필요한 정보만 갱신한다.
        //         즉, GuildName만 수정했으면 GuildName만 변경된다.

        /// Connected(Reload) vs Disconnected(Full) Update ///
        // Disconnected : Update 단계가 한 번에 일어나지 않고 끊겨서 진행되는 경우. (REST API등) 
        // 1] 처리 방식
        //      1. Reload 방식 : 필요한 정보만 보내서 1-2-3 Step
        //      2. Full Update방식 : 모든 정보를 주고받는 방식. Entity를 재생성해서 전체를 Update

        #endregion
        public static void UpdateDate() {
            Console.WriteLine("Input PlayerName:");
            Console.Write(">>> ");
            string name=  Console.ReadLine();

            using(AppDbContext db = new AppDbContext()) {
                var items = db.Items.Include(i => i.Owner)
                                    .Where(i => i.Owner.Name == name);

                foreach(Item item in items) {
                    item.CreateDate = DateTime.Now;
                }

                db.SaveChanges();
            }

            ReadAll();
        }

        public static void UpdateTest() {
            //----Tracked Entity 가져오기-----
            // SELECT TOP(2) GuildID, GuildName
            // FROM [Guild]
            // WHERE GuildName = N'TestGuild';
            //
            //-----SaveChanges-----
            // SET NOCOUNT ON
            // UPDATE [Guild]
            // SET GuildName = @p0
            // WHERE GuildID = @p1;
            // SELECT @@ROWCOUNT;

            using(AppDbContext db = new AppDbContext()) {
                //1. Tracked Entity를 얻어온다.
                var guild = db.Guilds.Single(g => g.GuildName == "TestGuild");

                //2. Entity Class의 Property 변경
                guild.GuildName = "DWG";

                //3. SaveChanges
                db.SaveChanges();
            }
        }

        #region Update Patterns
        /// <summary>
        /// Reload 방식 : 필요한 정보만 보내서 1-2-3 Step
        /// 장점 : 최소한의 정보로 Update 가능
        /// 단점 : Read를 두 번 한다.
        /// </summary>
        public static void UpdateReload() {
            ShowGuilds();

            //외부에서 수정을 원하는 데이터의 ID와 정보를 넘겨줬다고 가정
            Console.WriteLine("Input GuildID");
            Console.Write(">>> ");
            int id = int.Parse(Console.ReadLine());
            Console.WriteLine("Input GuildName");
            Console.Write(">>> ");
            string name = Console.ReadLine();

            using(AppDbContext db = new AppDbContext()) {
                Guild guild = db.Find<Guild>(id);
                guild.GuildName = name;
                db.SaveChanges();
            }

            Console.WriteLine("--------Update Completed--------");
            ShowGuilds();
        }

        /// <summary>
        /// Full Update방식 : 모든 정보를 주고받는 방식. Entity를 재생성해서 전체를 Update
        /// 장점 : DB에 다시 Read할 필요 없이 바로 Update
        /// 단점 : 모든 정보를 공유해야 한다. 보안 문제에 직결된다.
        /// </summary>
        public static void UpdateFull() {
            ShowGuilds();

            string jsonStr = MakeUpdateJsonStr();
            Guild guild = JsonConvert.DeserializeObject<Guild>(jsonStr);

            using(AppDbContext db = new AppDbContext()) {
                db.Guilds.Update(guild);
                db.SaveChanges();
            }

            Console.WriteLine("--------Update Completed--------");
            ShowGuilds();
        }

        public static string MakeUpdateJsonStr() {
            var jsonStr = "{\"GuildID\": 1, \"GuildName\": \"Hello\", \"Members\": null}";
            return jsonStr;
        }
        #endregion

        #region Relationship Update

        public static void Update1v1() {
            ReadAll();

            Console.WriteLine("Input ItemSwitch PlayerID");
            Console.Write(">>> ");
            int id = int.Parse(Console.ReadLine());

            using(AppDbContext db = new AppDbContext()) {
                Player player = db.Players
                    .Include(p => p.Item)
                    .Single(p => p.PlayerID == id);

                player.Item = new Item() {
                    TemplateID = 777,
                    CreateDate = DateTime.Now
                };

                db.SaveChanges();
            }

            ReadAll();
        }
        public static void Update1vM() {
            ShowGuilds();

            Console.WriteLine("Input GuildID");
            Console.Write(">>> ");
            int id = int.Parse(Console.ReadLine());

            using(AppDbContext db = new AppDbContext()) {
                #region Include를 하지 않고 Members에 new List로 할당
                /// Include를 하지 않고 new를 통해서 할당을 해 줄 경우 기존의 데이터에 추가한다. ///
                //Guild guild = db.Guilds
                //    .Single(g => g.GuildID == id);

                //guild.Members = new List<Player>() {
                //    new Player(){ Name = "Rookiss" }
                //};
                #endregion
                #region Include후 Members에 new List로 할당
                /// Include후 new를 통해서 할당을 해 줄 경우 기존의 데이터는 사라진다. ///
                //Guild guild = db.Guilds
                //    .Include(g => g.Members)
                //    .Single(g => g.GuildID == id);

                //guild.Members = new List<Player>() {
                //    new Player(){ Name = "Rookiss" }
                //};
                #endregion

                db.SaveChanges();
            }
            ShowGuilds();
        }

        #endregion

        #endregion

        #region CRUD: Delete
        public static void DeleteItem() {
            Console.WriteLine("Input PlayerName:");
            Console.Write(">>> ");
            string name=  Console.ReadLine();

            using(AppDbContext db = new AppDbContext()) {
                var items = db.Items.Include(i => i.Owner)
                                    .Where(i => i.Owner.Name == name);

                db.Items.RemoveRange(items);
                db.SaveChanges();
            }

            ReadAll();
        }

        #region Dependent Data가 Principal Data 없이 존재할 수 있을까?
        /*
         * FK가 Nullable이 아닐 경우, Principal Data가 삭제되면 Dependent Data도 같이 삭제된다.
         * FK가 Nullable일 경우, Principal Data와는 별개로 살아남는다. 대신 Include를 사용해서 FK를 읽을 수 있어야 한다.
         */
        public static void FKNullableTest() {
            ReadAll();

            Console.WriteLine("Input Delete PlayerID");
            Console.Write(">>> ");
            int id = int.Parse(Console.ReadLine());

            using(AppDbContext db = new AppDbContext()) {
                Player player = db.Players
                    //Nullable FK가 있을 때 Include를 해주지 않으면 FK참조를 끊을 수 없기 때문에 에러가 발생한다!
                    .Include(p => p.Item)   
                    .Single(p => p.PlayerID == id);

                db.Players.Remove(player);
                db.SaveChanges();
            }

            Console.WriteLine("---Test Completed---");
            ReadAll();
        }
        #endregion

        public static void TestDelete() {
            ReadAll();
            Console.WriteLine("Select Delete ItemID");
            Console.Write(">>> ");
            int id = int.Parse(Console.ReadLine());

            using(AppDbContext db = new AppDbContext()) {
                Item item = db.Find<Item>(id);
                //db.Items.Remove(item);
                item.SoftDeleted = true;
                db.SaveChanges();
            }

            Console.WriteLine("Test Delete Completed");
            ReadAll();
        }

        #endregion
    }
}
