using Firebase.Auth.Providers;
using Firebase.Auth;
using Microsoft.AspNetCore.DataProtection;

namespace WebAppCondominio.FirebaseAuth
{
	public static class FirebaseAuthHelper
	{
		public const string firebaseAppId = "Your AppId";
		public const string firebaseApiKey = "Your ApiKey";

		public static FirebaseAuthClient setFirebaseAuthClient()
		{
			var response = new FirebaseAuthClient(new FirebaseAuthConfig
			{
				ApiKey = firebaseApiKey,
				AuthDomain = $"{firebaseAppId}.firebaseapp.com",
				Providers = new FirebaseAuthProvider[]
					{
						new EmailProvider()
					}
			});

			return response;
		}
	}
}
