using System;
using System.Collections.Generic;
using System.Data.Entity;
using airmily.AppService.DataObjects;
using airmily.AppService.Migrations;
using airmily.AppService.Models;

namespace airmily.AppService
{
	public class MobileServiceInitializer : CreateDatabaseIfNotExists<MobileServiceContext>
	{
		protected override void Seed(MobileServiceContext context)
		{
			List<User> users = new List<User>
			{
				new User {
					Id = Guid.NewGuid().ToString(),
					UserName = "Suzy",
					UserID = "588842",
					Active = true,
					UnionID = "",
					OpenID = "",
					FairFX = "c3V6eS5waWVyY2VAYmVpZXIzNjAuY29tQEp1TGkyMjM="
				},
				new User {
					Id = Guid.NewGuid().ToString(),
					UserName = "Ashley",
					UserID = "668788",
					Active = true,
					UnionID = "",
					OpenID = "",
					FairFX = "YXNobGV5LnN3YW5zb25AYmVpZXIzNjAuY28udWtAU3dhbnNvbjE5OTY="
				},
				new User {
					Id = Guid.NewGuid().ToString(),
					UserName = "Grant",
					UserID = "667413",
					Active = true,
					UnionID = "",
					OpenID = "",
					FairFX = ""
				}
			};
			foreach (User u in users)
				context.Set<User>().Add(u);

			base.Seed(context);
		}
	}
}