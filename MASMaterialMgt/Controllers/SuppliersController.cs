using MASMaterialMgt.Data;
using MASMaterialMgt.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;

namespace MASMaterialMgt.Controllers
{
    [Authorize]
    public class SuppliersController : Controller
    {
        private readonly MASDbContext context;

        public SuppliersController(MASDbContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            try
            {
                var suppliers = context.Suppliers.OrderByDescending(s => s.Id).ToList();
                return View(suppliers);
            }
            catch (Exception ex)
            {
                
                return RedirectToAction("Error", "Home"); 
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(SupplierDto supplierDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(supplierDto);
                }

                //save data
                Supplier supplier = new Supplier()
                {
                    Name = supplierDto.Name,
                    Address = supplierDto.Address,
                    Phone = supplierDto.Phone,
                    Website = supplierDto.Website,
                    Activity = supplierDto.Activity,
                    CreatedAt = DateTime.Now,
                };

                context.Suppliers.Add(supplier);
                context.SaveChanges();

                return RedirectToAction("Index", "Suppliers");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home"); 
            }
        }

        public IActionResult Edit(int id)
        {
            try
            {
                var supplier = context.Suppliers.Find(id);

                if (supplier == null)
                {
                    return RedirectToAction("Index", "Suppliers");
                }

                var supplierDto = new SupplierDto()
                {
                    Name = supplier.Name,
                    Address = supplier.Address,
                    Phone = supplier.Phone,
                    Website = supplier.Website,
                    Activity = supplier.Activity,
                };

                ViewData["SupplierId"] = supplier.Id;
                ViewData["CreatedAt"] = supplier.CreatedAt.ToString("MM/dd/yyyy");

                return View(supplierDto);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home"); 
            }
        }

        [HttpPost]
        public IActionResult Edit(int id, SupplierDto supplierDto)
        {
            try
            {
                var supplier = context.Suppliers.Find(id);

                if (supplier == null)
                {
                    return RedirectToAction("Index", "Suppliers");
                }

                if (!ModelState.IsValid)
                {
                    ViewData["SupplierId"] = supplier.Id;
                    ViewData["CreatedAt"] = supplier.CreatedAt.ToString("MM/dd/yyyy");

                    return View(supplierDto);
                }

                //update data
                supplier.Name = supplierDto.Name;
                supplier.Address = supplierDto.Address;
                supplier.Phone = supplierDto.Phone;
                supplier.Website = supplierDto.Website;
                supplier.Activity = supplierDto.Activity;

                context.SaveChanges();

                return RedirectToAction("Index", "Suppliers");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home"); 
            }
        }

        public IActionResult Delete(int id)
        {
            try
            {
                var supplier = context.Suppliers.Find(id);
                if (supplier == null)
                {
                    return RedirectToAction("Index", "Suppliers");
                }

                context.Suppliers.Remove(supplier);
                context.SaveChanges(true);

                return RedirectToAction("Index", "Suppliers");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
