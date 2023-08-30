using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PandaSearch.Server.Data;
using PandaSearch.Shared;

namespace PandaSearch.Server.Controllers
{
    //[Authorize]

    [ApiController]
    [Route("[controller]")]
    public class BrandController : ControllerBase
    {

        private readonly ApplicationDbContext _context;

        public BrandController(ApplicationDbContext context)
        {

            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public List<Brand> Get()
        {
            return _context.Brands.ToList();
        }
        [AllowAnonymous]
        [HttpPost("Update")]
        public void Update(Brand brand)
        {
            var aux = _context.Brands.FirstOrDefault(x => x.Id == brand.Id);
            if (aux != null)
            {
                _context.Brands.Remove(aux);
                _context.SaveChanges();
                _context.Add(brand);
                _context.SaveChanges();
            }

        }
        [AllowAnonymous]
        [HttpPost]
        public void Create(Brand brand)
        {
            _context.Brands.Add(brand);
            _context.SaveChanges();
        }
        [AllowAnonymous]
        [HttpGet("Delete/{id:guid}")]
        public void Delete(Guid id)
        {
            var aux = _context.Brands.FirstOrDefault(x => x.Id == id);
            if (aux != null)
                _context.Brands.Remove(aux);
            _context.SaveChanges();

        }
    }
}