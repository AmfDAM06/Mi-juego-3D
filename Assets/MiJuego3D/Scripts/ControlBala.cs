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
        // 1. Si chocamos con el JUGADOR (El enemigo nos dispara a nosotros)
        if (other.CompareTag("Jugador") || other.CompareTag("Player"))
        {
            // Usamos GetComponent de forma segura
            ControlJugador jugador = other.GetComponent<ControlJugador>();
            if (jugador != null)
            {
                jugador.QuitarVidasJugador(cantidadVida);
            }
        }
        // 2. Si chocamos con el ENEMIGO (Nosotros le disparamos a él)
        else if (other.CompareTag("Enemigo"))
        {
            // --- EL CAMBIO CLAVE ESTÁ AQUÍ ---
            // Buscamos el script 'ControlEnemigoMejorado', NO el antiguo
            ControlEnemigoMejorado enemigo = other.GetComponent<ControlEnemigoMejorado>();

            if (enemigo != null)
            {
                // Llamamos al método RecibirDaño que creamos antes
                enemigo.RecibirDaño(cantidadVida);
            }
            // --------------------------------
        }

        // 3. Generar las partículas de explosión (si tienes el prefab asignado)
        if (particulasExplosion != null)
        {
            GameObject particulas = Instantiate(particulasExplosion, transform.position, Quaternion.identity);
            Destroy(particulas, 1f); // Limpiamos las partículas después de 1 segundo
        }

        // 4. Desactivar la bala (porque usáis Pool de Objetos)
        gameObject.SetActive(false);
    }
}