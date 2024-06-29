using PYGS.Api.Models;
using System.Text.Json.Nodes;

namespace PYGS.Api.Services
{
    public interface IServiceLayerComponent
    {
        bool IsError { get; }
        string ErrorMessage { get; }
        Task<bool> Login(CancellationToken cancellationToken = default);

        Task<bool> Login(string db, string usuario, string passw);
        Task Logout(CancellationToken cancellationToken = default);
        Task<string> GetItemCode(string IdAdicional, CancellationToken cancellationToken = default);
        Task<Document> CreatePurchaseDeliveryNotes(Document document, CancellationToken cancellationToken = default);
        Task<Document> CreateInventoryGenExits(Document document, CancellationToken cancellationToken = default);
 
        Task<Document> CreateInventoryGenEntries(Document inventoryGenEntries, CancellationToken cancellationToken = default);
        Task SetProductionOrderStatus(int absoluteEntry, string productionOrderStatus, CancellationToken cancellationToken = default);
        Task<bool> VerifConexion(CancellationToken cancellationToken = default);
        Task<JsonObject?> GetCardDetails(string RUC, CancellationToken cancellationToken = default);
        Task<bool> CreateBusinessPartner(BusinessPartner businessPartner, CancellationToken cancellationToken = default);
    }
}
