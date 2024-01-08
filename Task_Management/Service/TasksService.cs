using System;
using System.Collections.Generic;
using System.Linq;
using Task_Management.DAL;
using Task_Management.Model;
using Microsoft.EntityFrameworkCore;

namespace Task_Management.Service
{
    public class TasksService
    {
        private readonly Contexto _contexto;

        public TasksService(Contexto contexto)
        {
            _contexto = contexto;
        }

        public bool AddTask(TaskModel taskModel)
        {
            if (taskModel.Id == 0)
            {
                return Insertar(taskModel);
            }
            else
            {
                return Modificar(taskModel);
            }
        }

        public bool Insertar(TaskModel taskModel)
        {
            try
            {
                if (_contexto == null || _contexto.TaskModel == null)
                {
                    return false;
                }

                _contexto.TaskModel.Add(taskModel);
                int cantidad = _contexto.SaveChanges();
                return cantidad > 0;
            }
           
            catch (Exception ex)
            {
                // Manejar otras excepciones según sea necesario
                Console.WriteLine($"Otro error al insertar: {ex.Message}");
                throw;
            }
        }

        public TaskModel BuscarTask(int id)
        {
            var result = _contexto.TaskModel
                .Include(task => task.User)
                .FirstOrDefault(task => task.Id == id);

            return result;
        }


        public List<TaskModel> GetTasks()
        {
            return _contexto.TaskModel
                .Include(task => task.User) // Carga la relación con User
                .ToList();
        }

        public void ModificarTask(TaskModel taskModel)
        {
            _contexto.Entry(taskModel).State = EntityState.Modified;
            _contexto.SaveChanges();
        }

        public bool Modificar(TaskModel taskModel)
        {
            try
            {
                _contexto.Entry(taskModel).State = EntityState.Modified;
                int cantidad = _contexto.SaveChanges();
                return cantidad > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al modificar: {ex.Message}");
                throw;
            }
        }

        public bool Eliminar(int Id)
        {
            bool eliminado = false;

            try
            {
                var taskModel = _contexto.TaskModel.Find(Id);

                if (taskModel != null)
                {
                    _contexto.TaskModel.Remove(taskModel);
                    eliminado = _contexto.SaveChanges() > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar: {ex.Message}");
                throw;
            }

            return eliminado;
        }
    }
}
