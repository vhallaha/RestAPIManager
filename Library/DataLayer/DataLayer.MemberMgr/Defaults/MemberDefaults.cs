using DataLayer.MemberMgr.Models;
using System;
using System.Data.Entity.Migrations;
using Utilities;
using Utilities.Member.Enum;

namespace DataLayer.MemberMgr.Defaults
{
    public class MemberDefaults
    {

        internal static void SetDefault(MemberManagerDbContext context)
        {
            var currDate = DateTime.UtcNow;

            /*-----------------------------------------------------------------
                INSERT / UPDATE : Member Manager
            ------------------------------------------------------------------*/
            var manager = new MemberManager()
            {
                Identity = Guid.NewGuid().ToString("N"),
                Name = "System Member Manager",
                OwnerId = 0,
                CreateDate = currDate,
                Settings = new MemberManagerSettings()
                {
                    Status = MemberManagerStatus.Active,
                    AutoValidateUser = true,
                    RestrictEmail = true
                }
            };

            context.MemberManager.AddOrUpdate(e => e.Name, manager);
            context.SaveChanges();

            /*-----------------------------------------------------------------
                INSERT / UPDATE : Member
            ------------------------------------------------------------------*/
            var member = new Member()
            { 
                Username = "System Member Admin",
                Email = "system@resproject.com",
                DisplayName = "System Admin",
                CreateDate = currDate,
                CryptoKey = Guid.NewGuid().ToString("N"),
                Password = Cryptography.GenerateHash("systemPassword"),
                Options = new MemberOptions()
                {
                    IsValidated = true
                }
            };

            context.Member.AddOrUpdate(e => e.Email, member);
            context.SaveChanges();

            /*-----------------------------------------------------------------
                INSERT / UPDATE : Member Login
            ------------------------------------------------------------------*/
            var login = new MemberLogin()
            {
                MemberId = member.Id,
                MemberManagerId = manager.Id,
                CreateDate = currDate,
                Status = MemberStatus.Active,
                ProviderKey = Guid.NewGuid().ToString("N")
            };

            context.MemberLogin.AddOrUpdate(e => new { e.MemberManagerId, e.MemberId}, login);
            context.SaveChanges();
        }

    }
}
