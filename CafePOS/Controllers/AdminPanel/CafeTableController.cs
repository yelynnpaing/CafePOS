using CafePOS.Data;
using CafePOS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace CafePOS.Controllers.AdminPanel
{
    [Route("admin/cafe-tables/")]
    public class CafeTableController : Controller
    {
        private Repository<CafeTable> _cafeTables;

        public CafeTableController(AppDbContext context)
        {
            _cafeTables = new Repository<CafeTable>(context);
        }

        [Route("list")]
        public async Task<IActionResult> List()
        {
            return View(await _cafeTables.GetAllAsync());
        }

        [Route("addEdit")]
        [HttpGet]
        public async Task<IActionResult> AddEdit(Guid id)
        {
            if(id == Guid.Empty)
            {
                ViewBag.Operation = "Table Add";
                return View(new CafeTable());
            }
            else
            {
                var table = await _cafeTables.GetByIdAsync(id, new QueryOptions<CafeTable> { Includes = "Orders" });
                if (table is null) return NotFound();
                ViewBag.Operation = "Table Edit";
                return View(table);
            }           
        }

        [Route("addEdit")]
        [HttpPost]
        public async Task<IActionResult> AddEdit(CafeTable cafeTable, Guid id)
        {
            if(cafeTable.CafeTableId == Guid.Empty)
            {
                try
                {
                    await _cafeTables.AddAsync(cafeTable);
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError("", $"Error = {ex.GetBaseException().Message}");
                    return RedirectToAction("List", "CafeTable");
                }                
            }
            else
            {
                var existingTable = await _cafeTables.GetByIdAsync(id, new QueryOptions<CafeTable> { Includes = "Orders" });
                if (existingTable is null) return NotFound();
                existingTable.TableNumber = cafeTable.TableNumber;
                existingTable.Note = cafeTable.Note;
                try
                {
                    await _cafeTables.UpdateAsync(existingTable);
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError("", $"Error = {ex.GetBaseException().Message}");
                    return View(cafeTable);
                }
            }
            return RedirectToAction("List", "CafeTable");
        }

        [Route("delete")]
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var table = await _cafeTables.GetByIdAsync(id, new QueryOptions<CafeTable> { Includes = "Orders" });
            if (table is null) return NotFound();
            await _cafeTables.DeleteAsync(table.CafeTableId);
            return RedirectToAction("List");
        }
    }
}
