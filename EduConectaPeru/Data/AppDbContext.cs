using Microsoft.EntityFrameworkCore;
using EduConectaPeru.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace EduConectaPeru.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<PaymentStatus> PaymentStatus { get; set; }
        public DbSet<PaymentType> PaymentTypes { get; set; }
        public DbSet<LegalGuardian> LegalGuardians { get; set; }
        public DbSet<Docente> Docentes { get; set; }
        public DbSet<GradoSeccion> GradoSecciones { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Horario> Horarios { get; set; }
        public DbSet<AsignacionDocente> AsignacionesDocentes { get; set; }
        public DbSet<Matricula> Matriculas { get; set; }
        public DbSet<Quota> Quotas { get; set; }
        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Matricula>()
                .Property(m => m.MontoMatricula)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Quota>()
                .Property(q => q.Monto)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Payment>()
                .Property(p => p.MontoTotal)
                .HasPrecision(10, 2);

            modelBuilder.Entity<Student>()
                .HasOne(s => s.LegalGuardian)
                .WithMany(lg => lg.Students)
                .HasForeignKey(s => s.LegalGuardianId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Horario>()
                .HasOne(h => h.GradoSeccion)
                .WithMany(gs => gs.Horarios)
                .HasForeignKey(h => h.GradoSeccionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AsignacionDocente>()
                .HasOne(ad => ad.Docente)
                .WithMany(d => d.AsignacionesDocentes)
                .HasForeignKey(ad => ad.DocenteId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AsignacionDocente>()
                .HasOne(ad => ad.Horario)
                .WithMany(h => h.AsignacionesDocentes)
                .HasForeignKey(ad => ad.HorarioId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Matricula>()
                .HasOne(m => m.Student)
                .WithMany(s => s.Matriculas)
                .HasForeignKey(m => m.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Matricula>()
                .HasOne(m => m.LegalGuardian)
                .WithMany(lg => lg.Matriculas)
                .HasForeignKey(m => m.LegalGuardianId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Matricula>()
                .HasOne(m => m.GradoSeccion)
                .WithMany(gs => gs.Matriculas)
                .HasForeignKey(m => m.GradoSeccionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Quota>()
                .HasOne(q => q.Matricula)
                .WithMany(m => m.Quotas)
                .HasForeignKey(q => q.MatriculaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Quota>()
                .HasOne(q => q.Student)
                .WithMany(s => s.Quotas)
                .HasForeignKey(q => q.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Quota>()
                .HasOne(q => q.PaymentStatus)
                .WithMany(ps => ps.Quotas)
                .HasForeignKey(q => q.PaymentStatusId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Student)
                .WithMany(s => s.Payments)
                .HasForeignKey(p => p.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.PaymentType)
                .WithMany(pt => pt.Payments)
                .HasForeignKey(p => p.PaymentTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Bank)
                .WithMany(b => b.Payments)
                .HasForeignKey(p => p.BankId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}