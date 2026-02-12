using UnityEngine;

public enum TipoExtra
{
    Vida,
    Balas
}
public class ControlExtras : MonoBehaviour
{
    public TipoExtra tipo;
    public int cantidad;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Jugador"))
        {
            ControlJugador jugador = other.GetComponent<ControlJugador>();
            switch (tipo)
            {
                case TipoExtra.Vida:
                    jugador.IncrementaVida(cantidad);
                    break;
                case TipoExtra.Balas:
                    jugador.IncrementarBalas(cantidad);
                    break;
            }
            Destroy(gameObject);
        }
    }
}
