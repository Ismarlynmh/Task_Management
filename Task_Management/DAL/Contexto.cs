using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using Task_Management.Model;

namespace Task_Management.DAL
{
    public class Contexto : DbContext
    {
        public DbSet<TaskModel> TaskModel { get; set; }
        public DbSet<User> User { get; set; }

        public Contexto(DbContextOptions<Contexto> options) : base(options)
        {
        }
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);
        //    modelBuilder.Entity<TaskModel>()
        //    .HasOne(t => t.User)
        //    .WithMany(u => u.TaskModels)
        //    .HasForeignKey(t => t.UserId);

        //}
    }
}
