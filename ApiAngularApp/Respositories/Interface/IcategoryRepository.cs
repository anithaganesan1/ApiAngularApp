using ApiAngularApp.Models.Domain;

namespace ApiAngularApp.Respositories.Interface
{
    public interface IcategoryRepository
    {
        Task<Category> CreateAsync(Category category);
        Task<IEnumerable<Category>> Getcategory();
        Task<Category> Getcategoryid(Guid Id);
        Task<Category> UpdateCategory(Category category);
       // Task<Category> DeleteCategory(Guid id);

    }
}
