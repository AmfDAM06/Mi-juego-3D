using UnityEngine;
using TMPro; // Necesario para modificar los textos
using UnityEngine.UI;

public class ControlHUD : MonoBehaviour
{
    [Header("Ventanas")]
    public GameObject ventanaPausa;
    public GameObject ventanaFinJuego;

    [Header("Textos Modificables")]
    public TextMeshProUGUI tituloFinJuego; // Arrastra aquí el texto de título de FinJuego
    public TextMeshProUGUI tituloPausa;    // Arrastra aquí el texto de título de Pausa

    [Header("UI Juego (Arrastra los textos aquí)")]
    public TextMeshProUGUI textoMunicion;
    public TextMeshProUGUI textoPuntuacion;

    // Variable para saber si el juego sigue en marcha
    private bool juegoActivo = true;

    public static ControlHUD instancia;

    void Start()
    {
        // Nos aseguramos de que las ventanas empiecen cerradas
        if (ventanaPausa != null) ventanaPausa.SetActive(false);
        if (ventanaFinJuego != null) ventanaFinJuego.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked; // Ratón oculto al empezar
        Cursor.visible = false;
    }

    private void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        // Tu lógica de pausa original (tecla ESC)
        if (Input.GetKeyDown(KeyCode.Escape) && juegoActivo)
        {
            if (ventanaPausa.activeSelf)
            {
                ReanudarJuego();
            }
            else
            {
                PausarJuego("PAUSA"); // Texto por defecto
            }
        }
    }

    // --- MÉTODOS DE CONTROL ---

    public void ReanudarJuego()
    {
        ventanaPausa.SetActive(false);
        Time.timeScale = 1; // Tiempo normal
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void PausarJuego(string mensaje)
    {
        ventanaPausa.SetActive(true);
        if (tituloPausa != null) tituloPausa.text = mensaje;

        Time.timeScale = 0; // Congelar tiempo
        Cursor.lockState = CursorLockMode.None; // Mostrar ratón
        Cursor.visible = true;
    }

    // --- LO QUE ME HAS PEDIDO ---

    // 1. Esto lo llamará el Jugador al morir
    public void MostrarGameOver()
    {
        juegoActivo = false;
        ventanaFinJuego.SetActive(true);
        if (tituloFinJuego != null) tituloFinJuego.text = "GAME OVER";

        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // 2. Esto lo llamará el Enemigo al morir
    public void MostrarVictoria()
    {
        juegoActivo = false;
        // Pediste usar la ventana de Pausa con el mensaje "Has ganado"
        PausarJuego("HAS GANADO");
    }

    // 1. Método para las balas (Lo pide ControlArma)
    public void ActualizarBalas(int actuales, int maximas)
    {
        if (textoMunicion != null)
        {
            textoMunicion.text = actuales + " / " + maximas;
        }
    }

    // 2. Método para la puntuación (Lo pide ControlJuego)
    public void ActualizarPuntuacion(int puntos)
    {
        if (textoPuntuacion != null)
        {
            textoPuntuacion.text = "Puntos: " + puntos;
        }
    }
}