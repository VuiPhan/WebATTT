using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Models.Data;

namespace Web.Models.ViewModels
{
    public class HomeVM
    {
        public List<Product> products { get; set; }

    }
    public class ProductViewModel
    {
        public List<Product> products { get; set; }

        public int BlogPerPage { get; set; }
        public int CurrentPage { get; set; }

        public int PageCount()
        {
            return Convert.ToInt32(Math.Ceiling(products.Count() / (double)BlogPerPage));
        }
        public IEnumerable<Product> PaginatedBlogs()
        {
            int start = (CurrentPage - 1) * BlogPerPage;
            return products.OrderBy(b => b.IDProduct).Skip(start).Take(BlogPerPage);
        }
    }

    public class CategoryViewModel
    {
        public List<ProductType> category { get; set; }
    }
}