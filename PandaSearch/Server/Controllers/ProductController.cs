using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PandaSearch.Server.Data;
using PandaSearch.Shared;

namespace PandaSearch.Server.Controllers
{
    //[Authorize]

    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {

        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {

            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public List<Product> Get()
        {
            var lsProducts = _context.Products.ToList();
            //var lsBrands =
            foreach (var item in lsProducts)
            {
                string aux = _context.Brands.Where(x => x.Id == item.BrandId).Select(x => x.Name).First();
                item.Brand.Name = aux != null ? aux : "";
            }
            return lsProducts;
        }
        [AllowAnonymous]
        [HttpPost("Update")]
        public void Update(Product Product)
        {
            var aux = _context.Products.FirstOrDefault(x => x.Id == Product.Id);
            if (aux != null)
            {
                _context.Products.Remove(aux);
                _context.SaveChanges();
                _context.Add(Product);
                _context.SaveChanges();
            }

        }
        [AllowAnonymous]
        [HttpPost]
        public void Create(Product Product)
        {
            _context.Products.Add(Product);
            _context.SaveChanges();
        }
        [HttpGet("Delete/{id:guid}")]
        public void Delete(Guid id)
        {
            var aux = _context.Products.FirstOrDefault(x => x.Id == id);
            if (aux != null)
                _context.Products.Remove(aux);
            _context.SaveChanges();

        }
    }
}