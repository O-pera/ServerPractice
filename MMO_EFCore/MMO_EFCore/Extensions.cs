namespace MMO_EFCore {
    public static class Extensions {
        //LINQ를 사용할 떄 자주 등장하는 IEnumerable (LINQ to Object / LINQ to XML 쿼리에 자주 사용된다)
        // IQueryable (LINQ to SQL 쿼리)
        public static IQueryable<GuildDto> MapGuildToDto(this IQueryable<Guild> guild) {
            return guild.Select(g => new GuildDto() {
                Name = g.GuildName,
                MemberCount = g.Members.Count
            });
        }
    }
}
