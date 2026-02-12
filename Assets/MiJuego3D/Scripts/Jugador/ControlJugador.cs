using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para reiniciar el nivel si mueres

public class ControlJugador : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public float velocidad = 5f;
    public float fuerzaSalto = 6f;

    [Header("Configuración de Cámara")]
    public Camera camaraJugador; // Arrastra tu Main Camera aquí en el inspector
    public float sensibilidadRaton = 2f;
    public float maxVistaX = 90f;
    public float minVistaX = -90f;
    private float rotacionVertical = 0f;

    [Header("Estadísticas")]
    public int vida = 100;
    public int vidaMaxima = 100;
    public int balas = 30;
    public int balasMaximas = 99;

    [Header("Interacción")]
    public ControlArma arma; // <--- NUEVO: Arrastra aquí tu arma
    // Referencias internas

    [Header("Referencias")]
    public ControlHUD scriptHUD; // <--- NUEVO: Para conectar con el HUD

    private Rigidbody rb;
    private bool tocandoSuelo = true;

    void Start()
    {
        // Bloquear el cursor en el centro de la pantalla
        Cursor.lockState = CursorLockMode.Locked;

        rb = GetComponent<Rigidbody>();

        // Aseguramos que el jugador no rote como un balón de fútbol
        if (rb != null)
            rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void Update()
    {
        // 1. Control de Cámara
        ControlarCamara();

        // 2. Detectar Salto
        if (Input.GetButtonDown("Jump") && tocandoSuelo)
        {
            Saltar();
        }

        // 3. DETECTAR DISPARO (NUEVO)
        // Si pulsamos clic izquierdo (Fire1) y tenemos un arma asignada
        if (Input.GetButton("Fire1") && arma != null)
        {
            arma.Disparar(); // Llamamos al método del otro script
        }
    }

    void FixedUpdate()
    {
        // 3. Movimiento Físico (WASD)
        MoverJugador();
    }

    // --- MÉTODOS DE CONTROL (MOVIMIENTO Y CÁMARA) ---

    void ControlarCamara()
    {
        if (camaraJugador == null) return;

        // Rotación horizontal (Girar el cuerpo del jugador)
        float mouseX = Input.GetAxis("Mouse X") * sensibilidadRaton;
        transform.Rotate(0, mouseX, 0);

        // Rotación vertical (Mover solo la cámara arriba/abajo)
        rotacionVertical -= Input.GetAxis("Mouse Y") * sensibilidadRaton;
        rotacionVertical = Mathf.Clamp(rotacionVertical, minVistaX, maxVistaX); // Limita el ángulo

        camaraJugador.transform.localRotation = Quaternion.Euler(rotacionVertical, 0, 0);
    }

    void MoverJugador()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        // Calculamos la dirección relativa a hacia donde mira el jugador
        Vector3 movimiento = transform.right * x + transform.forward * z;

        // Aplicamos velocidad manteniendo la gravedad actual (rb.velocity.y)
        Vector3 velocidadFinal = movimiento.normalized * velocidad;
        rb.linearVelocity = new Vector3(velocidadFinal.x, rb.linearVelocity.y, velocidadFinal.z);
    }

    void Saltar()
    {
        // Reseteamos la velocidad vertical para un salto consistente
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        // Impulso instantáneo hacia arriba
        rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
        tocandoSuelo = false;
    }

    // --- MÉTODOS PARA INTERACCIÓN (VIDA Y BALAS) ---
    // Estos son los que pedían los otros scripts (ControlExtras y ControlBala)

    public void IncrementaVida(int cantidad)
    {
        vida += cantidad;
        if (vida > vidaMaxima) vida = vidaMaxima;
        Debug.Log("Vida recuperada: " + vida);
    }

    public void IncrementarBalas(int cantidad)
    {
        balas += cantidad;
        if (balas > balasMaximas) balas = balasMaximas;
        Debug.Log("Munición recargada: " + balas);
    }

    public void QuitarVidasJugador(int cantidad)
    {
        vida -= cantidad;
        Debug.Log("¡Daño recibido! Vida: " + vida);

        if (vida <= 0)
        {
            vida = 0;
            Morir();
        }
    }

    void Morir()
    {
        Debug.Log("El jugador ha muerto");

        if (scriptHUD != null)
        {
            scriptHUD.MostrarGameOver(); // Llamamos al HUD
        }
        else
        {
            Debug.LogError("¡Falta asignar el Script HUD en el Jugador!");
        }
    }

    // --- DETECCIÓN DE SUELO ---

    private void OnCollisionEnter(Collision collision)
    {
        // Si chocamos con algo que tiene el tag "Suelo" (asegúrate de ponerle el tag a tu piso)
        // O simplemente cualquier colisión por debajo
        if (collision.gameObject.CompareTag("Suelo") || collision.contacts[0].normal.y > 0.5f)
        {
            tocandoSuelo = true;
        }
    }
}