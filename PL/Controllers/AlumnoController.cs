using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Threading.Tasks;//Metodos asincronos

namespace PL.Controllers
{
    public class AlumnoController : Controller
    {
        // GET: Alumno
        //[HttpGet]
        //public ActionResult GetAll()
        //{
        //    ML.Result result = BL.Alumno.GetAll();
        //    ML.Alumno alumno = new ML.Alumno();
        //    alumno.Alumnos = result.Objects.ToList();
        //    return View(alumno);
        //}
        //WebService WCF
        //=======================================================================
        [HttpGet]
        public ActionResult GetAll()
        {
            ML.Alumno alumno = new ML.Alumno();
            try
            {
                ServiceReferenceAlumno.AlumnoClient alumnoClient = new ServiceReferenceAlumno.AlumnoClient();
                var result = alumnoClient.GetAll();
                alumno.Alumnos = result.Objects.ToList();
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
            }
            return View(alumno);
        }
        //Asincrono
        /*[HttpGet]
        public async Task<ActionResult> GetAll()
        {
            ML.Alumno alumno = new ML.Alumno();
            try
            {
                ServiceReferenceAlumno.AlumnoClient alumnoClient = new ServiceReferenceAlumno.AlumnoClient();
                var result = await alumnoClient.GetAllAsync();
                alumno.Alumnos = result.Objects.ToList();

            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
            }
            return View(alumno);
        }*/
        //=======================================================================
        //WebAPI
        //=======================================================================
        [HttpGet]
        public ActionResult GetAllAPI()
        {
            ML.Alumno alumno = new ML.Alumno();
            alumno.Alumnos = new List<object>();
            using (var cliente = new HttpClient())
            {
                cliente.BaseAddress = new Uri("http://localhost:50404/");
                var responseTask = cliente.GetAsync("api/alumno");
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<ML.Result>();
                    readTask.Wait();
                    foreach (var item in readTask.Result.Objects)
                    {
                        ML.Alumno resultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Alumno>(item.ToString());
                        alumno.Alumnos.Add(resultItemList);
                    }
                }
            }
            return View(alumno);
        }
        //=======================================================================
        /*[HttpGet]
        public ActionResult Form(int? IdAlumno)
        {
            ML.Result result = new ML.Result();
            ML.Alumno alumno = new ML.Alumno();
            if (IdAlumno==null)
            {
                return View(alumno);
            }
            else
            {
                result = BL.Alumno.GetById(IdAlumno.Value);
                if (result.Correct)
                {
                    alumno = (ML.Alumno)result.Object;
                    return View(alumno);
                }
                else
                {
                    ViewBag.Message = result.ErrorMessage;
                    return PartialView("Modal");//aprende a crear el modal, es un mensaje generico para errores
                }
            }
        }
        [HttpPost]
        public ActionResult Form(ML.Alumno alumno)
        {
            ML.Result result = new ML.Result();
            if (alumno.IdAlumno==0)
            {
                result = BL.Alumno.Add(alumno);
                ViewBag.Message = "Agregado con exito";
            }
            else
            {
                result = BL.Alumno.Update(alumno);
                ViewBag.Message = "Actualizado";
            }
            if (!result.Correct)
            {
                ViewBag.Message = result.ErrorMessage;
            }
            return PartialView("Modal");
        }*/
        [HttpGet]
        public ActionResult Form(int? IdAlumno)
        {
            ML.Alumno alumno = new ML.Alumno();
            if (IdAlumno==null)
            {
                return View(alumno);
            }
            else
            {
                try
                {
                    using (var cliente = new HttpClient())
                    {
                        cliente.BaseAddress = new Uri("http://localhost:50404/");
                        var responseTask = cliente.GetAsync("api/alumno/" + IdAlumno);
                        responseTask.Wait();
                        var result = responseTask.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            var readTask = result.Content.ReadAsAsync<ML.Result>();
                            readTask.Wait();
                            ML.Alumno alumnoItem = new ML.Alumno();
                            alumnoItem = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Alumno>(readTask.Result.Object.ToString());
                            alumno = alumnoItem;
                        }
                        else
                        {
                            ViewBag.Message = "Ocurrio error con servicio";
                            return PartialView("Modal");
                        }
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Message = ex.Message;
                }
            }
            return View(alumno);
        }

        [HttpPost]
        public ActionResult Form(ML.Alumno alumno)
        {
            try
            {
                using (var cliente = new HttpClient())
                {
                    cliente.BaseAddress = new Uri("http://localhost:50404/");
                    if (alumno.IdAlumno == 0)
                    {
                        var postTask = cliente.PostAsJsonAsync<ML.Alumno>("api/alumno", alumno);
                        postTask.Wait();
                        var result = postTask.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            return RedirectToAction("GetAll");
                        }
                    }
                    else
                    {
                        var putTask = cliente.PutAsJsonAsync<ML.Alumno>("api/alumno/" + alumno.IdAlumno, alumno);
                        putTask.Wait();
                        var result = putTask.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            return RedirectToAction("GetAll");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return PartialView("Modal");
            }
            return View("GetAll");// estoy en duda puede ser redirect 
        }
    }
}