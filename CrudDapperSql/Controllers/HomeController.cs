using CrudDapperSql.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CrudDapperSql.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private string _connection = "Server=127.0.0.1;Port=3306;Database=gestionpedidos;Uid=root;AllowUserVariables=True;";

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Productos> listProductos = null;
            using (var db = new MySqlConnection(_connection))
            {
                var sql = "select * from productos";
                listProductos = await db.QueryAsync<Productos>(sql);
            }
            return Json(listProductos);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
