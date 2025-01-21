using Bogus;
using MultipleDatabaseConn.Models;

namespace MultipleDatabaseConn
{
    public class MemberFaker
    {
        public static Faker<Member> CreateMemberFaker()
        {
            return new Faker<Member>("zh_TW")
                .RuleFor(m => m.Id, f => Guid.NewGuid())
                .RuleFor(m => m.Name, f => {
                    var fullName = f.Name.FullName();
                    return fullName.Length > 20 ? fullName.Substring(0, 20) : fullName;
                })
                .RuleFor(m => m.Email, f => {
                    var email = f.Internet.Email();
                    return email.Length > 50 ? email.Substring(0, 50) : email;
                })
                .RuleFor(m => m.Age, f => f.Random.Int(18, 80))
                .RuleFor(m => m.CreatedAt, f => f.Date.PastOffset(2).ToUniversalTime())
                .RuleFor(m => m.CreatedBy, f => {
                    var createdBy = f.Name.FullName();
                    return createdBy.Length > 20 ? createdBy.Substring(0, 20) : createdBy;
                })
                .RuleFor(m => m.ChangedAt, f => f.Random.Bool(0.5f) ?
                    f.Date.PastOffset(1).ToUniversalTime() : (DateTimeOffset?)null)
                .RuleFor(m => m.ChangedBy, (f, m) => {
                    if (!m.ChangedAt.HasValue) return null;
                    var changedBy = f.Name.FullName();
                    return changedBy.Length > 20 ? changedBy.Substring(0, 20) : changedBy;
                });
        }

        public static Member GenerateMember()
        {
            return CreateMemberFaker().Generate();
        }

        public static List<Member> GenerateMembers(int count)
        {
            return CreateMemberFaker().Generate(count);
        }
    }
}
