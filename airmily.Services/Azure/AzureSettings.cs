namespace airmily.Services.Azure
{
	public static class AzureSettings
	{
		private const bool OldService = false;
		private const bool UseFairFX = true;

		public static string ApplicationUrl = OldService ? @"https://airmilyapp.azurewebsites.net" : @"https://airmilyappserviceash.azurewebsites.net";

		public static string FairFXUrl = UseFairFX ? @"https://restapi.fairfx.com" : @"don't update ffx";

		public static string LocalSQLiteStore = @"airmilyLocalSQLiteStore.db";
	}
}
