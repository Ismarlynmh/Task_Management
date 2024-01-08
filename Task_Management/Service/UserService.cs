using Task_Management.Model;
using Task_Management.DAL;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using BCryptNet = BCrypt.Net.BCrypt;

public class UserService
{
    private readonly Contexto _contexto;

    public UserService(Contexto contexto)
    {
        _contexto = contexto;
    }

    public List<User> GetUsers()
    {
        return _contexto.User.ToList();
    }

    public bool Login(User loginUser)
    {
        // Buscar al usuario por correo electrónico
        var user = _contexto.User.FirstOrDefault(u => u.Email == loginUser.Email);

        if (user != null)
        {
            // Verificar la contraseña utilizando BCrypt
            if (BCrypt.Net.BCrypt.Verify(loginUser.Password, user.Password))
            {
                return true;
            }
        }

        return false;
    }
    private string EncryptPassword(string password)
    {
        // Define la complejidad del cifrado (cost factor)
        // Cuanto mayor sea el valor, más seguro pero más lento será
        int workFactor = 12;

        // Genera un hash de la contraseña usando BCrypt
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, workFactor);

        return hashedPassword;
    }



    public void AddUser(User user)
    {
        // Asegurarnos de que la contraseña esté cifrada antes de almacenarla en la base de datos
        user.Password = EncryptPassword(user.Password);
        _contexto.User.Add(user);
        _contexto.SaveChanges();
    }

}
