using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Douglas : MonoBehaviour {
    public Transform[] puntosPatrulla;
    public float distanciaDeteccionVisual;//A partir de qué distancia he visto a Ryan
    public float distanciaAviso;//A partir de qué distancia avisa que puede ver a Ryan
    public float distanciaDeteccionPasos;//A partir de qué distancia he escuchado andar a Ryan
    public float distanciaDeteccionCarrera;//A partir de qué distancia he escuchado correr a Ryan
    public float anguloVision;//Angulo de vision
    public float distanciaAlPlayer;//Distancia al player

    private NavMeshAgent agente;
    private Animator animador;
    private const string ANIM_ISWALKING = "isWalking";
    private int indiceNavegacion = 0;
    private enum Estado { Idle, Walking };
    private Estado estado = Estado.Idle;

    private Player player;//Referencia al script del jugador

    private void Awake()
    {
        agente = GetComponent<NavMeshAgent>();
        animador = GetComponent<Animator>();
    }
    private void Start()
    {
        AsignarDestinoReal(indiceNavegacion);
        player = GameObject.Find("Ryan").GetComponent<Player>();
    }
    private void AsignarDestinoReal(int indice)
    {
        estado = Estado.Walking;
        agente.destination = puntosPatrulla[indice].position;
        animador.SetBool(ANIM_ISWALKING, true);
    }
    private void Update()
    {
        Detectar();
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
    private void Detectar()
    {
        distanciaAlPlayer = ObtenerDistanciaAlPlayer();
    }
    private float ObtenerDistanciaAlPlayer()
    {
        return Vector3.Distance(transform.position, player.gameObject.transform.position);
    }
}
