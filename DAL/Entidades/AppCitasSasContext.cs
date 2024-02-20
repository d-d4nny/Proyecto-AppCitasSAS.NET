using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DAL.Entidades;

public partial class AppCitasSasContext : DbContext
{
    public AppCitasSasContext()
    {
    }

    public AppCitasSasContext(DbContextOptions<AppCitasSasContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cita> Citas { get; set; }

    public virtual DbSet<ConsultasTurno> ConsultasTurnos { get; set; }

    public virtual DbSet<Doctore> Doctores { get; set; }

    public virtual DbSet<Informe> Informes { get; set; }

    public virtual DbSet<Paciente> Pacientes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Name=PostgresConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cita>(entity =>
        {
            entity.HasKey(e => e.IdCita).HasName("citas_pkey");

            entity.ToTable("citas", "sch_sas");

            entity.Property(e => e.IdCita)
                .HasDefaultValueSql("nextval('citas_id_cita_seq'::regclass)")
                .HasColumnName("id_cita");
            entity.Property(e => e.EstadoCita)
                .HasMaxLength(255)
                .HasColumnName("estado_cita");
            entity.Property(e => e.FechaCita).HasColumnName("fecha_cita");
            entity.Property(e => e.HoraCita)
                .HasPrecision(6)
                .HasColumnName("hora_cita");
            entity.Property(e => e.IdDoctor).HasColumnName("id_doctor");
            entity.Property(e => e.IdPaciente).HasColumnName("id_paciente");
            entity.Property(e => e.MotivoCita)
                .HasMaxLength(255)
                .HasColumnName("motivo_cita");

            entity.HasOne(d => d.IdDoctorNavigation).WithMany(p => p.Cita)
                .HasForeignKey(d => d.IdDoctor)
                .HasConstraintName("fkehbckgw69ryt2otrfl2kand3q");

            entity.HasOne(d => d.IdPacienteNavigation).WithMany(p => p.Cita)
                .HasForeignKey(d => d.IdPaciente)
                .HasConstraintName("fka6jbqxi4v5ij2jdlgrmwnt94o");
        });

        modelBuilder.Entity<ConsultasTurno>(entity =>
        {
            entity.HasKey(e => e.IdConsultaTurno).HasName("consultas_turnos_pkey");

            entity.ToTable("consultas_turnos", "sch_sas");

            entity.Property(e => e.IdConsultaTurno)
                .HasDefaultValueSql("nextval('consultas_turnos_id_consulta_turno_seq'::regclass)")
                .HasColumnName("id_consulta_turno");
            entity.Property(e => e.NumConsulta).HasColumnName("num_consulta");
            entity.Property(e => e.TramoHoraTurnoFin)
                .HasPrecision(6)
                .HasColumnName("tramo_hora_turno_fin");
            entity.Property(e => e.TramoHoraTurnoInicio)
                .HasPrecision(6)
                .HasColumnName("tramo_hora_turno_inicio");
        });

        modelBuilder.Entity<Doctore>(entity =>
        {
            entity.HasKey(e => e.IdDoctor).HasName("doctores_pkey");

            entity.ToTable("doctores", "sch_sas");

            entity.Property(e => e.IdDoctor)
                .HasDefaultValueSql("nextval('doctores_id_doctor_seq'::regclass)")
                .HasColumnName("id_doctor");
            entity.Property(e => e.EspecialidadDoctor)
                .HasMaxLength(50)
                .HasColumnName("especialidad_doctor");
            entity.Property(e => e.IdConsultaTurno).HasColumnName("id_consulta_turno");
            entity.Property(e => e.NombreCompletoDoctor)
                .HasMaxLength(50)
                .HasColumnName("nombre_completo_doctor");

            entity.HasOne(d => d.IdConsultaTurnoNavigation).WithMany(p => p.Doctores)
                .HasForeignKey(d => d.IdConsultaTurno)
                .HasConstraintName("fkeg7tmbo03sia7aros017upxlj");
        });

        modelBuilder.Entity<Informe>(entity =>
        {
            entity.HasKey(e => e.IdInforme).HasName("informes_pkey");

            entity.ToTable("informes", "sch_sas");

            entity.Property(e => e.IdInforme)
                .HasDefaultValueSql("nextval('informes_id_informe_seq'::regclass)")
                .HasColumnName("id_informe");
            entity.Property(e => e.DescInforme)
                .HasMaxLength(255)
                .HasColumnName("desc_informe");
            entity.Property(e => e.FchInforme)
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("fch_informe");
            entity.Property(e => e.IdPaciente).HasColumnName("id_paciente");
            entity.Property(e => e.NombreInforme)
                .HasMaxLength(50)
                .HasColumnName("nombre_informe");

            entity.HasOne(d => d.IdPacienteNavigation).WithMany(p => p.Informes)
                .HasForeignKey(d => d.IdPaciente)
                .HasConstraintName("fk588s5l4oipo3odqij6unp8hkj");
        });

        modelBuilder.Entity<Paciente>(entity =>
        {
            entity.HasKey(e => e.IdPaciente).HasName("pacientes_pkey");

            entity.ToTable("pacientes", "sch_sas");

            entity.HasIndex(e => e.DniPaciente, "uk_1nhe0bxh333bfggnv06rlqga2").IsUnique();

            entity.HasIndex(e => e.EmailPaciente, "uk_ove2n30kxa21v6971d9j2mf1u").IsUnique();

            entity.Property(e => e.IdPaciente)
                .HasDefaultValueSql("nextval('pacientes_id_paciente_seq'::regclass)")
                .HasColumnName("id_paciente");
            entity.Property(e => e.ContraseñaPaciente)
                .HasMaxLength(100)
                .HasColumnName("contraseña_paciente");
            entity.Property(e => e.CuentaConfirmada)
                .HasDefaultValue(false)
                .HasColumnName("cuenta_confirmada");
            entity.Property(e => e.DireccionPaciente)
                .HasMaxLength(100)
                .HasColumnName("direccion_paciente");
            entity.Property(e => e.DniPaciente)
                .HasMaxLength(9)
                .HasColumnName("dni_paciente");
            entity.Property(e => e.EmailPaciente)
                .HasMaxLength(50)
                .HasColumnName("email_paciente");
            entity.Property(e => e.ExpiracionToken)
                .HasColumnType("timestamp(6) without time zone")
                .HasColumnName("expiracion_token");
            entity.Property(e => e.GeneroPaciente)
                .HasMaxLength(10)
                .HasColumnName("genero_paciente");
            entity.Property(e => e.NombreCompletoPaciente)
                .HasMaxLength(50)
                .HasColumnName("nombre_completo_paciente");
            entity.Property(e => e.ProfilePicture).HasColumnName("profile_picture");
            entity.Property(e => e.RolPaciente)
                .HasMaxLength(12)
                .HasColumnName("rol_paciente");
            entity.Property(e => e.TlfPaciente)
                .HasMaxLength(9)
                .HasColumnName("tlf_paciente");
            entity.Property(e => e.TokenRecuperacion)
                .HasMaxLength(100)
                .HasColumnName("token_recuperacion");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
