using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SL.Controllers
{
    public class AlumnoController : ApiController
    {
        // GET api/<controller>
        [Route("api/alumno")]
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            ML.Result result = BL.Alumno.GetAll();
            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return Content(HttpStatusCode.NotFound, result);
            }
        }

        // GET api/<controller>/5
        [Route("api/alumno/{IdAlumno}")]
        [HttpGet]
        public IHttpActionResult GetById(int IdAlumno)
        {
            ML.Result result = BL.Alumno.GetById(IdAlumno);
            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return Content(HttpStatusCode.NotFound, result);
            }
        }

        // POST api/<controller>
        [Route("api/alumno")]
        [HttpPost]
        public IHttpActionResult Post([FromBody]ML.Alumno alumno)
        {
            ML.Result result = BL.Alumno.Add(alumno);
            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return Content(HttpStatusCode.NotImplemented, result);
            }
        }

        // PUT api/<controller>/5
        [Route("api/alumno/{IdAlumno}")]
        [HttpPut]
        public IHttpActionResult Put(int IdAlumno, [FromBody]ML.Alumno alumno)
        {
            alumno.IdAlumno = IdAlumno;
            ML.Result result = BL.Alumno.Update(alumno);
            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return Content(HttpStatusCode.NotModified, result);
            }
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
            // metodo sin realizar
        }
    }
}