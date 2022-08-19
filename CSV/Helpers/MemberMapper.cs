using CSV.Models;
using DapperExtensions.Mapper;

namespace CSV.Helpers;

public class MemberMapper : ClassMapper<Terminal>
{
    public MemberMapper()
    {
        Table("Terminals");
        Map(x => x.properties).Ignore();
        AutoMap();
    }
}