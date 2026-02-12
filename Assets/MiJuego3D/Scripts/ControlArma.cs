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
        ultimoTiempoDisparo = Time.time;
        municionActual--;

        GameObject bala = balaPool.getObjeto();
        // Asegúrate de que puntoSalida existe, si no dará error
        if (puntoSalida != null)
        {
            bala.transform.position = puntoSalida.position;
            bala.transform.rotation = puntoSalida.rotation;
            bala.GetComponent<Rigidbody>().linearVelocity = puntoSalida.forward * velocidadBala;
        }

        if (esJugador)
        {
            // CORREGIDO: Llamamos al método nuevo ActualizarBalas
            ControlHUD.instancia.ActualizarBalas(municionActual, municionMax);
        }
    }
}
