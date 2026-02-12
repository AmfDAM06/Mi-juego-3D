using System;
using UnityEngine;
public class ControlJuego : MonoBehaviour
{
    public int puntuacionParaGanar;
    public int puntuacionActual;
    public bool juegoPausado;
    public static ControlJuego instancia;
    public void Awake()
    {
        instancia = this;
    }
    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
            cambiarPausa();

        int numEnemigos = GameObject.FindGameObjectsWithTag("Enemigo").Length;

        if (numEnemigos <= 0) ganarJuego();
    }
    public void cambiarPausa()
    {
        juegoPausado = !juegoPausado;

        // CORREGIDO: Usamos la lógica del HUD nuevo
        if (juegoPausado)
            ControlHUD.instancia.PausarJuego("PAUSA");
        else
            ControlHUD.instancia.ReanudarJuego();
    }

    public void PonerPuntuacion(int puntuacion)
    {
        puntuacionActual += puntuacion;

        // CORREGIDO: Llamada al método nuevo
        ControlHUD.instancia.ActualizarPuntuacion(puntuacionActual);

        if (puntuacionActual >= puntuacionParaGanar)
            ganarJuego();
    }

    public void ganarJuego()
    {
        // CORREGIDO: Llamada al método de victoria
        ControlHUD.instancia.MostrarVictoria();
    }
}