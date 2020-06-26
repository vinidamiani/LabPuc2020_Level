using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trdCam : MonoBehaviour
{
    public GameObject player;
    public Vector3 ajust;
    public Vector3 ajustlook;
    GameObject fakeObject;


    float zajust=-3;
    // Start is called before the first frame update
    void Awake()
    {
        fakeObject = new GameObject();
    }

    public GameObject GetRefereceObject()
    {
        if (!fakeObject)
            fakeObject = new GameObject ();
        return fakeObject;
    }

    // Update is called once per frame
    void Update()
    {
        fakeObject.transform.position = Vector3.Lerp(fakeObject.transform.position,player.transform.position, Time.deltaTime*10); //interpolacao do objeto falso

        Vector3 dirback = transform.position- (player.transform.position + ajustlook) ; //direcao do personagem a camera
        float distancetohit=10;// distancia pra armazenar
       
        //raycast pra testar a colisao atraz da camera
        if (Physics.Raycast(player.transform.position + ajustlook, dirback, out RaycastHit hit, 10, 65279))
        {
            distancetohit= hit.distance; 
            Debug.DrawLine(player.transform.position + ajustlook, hit.point);
        }
        //vetor de distanciamento
        Vector3 backvector = (fakeObject.transform.forward * ajust.z);
        //limite de tamanho do vetor 
        backvector = Vector3.ClampMagnitude(backvector, distancetohit);

        //aplicacao da posicao da camera
        transform.position = fakeObject.transform.position + backvector + fakeObject.transform.up*ajust.y;

        //olhar para o jogador
        transform.LookAt(player.transform.position + ajustlook);

        //ajuste de distancia pelo mouse 
        zajust = Mathf.Clamp(zajust + Input.mouseScrollDelta.y, -6, -1);
        ajust =new Vector3(0, ajust.y, zajust);

        //aplicacao da rotaçao pelo mouse na camera
        fakeObject.transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X"), 0));
       
    }
}
