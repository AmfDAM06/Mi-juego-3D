using UnityEngine;
using TMPro;
using UnityEngine.UI; // <--- NECESARIO PARA LA BARRA DE VIDA (Image)
using UnityEngine.SceneManagement;

public class ControlHUD : MonoBehaviour
{
    public static ControlHUD instancia;

    private void Awake()
    {
        if (instancia == null) instancia = this;
        else Destroy(gameObject);
    }

    [Header("Ventanas")]
    public GameObject ventanaPausa;
    public GameObject ventanaFinJuego;

    [Header("Textos UI")]
    public TextMeshProUGUI tituloFinJuego;
    public TextMeshProUGUI tituloPausa;

    [Header("HUD In-Game")] // --- NUEVO ---
    public TextMeshProUGUI textoMunicion;
    public TextMeshProUGUI textoPuntuacion;
    public Image barraVidaRoja; // Arrastra aquí tu imagen "BarraVidaRoja"

    void Start()
    {
        Time.timeScale = 1f;
        if (ventanaPausa != null) ventanaPausa.SetActive(false);
        if (ventanaFinJuego != null) ventanaFinJuego.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (ventanaPausa.activeSelf) ReanudarJuego();
            else PausarJuego("PAUSA");
        }
    }

    // --- MÉTODOS DE ACTUALIZACIÓN DEL HUD ---

    // 1. Método para la VIDA (NUEVO)
    public void ActualizarVida(int vidaActual, int vidaMaxima)
    {
        if (barraVidaRoja != null)
        {
            // Convertimos a float para tener decimales (ej: 50/100 = 0.5)
            float porcentaje = (float)vidaActual / (float)vidaMaxima;
            barraVidaRoja.fillAmount = porcentaje;
        }
    }

    // 2. Método para las BALAS
    public void ActualizarBalas(int actuales, int maximas)
    {
        if (textoMunicion != null)
        {
            textoMunicion.text = actuales + " / " + maximas;
        }
    }

    // 3. Método para la PUNTUACIÓN
    public void ActualizarPuntuacion(int puntos)
    {
        if (textoPuntuacion != null)
        {
            // Usamos D5 para que rellene con ceros (ej: 00150)
            textoPuntuacion.text = puntos.ToString("D5");
        }
    }

    // --- GESTIÓN DE VENTANAS (Igual que antes) ---
    public void ReanudarJuego()
    {
        ventanaPausa.SetActive(false);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void PausarJuego(string mensaje)
    {
        ventanaPausa.SetActive(true);
        if (tituloPausa != null) tituloPausa.text = mensaje;
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void MostrarGameOver()
    {
        ventanaFinJuego.SetActive(true);
        if (tituloFinJuego != null) tituloFinJuego.text = "GAME OVER";
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void MostrarVictoria()
    {
        PausarJuego("HAS GANADO");
    }

    public void ReiniciarNivel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void IrAlMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
}