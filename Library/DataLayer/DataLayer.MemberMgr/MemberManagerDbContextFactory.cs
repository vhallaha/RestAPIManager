using System.Data.Entity.Infrastructure;

namespace DataLayer.MemberMgr
{
    internal class MemberManagerDbContextFactory : IDbContextFactory<MemberManagerDbContext>
    {

        public MemberManagerDbContext Create()
        {
            return MemberManagerDbContext.Create();
        }

    }
}
