using Server.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Server.Data.Db;

public static class DbInitializer
{
    public static void Seed(AppDbContext context)
    {
        //if (context.Users.Any()) return;

        //var manager = new User
        //{
            //Username = "manager",
            //PasswordHash = BCrypt.Net.BCrypt.HashPassword("manager123"),
            //Role = "Manager"
        //};

        //var operatorUser = new User
        //{
            //Username = "operator",
            //PasswordHash = BCrypt.Net.BCrypt.HashPassword("operator123"),
            //Role = "Operator"
        //};

        //context.Users.AddRange(manager, operatorUser);
        //context.SaveChanges();
    }
}
