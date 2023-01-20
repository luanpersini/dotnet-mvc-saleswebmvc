using SalesWebMvc.Models;

namespace SalesWebMvc.Services
{
    public interface IDepartmentService
    {
       Task<List<Department>> FindAllAsync();
    }
}
