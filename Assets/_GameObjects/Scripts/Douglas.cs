using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Douglas : MonoBehaviour {
    public GameObject warningQuad;
    public Transform posOjos;//Posicion de los ojos para comprobar la linea de vision
    public Transform[] puntosPatrulla;
    public float distanciaDeteccionVisual;//A partir de qué distancia he visto a Ryan
    public float umbralVision;//Grados dentro de los cuales el vigilante ve a Ryan
    public float distanciaAviso;//A partir de qué distancia avisa que puede ver a Ryan
    public float distanciaDeteccionPasos;//A partir de qué distancia he escuchado andar a Ryan
    public float distanciaDeteccionCarrera;//A partir de qué distancia he escuchado correr a Ryan
    public float anguloVision;//Angulo de vision con respecto a Ryan
    public float distanciaAlPlayer;//Distancia a Ryan
    public bool enloquecido = false;//Determina si esta siguiendo a un senyuelo

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
    private void Update()
    {
        Detectar();
        if (estado != Estado.Idle && agente.remainingDistance < agente.stoppingDistance)
        {
            warningQuad.SetActive(false);
            enloquecido = false;
            estado = Estado.Idle;
            animador.SetBool(ANIM_ISWALKING, false);
            Invoke("AsignarDestino", 2);
        }
    }
    private void AsignarDestinoReal(int indice)
    {
        if (!enloquecido)
        {
            estado = Estado.Walking;
            agente.destination = puntosPatrulla[indice].position;
            animador.SetBool(ANIM_ISWALKING, true);
        }
    }
    private void AsignarDestinoReal(Vector3 nuevoDestino)
    {
        if (!enloquecido)
        {
            estado = Estado.Walking;
            agente.destination = nuevoDestino;
            animador.SetBool(ANIM_ISWALKING, true);
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
        if (distanciaAlPlayer <= distanciaDeteccionVisual)
        {
            anguloVision = ObtenerAnguloAlPlayer();
            if (anguloVision <= umbralVision)
            {
                //TE ESTA VIENDO
                HayVisionDirecta();
            }
        }
        switch (player.State){
            case Player.Estado.Idle:
                break;
            case Player.Estado.Walking:
                if (distanciaAlPlayer <= distanciaDeteccionPasos)
                {
                    //He escuchado al player
                    warningQuad.SetActive(true);
                    AsignarDestinoReal(player.gameObject.transform.position);
                }
                break;
            case Player.Estado.Running:
                if (distanciaAlPlayer <= distanciaDeteccionCarrera)
                {
                    //He escuchado al player
                    warningQuad.SetActive(true);
                    AsignarDestinoReal(player.gameObject.transform.position);
                }
                break;
            default:
                break;
        }
    }
    private float ObtenerDistanciaAlPlayer()
    {
        return Vector3.Distance(transform.position, player.gameObject.transform.position);
    }
    private float ObtenerAnguloAlPlayer()
    {
        float angulo = 0;
        Vector3 direccion = player.gameObject.transform.position - transform.position;
        angulo = Vector3.Angle(transform.forward, direccion);
        return angulo;
    }
    public void SetSenyuelo(Vector3 posSenyuelo)
    {
        AsignarDestinoReal(posSenyuelo);
        enloquecido = true;
    }
    private bool HayVisionDirecta()
    {
        RaycastHit rch;
        bool hayVisionDirecta = false;
        Vector3 direccion = player.transform.position - posOjos.position;
        //Debug.DrawRay(posOjos.position, direccion * 10, Color.red, 1);
        bool hayColision = Physics.Raycast(posOjos.position, direccion, out rch, Mathf.Infinity);
        if (hayColision)
        {
            if (rch.transform.name == "Ryan")
            {
                rch.transform.gameObject.GetComponent<Player>().Kill();
            }
        }
        return hayVisionDirecta;
    }
}