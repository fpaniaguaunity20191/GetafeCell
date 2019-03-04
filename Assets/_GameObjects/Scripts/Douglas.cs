using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Douglas : MonoBehaviour {
    public Transform[] puntosPatrulla;
    private NavMeshAgent agente;
    private Animator animador;
    private const string ANIM_ISWALKING = "isWalking";
    private int indiceNavegacion = 0;
    private enum Estado { Idle, Walking };
    private Estado estado = Estado.Idle;
    private void Awake()
    {
        agente = GetComponent<NavMeshAgent>();
        animador = GetComponent<Animator>();
    }

    private void Start()
    {
        AsignarDestinoReal(indiceNavegacion);
    }

    private void AsignarDestinoReal(int indice)
    {
        estado = Estado.Walking;
        agente.destination = puntosPatrulla[indice].position;
        animador.SetBool(ANIM_ISWALKING, true);
    }

    private void Update()
    {
        if (estado!=Estado.Idle && agente.remainingDistance < 0.1f)
        {
            estado = Estado.Idle;
            animador.SetBool(ANIM_ISWALKING, false);
            Invoke("AsignarDestino", 2);
        }
    }

    private void AsignarDestino()
    {
        //Navegacion 'aleatoria' con control de errores
        int nuevoIndice = Random.Range(0, puntosPatrulla.Length);
        if (nuevoIndice == indiceNavegacion)
        {
            nuevoIndice++;
        }
        if (nuevoIndice == puntosPatrulla.Length)
        {
            nuevoIndice = 0;
        }
        indiceNavegacion = nuevoIndice;

        /*
        //Navegacion aleatoria pura
        int nuevoIndice;
        do
        {
            nuevoIndice = Random.Range(0, puntosPatrulla.Length);
        } while (nuevoIndice == indiceNavegacion);
        indiceNavegacion = nuevoIndice;
        */

        /*
        //Navegacion secuencial
        indiceNavegacion++;
        if (indiceNavegacion == puntosPatrulla.Length)
        {
            indiceNavegacion = 0;
        }
        */
        AsignarDestinoReal(indiceNavegacion);
    }
}
