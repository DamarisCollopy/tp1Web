using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClassLibraryFriendly;
using ClassLibraryFriendly.Models;
using Microsoft.AspNetCore.Http;

namespace WebLibraryFriends.Controllers
{
    public class FriendsController : Controller
    {
        private readonly ListaAmigosContext _context;

        public FriendsController(ListaAmigosContext context)
        {
            _context = context;
        }

        // GET: Friends
        public async Task<IActionResult> Index()
        {
            return View(await _context.Table.ToListAsync());
        }

        // GET: Friends/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var table = await _context.Table
                .FirstOrDefaultAsync(m => m.Id == id);
            if (table == null)
            {
                return NotFound();
            }

            return View(table);
        }

        // GET: Friends/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Friends/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Sobrenome,Email,DataNascimento")] Table table)
        {
            if (ModelState.IsValid)
            {
                string data = table.DataNascimento.ToString("dd/MM/yyyy HH:mm:ss");
                CookieOptions option = new CookieOptions();
                option.Expires = DateTime.Now.AddMinutes(10);
                Response.Cookies.Append("nome", table.Nome, option);
                Response.Cookies.Append("Sobrenome", table.Sobrenome, option);
                Response.Cookies.Append("email", table.Email, option);
                Response.Cookies.Append("data", data, option);

                return RedirectToAction("SalvarInformacao");

                //if (ModelState.IsValid)
            //{
               // _context.Add(table);
                //await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));

            }
            return View(table);
        }

        public IActionResult SalvarInformacao()
        {
            string nome = Request.Cookies["Nome"];
            string sobrenome = Request.Cookies["sobrenome"];
            string email = Request.Cookies["Email"];
            string data = Request.Cookies["Data"];
            DateTime dataConverte = DateTime.Parse(data);

            if (nome == null)
            {
                ViewBag.Dados = "No cookie found";
            }
            else
            {
                ViewData["Message"] = new Table()
                {
                    Nome = nome,
                    Sobrenome = sobrenome,
                    Email = email,
                    DataNascimento = dataConverte,
                };

                return View();
            }
            return View();
        }

        // GET: Friends/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var table = await _context.Table.FindAsync(id);
            if (table == null)
            {
                return NotFound();
            }
            return View(table);
        }

        // POST: Friends/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Sobrenome,Email,DataNascimento")] Table table)
        {
            if (id != table.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(table);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TableExists(table.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(table);
        }

        // GET: Friends/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var table = await _context.Table
                .FirstOrDefaultAsync(m => m.Id == id);
            if (table == null)
            {
                return NotFound();
            }

            return View(table);
        }

        // POST: Friends/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var table = await _context.Table.FindAsync(id);
            _context.Table.Remove(table);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TableExists(int id)
        {
            return _context.Table.Any(e => e.Id == id);
        }
    }
}
