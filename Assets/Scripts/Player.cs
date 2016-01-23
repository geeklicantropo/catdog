using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    private Rigidbody2D myRigidBody;
    private Animator myAnimator;

    [SerializeField]
    private float movementSpeed;

    //São os colliders no pé do player
    [SerializeField]
    private Transform[] groundPoints;

    //Collider circular que ficará em volta dos groundPoints no pé do player
    [SerializeField]
    private float groundRadius;

    //Indica em qual layer algo está
    [SerializeField]
    private LayerMask whatIsGrounded;

    private bool isGrounded;

    private bool jump;

    private bool flyingKick;

    [SerializeField]
    private bool airControl;

    [SerializeField]
    private float jumpForce;

    private bool facingRight;

    private bool uppercutAttack;

    // Use this for initialization
    void Start()
    {
        facingRight = true;
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        HandleInputs();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");//Move para esquerda e direita ( eixo x)

        isGrounded = IsGrounded();

        HandleMovement(horizontal);

        //Chama a função de virar o personagem para o lado que o botão está sendo pressionado.
        Flip(horizontal);

        HandleAttacks();

        HandleLayers();

        ResetValues();
    }

    private void HandleMovement(float horizontal)
    {   
        //Se estivermos caindo, a animação de caindo será setada para true
        if(myRigidBody.velocity.y < 0)
        {
            myAnimator.SetBool("land", true);
        }

        //Pega o estado atual do layer 0 ( layer base) e verifica se está a tag referente à ele é "Attack"
        //Se a tag não for "Attack", o personagem não está atacando, e só então ele pode se mover.
        if(!this.myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            myRigidBody.velocity = new Vector2(horizontal * movementSpeed, myRigidBody.velocity.y);
        }

        //If estranho. Se ele está no chão e pulando ao mesmo tempo, então ele não está no chão. Logo, ele está pulando.
        if(isGrounded && jump)
        {
            isGrounded = false;
            //Adiciona uma força que o impulsiona para cima no pulo
            myRigidBody.AddForce(new Vector2(0, jumpForce));
            //Chama a animação de pular.
            myAnimator.SetTrigger("jump");
        }
               
        myAnimator.SetFloat("speed", Mathf.Abs(horizontal));//Horizontal permite movimentos em x negativo, porém ele deve sempre retornar um número positivo para o animator.

    }

    private void HandleAttacks()
    {
        //Se ele executa o uppercut, esse if impede que ele se movimente ao mesmo tempo que ataque.
        if(uppercutAttack && isGrounded && !this.myAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack") && (isGrounded || airControl))
        {
            myAnimator.SetTrigger("uppercut");
            //Desliga a inércia do rigidBody depois de atacar, para evitar que o personagem deslize.
            myRigidBody.velocity = Vector2.zero;
        }

        //Se pressionarmos o botão de flying kick e não estamos no chão e nem estamos fazendo já o flyingKick, então:
        if(flyingKick && !isGrounded && !this.myAnimator.GetCurrentAnimatorStateInfo(1).IsName("flyingKick"))
        {
            myAnimator.SetBool("flyingKick", true);
        }

        if (!flyingKick && !this.myAnimator.GetCurrentAnimatorStateInfo(1).IsName("flyingKick"))
        {
            myAnimator.SetBool("flyingKick", false);
        }
    }

    private void HandleInputs()
    {
        //Aperta o espaço para pular
        if(Input.GetKeyDown(KeyCode.Space))
        {
            jump = true;
        }


        //Pressiona leftShift para realizar um uppercut
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            uppercutAttack = true;
            flyingKick = true;
        }
    }

    //Vira o personagem para a direção horizontal em que o botão está sendo apertado
    private void Flip(float horizontal)
    {
        if((horizontal > 0 && !facingRight) ||(horizontal < 0 && facingRight))
        {
            facingRight = !facingRight;

            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }

    //Função responsável por resetar os valores pertinentes, para que eles não fiquem executando eternamente.
    private void ResetValues()
    {
        uppercutAttack = false;
        jump = false;
        flyingKick = false;
    }

    //Função responsável por verificar se o player está no chão
    private bool IsGrounded()
    {
        //Verifica se o player está caindo
        if(myRigidBody.velocity.y <= 0)
        {
            //Se o player estiver caindo, ele percorrerá todos os gameObjects no pé do player
            foreach(Transform point in groundPoints)
            {
                //colliders conterão todos os pontos em que o groundRadius está circundando.
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, whatIsGrounded);

                for (int i = 0; i < colliders.Length; i++)
                {
                    //Se o collider sendo verificado nesse momento, não é o player (gameObject), faça:
                    //Retornarmos true se colidimos com alguma coisa que não seja o player, com os pés. 
                    if(colliders[i].gameObject != gameObject)
                    {
                        //se o player está no chão, temos que resetar a sua habilidade de pular ou ele ficará pulando infinitamente.
                        myAnimator.ResetTrigger("jump");
                        //Quando terminarmos de chegar ao chão ( land), não precisaremos mais realizar essa tarefa enquanto estivermos no chão.
                        myAnimator.SetBool("land", false);
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private void HandleLayers()
    {
        //Se o player não está no chão, ou seja, está no ar:
        if(!isGrounded)
        {
            //Coloca o peso do layer 1 para 1, tendo prioridade sobre os layers que tiverem peso abaixo dele.
            myAnimator.SetLayerWeight(1, 1);
        }
        else
        {
            //Senão, o peso do layer do índice 1, será colocado para zero.
            myAnimator.SetLayerWeight(1, 0);
        }
    }
}