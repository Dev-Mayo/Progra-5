Base de Datos

Base de datos en MongoDB:

Tarea2

Colecciones utilizadas:

clientes
catalogo
ordenes

Estructura:

Tarea2
 ├── clientes
 ├── catalogo
 └── ordenes
URL del API Gateway

Todas las pruebas se realizan a través del Gateway.

https://localhost:7161
Endpoints del Proyecto
1. Clientes
Crear Cliente

POST

https://localhost:7161/api/clientes

Body

{
  "nombreCompleto": "David",
  "tipoIdentificacion": "01",
  "identificacion": "305550888",
  "telefono": "85151530",
  "email": "david@example.com"
}

Respuesta esperada

201 Created
Listar Clientes

GET

https://localhost:7161/api/clientes

Respuesta ejemplo

{
  "codigo": 200,
  "descripcion": "Lista de clientes",
  "data": []
}
2. Catálogo (Productos)
Crear Producto

POST

https://localhost:7161/api/productos

Body

{
  "nombre": "Laptop Acer 1080HD",
  "precio": 650,
  "stock": 8
}

Respuesta

201 Created
Listar Productos

GET

https://localhost:7161/api/productos

Respuesta ejemplo

[
  {
    "id": "69a0fa704dcbe6dd3a92687a",
    "nombre": "Mouse Inalámbrico",
    "precio": 25,
    "stock": 100
  }
]
Actualizar Producto

PUT

https://localhost:7161/api/productos/{id}

Ejemplo

https://localhost:7161/api/productos/69a2df718a4ae63d93ad727a

Body

{
  "id": "69a2df718a4ae63d93ad727a",
  "nombre": "Monitor Editado",
  "precio": 350,
  "stock": 5
}

Respuesta

204 No Content
Eliminar Producto

DELETE

https://localhost:7161/api/productos/{id}

Ejemplo

https://localhost:7161/api/productos/69a1e96eeb6dff43c6ad76e0

Respuesta

204 No Content
3. Órdenes
Crear Orden

POST

https://localhost:7161/api/pedidos

Body

{
  "productoId": "69a0fa704dcbe6dd3a92687a",
  "cantidad": 1,
  "fecha": "2026-02-27T10:00:00Z"
}

Respuesta ejemplo

{
  "id": "69a2e2215b7ed8fc257089d3",
  "productoId": "69a0fa704dcbe6dd3a92687a",
  "cantidad": 1,
  "fecha": "2026-02-27T10:00:00Z"
}
Listar Órdenes

GET

https://localhost:7161/api/pedidos

Respuesta ejemplo

[
  {
    "id": "69a2e2215b7ed8fc257089d3",
    "productoId": "69a0fa704dcbe6dd3a92687a",
    "cantidad": 1,
    "fecha": "2026-02-27T10:00:00Z"
  }
]
4. Gateway – Resumen de Orden

Este endpoint utiliza dos microservicios:

Servicio de Órdenes

Servicio de Catálogo

para devolver información combinada.

Obtener Resumen de Orden

GET

https://localhost:7161/api/resumen-orden/{idOrden}

Ejemplo

https://localhost:7161/api/resumen-orden/69a2e2215b7ed8fc257089d3

Respuesta ejemplo

{
  "ordenId": "69a2e2215b7ed8fc257089d3",
  "fecha": "2026-02-27T10:00:00Z",
  "detalleProducto": {
    "id": "69a0fa704dcbe6dd3a92687a",
    "nombre": "Mouse Inalámbrico",
    "precio": 25,
    "stock": 100
  }
}
Flujo de Prueba del Sistema

Para probar el funcionamiento completo del sistema se recomienda el siguiente flujo:

Crear un producto en el servicio de Catálogo

Copiar el ID del producto

Crear una orden utilizando ese productoId

Copiar el ID de la orden

Consultar el resumen de la orden mediante el Gateway

Este flujo demuestra:

CRUD de Catálogo

CRUD de Órdenes

Integración entre microservicios

Funcionamiento del API Gateway

Flujo de Arquitectura
Cliente
   ↓
API Gateway
   ↓
Microservicio correspondiente
   ↓
MongoDB
