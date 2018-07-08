using DataLayer.ResourceMgr.Models;
using System;
using System.Data.Entity.Migrations;
using Utilities.Resource.Enums;
using Utilities.Shared;

namespace DataLayer.ResourceMgr.Defaults
{
    public class MemberResDefaults
    {

        internal static void SetDefault(ResourceManagerDbContext context)
        {
            var currDate = DateTime.UtcNow;

            /*-----------------------------------------------------------------
                INSERT / UPDATE : Member Resource Manager
            ------------------------------------------------------------------*/
            var member = new Resource()
            {
                Name = "Member Resource",
                CreateDate = currDate,
                UpdateDate = currDate,
                Settings = new ResourceSettings() { Status = ResourceStatus.Live },
                Type = ResourceType.Member
            };

            context.Resource.AddOrUpdate(e => e.Name, member);
            context.SaveChanges();

            /*-----------------------------------------------------------------
                INSERT / UPDATE : Member Resource Manager Settings
            -----------------------------------------------------------------*/
            var read = new ResourceClaim() { ResourceId = member.Id, ClaimName = MemberClaim.Read, CreateDate = currDate, UpdateDate = currDate };
            var write = new ResourceClaim() { ResourceId = member.Id, ClaimName = MemberClaim.Write, CreateDate = currDate, UpdateDate = currDate };
            var create = new ResourceClaim() { ResourceId = member.Id, ClaimName = MemberClaim.Create, CreateDate = currDate, UpdateDate = currDate };
            var delete = new ResourceClaim() { ResourceId = member.Id, ClaimName = MemberClaim.Delete, CreateDate = currDate, UpdateDate = currDate };

            context.ResourceClaim.AddOrUpdate(e => e.ClaimName, read, write, create, delete);
            context.SaveChanges();

            /*-----------------------------------------------------------------
                INSERT / UPDATE : Client Admin
            ------------------------------------------------------------------*/
            var client = new Client()
            {
                Name = "Member Resource Admin Client",
                OwnerId = 0,
                CreateDate = currDate,
                UpdateDate = currDate
            };

            context.Client.AddOrUpdate(e => e.Name, client);
            context.SaveChanges();

            var clientKey = new ClientKey()
            {
                ClientId = client.Id,
                APIKey = "509e567218264608a91aa11213892886",
                APISecret = "2ed65ad45ca8486a8ff781122e06702f",
                Status = ClientKeyStatus.Open,
                CreateDate = currDate,
                UpdateDate = currDate
            };

            context.ClientKey.AddOrUpdate(e => e.ClientId, clientKey);
            context.SaveChanges();

            /*-----------------------------------------------------------------
                INSERT / UPDATE : Client Admin Data Access
            ------------------------------------------------------------------*/
            var clientDataAccess = new ClientResourceAccess()
            {
                ResourceKey = "6132c038ff0146f09a4e2dfab03a28aa",
                ClientId = client.Id,
                ResourceId = member.Id,
                ResourceValue = 0,
                Status = ClientResourceAccessStatus.Allow,
                CreateDate = currDate,
                UpdateDate = currDate
            };

            context.ClientResourceAccess.AddOrUpdate(e => e.ClientId, clientDataAccess);
            context.SaveChanges();

            /*-----------------------------------------------------------------
                INSERT / UPDATE : Client Admin Data Access Claims
            ------------------------------------------------------------------*/
            context.ClientResourceAccessClaim.AddOrUpdate(e => e.ResourceClaimId,
                new ClientResourceAccessClaim() { ClientResourceAccessId = clientDataAccess.Id, ResourceClaimId = read.Id, Access = ClientResourceClaimsAccess.Allow },
                new ClientResourceAccessClaim() { ClientResourceAccessId = clientDataAccess.Id, ResourceClaimId = write.Id, Access = ClientResourceClaimsAccess.Allow },
                new ClientResourceAccessClaim() { ClientResourceAccessId = clientDataAccess.Id, ResourceClaimId = create.Id, Access = ClientResourceClaimsAccess.Allow },
                new ClientResourceAccessClaim() { ClientResourceAccessId = clientDataAccess.Id, ResourceClaimId = delete.Id, Access = ClientResourceClaimsAccess.Allow }); 
            context.SaveChanges();
        }

    }
}
