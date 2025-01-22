# Proyecto PMDM RollABall

## Descripción

Este proyecto es una implementación de un juego de tipo *Roll-a-ball* en Unity, donde se controla a un jugador (representado por una esfera) que puede moverse en un entorno 3D y recoger objetos. El juego también incluye dos tipos de cámaras: una en tercera persona y otra en primera persona, que el jugador puede alternar. El objetivo es familiarizarse con el uso de *Rigidbody*, control de entrada del teclado, y manejo de cámaras en Unity.

## Scripts Utilizados

### `PlayerController.cs`

El script `PlayerController.cs` maneja el control del jugador, permitiendo su movimiento en función de las teclas de flecha y la interacción con objetos dentro del juego. A continuación, se explica cómo se implementa el movimiento del jugador:

1. **Movimiento del Jugador**:  
   El jugador se mueve en el espacio 3D utilizando un `Rigidbody` para aplicar fuerzas de movimiento. Las teclas de flecha controlan el movimiento del jugador en el plano horizontal (X y Y).
   
   - Se detecta si se presionan las teclas de flecha (`Keyboard.current.upArrowKey`, `Keyboard.current.downArrowKey`, `Keyboard.current.leftArrowKey`, `Keyboard.current.rightArrowKey`).
   - Cuando se detecta un movimiento, se actualizan las variables `movementX` y `movementY` con los valores correspondientes.
   - En el método `FixedUpdate()`, se calcula el movimiento relativo a la cámara para que el jugador se mueva en la misma dirección en que la cámara está mirando.

2. **Interacción con Objetos**:  
   El jugador puede recoger objetos etiquetados con "PickUp". Cada vez que el jugador entra en contacto con un objeto de este tipo, el contador aumenta y el objeto se desactiva.

### `CameraController.cs`

El script `CameraController.cs` gestiona las dos cámaras (tercera y primera persona) y permite alternar entre ellas.

1. **Vista en Tercera Persona**:
   - La cámara sigue al jugador desde una posición detrás y por encima de él, con un offset calculado dinámicamente en el inicio (`thirdPersonOffset`).
   - El jugador siempre está en el centro de la vista, y la cámara está fija mirando al jugador.
   - Se ajusta la altura (`thirdPersonHeight`) y la distancia (`thirdPersonDistance`) de la cámara con respecto al jugador.

2. **Vista en Primera Persona**:
   - En este modo, la cámara se posiciona a nivel del jugador y sigue su rotación.
   - Se permite la rotación horizontal (con las teclas `A` y `D`) y vertical (con las teclas `W` y `S`), pero se limita la rotación vertical para evitar que el jugador se vea por encima o por debajo de sí mismo.
   - La velocidad de rotación se controla con la variable `rotationSpeed`.

3. **Alternar entre Cámaras**:
   - El jugador puede alternar entre la vista en primera y tercera persona presionando las teclas `1` y `2`.
   - Cuando la vista está en primera persona, la cámara sigue al jugador con un pequeño ajuste en la altura (`firstPersonHeightOffset`).
   - En vista en tercera persona, la cámara se posiciona detrás del jugador y mantiene una vista fija en su posición.

### **Uso de Euler en la Rotación**

En el modo de cámara en primera persona, utilizamos la rotación de la cámara basada en los ángulos de Euler (específicamente `Quaternion.Euler(rotationX, rotationY, 0f)`) para controlar la orientación de la cámara:

- **¿Por qué usar Euler?**
  La rotación en 3D puede ser complicada debido al fenómeno conocido como *gimbal lock* (bloqueo de cardán), que ocurre cuando las rotaciones en los tres ejes (X, Y, Z) se alinean de tal forma que se pierde un grado de libertad. Sin embargo, en este caso, para una rotación simple y controlada en los ejes X (vertical) y Y (horizontal), utilizar ángulos de Euler es adecuado para los movimientos básicos de la cámara. Usamos `Quaternion.Euler()` para convertir los valores de los ángulos en una representación adecuada para Unity sin tener que preocuparnos por las complejidades de las matrices de rotación o los cuaterniones manualmente.

  El uso de Euler permite aplicar rotaciones sencillas basadas en la entrada del usuario, sin necesidad de cálculos adicionales, y resulta eficaz para un control directo de la cámara en un entorno de juego como el de este proyecto.

## Conclusión

Con este proyecto, se implementaron dos sistemas de cámaras en Unity que permiten alternar entre vista en primera persona y tercera persona, así como un sistema de control para el jugador utilizando un `Rigidbody` y la entrada del teclado. Esta tarea ayuda a comprender cómo trabajar con la cámara y los controles del jugador en Unity, así como a manejar interacciones básicas con objetos dentro del juego.

La implementación de la rotación de la cámara en primera persona utilizando Euler ha permitido un control preciso y sencillo de la orientación del jugador sin necesidad de complejidades adicionales.
