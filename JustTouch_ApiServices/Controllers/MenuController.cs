using JustTouch_ApiServices.Helpers;
using JustTouch_ApiServices.SupabaseService;
using JustTouch_Shared.Dtos;
using JustTouch_Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JustTouch_ApiServices.Controllers
{
    [Route("api/menu")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly ISupabaseRepository supabase;

        public MenuController(ISupabaseRepository _supabase)
        {
            supabase = _supabase;
        }

        [HttpGet("PublicMenu/{branchCode}")]
        public async Task<IActionResult> GetPublicMenu(string branchCode)
        {
            try
            {
                var branchMenu = await supabase.GetWith<Branch>(x => x.BranchCode == branchCode, ["Menu", "Product"]);
                var menuWrapped = new PublicMenuDto();
                var error = new ErrorResponse()
                {
                    Message = "No pudimos encontrar una sucursal asociada al codigo",
                    StatusCode = 404
                };

                if (branchMenu == null) return NotFound(error);
                menuWrapped.branch = Mapper.Map<Branch, BranchDto>(branchMenu);
                if (branchMenu.Menus != null)
                {
                    foreach (var c in branchMenu.Menus)
                    {
                        var catalog = Mapper.Map<Menu, CatalogDto>(c);
                        if (c.Products!.Count > 0)
                        {
                            foreach (var p in c.Products!)
                            {
                                var productWrapped = Mapper.Map<Product, ProductDto>(p);
                                catalog.products.Add(productWrapped);
                            }
                        }
                        menuWrapped.catalogs.Add(catalog);
                    }
                }
                var images = menuWrapped.catalogs.Select(async x =>
                {
                    foreach (var p in x.products)
                    {
                        if (p.pictureUrl != null)
                        {
                            var imageUrl = await supabase.GetFromBucket("justtouch", "Product_Images", p.pictureUrl);
                            p.signedUrl = imageUrl ?? string.Empty;
                        }
                    }
                });
                await Task.WhenAll(images);
                return Ok(menuWrapped);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [Authorize]
        [HttpGet("{branchCode}")]
        public async Task<IActionResult> GetMenu(string branchCode)
        {
            try
            {
                var branchMenu = await supabase.GetWith<Branch>(x => x.BranchCode == branchCode, ["Menu", "Product"]);
                var menuWrapped = new MenuDto();
                var error = new ErrorResponse()
                {
                    Message = "No pudimos encontrar una sucursal asociada al codigo",
                    StatusCode = 404
                };

                if (branchMenu == null) return NotFound(error);
                if (branchMenu.Menus != null)
                {
                    foreach (var c in branchMenu.Menus)
                    {
                        var catalog = Mapper.Map<Menu, CatalogDto>(c);
                        if (c.Products!.Count > 0)
                        {
                            foreach (var p in c.Products!)
                            {
                                var productWrapped = Mapper.Map<Product, ProductDto>(p);
                                catalog.products.Add(productWrapped);
                            }
                        }
                        menuWrapped.catalogs.Add(catalog);
                    }
                }
                var images = menuWrapped.catalogs.Select(async x =>
                {
                    foreach (var p in x.products)
                    {
                        if (p.pictureUrl != null)
                        {
                            var imageUrl = await supabase.GetFromBucket("justtouch", "Product_Images", p.pictureUrl);
                            p.signedUrl = imageUrl ?? string.Empty;
                        }
                    }
                });
                await Task.WhenAll(images);
                return Ok(menuWrapped);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [Authorize]
        [HttpGet("Catalog/{catalogCode}")]
        public async Task<IActionResult> GetCatalog(string catalogCode)
        {
            try
            {
                var data = await supabase.GetBy<Menu>(x => x.CatalogCode == catalogCode);
                var catalog = data.FirstOrDefault();
                if (catalog == null)
                {
                    var error = new ErrorResponse()
                    {
                        Message = "No se encontro catalogo con este codigo",
                        StatusCode = 404
                    };
                    return NotFound(error);
                }

                var catalogWrapped = Mapper.Map<Menu, CatalogDto>(catalog);
                if (catalog.Products!.Count > 0)
                {
                    foreach (var p in catalog.Products)
                    {
                        var pWrapped = Mapper.Map<Product, ProductDto>(p);
                        catalogWrapped.products.Add(pWrapped);
                    }
                }

                var images = catalogWrapped.products.Select(async p =>
                {
                    string? imageUrl = null;
                    if (p.pictureUrl != null)
                    {
                        imageUrl = await supabase.GetFromBucket("justtouch", "Product_Images", p.pictureUrl)!;
                    }
                    p.pictureUrl = imageUrl!;
                });

                await Task.WhenAll(images);
                return Ok(catalogWrapped);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [Authorize]
        [HttpPost("NewCatalog")]
        public async Task<IActionResult> InsertCatalog(CatalogDto catalog)
        {
            try
            {
                var catalogWrapped = Mapper.Map<CatalogDto, Menu>(catalog);
                var newCatalog = await supabase.Insert(catalogWrapped);
                var error = new ErrorResponse()
                {
                    Message = "No pudimos añadir el nuevo catalogo al menu",
                    StatusCode = 400
                };
                if (newCatalog == null) return BadRequest(error);

                return Ok(newCatalog);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [Authorize]
        [HttpPost("UpdateCatalog")]
        public async Task<IActionResult> UpdateCatalog(CatalogDto catalog)
        {
            try
            {
                var newProducts = catalog.products.Where(x => x.id == 0).ToList();
                var updatedProducts = catalog.products.Where(x => x.id == 1).ToList();

                var menuWrapped = Mapper.Map<CatalogDto, Menu>(catalog);
                var updatedMenu = await supabase.Update(menuWrapped, x => x.CatalogCode == menuWrapped.CatalogCode);
                var error = new ErrorResponse()
                {
                    Message = "No pudimos actualizar el catalogo",
                    StatusCode = 400
                };

                if (updatedMenu == null) return BadRequest(error);

                if (newProducts.Count > 0)
                {
                    foreach (var p in newProducts)
                    {
                        var pWrapped = Mapper.Map<ProductDto, Product>(p);
                        pWrapped.MenuId = updatedMenu.IdMenu;
                        var pushedProduct = await supabase.Insert(pWrapped);
                        if (pushedProduct == null) return BadRequest(error);
                    }
                }

                if (updatedProducts.Count > 0)
                {
                    foreach (var p in updatedProducts)
                    {
                        var pWrapped = Mapper.Map<ProductDto, Product>(p);
                        pWrapped.MenuId = updatedMenu.IdMenu;

                        var UpdatedProduct = await supabase.Update(pWrapped, x => x.ProductCode == pWrapped.ProductCode);
                        if (UpdatedProduct == null) return BadRequest(error);
                    }
                }

                return Ok(true);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }
}
