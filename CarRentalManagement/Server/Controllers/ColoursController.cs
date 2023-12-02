using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarRentalManagement.Server.Data;
using CarRentalManagement.Shared.Domain;
using CarRentalManagement.Server.IRepository;
using System.Net.WebSockets;

namespace CarRentalManagement.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColoursController : ControllerBase
    {
        //private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        //public ColoursController(ApplicationDbContext context)
        public ColoursController(IUnitOfWork unitOfWork)
        {
            //_context = context;
            _unitOfWork = unitOfWork;
        }

        // GET: api/Colours
        [HttpGet]
        //public async Task<ActionResult<IEnumerable<Colour>>> GetColours()
        public async Task<IActionResult> GetColours()
        {
            var Colours = await _unitOfWork.Colours.GetAll();

            //if (_context.Colours == null)
            if (Colours == null)
            {
                return NotFound();
            }
            //return await _context.Colours.ToListAsync();
            return Ok(Colours);
        }

        // GET: api/Colours/5
        [HttpGet("{id}")]
        //public async Task<ActionResult<Colour>> GetColour(int id)
        public async Task<IActionResult> GetColour(int id)
        {
            //if (_context.Colours == null)
            //{
            //return NotFound();
            //}
            //var Colour = await _context.Colours.FindAsync(id);
            var Colour = await _unitOfWork.Colours.Get(q => q.Id == id);

            if (Colour == null)
            {
                return NotFound();
            }

            return Ok(Colour);
        }

        // PUT: api/Colours/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutColour(int id, Colour Colour)
        {
            if (id != Colour.Id)
            {
                return BadRequest();
            }

            //_context.Entry(Colour).State = EntityState.Modified;
            _unitOfWork.Colours.Update(Colour);

            try
            {
                //await _context.SaveChangesAsync();
                await _unitOfWork.Save(HttpContext);
            }
            catch (DbUpdateConcurrencyException)
            {
                //if (!ColourExists(id))
                if (!await ColourExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Colours
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Colour>> PostColour(Colour Colour)
        {
            //if (_context.Colours == null)
            //{
            //return Problem("Entity set 'ApplicationDbContext.Colours'  is null.");
            //}
            //_context.Colours.Add(Colour);
            //await _context.SaveChangesAsync();
            await _unitOfWork.Colours.Insert(Colour);
            await _unitOfWork.Save(HttpContext);

            return CreatedAtAction("GetColour", new { id = Colour.Id }, Colour);
        }

        // DELETE: api/Colours/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteColour(int id)
        {
            //if (_context.Colours == null)
            //{
            //return NotFound();
            //}
            //var Colour = await _context.Colours.FindAsync(id);
            var Colour = _unitOfWork.Colours.Get(q => q.Id == id);
            if (Colour == null)
            {
                return NotFound();
            }

            //_context.Colours.Remove(Colour);
            //await _context.SaveChangesAsync();
            await _unitOfWork.Colours.Delete(id);
            await _unitOfWork.Save(HttpContext);

            return NoContent();
        }

        //private bool ColourExists(int id)
        private async Task<bool> ColourExists(int id)
        {
            //return (_context.Colours?.Any(e => e.Id == id)).GetValueOrDefault();
            var Colour = await _unitOfWork.Colours.Get(q => q.Id == id);

            return Colour != null;
        }
    }
}

