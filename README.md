# User Management API

API REST en .NET 8 para gestionar usuarios, con autenticación JWT y autorización por permisos, construida con arquitectura hexagonal y Entity Framework Core sobre MySQL.

## Inicio rápido

Requisitos: **.NET SDK 8**, **Docker**.

```bash
# 1. Levantar la base de datos
docker compose up -d

# 2. Restaurar la herramienta de migraciones (dotnet-ef 8.0.31, definida en .config/dotnet-tools.json)
dotnet tool restore

# 3. Aplicar migraciones (crea el esquema y carga los datos semilla)
dotnet ef database update --project src/UserManagement.Infrastructure --startup-project src/UserManagement.Api

# 4. Ejecutar la API
dotnet run --project src/UserManagement.Api
```

Swagger: http://localhost:5155/swagger (botón **Authorize** para enviar el token).

Pruebas unitarias:

```bash
dotnet test
```

## Base de datos

| Dato | Valor |
|------|-------|
| Motor | MySQL **8.4.11** |
| Imagen Docker | `mysql:8.4.11` |
| Host / Puerto | `localhost:3307` (se usa 3307 para no chocar con un MySQL local en 3306) |
| Base de datos | `user_management` |
| Usuario / Contraseña | `app_user` / `app_dev_password` |
| Root | `root_dev_password` |

La cadena de conexión se lee de `ConnectionStrings:Default` en `src/UserManagement.Api/appsettings.json`. El esquema se crea solo con migraciones versionadas (`src/UserManagement.Infrastructure/Persistence/Migrations`).

## Usuarios de prueba

| Email | Contraseña | Rol | Permisos | Estado |
|-------|------------|-----|----------|--------|
| admin@demo.com | `Admin123!` | Administrador | users.read, users.update | Activo |
| consulta@demo.com | `Consulta123!` | Consulta | users.read | Activo |
| inactivo@demo.com | `Inactivo123!` | Consulta | users.read | Inactivo |

## Endpoints

| Método | Ruta | Acceso |
|--------|------|--------|
| POST | `/api/v1/auth/token` | Público |
| GET | `/api/v1/users?page=1&pageSize=10` | JWT válido |
| PUT | `/api/v1/users/{id}` | JWT válido + permiso `users.update` |

Colección de peticiones: `src/UserManagement.Api/UserManagement.Api.http` (incluye un caso por cada código de respuesta: 200, 400, 401, 403, 404 y 409).

Los errores se devuelven en formato ProblemDetails (RFC 9110).

## Estructura

```
src/
├── UserManagement.Domain          Entidades, reglas de negocio y excepciones de dominio. Sin dependencias.
├── UserManagement.Application     Casos de uso, puertos de entrada y salida, DTOs. Depende solo de Domain.
├── UserManagement.Infrastructure  Adaptadores de salida: EF Core + MySQL, BCrypt, JWT.
└── UserManagement.Api             Adaptador de entrada: controllers, políticas, manejo de errores, composición.
tests/
└── UserManagement.Application.Tests  Pruebas unitarias del caso de uso de actualización, sin base de datos.
```

## Decisiones de diseño

| Tema | Decisión | Por qué |
|------|----------|---------|
| Arquitectura | Hexagonal: puertos (interfaces) en Application, adaptadores en Infrastructure y Api | El núcleo no conoce MySQL, EF Core, ASP.NET ni JWT; cambiar de motor o de ORM no toca Domain ni Application |
| Composition Root | `Program.cs` es el único lugar que conecta puertos con adaptadores | Api referencia Infrastructure solo para registrar dependencias |
| Patrón Repository | `IUserRepository` implementado por `EfUserRepository` | Permite probar los casos de uso con un repositorio falso en memoria |
| Modelo de dominio rico | Propiedades con `private set` y método `User.Update` que valida todo antes de asignar | La entidad nunca queda en un estado inválido ni a medio actualizar |
| DTOs | Los casos de uso devuelven DTOs, nunca entidades | Lista blanca de campos: el hash de contraseña no puede filtrarse |
| Autorización | Políticas de ASP.NET Core sobre el claim `permission` del JWT | Se decide por permiso y no por rol, sin `if` en los controllers |
| Errores | `IExceptionHandler` centralizado que traduce excepciones de dominio a 400/401/404/409 | Los casos de uso no conocen HTTP; un error inesperado devuelve 500 sin exponer detalles |
| Credenciales | Mismo 401 para email inexistente y contraseña incorrecta | Evita revelar qué emails están registrados |
| Email único | Validación en el caso de uso + índice UNIQUE en MySQL | El índice cubre la condición de carrera entre dos peticiones simultáneas |
| Mapeo ORM | Fluent API en Infrastructure | El dominio no tiene atributos ni referencias al ORM |
| Datos semilla | `HasData` en la migración con hashes BCrypt precalculados | La migración es determinista; las contraseñas nunca se guardan en texto plano |

### Siguiente iteración

- Exigir el permiso `users.read` en `GET /users` (hoy solo requiere JWT válido, como indica el enunciado).
- Convertir `Email` en un Value Object.
- Pruebas de integración de los endpoints con una base de datos real en contenedor.
- Mover la clave de firma JWT a variables de entorno o a un gestor de secretos.
- Refresh tokens.
