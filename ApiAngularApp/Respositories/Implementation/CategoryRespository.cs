using ApiAngularApp.Data;
using ApiAngularApp.Models.Domain;
using ApiAngularApp.Respositories.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;

namespace ApiAngularApp.Respositories.Implementation
{
    public class CategoryRespository : IcategoryRepository
    {
        private readonly AppDBcontext dBcontext;

        public CategoryRespository(AppDBcontext dBcontext)
        {
            this.dBcontext = dBcontext;
        }
        public async Task<Category> CreateAsync(Category category)
        {
            await dBcontext.Categories.AddAsync(category);
            await dBcontext.SaveChangesAsync();
            return category;

        }
        public async Task<IEnumerable<Category>> Getcategory()
        {
            return await dBcontext.Categories.ToListAsync();
        }

        public async Task<Category> Getcategoryid(Guid Id)
        {
            return await dBcontext.Categories.FirstOrDefaultAsync(e => e.ID == Id);
        }
        public async Task<Category> UpdateCategory(Category category)
        {
            var result = await dBcontext.Categories
                .FirstOrDefaultAsync(e => e.ID == category.ID);

            if (result != null)
            {
                result.Name = category.Name;
                result.UrlHandle = category.UrlHandle;
               
                await dBcontext.SaveChangesAsync();

                return result;
            }

            return null;
        }


        public async void DeleteCategory(Guid id)
        {
            var result = await dBcontext.Categories
                .FirstOrDefaultAsync(e => e.ID == id);
            if (result != null)
            {
                dBcontext.Categories.Remove(result);
                await dBcontext.SaveChangesAsync();
            }
        }



    }
}
