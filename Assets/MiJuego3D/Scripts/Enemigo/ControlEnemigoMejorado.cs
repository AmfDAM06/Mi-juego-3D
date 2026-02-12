using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class ControlEnemigoMejorado : MonoBehaviour
{
    [Header("Estadística")]
    public int vidasActual;
    public int vidasMax;
    public int puntuacionEnemigo;

    [Header("Movimiento")]
    public float velocidad;
    public float rangoAtaque;
    private List<Vector3> listaCaminos;
    private ControlArma arma;
    private GameObject objetivo;

    [Header("Referencias")]
    public ControlHUD scriptHUD; // <--- NUEVO: Arrastra aquí el Canvas/HUD

    void Start()
    {
        arma = GetComponent<ControlArma>();
        objetivo = GameObject.FindGameObjectWithTag("Jugador");

        ActualizarCaminos();
        // Cada medio segundo repite la creación de la lista de caminos
        InvokeRepeating("ActualizarCaminos", 0.0f, 0.2f);
    }

    void ActualizarCaminos()
    {
        NavMeshPath caminoCalulado = new NavMeshPath();
        NavMesh.CalculatePath(transform.position, objetivo.transform.position, NavMesh.AllAreas, caminoCalulado);
        listaCaminos = caminoCalulado.corners.ToList();
        foreach (var item in listaCaminos)
        {
            Debug.Log(Time.time + " " + item);
        }
    }

    private void Update()
    {
        float distancia = Vector3.Distance(transform.position, objetivo.transform.position);
        if (distancia > rangoAtaque) PerseguirObjetivo();
        else
        {
            if (arma.PuedeDisparar()) arma.Disparar();
        }
        // rota el enemigo para que dispare en dirección al jugador
        Vector3 direccion = (objetivo.transform.position - transform.position).normalized;
        float angulo = Mathf.Atan2(direccion.x, direccion.z) * Mathf.Rad2Deg;
        transform.eulerAngles = Vector3.up * angulo;
    }

    private void PerseguirObjetivo()
    {
        if (listaCaminos.Count == 0)
            return;

        Vector3 destino = new Vector3(listaCaminos[0].x, transform.position.y, listaCaminos[0].z);
        transform.position = Vector3.MoveTowards(transform.position, destino, velocidad * Time.deltaTime);
        if (transform.position == destino) listaCaminos.RemoveAt(0);
    }

    public void RecibirDaño(int danio)
    {
        vidasActual -= danio;

        if (vidasActual <= 0)
        {
            // --- NUEVO: AVISAR DE VICTORIA ---
            if (scriptHUD != null)
            {
                scriptHUD.MostrarVictoria(); // Abre la ventana de pausa con "Has Ganado"
            }
            else
            {
                Debug.LogWarning("¡Ojo! Falta asignar el scriptHUD en el Enemigo");
            }
            // ---------------------------------

            // Aquí seguramente tengas código para soltar loot o partículas
            // ...

            Destroy(gameObject); // Destruir al enemigo
        }
    }
}
