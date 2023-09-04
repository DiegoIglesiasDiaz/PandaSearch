using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PandaSearch.Server.Data;
using PandaSearch.Shared;
using System.Security.Cryptography;

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
            string path = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
            var files = Directory.GetFiles(path);
            foreach (var item in lsProducts)
            {
                var aux = _context.Brands.ToList().FirstOrDefault(x => x.Id == item.BrandId).Name;
                item.Brand.Name = aux != null ? aux : "";
                var img = files.FirstOrDefault(x => x.ToLower().Contains(item.Id.ToString().ToLower()));
                if (img != null)
                {
                    item.imgbyte =  System.IO.File.ReadAllBytes(img);
                }
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
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
                var files = Directory.GetFiles(filePath);
                var imgDelete = files.FirstOrDefault(x => x.Contains(Product.Id.ToString()));
                var img = files.FirstOrDefault(x => x.Contains("update"));
                if (imgDelete != null && img != null)
                    System.IO.File.Delete(imgDelete);
                if (img != null)
                {
                    System.IO.File.Move(img, filePath + "/" + Product.Id + Path.GetExtension(img));

                }

                _context.SaveChanges();
            }
            

        }
        [AllowAnonymous]
        [HttpPost]
        public void Create(Product Product)
        {
            _context.Products.Add(Product);
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
            var files = Directory.GetFiles(filePath);
            var img = files.FirstOrDefault(x => x.Contains("new"));
            if (img != null)
            {
                System.IO.File.Move(img, filePath + "/" + Product.Id + Path.GetExtension(img));

            }

            _context.SaveChanges();
        }
        [HttpGet("Delete/{id:guid}")]
        public void Delete(Guid id)
        {
            var aux = _context.Products.FirstOrDefault(x => x.Id == id);
            if (aux != null)
                _context.Products.Remove(aux);

            string path = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
            var files = Directory.GetFiles(path);
            var img = files.FirstOrDefault(x => x.Contains(id.ToString()));
            if (img != null)
                System.IO.File.Delete(img);
            _context.SaveChanges();

        }
        [HttpPost("UploadFile")]
        [AllowAnonymous]
        public async Task Upload([FromHeader] string Id)
        {
            var files = Request.Form.Files;
            string path = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    var ext = Path.GetExtension(file.FileName);
                    var filePath = path + "/" + Id + ext;
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        try
                        {
                             await file.CopyToAsync(stream);
                        }
                        catch (Exception ex)
                        {

                        }


                    }
                }
            }
        }

        [HttpPost("UploadMultipleFile")]
        [AllowAnonymous]
        public void UploadMultipleFiles ()
        {
            var files = Request.Form.Files;
            string path = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    var filePath = path + "/" +file.FileName;
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        try
                        {
                             file.CopyTo(stream);
                        }
                        catch (Exception ex)
                        {

                        }
                         stream.Close();


                    }
                }
            }
        }

        [HttpGet("DeleteImage/{status}")]
        [AllowAnonymous]
        public void DeleteImage(string status)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
            var files = Directory.GetFiles(path);
            var img = files.FirstOrDefault(x => x.Contains(status));
            if (img != null)
                System.IO.File.Delete(img);
        }
        [HttpGet("Img/{id:guid}")]
        [AllowAnonymous]
        public byte[] GetImgById(Guid id)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
            var files = Directory.GetFiles(path);
            var img = files.FirstOrDefault(x => x.Contains(id.ToString()));
            if (img != null)
                return System.IO.File.ReadAllBytes(img);
            return null;
        }
    }
}