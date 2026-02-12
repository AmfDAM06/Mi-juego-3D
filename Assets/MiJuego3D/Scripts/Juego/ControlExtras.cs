using UnityEngine;

public class ControlExtras : MonoBehaviour
{
    // Creamos un selector para elegir el tipo de objeto desde Unity
    public enum TipoObjeto { Vida, Municion }

    [Header("Configuración")]
    public TipoObjeto tipoDeObjeto; // <-- Aquí elegirás qué es
    public int cantidad = 10;       // Cuánta vida o balas da

    [Header("Efectos")]
    public GameObject efectoParticulas; // Opcional: si quieres brillos al cogerlo
    // public AudioClip sonidoCoger;    // Opcional: para sonido más adelante

    private void OnTriggerEnter(Collider other)
    {
        // Solo nos interesa si choca con el Jugador
        if (other.CompareTag("Jugador") || other.CompareTag("Player"))
        {
            ControlJugador jugador = other.GetComponent<ControlJugador>();

            if (jugador != null)
            {
                // Dependiendo de qué hayamos elegido en el inspector...
                switch (tipoDeObjeto)
                {
                    case TipoObjeto.Vida:
                        jugador.IncrementaVida(cantidad);
                        Debug.Log("Vida recogida!");
                        break;

                    case TipoObjeto.Municion:
                        jugador.IncrementarBalas(cantidad);
                        Debug.Log("Munición recogida!");
                        break;
                }

                // Generar efecto visual si existe
                if (efectoParticulas != null)
                {
                    Instantiate(efectoParticulas, transform.position, Quaternion.identity);
                }

                // Destruir el objeto (la caja/corazón desaparece)
                Destroy(gameObject);
            }
        }
    }
}