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

---

## 👤 Vista Usuario 👤

Una vez logeados

* En la parte superior izquierda se podrá ver el nombre de la persona conectada y un botón para cerrar sesión y ademas el cambio de lenguaje Ingles/Español.
* En la parte central superior de la aplicación se puede ver la barra de navegación con la que podremos navegar entre distintas pantallas de la web:

### Buscador

En esta vista se encuentran los filtros "Buscar por ciudad", "Fecha de ida" y "Fecha de vuelta" para reservar rápidamente.

### Alojamientos

Dentro de esta vista podremos elegir los distintos tipos de alojamientos que nos ofrece la aplicación (Cabañas o Hoteles). Tenemos distintas herramientas para realizar filtrados para nuestras preferencias, entre estos filtrados están:
  
* **Precio**: Se puede colocar un precio minimo para el filtrado.
* **Estrella**: La cantidad de estrellas del alojamiento.
* **Personas**: La cantidad de personas para el alojamiento.

Una vez completadas estas opciones a la derecha se encuentra un botón **filtrar** que ejecuta estas preferencias y nos lo muestra en un grid view en la parte central de la web.

Al ingresar en una tarjeta/card de alojamiento nos enviara al detalle de la misma con la que podremos ver:

* **Barrio**
* **Estrellas**
* **Cantidad de personas**
* **TV**
* **Precio por persona**

Y podremos ademas ingresar la fecha desde y hasta para poder **reservar** el alojamiento.

### Mis Reservaciones

Dentro de esta vista podremos ver los distintos tipos de alojamientos que hemos reservado con anticipacion y podremos ver con mas detalle su informacion al hacer click en el boton 'Ver'

Al ingresar al detalle, podremos volver a la pantalla anterior o borrar la reservacón de la misma si lo desea

### Mis Datos

En esta vista podremos ver nuestros datos de la cuenta y poder cambiar tanto Nombre, Email como Contraseña si lo desea.

Este ordenamiento se realiza de menor a mayor

---

## 👑 Vista Admin 👑

En el caso de la vista del administrador tenemos las mismas opciones en la parte superior izquierda de la web. (Cambio de lenguaje, Nombre de usuario y Log out)

### Alojamientos

En la Vista Alojamientos ahora se muestra una tabla que nos mostrará todos los alojamientos disponibles y ademas podremos hacer 3 acciones.

* **Agregar un Alojamiento**
* **Modificar un Alojamiento**
* **Eliminar un Alojamiento**

#### Vista Agregar Alojamiento

En la parte central de la web se encuentran unos campos para completar con la información del alojamiento que queremos agregar, estos campos son:

* Código
* Ciudad
* Barrio
* Cantidad de personas
* Estrellas
* ¿Tiene tv?
* Precio por persona

* Precio por día
* Habitaciones
* Baños

Una vez completado dicho formulario se debe presionar el botón Crear para incluirlo en el listado de alojamientos a reservar

#### Vista Modificar Alojamiento

Para modificar un alojamiento ya registrado se debe seleccionar cual es el que se quiere modificar y en la parte derecha de la tabla se encuentra un botón con el nombre **Editar**, una vez presionado nos enviará a la vista Edit de Alojamiento con los datos precargados de ese alojamiento donde se hará dicha modificación si es necesaria, una vez terminado el cambio, se deberá pulsar el botón **Guardar** para cargar los nuevos datos al alojamiento que se quiso modificar.

#### Vista Eliminar Alojamiento

Muy parecido al botón Modificar, el botón borrar se encuentra en la tabla de los alojamientos con un color rojo, este nos servirá para eliminar el alojamiento seleccionado del registro de alojamientos.

#### Vista Reservas

Se muestran todas las reservas hechas por los usuarios registrados en la aplicación. En este mismo vamos a tener la posibilidad de **Modificar** o **Borrar** dichas reservas de la misma manera que se hace con los alojamientos. En el apartado modificar el único dato que se puede cambiar es la **fecha de reserva**.

#### Vista Usuarios

Aparecen todos los usuarios registrados en la aplicación y tendremos las mismas opciones antes mencionadas, **Modificar** y **Borrar**.

En la opción **Modificar**, en este caso, podremos cambiar todos los datos y a su vez se podrá realizar el cambio de tipo de usuario de Usuario común a Usuario administrador, además, desde esta opción es donde se podrá realizar el desbloqueo de los usuarios que hayan sido bloqueados por haberse logeado de manera incorrecta más de 3 veces seguidas.

---

## Extras Realizados

* Agregamos fotos a los alojamientos para vizualizar los resultados de la busqueda para Cabañas o Hoteles.
* Manejo de Multilenguaje.
* Login con manejo de cookes de sesión y encriptado de contraseñas a nivel base de datos
* Mejoras a nivel visual (Uso de Bootstrap + FontAwesome)
* Agregamos Sonidos de confirmacion y error.
* Validacion de Tiempo de sesión con las propiedades de Autenticación

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
