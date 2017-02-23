namespace airmily.Services.Azure
{
	public static class AzureSettings
	{
		private const bool OldService = true;
		private const bool UseFairFX = false;

		public static string ApplicationUrl = OldService ? @"https://airmilyapp.azurewebsites.net" : @"https://airmilyappserviceash.azurewebsites.net";

		public static string FairFXUrl = UseFairFX ? @"https://restapi.fairfx.com" : @"don't update ffx";

		public static string LocalSQLiteStore = @"airmilyLocalAppService.db";

		public static string StorageConnectionString = @"DefaultEndpointsProtocol=https;AccountName=airmilystorage;AccountKey=RRdg9CkiTZVa6DNI5erUaRaAOiU6yAfUhxu0Hd7yZHAd5XAO/EvUyhvXBcrwUXt4QiHGZfQsbI6cZYeaFnS/2A==";
	}
}
