# 🏛️ Hub Municipal de Gestión de Trámites

Sistema centralizado en **.NET 10** y **C#** para la Municipalidad de Valparaíso. Gestiona múltiples trámites de licencias de conducir con arquitectura **Clean Architecture** y UX/UI optimizada para adultos mayores.

## 📋 Características Principales

- **9 Módulos Municipales**: F8, Módulo 12, Módulo 16, Hoja de Ruta, Examen Psicométrico, Examen Teórico, Cambio de Domicilio, Gestión de Licencias, Archivo Plano
- **Clean Architecture**: Separación clara de responsabilidades (Core, Infrastructure, Web)
- **TPH Pattern**: Table Per Hierarchy con Entity Framework Core
- **Validador RUT Chileno**: Validación automática según normas SII
- **Windows Server Integration**: Gestión de carpetas en UNC (\\Servidor\Gestion\...)
- **Blazor WebAssembly + MudBlazor**: Interfaz moderna con alta accesibilidad
- **Accesibilidad Senior**: Tipografía 18px+, botones grandes, alto contraste
- **Sistema de Roles**: Admin, Operador, Visor
- **CORS Habilitado**: Para desarrollo y producción

## 🏗️ Estructura del Proyecto

```
ValpoControl/
├── src/
│   ├── ValpoControl.Core/
│   │   ├── Domain/
│   │   │   ├── Entities/          # BaseTramite + 9 módulos
│   │   │   └── Interfaces/        # ITramiteService<T>
│   │   └── Common/
│   │       └── Validators/        # RutValidator
│   │
│   ├── ValpoControl.Infrastructure/
│   │   ├── Persistence/           # ValpoControlDbContext
│   │   ├── Services/              # BaseTramiteService<T> + Módulos
│   │   └── Extensions/            # AddMunicipalidadModules()
│   │
│   └── ValpoControl.Web/
│       ├── Components/            # HubDashboard.razor
│       ├── Services/              # Servicios del cliente
│       └── Models/                # ViewModels
└── README.md
```

## 🚀 Inicio Rápido

### Requisitos
- .NET 10 SDK
- SQL Server, SQLite o PostgreSQL
- Visual Studio 2022 o VS Code

### Instalación

```bash
# Clonar repositorio
git clone https://github.com/corsal2025/ValpoControl-Hub.git
cd ValpoControl

# Restaurar dependencias
dotnet restore

# Crear base de datos (SQLite para desarrollo)
dotnet ef database update --project src/ValpoControl.Infrastructure

# Ejecutar aplicación
dotnet run --project src/ValpoControl.Web
```

La aplicación estará disponible en: **http://localhost:5002**

## 📦 Clases Principales

### BaseTramite
Clase base abstracta con propiedades comunes:
```csharp
public abstract class BaseTramite
{
    public int Id { get; set; }
    public required string RUT { get; set; }
    public required string Nombre { get; set; }
    public required string Email { get; set; }
    public required string Telefono { get; set; }
    public DateTime FechaIngreso { get; set; }
    public required string Estado { get; set; }
    public required string UsuarioId { get; set; }
    // ... más propiedades
}
```

### Validador RUT
```csharp
// Validación automática de RUT chileno
var (isValid, mensaje) = RutValidator.ValidarRut("12.345.678-K");

// Formateo de RUT
string rutFormato = RutValidator.FormatearRut("12345678K"); // "12.345.678-K"
```

### ITramiteService<T>
Interfaz genérica para todos los servicios:
```csharp
public interface ITramiteService<T> where T : BaseTramite
{
    Task<T?> ObtenerPorIdAsync(int id);
    Task<T?> ObtenerPorRutAsync(string rut);
    Task<IEnumerable<T>> ObtenerTodosAsync(int pagina, int pageSize);
    Task<T> CrearAsync(T tramite);
    Task<T> ActualizarAsync(T tramite);
    Task<T> CambiarEstadoAsync(int id, string nuevoEstado, string usuarioId);
    // ... más métodos
}
```

### Inyección de Dependencias
Registra todos los servicios con una sola línea:
```csharp
// En Program.cs
services.AddMunicipalidadModules(connectionString, rutaBaseServidor);
```

## 🎨 Interfaz de Usuario

### Dashboard Principal
- **Estadísticas Globales**: Total, Completados, En Proceso, Rechazados
- **Tarjetas de Módulos**: Una para cada municipio
- **Actividades Recientes**: Seguimiento en tiempo real
- **Acciones Rápidas**: Nuevo trámite, Búsqueda, Reportes

### Accesibilidad
- ✅ Tipografía mínimo 18px
- ✅ Botones grandes (60px altura)
- ✅ Alto contraste (#00d4ff sobre #1a1a2e)
- ✅ Navegación por teclado
- ✅ Tema oscuro para reducir fatiga ocular
- ✅ Iconos + Texto en botones

## 🔒 Seguridad

### Roles y Permisos
- **Admin**: Control total, configuración
- **Operador**: Gestión diaria de trámites
- **Visor**: Solo lectura de estadísticas

### Validación
- Validación de RUT integrada
- Validación de emails
- Validación de datos específicos por módulo

## 📁 Almacenamiento de Archivos

Los archivos se organizan automáticamente:
```
\\Servidor\Gestion\
├── GestionF8\
│   ├── 2026\
│   │   ├── 12345678\     (RUT del usuario)
│   │   │   ├── documentos/
│   │   │   └── logs/
│   ├── 2025\
│   │   ...
├── Modulo12\
│   ...
└── ...
```

## 🌐 API REST

### Endpoints principales
```
GET    /api/tramites                    # Listar trámites
POST   /api/tramites                    # Crear trámite
GET    /api/tramites/{id}               # Obtener por ID
PUT    /api/tramites/{id}               # Actualizar
GET    /api/tramites/buscar-rut/{rut}   # Buscar por RUT
PUT    /api/tramites/{id}/estado        # Cambiar estado
GET    /api/dashboard/stats             # Estadísticas
GET    /api/dashboard/actividad         # Actividades recientes
```

## 🗄️ Base de Datos

### TPH Pattern (Table Per Hierarchy)
Una tabla `Tramites` con columna discriminadora `TipoModulo`:
```
Tramites
├── Id (PK)
├── RUT
├── Nombre
├── Email
├── ... propiedades base
├── TipoModulo (discriminador: GestionF8, Modulo12, etc.)
└── ... propiedades específicas del módulo
```

## 🐛 Debugging

Para habilitar logs detallados:
```csharp
.EnableSensitiveDataLogging()  // En DbContext (solo desarrollo)
```

## 📝 Notas de Desarrollo

- El validador de RUT acepta formatos: "12345678K", "12.345.678-K", "12345678-K"
- El patrón TPH reduce complejidad vs. TPT (Table Per Type)
- MudBlazor proporciona componentes accesibles WCAG 2.1 AA
- Los servicios heredan de `BaseTramiteService<T>` para reutilización de código

## 👥 Roles Implementados

| Rol | Crear | Leer | Actualizar | Eliminar | Config |
|-----|-------|------|-----------|----------|--------|
| **Admin** | ✅ | ✅ | ✅ | ✅ | ✅ |
| **Operador** | ✅ | ✅ | ✅ | ❌ | ❌ |
| **Visor** | ❌ | ✅ | ❌ | ❌ | ❌ |

## 📞 Contacto

Para preguntas o contribuciones, contactar a la Municipalidad de Valparaíso.

---

**Versión**: 1.0.0
**Framework**: .NET 10.0
**UI**: Blazor WebAssembly + MudBlazor 7.14
**BD**: PostgreSQL / SQL Server / SQLite
