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

        // Llamamos a los métodos que YA creamos en el HUD anteriormente
        if (juegoPausado)
        {
            ControlHUD.instancia.PausarJuego("PAUSA");
        }
        else
        {
            ControlHUD.instancia.ReanudarJuego();
        }
    }

    public void PonerPuntuacion(int puntuacion)
    {
        puntuacionActual += puntuacion;

        // CORREGIDO: Llamamos al método nuevo con mayúscula
        ControlHUD.instancia.ActualizarPuntuacion(puntuacionActual);

        if (puntuacionActual >= puntuacionParaGanar)
            ganarJuego();
    }

    public void ganarJuego()
    {
        // CORREGIDO: Usamos el método de Victoria que creamos antes
        ControlHUD.instancia.MostrarVictoria();
    }
}