using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace MMO_EFCore {
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

            List<Item> items = new List<Item>{
                new Item(){
                    TemplateID = 101,
                    CreateDate = DateTime.Now,
                    Owner = opera
                },
                new Item(){
                    TemplateID = 102,
                    CreateDate = DateTime.Now,
                    Owner = opera
                },
                new Item(){
                    TemplateID = 103,
                    CreateDate = DateTime.Now,
                    Owner = faker
                }
            };

            Guild guild = new Guild(){
                GuildName = "TestGuild",
                Members = new List<Player>(){opera, faker, daft}
            };

            db.Items.AddRange(items);
            db.Guilds.Add(guild);
            db.SaveChanges();
        }


        #endregion

        #region CRUD: Read
        public static void ReadAll() {
            using(AppDbContext db = new AppDbContext()) {
                //AsNoTracking이 수정을 가하지 않는다는 뜻. ReadOnly용도
                //Tracking Snapshot을 통해 하지도 않은 데이터 변경이 감지되지 않도록 가져온다.
                //Include : Eager Loading (즉시 로딩)
                foreach(Item item in db.Items.AsNoTracking().Include(i => i.Owner)) {
                    Console.WriteLine($"TemplateID: {item.TemplateID} Owner: {item.Owner.Name} Created: {item.CreateDate}");
                }
            }
        }

        public static void ShowItems() {
            Console.WriteLine("Input PlayerName");
            Console.Write(">>> ");
            string name = Console.ReadLine();

            using(AppDbContext db = new AppDbContext()) {
                foreach(Player player in db.Players.AsNoTracking().Where(p => p.Name == name).Include(p => p.Items)) {
                    foreach(Item item in player.Items) {
                        Console.WriteLine($"TemplateID: {item.TemplateID}");
                    }
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
                                            .ThenInclude(p => p.Items)
                                       .First();
                //Include에서 가져온 player에서 한번 더 Include를 하기 위해선 ThenInclude를 쓴다.
                //한번 더 ThenInclude를 쓰면 Item으로 들어갈 수 있다.

                foreach(Player player in guild.Members) {
                    foreach(Item item in player.Items) {
                        Console.WriteLine($"TemplateID: {item.TemplateID} Owner: {player.Name}");
                    }
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
                    db.Entry(player).Collection(p => p.Items).Load();
                    //db.Entry(player).Reference(p => p.Item).Load(); -> Item이 1:1일 경우 하나만 가져오는 Reference를 쓴다.
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
        //특정 플레이어가 소지한 아이템들의 생성날짜
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


        #endregion
    }
}
