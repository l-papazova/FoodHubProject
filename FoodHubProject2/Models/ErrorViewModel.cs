using FoodHubProject2.Repositories;

namespace FoodHubProject2.Models
{
    public class ErrorViewModel : IEntity
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        IRepository<ErrorViewModel> errorviewmodelRepository;
    }
}
