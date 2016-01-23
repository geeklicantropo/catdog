using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private float xMax;

    [SerializeField]
    private float yMax;

    [SerializeField]
    private float xMin;

    [SerializeField]
    private float yMin;

    private Transform target;

    // Use this for initialization
    void Start ()
    {
        //Referência para a transform do Player
        target = GameObject.Find("Player").transform;
	}
	
	// O Player tem que se mover primeiro para que depois a camera o acompanhe.
	void LateUpdate ()
    {
        //Determina que a posição da camera não possa ser maior do que um mínimo e um máximo x e y de posição
        transform.position = new Vector3(Mathf.Clamp(target.position.x, xMin, xMax), Mathf.Clamp(target.position.y, yMin, yMax), transform.position.z);
	}
}
