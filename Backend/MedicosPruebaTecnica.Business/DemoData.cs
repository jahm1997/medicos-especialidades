using MedicosPruebaTecnica.Entities;

namespace MedicosPruebaTecnica.Business
{
    public static class DemoData
    {
        public static List<Especialidad> GetEspecialidades()
        {
            return new List<Especialidad>
            {
                new Especialidad { Id = 1, Nombre = "Cardiología", Descripcion = "Especialidad médica que se encarga del estudio, diagnóstico y tratamiento de las enfermedades del corazón y del aparato circulatorio." },
                new Especialidad { Id = 2, Nombre = "Dermatología", Descripcion = "Especialidad médica que se encarga del estudio de la estructura y función de la piel, así como de las enfermedades que la afectan." },
                new Especialidad { Id = 3, Nombre = "Pediatría", Descripcion = "Especialidad médica que estudia al niño y sus enfermedades desde el nacimiento hasta la adolescencia." },
                new Especialidad { Id = 4, Nombre = "Neurología", Descripcion = "Especialidad médica que trata los trastornos del sistema nervioso." },
                new Especialidad { Id = 5, Nombre = "Ginecología", Descripcion = "Especialidad médica que se encarga de la salud del aparato reproductor femenino." }
            };
        }

        public static List<Medico> GetMedicos()
        {
            return new List<Medico>
            {
                new Medico { Id = 1, Nombre = "Dr. Carlos", Apellido = "Rodríguez", NumeroLicencia = "12345678", Email = "carlos.rodriguez@hospital.com", Telefono = "555-0101", EspecialidadId = 1 },
                new Medico { Id = 2, Nombre = "Dra. María", Apellido = "González", NumeroLicencia = "23456789", Email = "maria.gonzalez@hospital.com", Telefono = "555-0102", EspecialidadId = 1 },
                new Medico { Id = 3, Nombre = "Dr. Luis", Apellido = "Martínez", NumeroLicencia = "34567890", Email = "luis.martinez@hospital.com", Telefono = "555-0103", EspecialidadId = 2 },
                new Medico { Id = 4, Nombre = "Dra. Ana", Apellido = "López", NumeroLicencia = "45678901", Email = "ana.lopez@hospital.com", Telefono = "555-0104", EspecialidadId = 2 },
                new Medico { Id = 5, Nombre = "Dr. Pedro", Apellido = "Sánchez", NumeroLicencia = "56789012", Email = "pedro.sanchez@hospital.com", Telefono = "555-0105", EspecialidadId = 3 },
                new Medico { Id = 6, Nombre = "Dra. Carmen", Apellido = "Ruiz", NumeroLicencia = "67890123", Email = "carmen.ruiz@hospital.com", Telefono = "555-0106", EspecialidadId = 3 },
                new Medico { Id = 7, Nombre = "Dr. Miguel", Apellido = "Torres", NumeroLicencia = "78901234", Email = "miguel.torres@hospital.com", Telefono = "555-0107", EspecialidadId = 4 },
                new Medico { Id = 8, Nombre = "Dra. Isabel", Apellido = "Morales", NumeroLicencia = "89012345", Email = "isabel.morales@hospital.com", Telefono = "555-0108", EspecialidadId = 5 }
            };
        }

        public static List<Paciente> GetPacientes()
        {
            return new List<Paciente>
            {
                new Paciente { Id = 1, Nombre = "Juan", Apellido = "Pérez", Cedula = "11111111", Email = "juan.perez@email.com", Telefono = "555-1001", FechaNacimiento = new DateTime(1985, 3, 15) },
                new Paciente { Id = 2, Nombre = "María", Apellido = "García", Cedula = "22222222", Email = "maria.garcia@email.com", Telefono = "555-1002", FechaNacimiento = new DateTime(1990, 7, 22) },
                new Paciente { Id = 3, Nombre = "Carlos", Apellido = "López", Cedula = "33333333", Email = "carlos.lopez@email.com", Telefono = "555-1003", FechaNacimiento = new DateTime(1978, 11, 8) },
                new Paciente { Id = 4, Nombre = "Ana", Apellido = "Martín", Cedula = "44444444", Email = "ana.martin@email.com", Telefono = "555-1004", FechaNacimiento = new DateTime(1995, 2, 14) },
                new Paciente { Id = 5, Nombre = "Luis", Apellido = "Fernández", Cedula = "55555555", Email = "luis.fernandez@email.com", Telefono = "555-1005", FechaNacimiento = new DateTime(1982, 9, 30) },
                new Paciente { Id = 6, Nombre = "Carmen", Apellido = "Jiménez", Cedula = "66666666", Email = "carmen.jimenez@email.com", Telefono = "555-1006", FechaNacimiento = new DateTime(1988, 5, 18) }
            };
        }

        public static List<HorarioDisponible> GetHorariosDisponibles()
        {
            var horarios = new List<HorarioDisponible>();
            var medicos = GetMedicos();
            var fechaInicio = DateTime.Today;

            foreach (var medico in medicos)
            {
                // Generar horarios para los próximos 30 días
                for (int dia = 0; dia < 30; dia++)
                {
                    var fecha = fechaInicio.AddDays(dia);
                    
                    // Solo días laborables (lunes a viernes)
                    if (fecha.DayOfWeek != DayOfWeek.Saturday && fecha.DayOfWeek != DayOfWeek.Sunday)
                    {
                        // Horarios de mañana: 8:00 AM - 12:00 PM
                        for (int hora = 8; hora < 12; hora++)
                        {
                            horarios.Add(new HorarioDisponible
                            {
                                Id = horarios.Count + 1,
                                MedicoId = medico.Id,
                                Fecha = fecha,
                                HoraInicio = new TimeSpan(hora, 0, 0),
                                HoraFin = new TimeSpan(hora + 1, 0, 0),
                                Disponible = true
                            });
                        }

                        // Horarios de tarde: 2:00 PM - 6:00 PM
                        for (int hora = 14; hora < 18; hora++)
                        {
                            horarios.Add(new HorarioDisponible
                            {
                                Id = horarios.Count + 1,
                                MedicoId = medico.Id,
                                Fecha = fecha,
                                HoraInicio = new TimeSpan(hora, 0, 0),
                                HoraFin = new TimeSpan(hora + 1, 0, 0),
                                Disponible = true
                            });
                        }
                    }
                }
            }

            return horarios;
        }

        public static List<Cita> GetCitas()
        {
            var pacientes = GetPacientes();
            var medicos = GetMedicos();
            var horarios = GetHorariosDisponibles().Take(10).ToList(); // Solo las primeras 10 para ejemplo

            return new List<Cita>
            {
                new Cita { Id = 1, PacienteId = 1, MedicoId = 1, EspecialidadId = 1, HorarioDisponibleId = 1, FechaHora = DateTime.Today.AddDays(1).AddHours(9), Estado = "Agendada", Motivo = "Consulta de rutina cardiológica" },
                new Cita { Id = 2, PacienteId = 2, MedicoId = 3, EspecialidadId = 3, HorarioDisponibleId = 2, FechaHora = DateTime.Today.AddDays(2).AddHours(10), Estado = "Confirmada", Motivo = "Revisión dermatológica" },
                new Cita { Id = 3, PacienteId = 3, MedicoId = 5, EspecialidadId = 5, HorarioDisponibleId = 3, FechaHora = DateTime.Today.AddDays(3).AddHours(11), Estado = "Agendada", Motivo = "Control pediátrico" },
                new Cita { Id = 4, PacienteId = 4, MedicoId = 7, EspecialidadId = 7, HorarioDisponibleId = 4, FechaHora = DateTime.Today.AddDays(4).AddHours(14), Estado = "Completada", Motivo = "Consulta neurológica" },
                new Cita { Id = 5, PacienteId = 5, MedicoId = 8, EspecialidadId = 8, HorarioDisponibleId = 5, FechaHora = DateTime.Today.AddDays(5).AddHours(15), Estado = "Agendada", Motivo = "Consulta ginecológica" }
            };
        }

        public static List<Usuario> GetUsuarios()
        {
            return new List<Usuario>
            {
                new Usuario { Id = 1, Nombre = "admin", Email = "admin@hospital.com", Password = "admin123" },
                new Usuario { Id = 2, Nombre = "recepcion", Email = "recepcion@hospital.com", Password = "recep123" },
                new Usuario { Id = 3, Nombre = "medico1", Email = "medico1@hospital.com", Password = "med123" }
            };
        }
    }
}