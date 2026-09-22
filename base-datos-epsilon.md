# Esquema de Base de Datos — Epsilon

Documento de referencia con la estructura de tablas y vistas de la base de datos de **Epsilon**.

---

## Índice

- [Tablas](#tablas)
  - [Agenda](#agenda)
  - [Citas](#citas)
  - [CitaTratamientos](#citatratamientos)
  - [Clinicas](#clinicas)
  - [CorreosElectronicos](#correoselectronicos)
  - [EstadosUsuario](#estadosusuario)
  - [Facturacion](#facturacion)
  - [Logs](#logs)
  - [Medicos](#medicos)
  - [Pacientes](#pacientes)
  - [Radiologia](#radiologia)
  - [Roles](#roles)
  - [sysdiagrams](#sysdiagrams)
  - [Tratamientos](#tratamientos)
  - [Usuarios](#usuarios)
  - [UsuariosRoles](#usuariosroles)
- [Vistas](#vistas)
  - [vDatosHistorialPaciente](#vdatoshistorialpaciente)
  - [vDatosMedicos](#vdatosmedicos)
  - [vDatosPacientes](#vdatospacientes)
  - [vDatosPeriodos](#vdatosperiodos)
  - [vDatosTratamientos](#vdatostratamientos)
  - [vDatosUsuarios](#vdatosusuarios)

---

## Tablas

### Agenda

| Columna | Tipo | Nullable | Default |
|---|---|---|---|
| IdAgenda | int | NO | NULL |
| IdMedico | int | NO | NULL |
| HoraInicio | datetime | NO | NULL |
| HoraFin | datetime | NO | NULL |
| Disponible | bit | NO | NULL |
| IdCita | int | YES | NULL |

### Citas

| Columna | Tipo | Nullable | Default |
|---|---|---|---|
| IdCita | int | NO | NULL |
| IdClinica | int | NO | NULL |
| FechaInicio | datetime | NO | NULL |
| FechaFin | datetime | NO | NULL |
| IdPaciente | int | NO | NULL |
| IdMedico | int | NO | NULL |
| Observaciones | nvarchar | YES | NULL |

### CitaTratamientos

| Columna | Tipo | Nullable | Default |
|---|---|---|---|
| IdCitaTratamiento | int | NO | NULL |
| IdCita | int | YES | NULL |
| IdTratamiento | int | YES | NULL |

### Clinicas

| Columna | Tipo | Nullable | Default |
|---|---|---|---|
| IdClinica | int | NO | NULL |
| NombreClinica | nvarchar | NO | NULL |
| DireccionClinica | nvarchar | NO | NULL |
| LocalidadClinica | nvarchar | NO | NULL |
| TelefonoClinica | int | NO | NULL |
| EMailClinica | nvarchar | YES | NULL |
| DirectorClinica | varchar | YES | NULL |

### CorreosElectronicos

| Columna | Tipo | Nullable | Default |
|---|---|---|---|
| IdCorreo | int | NO | NULL |
| Asunto | varchar | YES | NULL |
| NombreCorreo | varchar | NO | NULL |
| CuerpoMensaje | nvarchar | YES | NULL |

### EstadosUsuario

| Columna | Tipo | Nullable | Default |
|---|---|---|---|
| IdEstadoUsuario | int | NO | NULL |
| EstadoUsuario | nvarchar | NO | NULL |

### Facturacion

| Columna | Tipo | Nullable | Default |
|---|---|---|---|
| IdFactura | int | NO | NULL |
| Importe | float | YES | NULL |
| FechaFactura | datetime | YES | NULL |
| IdCita | int | NO | NULL |

### Logs

| Columna | Tipo | Nullable | Default |
|---|---|---|---|
| IdLog | int | NO | NULL |
| FechaLog | datetime | NO | NULL |
| IdUsuario | int | NO | NULL |
| MensajeLog | nvarchar | YES | NULL |

### Medicos

| Columna | Tipo | Nullable | Default |
|---|---|---|---|
| IdMedico | int | NO | NULL |
| NombreMedico | nvarchar | NO | NULL |
| DNI | nvarchar | NO | NULL |
| NumeroColegiado | int | NO | NULL |
| Especialidad | nvarchar | NO | NULL |
| Telefono | nvarchar | NO | NULL |
| EMail | nvarchar | NO | NULL |
| FechaContratacion | nchar | NO | NULL |
| Activo | bit | NO | NULL |
| Observaciones | varchar | YES | NULL |
| Foto | varbinary | YES | NULL |
| Titulacion | varchar | YES | NULL |
| IdClinica | int | YES | NULL |
| IdUsuario | int | YES | NULL |

### Pacientes

| Columna | Tipo | Nullable | Default |
|---|---|---|---|
| IdPaciente | int | NO | NULL |
| NombrePaciente | nvarchar | NO | NULL |
| DNI | nvarchar | NO | NULL |
| Telefono | int | NO | NULL |
| EMail | nvarchar | YES | NULL |
| FechaNacimiento | datetime | NO | NULL |
| Direccion | nvarchar | NO | NULL |
| Ciudad | nvarchar | YES | NULL |
| FechaAlta | datetime | NO | NULL |
| NumeroConsultas | int | NO | NULL |
| Asegurado | bit | YES | NULL |
| Observaciones | nvarchar | YES | NULL |
| FechaPrimeraCita | datetime | NO | NULL |
| FechaUltimaCita | datetime | NO | NULL |
| Alergias | nvarchar | YES | NULL |
| Fumador | bit | NO | ((0)) |
| CondicionBucal | nvarchar | YES | NULL |

### Radiologia

| Columna | Tipo | Nullable | Default |
|---|---|---|---|
| IdRadiografia | int | NO | NULL |
| IdPaciente | int | NO | NULL |
| Archivo | nvarchar | NO | NULL |
| FechaArchivo | datetime | NO | NULL |
| Tipo | nvarchar | NO | NULL |
| Observaciones | nvarchar | YES | NULL |

### Roles

| Columna | Tipo | Nullable | Default |
|---|---|---|---|
| IdRol | int | NO | NULL |
| NombreRol | nvarchar | NO | NULL |


### Tratamientos

| Columna | Tipo | Nullable | Default |
|---|---|---|---|
| IdTratamiento | int | NO | NULL |
| NombreTratamiento | nvarchar | NO | NULL |
| Duracion | int | YES | NULL |
| Color | nchar | YES | NULL |
| Precio | float | YES | NULL |

### Usuarios

| Columna | Tipo | Nullable | Default |
|---|---|---|---|
| IdUsuario | int | NO | NULL |
| Nombre | varchar | NO | NULL |
| Password | varchar | NO | NULL |
| Email | varchar | NO | NULL |
| FechaAlta | datetime | NO | NULL |
| Telefono | int | NO | NULL |
| Activo | bit | YES | NULL |
| FotoPerfil | varbinary | YES | NULL |
| IdEstadoUsuario | int | NO | NULL |

### UsuariosRoles

| Columna | Tipo | Nullable | Default |
|---|---|---|---|
| IdUsuarioRol | int | NO | NULL |
| IdUsuario | int | NO | NULL |
| IdRol | int | NO | NULL |
| Activo | bit | NO | NULL |

---

## Vistas

### vDatosHistorialPaciente

| Columna | Tipo | Nullable | Default |
|---|---|---|---|
| IdPaciente | int | NO | NULL |
| NombrePaciente | nvarchar | NO | NULL |
| FechaAlta | datetime | NO | NULL |
| NumeroConsultas | int | NO | NULL |
| FechaFin | datetime | NO | NULL |
| FechaInicio | datetime | NO | NULL |
| NombreMedico | nvarchar | NO | NULL |
| NombreClinica | nvarchar | NO | NULL |
| Observaciones | nvarchar | YES | NULL |
| DNI | nvarchar | NO | NULL |
| IdCita | int | NO | NULL |
| NombreTratamiento | nvarchar | NO | NULL |
| Precio | float | YES | NULL |
| Duracion | nvarchar | YES | NULL |

### vDatosMedicos

| Columna | Tipo | Nullable | Default |
|---|---|---|---|
| IdMedico | int | NO | NULL |
| NombreMedico | nvarchar | NO | NULL |
| DNI | nvarchar | NO | NULL |
| IdUsuario | int | YES | NULL |
| IdClinica | int | YES | NULL |
| Titulacion | varchar | YES | NULL |
| Foto | varbinary | YES | NULL |
| Observaciones | varchar | YES | NULL |
| Activo | bit | NO | NULL |
| FechaContratacion | nchar | NO | NULL |
| EMail | nvarchar | NO | NULL |
| Telefono | nvarchar | NO | NULL |
| Especialidad | nvarchar | NO | NULL |
| NumeroColegiado | int | NO | NULL |
| NombreClinica | nvarchar | NO | NULL |

### vDatosPacientes

| Columna | Tipo | Nullable | Default |
|---|---|---|---|
| IdPaciente | int | NO | NULL |
| NombrePaciente | nvarchar | NO | NULL |
| DNI | nvarchar | NO | NULL |
| Telefono | int | NO | NULL |
| EMail | nvarchar | YES | NULL |
| FechaNacimiento | datetime | NO | NULL |
| Direccion | nvarchar | NO | NULL |
| Ciudad | nvarchar | YES | NULL |
| FechaAlta | datetime | NO | NULL |
| NumeroConsultas | int | NO | NULL |
| Asegurado | bit | YES | NULL |
| Observaciones | nvarchar | YES | NULL |
| FechaPrimeraCita | datetime | NO | NULL |
| Alergias | nvarchar | YES | NULL |
| CondicionBucal | nvarchar | YES | NULL |
| Fumador | bit | NO | NULL |
| FechaUltimaCita | datetime | NO | NULL |


### vDatosTratamientos

| Columna | Tipo | Nullable | Default |
|---|---|---|---|
| Precio | float | YES | NULL |
| Color | nchar | YES | NULL |
| Duracion | int | YES | NULL |
| NombreTratamiento | nvarchar | NO | NULL |
| IdTratamiento | int | NO | NULL |

### vDatosUsuarios

| Columna | Tipo | Nullable | Default |
|---|---|---|---|
| Activo | bit | YES | NULL |
| Telefono | int | NO | NULL |
| FechaAlta | datetime | NO | NULL |
| Email | varchar | NO | NULL |
| Password | varchar | NO | NULL |
| Nombre | varchar | NO | NULL |
| IdUsuario | int | NO | NULL |
| FotoPerfil | varbinary | YES | NULL |
| EstadoUsuario | nvarchar | YES | NULL |
| IdEstadoUsuario | int | YES | NULL |
