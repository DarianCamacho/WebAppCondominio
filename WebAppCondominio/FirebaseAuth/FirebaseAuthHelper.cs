using Firebase.Auth.Providers;
using Firebase.Auth;
using Microsoft.AspNetCore.DataProtection;

namespace WebAppCondominio.FirebaseAuth
{
    public class FirebaseAuthHelper
    {
        public static FirebaseAuthClient setFirebaseAuthClient()
        {
            var response = new FirebaseAuthClient(new FirebaseAuthConfig
            {
                ApiKey = "AIzaSyAUohhZ_VZsq-JzImrkMOn4ev014pG-bxc",
                AuthDomain = "condominio-cc812.firebaseapp.com",
                Providers = new FirebaseAuthProvider[]
                    {
                        new EmailProvider()
                    }
            });

            return response;
        }
    }
}
