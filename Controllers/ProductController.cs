using Microsoft.AspNetCore.Mvc;
using WebAPI.Data;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class ProductController : ControllerBase
    {
        [HttpPost]
        public ActionResult<int> AddProduct(string name, string description, decimal price)
        {
            // Implement adding a new product

            using (StorageContext storageContext = new StorageContext())
            {
                if (storageContext.Products.Any(p => p.Name == name))
                    return StatusCode(409);

                var product = new Product() { Name = name, Description = description, Price = price };

                storageContext.Products.Add(product);
                storageContext.SaveChanges();
                return Ok(product.Id);
            }


        }


        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetAllProducts()
        {
            using (StorageContext storageContext = new StorageContext())
            {
                var list = storageContext.Products.Select(p => new Product { Name = p.Name, Description = p.Description, Price = p.Price }).ToList();

                return Ok(list);
            }
        }


        [HttpDelete(template: "deleteproduct")]
        public ActionResult DeleteProduct(string productName)
        {
            try
            {

                using (var context = new StorageContext())
                {
                    if (!context.Products.Any(x => x.Name.ToLower() == productName.ToLower()))
                    {
                        return StatusCode(404);
                    }
                    else
                    {
                        var product = context.Products.FirstOrDefault(x => x.Name == productName);
                        if (product != null)
                        {
                            context.Remove(product);
                            context.SaveChanges();
                        }
                        else
                        {
                            return StatusCode(500);
                        }
                    }
                }
                return StatusCode(200);
            }
            catch
            {
                return StatusCode(500);
            }
        }



        [HttpPost(template: "addgroup")]
        public ActionResult AddGroup(string name, string description)
        {
            try
            {
                using (var context = new StorageContext())
                {
                    if (context.ProductGroups.Any(g => g.Name == name))
                    {
                        return StatusCode(409);
                    }
                    else
                    {
                        context.ProductGroups.Add(new ProductGroup { Name = name, Description = description });
                        context.SaveChanges();
                    }

                }
                return StatusCode(200);
            }
            catch
            {
                return StatusCode(500);
            }
        }


        [HttpDelete(template: "deletegroup")]
        public ActionResult DeleteGroup(string name)
        {
            try
            {
                using (var context = new StorageContext())
                {
                    if (!context.ProductGroups.Any(x => x.Name == name))
                    {
                        return StatusCode(404);
                    }
                    else
                    {
                        var group = context.ProductGroups.FirstOrDefault(x => x.Name == name);
                        if (group != null)
                        {
                            context.ProductGroups.Remove(group);
                            context.SaveChanges();
                        }
                        else
                        {
                            return StatusCode(500);
                        }
                    }
                }
                return StatusCode(200);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpGet(template: "getgroups")]
        public ActionResult<IEnumerable<ProductGroup>> GetGroups()
        {
            try
            {
                using (var context = new StorageContext())
                {
                    var groups = context.ProductGroups
                        .Select(g => new ProductGroup
                        {
                            Id = g.Id,
                            Name = g.Name,
                            Description = g.Description
                        })
                        .ToList();
                    return groups;
                }
            }
            catch
            {
                return StatusCode(500);
            }
        }





    }
}
