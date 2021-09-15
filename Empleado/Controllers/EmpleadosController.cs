using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;

namespace Empleado.Controllers
{
    public class EmpleadosController : Controller
    {
        private readonly ILogger<EmpleadosController> _logger;

        public EmpleadosController(ILogger<EmpleadosController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            List<Models.Empleado> lista = new List<Models.Empleado>();

            if (HttpContext.Session.GetString("lista") != null)
            {
                var cadena = HttpContext.Session.GetString("lista");
                lista = JsonSerializer.Deserialize<List<Models.Empleado>>(cadena);
            }

            return View(lista);
        }

        [HttpGet]
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Crear(Models.Empleado empleado)
        {
            if (BuscarB(empleado.Codigo))
                ViewBag.Datos = "Error: existe un empleado con este codigo.";
            else
                Listar(empleado, "agregado");

            return View();
        }

        public IActionResult Detalle(int id)
        {
            return View(Buscar(id));
        }

        [HttpGet]
        public IActionResult Actualizar(int id)
        {
            return View(Buscar(id));
        }

        [HttpPost]
        public IActionResult Actualizar(Models.Empleado empleado)
        {
            EliminarE(empleado.Codigo);
            Listar(empleado, "actualizado");

            return View();
        }

        public IActionResult Eliminar(int id)
        {
            EliminarE(id);
            return Index();
        }

        public Models.Empleado Buscar(int id)
        {
            var cadena = HttpContext.Session.GetString("lista");
            List<Models.Empleado> lista = new List<Models.Empleado>();
            Models.Empleado employee = new Models.Empleado();

            if (cadena != null)
            {
                lista = JsonSerializer.Deserialize<List<Models.Empleado>>(cadena);

                foreach (var empleado in lista)
                {
                    if (empleado.Codigo == id)
                    {
                        employee.Codigo = empleado.Codigo;
                        employee.Nombre = empleado.Nombre;
                        employee.SueldoBruto = empleado.SueldoBruto;
                        employee.SueldoNeto = empleado.SueldoNeto;
                        employee.FechaIngreso = empleado.FechaIngreso;
                    }
                }
            }

            return employee;
        }

        public bool BuscarB(int id)
        {
            bool existe = false;
            var cadena = HttpContext.Session.GetString("lista");
            List<Models.Empleado> lista = new List<Models.Empleado>();
            Models.Empleado employee = new Models.Empleado();

            if (cadena != null)
            {
                lista = JsonSerializer.Deserialize<List<Models.Empleado>>(cadena);

                foreach (var empleado in lista)
                {
                    if (empleado.Codigo == id)
                    {
                        existe = true;
                    }
                }
            }

            return existe;
        }


        public void Listar(Models.Empleado empleado, string respuesta)
        {
            ViewBag.Datos = "Empleado " + respuesta;

            var lista = new List<Models.Empleado>();

            if (HttpContext.Session.GetString("lista") != null)
            {
                var cadena = HttpContext.Session.GetString("lista");
                lista = JsonSerializer.Deserialize<List<Models.Empleado>>(cadena);
            }
            empleado.SueldoNeto = empleado.SueldoBruto - (empleado.SueldoBruto * 10) / 100;

            lista.Add(empleado);
            var s = JsonSerializer.Serialize(lista);
            HttpContext.Session.SetString("lista", s);
        }

        public void EliminarE(int id)
        {
            var cadena = HttpContext.Session.GetString("lista");
            List<Models.Empleado> lista = new List<Models.Empleado>();
            Models.Empleado employee = new Models.Empleado();
            List<Models.Empleado> lista2 = new List<Models.Empleado>();


            if (cadena != null)
            {
                lista = JsonSerializer.Deserialize<List<Models.Empleado>>(cadena);

                foreach (var empleado in lista)
                {
                    if (empleado.Codigo != id)
                    {
                        lista2.Add(empleado);
                    }
                }
            }

            var s = JsonSerializer.Serialize(lista2);
            HttpContext.Session.SetString("lista", s);
        }
    }
    
}
