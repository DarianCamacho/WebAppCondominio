using Firebase.Auth.Providers;
using Firebase.Auth;
using Microsoft.AspNetCore.DataProtection;

namespace WebAppCondominio.FirebaseAuth
{
	public static class FirebaseAuthHelper
	{
		public const string firebaseAppId = "";
		public const string firebaseApiKey = "";

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
