using UnityEngine;
using System.Collections;

public class CollisionScript : MonoBehaviour
{

    private BoxCollider2D playerCollider;

    [SerializeField]
    private BoxCollider2D platformCollider;

    [SerializeField]
    private BoxCollider2D platformTrigger;


	// Use this for initialization
	void Start ()
    {
        //Essa variável está recebendo a referência do BoxCollider2D do jogador em cena
        playerCollider = GameObject.Find("Player").GetComponent<BoxCollider2D>();

        //Existem 2 Boxcolliders na plataforma de grama. A instrução abaixo, impedirá que o primeiroCollider, que é um collider só de Trigger
        //colida com o jogador pelos lados, e consequentemente, o jogador poderá passar pela plataforma de grama e só irá colidir por cima, 
        //onde o segundo collider(que não ignora a colisão) estará contido.
        Physics2D.IgnoreCollision(platformCollider, platformTrigger, true);
	}
	
    //Função responsável por fazer o papel de trigger contido no segundo Box Collider. E, como explicado antes, esse collider, permitirá que o 
    //player ignore a colisão e, consequentemente, possa passar por ele ( plataforma de grama).
	void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.name == "Player")
        {
            //É aqui que a plataforma de trigger está ignorando a colisão entre a plataforma de grama e o jogador.
            Physics2D.IgnoreCollision(platformCollider, playerCollider, true);
        }
    }

    //Função que dará a instrução sobre o que fazer quando o gatilho de alguma coisa, tiver cumprido sua tarefa.
    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.name == "Player")
        {
            //É aqui que a plataforma de trigger não vai estar ignorando a colisão entre a plataforma de grama e o jogador por cima.
            Physics2D.IgnoreCollision(platformCollider, playerCollider, false);
        }
    }
}
