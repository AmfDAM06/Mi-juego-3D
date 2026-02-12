using UnityEngine;

public class ControlBala : MonoBehaviour
{
    public GameObject particulasExplosion;
    public int cantidadVida;
    public float tiempoActivo;
    private float tiempoDisparo;

    public void OnEnable()
    {
        tiempoDisparo = Time.time;
    }

    private void Update()
    {
        if (Time.time - tiempoDisparo >= tiempoActivo)
        {
            gameObject.SetActive(false);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        // Colisión con JUGADOR
        if (other.CompareTag("Jugador") || other.CompareTag("Player"))
        {
            var jugador = other.GetComponent<ControlJugador>();
            if (jugador != null) jugador.QuitarVidasJugador(cantidadVida);
        }
        // Colisión con ENEMIGO (Aquí estaba el fallo)
        else if (other.CompareTag("Enemigo"))
        {
            // Buscamos 'ControlEnemigoMejorado' en vez del antiguo
            var enemigo = other.GetComponent<ControlEnemigoMejorado>();

            if (enemigo != null)
            {
                enemigo.RecibirDaño(cantidadVida); // Asegúrate que se llama RecibirDaño en el script del enemigo
            }
        }

        // Efectos
        if (particulasExplosion != null)
        {
            GameObject particulas = Instantiate(particulasExplosion, transform.position, Quaternion.identity);
            Destroy(particulas, 1f);
        }

        gameObject.SetActive(false); // Desactivar bala
    }
}