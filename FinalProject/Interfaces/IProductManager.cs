using FinalProject.ServiceModels;
using FinalProject.ViewModels;

namespace FinalProject.Interfaces
{
    public interface IProductManager
    {
        Task<ProductListVm> GetFilteredProducts(ProductListQueryModel request);
    }
}
