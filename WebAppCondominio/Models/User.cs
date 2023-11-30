using Google.Cloud.Firestore;
using WebAppCondominio.FirebaseAuth;

namespace WebAppCondominio.Models
{
    public class User
    {
        public string? DocumentId { get; set; }
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PhotoPath { get; set; }
        public int? Role { get; set; }
        public string? Logo { get; set; }
        public string? HomeCode { get; set; }
        public string? Phone { get; set; }
        public string? PlacaLibre { get; set; }
        public string? Cedula { get; set; }
    }

    public class UsersHandler
    {
        public async Task<List<User>> GetUsersCollection()
        {
            List<User> usersList = new List<User>();
            Query query = FirestoreDb.Create(FirebaseAuthHelper.firebaseAppId).Collection("Users");
            QuerySnapshot querySnaphot = await query.GetSnapshotAsync();

            foreach (var item in querySnaphot)
            {
                Dictionary<string, object> data = item.ToDictionary();

                usersList.Add(new User
                {
                    Id = item.Id,
                    Name = data["Name"].ToString(),
                    Email = data["Email"].ToString(),
                    PhotoPath= data["PhotoPath"].ToString(),
					Role = Convert.ToInt16(data["Role"]),
					Logo = data["Logo"].ToString(),
                    HomeCode = data["HomeCode"].ToString(),
					Phone = data["Phone"].ToString(),
					PlacaLibre = data["PlacaLibre"].ToString(),
					Cedula = data["Cedula"].ToString(),
				});
            }

            return usersList;
        }
    }

}


