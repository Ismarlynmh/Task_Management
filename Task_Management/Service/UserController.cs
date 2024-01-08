using System;
using System.Collections.Generic;
using System.Linq;
using Task_Management.DAL;
using Task_Management.Model;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Security.Cryptography;


namespace Task_Management.Service
{
    public class UserController
    {
        private readonly Contexto _contexto;

        public UserController(Contexto contexto)
        {
            _contexto = contexto;
        }

        public bool AddUser(User user)
        {
            if (user.UserId == 0)
            {
                return Insertar(user);
            }
            else
            {
                return Modificar(user);
            }
        }

        public bool Insertar(User user)
        {
            try
            {
                if (_contexto == null || _contexto.User == null)
                {
                    return false;
                }

                _contexto.User.Add(user);
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

        public User GetUserById(int id)
        {
            var result = _contexto.User
                .Include(u => u.TaskModels)
                .FirstOrDefault(u => u.UserId == id);

            return result;
        }

        public List<User> GetUsers()
        {
            return _contexto.User
                .Include(u => u.TaskModels)
                .ToList();
        }

        public void UpdateUser(User user)
        {
            _contexto.Entry(user).State = EntityState.Modified;
            _contexto.SaveChanges();
        }

        public bool Modificar(User user)
        {
            try
            {
                _contexto.Entry(user).State = EntityState.Modified;
                int cantidad = _contexto.SaveChanges();
                return cantidad > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al modificar: {ex.Message}");
                throw;
            }
        }

        public bool DeleteUser(int userId)
        {
            bool eliminado = false;

            try
            {
                var user = _contexto.User.Find(userId);

                if (user != null)
                {
                    _contexto.User.Remove(user);
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
        public bool Validar(string nombre, string clave)
        {
            bool paso = false;

            try
            {
                string hashedClave = getHashSha256(clave);
                paso = _contexto.User.Any(u => u.Email == nombre && u.Password == hashedClave);
            }
            catch (Exception)
            {
                throw;
            }

            return paso;
        }

        public string getHashSha256(string clave)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(clave);
            SHA256Managed hashstring = new SHA256Managed();
            byte[] hash = hashstring.ComputeHash(bytes);
            string hashString = string.Empty;
            foreach (byte x in hash)
            {
                hashString += String.Format("{0:x2}", x);
            }
            return hashString;
        }

    }
}
