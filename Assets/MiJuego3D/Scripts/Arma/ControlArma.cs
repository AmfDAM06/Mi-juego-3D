using UnityEngine;

public class ControlArma : MonoBehaviour
{
    public int municionActual;
    public int municionMax;
    public bool municionInfinita;

    public float velocidadBala;
    public float frecuenciaDisparo;
    private float ultimoTiempoDisparo;
    private bool esJugador;
    private PoolObjetos balaPool;
    public Transform puntoSalida;
    private void Awake()
    {
        if (transform.tag == "Jugador")
        {
            esJugador = true;
        }

        balaPool = GetComponent<PoolObjetos>();
    }

    public bool PuedeDisparar()
    {
        if (Time.time -ultimoTiempoDisparo >= frecuenciaDisparo)
        {
            if (municionActual > 0 || municionInfinita == true)
            {
                return true;
            }
        }
        return false;
    }

    public void Disparar()
    {
        // --- 1. COMPROBACIÓN DE SEGURIDAD (EL PORTERO) ---
        // Si no tengo balas Y la munición NO es infinita... me voy.
        if (municionActual <= 0 && !municionInfinita)
        {
            // Opcional: Aquí podrías poner un sonido de "clic" de arma vacía
            return;
        }
        // -------------------------------------------------

        ultimoTiempoDisparo = Time.time;

        // --- 2. RESTAR MUNICIÓN ---
        // Solo restamos si NO es infinita
        if (!municionInfinita)
        {
            municionActual--;
        }

        // --- 3. DISPARO FÍSICO ---
        GameObject bala = balaPool.getObjeto();
        if (puntoSalida != null)
        {
            bala.transform.position = puntoSalida.position;
            bala.transform.rotation = puntoSalida.rotation;

            // Si usas Rigidbody para la bala
            if (bala.GetComponent<Rigidbody>() != null)
                bala.GetComponent<Rigidbody>().linearVelocity = puntoSalida.forward * velocidadBala;
        }

        // --- 4. ACTUALIZAR UI ---
        if (esJugador)
        {
            ControlHUD.instancia.ActualizarBalas(municionActual, municionMax);
        }
    }

    // --- NUEVO MÉTODO PARA RECARGAR ---
    public void RecargarArma(int cantidad)
    {
        municionActual += cantidad;

        // Evitar superar el máximo
        if (municionActual > municionMax)
        {
            municionActual = municionMax;
        }

        // Actualizamos el HUD inmediatamente para ver que ha subido el número
        if (esJugador && ControlHUD.instancia != null)
        {
            ControlHUD.instancia.ActualizarBalas(municionActual, municionMax);
        }

        Debug.Log("Arma recargada. Balas: " + municionActual);
    }
}
