# Trabajo Práctico Final

## Integrantes ✒️

* **Zarate Miranda Saul Denis** - *Desarrollador*
* **Bossio Alberto Federico** - *Desarrollador*
* **Mandirola Gabriel Nicolas** - *Desarrollador*
* **Toyama Rodrigo** - *Desarrollador*

## 🏨 Agencia de Viajes 🏨

Mediante la implementación del apartado visual tenemos como primer vista una interfaz de Login y de Registro en el que se muestran estos 2 apartados:

* **Login**
En la Vista de Login tenemos la opción de ingresar con nuestro DNI y una contraseña que previamente habremos puesto en el registro. El usuario registrado tendrá 3 intentos para realizar un login de manera correcta, si estos 3 intentos no se realizan de manera satisfactoria el usuario con el que se está intentando ingresar será bloqueado y solo un administrador podrá realizar el desbloqueo.

* **Registro**
En la interfaz de Registro tenemos 5 campos a completar:
  * Dni
  * Nombre
  * Mail
  * Contraseña
  * Repetir Contraseña

El dni y el mail son valores únicos, esto quiere decir que no pueden ser repetidos por otro usuario.
En el caso de repetirse se le avisará al usuario y este deberá ingresar los datos válidos.

---

## 👤 Vista Usuario 👤

Una vez logeados

* En la parte superior derecha se podrá ver el nombre de la persona conectada y un botón para cerrar sesión y además el cambio de lenguaje Ingles/Español.
* En la parte central superior de la aplicación se puede ver la barra de navegación con la que podremos navegar entre distintas pantallas de la web:

### Buscador

En esta vista se encuentran los filtros "Buscar por ciudad", "Tipo de alojamiento" (cabañas, hoteles o ambos), "Fecha de ida" y "Fecha de vuelta" para reservar rápidamente.

### Alojamientos

Tenemos distintas herramientas para realizar filtrados para nuestras preferencias, entre estos filtrados están:
  
* **Precio**: Se puede colocar un precio mínimo para el filtrado.
* **Estrella**: La cantidad de estrellas del alojamiento.
* **Personas**: La cantidad de personas para el alojamiento.

Una vez completadas estas opciones a la derecha se encuentra un botón **filtrar** que ejecuta estas preferencias y nos lo muestra en un grid view en la parte central de la web.

Al ingresar en una tarjeta/card de alojamiento (se haya filtrado o no) nos enviara al detalle de la misma con la que podremos ver:

El título con el tipo de alojamiento
* **Barrio**
* **Estrellas**
* **Cantidad de personas**
* **TV**
* **Precio por persona** (si es un hotel)
* **Precio por día** (si es un cabaña)
* **Habitaciones** (si es un cabaña)
* **Baños** (si es un cabaña)

Y deberemos ingresar una fecha desde y una fecha hasta para poder **reservar** el alojamiento, una vez oprimido el botón aparecerá un cartel para la confirmación de esta acción.

### Mis Reservaciones

Dentro de esta vista podremos ver los distintos tipos de alojamientos que hemos reservado con anticipación y podremos ver con mas detalle su información al hacer click en el botón 'Ver'.

Al ingresar al detalle, podremos volver a la pantalla anterior o borrar la reservación de la misma si lo desea.

### Mis Datos

En esta vista podremos ver nuestros datos de la cuenta y podremos cambiar tanto Nombre, Email como Contraseña si lo desea (para realizar estas acciones es necesaria la utilización de la contraseña actual).

---

## 👑 Vista Admin 👑

* En el caso de la vista del administrador tenemos las mismas opciones en la parte superior derecha de la web. (Cambio de lenguaje, Nombre de usuario y Cerrar sesión).
* En la parte central superior de la aplicación se puede ver la barra de navegación con la que podremos navegar entre distintas pantallas de la web:

### Alojamientos

En la Vista Alojamientos ahora se muestra una tabla que nos mostrará todos los alojamientos disponibles y además podremos hacer 3 acciones.

* **Agregar un Alojamiento**
* **Modificar un Alojamiento**
* **Eliminar un Alojamiento**

#### Vista Agregar Alojamiento

En la parte central de la web se encuentran unos campos para completar con la información del alojamiento que queremos agregar, estos campos son:

* Código
* Ciudad
* Barrio
* Estrellas
* Cantidad de personas
* ¿Tiene tv?
* Tipo
* Precio por persona
* Precio por día
* Habitaciones
* Baños

Una vez completado dicho formulario se debe presionar el botón Crear para incluirlo en el listado de alojamientos a reservar o podemos presionar Volver para ir a la vista de Alojamientos.

#### Vista Modificar Alojamiento

Para modificar un alojamiento ya registrado se debe seleccionar cual es el que se quiere modificar y en la parte derecha de la tabla se encuentra un botón con el nombre **Editar**, una vez presionado nos enviará a la vista Edit de Alojamiento con los datos precargados de ese alojamiento donde se hará dicha modificación si es necesaria, una vez terminado el cambio, se deberá pulsar el botón **Guardar** para cargar los nuevos datos al alojamiento que se quiso modificar o podemos presionar Volver para ir a la vista de Alojamientos.

#### Vista Eliminar Alojamiento

Muy parecido al botón Modificar, el botón borrar se encuentra en la tabla de los alojamientos con un color rojo, este nos servirá para eliminar el alojamiento seleccionado del registro de alojamientos o podemos presionar Volver para ir a la vista de Alojamientos.

#### Vista Reservas

Se muestran todas las reservas hechas por los usuarios registrados en la aplicación. En este mismo vamos a tener la posibilidad de **Modificar** o **Borrar** dichas reservas de la misma manera que se hace con los alojamientos. En el apartado modificar el único dato que se puede cambiar es la **fecha de reserva** (fecha desde y fecha hasta).

#### Vista Usuarios

Aparecen todos los usuarios registrados en la aplicación y tendremos las mismas opciones antes mencionadas, **Modificar** y **Borrar**.

En la opción **Modificar**, en este caso, podremos cambiar todos los datos (excepto la contraseña) y a su vez se podrá realizar el cambio de tipo de usuario de Usuario común a Usuario administrador, además, desde esta opción es donde se podrá realizar el desbloqueo de los usuarios que hayan sido bloqueados por haberse logeado de manera incorrecta más de 3 veces seguidas o eliminar los intentos de ingreso a la cuenta de ser necesario.

---

## Extras Realizados

* Agregamos fotos a los alojamientos para visualizar los resultados de la búsqueda para Cabañas u Hoteles.
* Manejo de Multilenguaje.
* Login con manejo de cookies de sesión y encriptado de contraseñas a nivel base de datos
* Mejoras a nivel visual (Uso de Bootstrap + FontAwesome)
* Agregamos Sonidos de confirmación y error.
* Validación de Tiempo de sesión con las propiedades de Autenticación.

---
## Screenshots

### Login Admin / User

![Login](https://i.imgur.com/pjZGzRW.png)

### Alojamiento

|              | Admin | User |
|--------------|-------|------|
| Alojamiento  |   ![AlojamientoAdmin](https://i.imgur.com/Og85ztY.png)    |  ![AlojamientoUser](https://i.imgur.com/xwFhNOS.png)    |

### Reservas

|              | Admin | User |
|--------------|-------|------|
| Reservas     |   ![ReservasAdmin](https://i.imgur.com/F32bcHx.png)    |   ![ReservasUser](https://i.imgur.com/kO291zY.png)   |

### Usuarios Admin

![Usuarios](https://i.imgur.com/rhTRMu1.png)

### Buscador User

![Buscardor](https://i.imgur.com/5V5ZYya.png)
