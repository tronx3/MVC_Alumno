using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Alumno
    {
        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.JBedollaControlEscolarEntities context = new DL.JBedollaControlEscolarEntities())
                {
                    var alumnos = context.AlumnoGetAll().ToList();
                    if (alumnos.Count>=1)
                    {
                        result.Objects = new List<object>();
                        foreach (var item in alumnos)
                        {
                            ML.Alumno alumno = new ML.Alumno();
                            alumno.IdAlumno = item.IdAlumno;
                            alumno.Nombre = item.Nombre;
                            alumno.ApellidoPaterno = item.ApellidoPaterno;
                            alumno.ApellidoMaterno = item.ApellidoMaterno;
                            result.Objects.Add(alumno);
                        }
                        result.Correct = true;
                        result.SuccesMessage = "Ejecucion exitosa";
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No hay registros";
                    }

                }
            }
            catch (Exception  ex)
            {
                result.ErrorMessage = ex.Message;
                result.Correct = false;
            }
            return result;
        }

        public static ML.Result Add(ML.Alumno alumno)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.JBedollaControlEscolarEntities context = new DL.JBedollaControlEscolarEntities())
                {
                    var query = context.AlumnoAdd(alumno.Nombre,alumno.ApellidoPaterno,alumno.ApellidoMaterno);
                    if (query>=1)
                    {
                        result.Correct = true;
                        result.SuccesMessage = "Agregado con exito";
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se agrego, error";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }
        public static ML.Result GetById(int IdAlumno)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.JBedollaControlEscolarEntities context = new DL.JBedollaControlEscolarEntities())
                {
                    var item = context.AlumnoGetById(IdAlumno).FirstOrDefault();
                    if (item!=null)
                    {
                        ML.Alumno alumno = new ML.Alumno();
                        alumno.IdAlumno = item.IdAlumno;
                        alumno.Nombre = item.Nombre;
                        alumno.ApellidoPaterno = item.ApellidoPaterno;
                        alumno.ApellidoMaterno = item.ApellidoMaterno;
                        result.Object = alumno;
                        result.Correct = true;
                        result.SuccesMessage = "Encontrado";
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No existe registro";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }
        public static ML.Result Update(ML.Alumno alumno)
        {
            ML.Result result = new ML.Result();

            return result;
        }
    }
}
