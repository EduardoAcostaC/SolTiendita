using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tiendita.Models;

namespace Tiendita.Controllers
{
    public class HomeController : Controller
    {
        Generacion22Entities db = new Generacion22Entities();

        // GET: Home
        public ActionResult Index()
        {
            try
            {
                List<Productos> lista = db.Productos.Include("Departamentos").ToList();
                return View(lista);
            }
            catch (Exception)
            {

                throw;
            }
        }

        // GET: Home/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Home/Create
        public ActionResult Create()
        {
            try
            {
                List<Departamentos> lista = db.Departamentos.OrderBy(x => x.nombreDepartamento).ToList();
                ViewBag.departamentoId = new SelectList(lista, "idDepartamento", "nombreDepartamento");
                return View();
            }
            catch (Exception)
            {

                throw;
            }
        }

        // POST: Home/Create
        [HttpPost]
        public ActionResult Create(Productos obj)
        {
            try
            {
                // TODO: Add insert logic here
                EsFechaPasada(obj);
                this.ValidarProductosRepetidos(obj.nombreProducto, obj.departamentoId);
                db.Productos.Add(obj);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                TempData["error"] = ex.Message;
                List<Departamentos> lista = db.Departamentos.OrderBy(x => x.nombreDepartamento).ToList();
                ViewBag.departamentoId = new SelectList(lista, "idDepartamento", "nombreDepartamento");
                return View();
            }
        }

        // GET: Home/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                Productos obj = db.Productos.Find(id);

                List<Departamentos> lista = db.Departamentos.OrderBy(x => x.nombreDepartamento).ToList();
                ViewBag.departamentoId = new SelectList(lista, "idDepartamento", "nombreDepartamento", obj.departamentoId);
                return View(obj);
            }
            catch (Exception)
            {

                throw;
            }
        }

        // POST: Home/Edit/5
        [HttpPost]
        public ActionResult Edit(Productos obj)
        { 
            try
            {
                // TODO: Add update logic here
                EsFechaPasada(obj);
                ValidarProductosEditar(obj.nombreProducto, obj.departamentoId, obj.idProducto);
                db.Productos.AddOrUpdate(obj);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("Edit", new {id = obj.idProducto});
            }
        }

        // GET: Home/Delete/5
        public ActionResult Delete(int id)
        {
            Productos obj = db.Productos.Find(id);
            db.Productos.Remove(obj);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // POST: Home/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        private void ValidarProductosRepetidos(string nombre, int id)
        {
            Productos obj = db.Productos.Where(x => x.nombreProducto == nombre && x.departamentoId == id).FirstOrDefault();
            if(obj != null)
            {
                throw new Exception("El producto" + nombre + " ya existe");
            }
        }

        private void ValidarProductosEditar(string nombre, int idDep, int id)
        {
            Productos obj = db.Productos.Where(x => x.nombreProducto == nombre && x.departamentoId == idDep && x.idProducto != id).FirstOrDefault();
            if (obj != null)
            {
                throw new Exception("El producto" + nombre + " ya existe");
            }
        }

        public ActionResult Buscar(string valor)
        {
            try
            {
                List<Productos> lista = db.Productos.Include("Departamentos").Where(x => x.nombreProducto.Contains(valor) || x.Departamentos.nombreDepartamento.Contains(valor)).ToList();
                return View("Index", lista);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void EsFechaPasada (Productos obj)
        {
            DateTime fechaActual = DateTime.Now;
            if (obj.fechaReabastecimiento < fechaActual)
            {
                throw new Exception("La fecha seleccionada es una fecha pasada. Por favor seleciona una fecha valida.");
            }
        }
        



    }
}
