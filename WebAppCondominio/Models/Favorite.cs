using WebAppCondominio.FirebaseAuth;
using Firebase.Storage;
using Google.Cloud.Firestore;

namespace WebAppCondominio.Models
{
    public class Favorite
    {
        public string? Id { get; set; }
        public string? Cedula { get; set; }
        public string? Name { get; set; }
        public string? Vehicle { get; set; }
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public string? Color { get; set; }
        public string? Date { get; set; }
    }

    public class FavoritesHandler
    {
        public async Task<List<Favorite>> GetFavoritesCollection()
        {
            List<Favorite> favoritesList = new List<Favorite>();
            Query query = FirestoreDb.Create(FirebaseAuthHelper.firebaseAppId).Collection("Favorites");
            QuerySnapshot querySnaphot = await query.GetSnapshotAsync();

            foreach (var item in querySnaphot)
            {
                Dictionary<string, object> data = item.ToDictionary();

                favoritesList.Add(new Favorite
                {
                    Id = item.Id,
                    Cedula = data["Cedula"].ToString(),
                    Name = data["Name"].ToString(),
                    Vehicle = data["Vehicle"].ToString(),
                    Brand = data["Brand"].ToString(),
                    Model = data["Model"].ToString(),
                    Color = data["Color"].ToString(),
                    Date = data["Date"].ToString(),
                });
            }

            return favoritesList;
        }

        public async Task<bool> Create(string cedula, string name, string vehicle, string brand, string model, string color, string date)
        {
            try
            {
                DocumentReference addedDocRef =
                 await FirestoreDb.Create(FirebaseAuthHelper.firebaseAppId)
                     .Collection("Favorites").AddAsync(new Dictionary<string, object>
                         {
                                    { "Cedula", cedula },
                                    { "Name", name },
                                    { "Vehicle", vehicle },
                                    { "Brand",  brand },
                                    { "Model", model },
                                    { "Color", color },
                                    { "Date", date }
                         });

                return true;
            }
            catch (FirebaseStorageException ex)
            {
                throw ex;
            }
        }

    }

}