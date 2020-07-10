
# Person Crud API Coding Challenge

Tecnologías utilizadas: 
Asp.Net Core V 3.1, Entity Framework Core y AutoMapper.

Descargar como zip o clonar el repositorio.
Los paquetes necesarios deberían cargarse al compilar la solución.
Antes de ejecutar se debe completar la cadena de conexión a su base de datos en el archivo appsettings.json, en la variable segun su ambiente (desarrollo o producción).
Una vez ejecutada la aplicación ejecutara la migración y actualización de la base, por lo que no hay que ejecutar ningún comando de entity framework en la consola, esta configurado en la clase Startup.

La base de datos utilizada es Sql Server, por lo que algunas restricciones y manejo de errores pueden no aplicarse si se utiliza una base diferente.

Al ejecutar la aplicación aparecerá directamente la pagina con la documentación del servicio, implementado con Swagger. 
Esta documentación muestra ejemplos para los requests, responses de los endpoints y da la posibilidad de probar cada endpoint desde la misma pagina.
