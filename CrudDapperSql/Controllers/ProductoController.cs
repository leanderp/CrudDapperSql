using CrudDapperSql.Models;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrudDapperSql.Controllers
{
    public class ProductoController : Controller
    {
        private readonly IConfiguration _configuration;
        public ProductoController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET: ProductoController       
        public async Task<ActionResult> Index()
        {
            IEnumerable<Producto> listProductos = null;
            using (var db = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                db.Open();
                var sql = "SELECT * FROM productos";
                listProductos = await db.QueryAsync<Producto>(sql);
            }
            return View(listProductos);
        }

        // GET: ProductoController/Details?codigoArticulo="AR01"
        [HttpGet]
        public async Task<ActionResult<Producto>> Details (string codigoArticulo)
        {
            if (codigoArticulo == null)
            {
                return NotFound();
            }

            var producto = new Producto();

            try
            {
                using (var db = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    db.Open();
                    var sql = "SELECT * FROM productos WHERE CODIGOARTICULO = @CodigoArticulo";
                    producto = await db.QueryFirstOrDefaultAsync<Producto>(sql, new { CodigoArticulo = codigoArticulo });
                }

                if (producto == null)
                {
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, $"Ha ocurrido un error guardando el producto: {e}");
            }
             
            return View(producto);
        }

        // GET: ProductoController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Seccion,NombreArticulo,Precio,Fecha,Importado,PaisDeOrigen")] Producto producto)
        {
            if (producto.CodigoArticulo == null)
            {
                string nuevoCodigoArticulo = await ObtenerNuevoCodigoArticuloAsync();
                producto.CodigoArticulo = nuevoCodigoArticulo;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    using (var db = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                    {
                        db.Open();
                        var sql = " INSERT INTO productos (CODIGOARTICULO,SECCION,NOMBREARTICULO,PRECIO,FECHA,IMPORTADO,PAISDEORIGEN,FOTO) VALUES (@CodigoArticulo,@Seccion,@NombreArticulo,@Precio,@Fecha,@Importado,@PaisDeOrigen,@Foto)";
                        var result = await db.ExecuteAsync(sql, producto);
                    }
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception e)
                {
                    ModelState.AddModelError(string.Empty, $"Ha ocurrido un error guardando el producto: {e}");
                }
            }
            ModelState.AddModelError(string.Empty, $"Ha ocurrio un error");
            return View(producto);
        }

        // GET: ProductoController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProductoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductoController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProductoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        private async Task<string> ObtenerNuevoCodigoArticuloAsync()
        {
            string ultimoCodigoString;
            using (var db = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                db.Open();
                var sql = "SELECT CODIGOARTICULO FROM productos ORDER BY CODIGOARTICULO DESC LIMIT 1";
                var response = await db.QueryAsync<string>(sql);
                ultimoCodigoString = response.First();
            }
            var ultimoCodigo = Int16.Parse(ultimoCodigoString.Replace("AR", ""));
            return $"AR{ultimoCodigo + 1}";
        }
    }
}
