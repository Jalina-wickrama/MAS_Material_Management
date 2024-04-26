using MASMaterialMgt.Data;
using MASMaterialMgt.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;

namespace MASMaterialMgt.Controllers
{
    [Authorize]
    public class MaterialsController : Controller
    {
        private readonly MASDbContext context;
        private readonly IWebHostEnvironment environment;

        public MaterialsController(MASDbContext context, IWebHostEnvironment environment)
        {
            this.context = context;
            this.environment = environment;
        }

        public IActionResult Index(string sortOrder, string searchString)
        {
            try
            {
                ViewData["CurrentFilter"] = searchString;
                ViewData["NameSortParam"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewData["PriceSortParam"] = sortOrder == "Price" ? "price_desc" : "Price";
                ViewData["QtySortParam"] = sortOrder == "Qty" ? "qty_desc" : "Qty";
                ViewData["SupplierSortParam"] = sortOrder == "Supplier" ? "supplier_desc" : "Supplier";

                var materials = from m in context.Materials select m;

                if (!string.IsNullOrEmpty(searchString))
                {
                    materials = materials.Where(m => m.Name.Contains(searchString) || m.Supplier.Contains(searchString));
                }

                switch (sortOrder)
                {
                    case "name_desc":
                        materials = materials.OrderByDescending(m => m.Name);
                        break;
                    case "Price":
                        materials = materials.OrderBy(m => m.Unit_Price);
                        break;
                    case "price_desc":
                        materials = materials.OrderByDescending(m => m.Unit_Price);
                        break;
                    case "Qty":
                        materials = materials.OrderBy(m => m.Qty);
                        break;
                    case "qty_desc":
                        materials = materials.OrderByDescending(m => m.Qty);
                        break;
                    case "Supplier":
                        materials = materials.OrderBy(m => m.Supplier);
                        break;
                    case "supplier_desc":
                        materials = materials.OrderByDescending(m => m.Supplier);
                        break;
                    default:
                        materials = materials.OrderBy(m => m.Name);
                        break;
                }

                return View(materials.ToList());
            }
            catch (Exception ex)
            {
                // Log in
                return StatusCode(500); 
            }
        }

        public IActionResult Create()
        {
            try
            {
                var suppliers = context.Suppliers.ToList();
                ViewBag.Suppliers = suppliers;
                return View();
            }
            catch (Exception ex)
            {
                return StatusCode(500); 
            }
        }

        [HttpPost]
        public IActionResult Create(MaterialDto materialDto)
        {
            try
            {
                if (materialDto.ImageFile == null)
                {
                    ModelState.AddModelError("ImageFile", "The image file is required");
                }

                if (!ModelState.IsValid)
                {
                    return View(materialDto);
                }

                //Save Image
                string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                newFileName += Path.GetExtension(materialDto.ImageFile!.FileName);

                string imageFullPath = Path.Combine(environment.WebRootPath, "materials", newFileName);
                using (var stream = System.IO.File.Create(imageFullPath))
                {
                    materialDto.ImageFile.CopyTo(stream);
                }

                //save data
                Material material = new Material()
                {
                    ImageFileName = newFileName,
                    Name = materialDto.Name,
                    Description = materialDto.Description,
                    Unit_Price = materialDto.Unit_Price,
                    Qty = materialDto.Qty,
                    Supplier = materialDto.Supplier,
                    CreatedAt = DateTime.Now,
                };

                context.Materials.Add(material);
                context.SaveChanges();

                return RedirectToAction("Index", "Materials");
            }
            catch (Exception ex)
            {
                return StatusCode(500); 
            }
        }

        public IActionResult Edit(int id)
        {
            try
            {
                var material = context.Materials.Find(id);

                if (material == null)
                {
                    return RedirectToAction("Index", "Materials");
                }

                var materialDto = new MaterialDto()
                {
                    Name = material.Name,
                    Description = material.Description,
                    Unit_Price = material.Unit_Price,
                    Qty = material.Qty,
                    Supplier = material.Supplier,
                };

                ViewData["MaterialId"] = material.Id;
                ViewData["ImageFileName"] = material.ImageFileName;
                ViewData["CreatedAt"] = material.CreatedAt.ToString("MM/dd/yyyy");

                return View(materialDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500); 
            }
        }

        [HttpPost]
        public IActionResult Edit(int id, MaterialDto materialDto)
        {
            try
            {
                var material = context.Materials.Find(id);

                if (material == null)
                {
                    return RedirectToAction("Index", "Materials");
                }

                if (!ModelState.IsValid)
                {
                    ViewData["MaterialId"] = material.Id;
                    ViewData["ImageFileName"] = material.ImageFileName;
                    ViewData["CreatedAt"] = material.CreatedAt.ToString("MM/dd/yyyy");

                    return View(materialDto);
                }

                //update Image
                string newFileName = material.ImageFileName;
                if (materialDto.ImageFile != null)
                {
                    newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    newFileName += Path.GetExtension(materialDto.ImageFile!.FileName);

                    string imageFullPath = Path.Combine(environment.WebRootPath, "materials", newFileName);
                    using (var stream = System.IO.File.Create(imageFullPath))
                    {
                        materialDto.ImageFile.CopyTo(stream);
                    }

                    //delete old img
                    string oldImageFullPath = Path.Combine(environment.WebRootPath, "materials", material.ImageFileName);
                    System.IO.File.Delete(oldImageFullPath);
                }

                //update data
                material.ImageFileName = newFileName;
                material.Name = materialDto.Name;
                material.Description = materialDto.Description;
                material.Unit_Price = materialDto.Unit_Price;
                material.Qty = materialDto.Qty;
                material.Supplier = materialDto.Supplier;

                context.SaveChanges();

                return RedirectToAction("Index", "Materials");
            }
            catch (Exception ex)
            {
                return StatusCode(500); 
            }
        }

        public IActionResult Delete(int id)
        {
            try
            {
                var material = context.Materials.Find(id);
                if (material == null)
                {
                    return RedirectToAction("Index", "Materials");
                }

                string imageFullPath = Path.Combine(environment.WebRootPath, "materials", material.ImageFileName);
                System.IO.File.Delete(imageFullPath);

                context.Materials.Remove(material);
                context.SaveChanges(true);

                return RedirectToAction("Index", "Materials");
            }
            catch (Exception ex)
            {

                return StatusCode(500); 
            }
        }
    }
}
