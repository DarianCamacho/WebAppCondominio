using WebAppCondominio.FirebaseAuth;
using Firebase.Storage;
using Google.Cloud.Firestore;

namespace WebAppCondominio.Models
{
    public class Delivery
    {
        public string? Id { get; set; }
        public string? DeliveryId { get; set; }
        public string? Vehicle { get; set; }
        public string? Items { get; set; }
        public string? Date { get; set; }
    }

    public class DeliverysHandler
    {
        public async Task<List<Delivery>> GetDeliverysCollection()
        {
            List<Delivery> deliverysList = new List<Delivery>();
            Query query = FirestoreDb.Create(FirebaseAuthHelper.firebaseAppId).Collection("Deliverys");
            QuerySnapshot querySnaphot = await query.GetSnapshotAsync();

            foreach (var item in querySnaphot)
            {
                Dictionary<string, object> data = item.ToDictionary();

                deliverysList.Add(new Delivery
                {
                    Id = item.Id,
                    DeliveryId = data["DeliveryId"].ToString(),
                    Vehicle = data["Vehicle"].ToString(),
                    Items = data["Items"].ToString(),
                    Date = data["Date"].ToString(),
                });
            }

            return deliverysList;
        }

        public async Task<bool> Create(string deliveryId , string vehicle, string items, string date)
        {
            try
            {
                DocumentReference addedDocRef =
                 await FirestoreDb.Create(FirebaseAuthHelper.firebaseAppId)
                     .Collection("Deliverys").AddAsync(new Dictionary<string, object>
                         {
                                    { "DeliveryId", deliveryId },
                                    { "Items", items },
                                    { "Vehicle", vehicle },
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