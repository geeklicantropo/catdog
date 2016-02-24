using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    //Criando um Singleton
    private static Player instance;
    public static Player Instance
    {
        get
        {
            if(instance == null)
            {
                instance = GameObject.FindObjectOfType<Player>();
            }
            return instance;
        }
    }

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

    [SerializeField]
    private bool airControl;

    [SerializeField]
    private float jumpForce;

    private bool facingRight;

    public Rigidbody2D MyRigidbody { get; set; }

    //São os chamados propriedades, que servem para usar essas funções em outros lugares depois. Por isso, eles possuem letra maiúscula 
    public bool Attack { get; set; }
    public bool Jump { get; set; }
    public bool OnGround { get; set; }


    // Use this for initialization
    void Start()
    {
        facingRight = true;
        MyRigidbody = GetComponent<Rigidbody2D>();
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

        OnGround = IsGrounded();

        HandleMovement(horizontal);

        //Chama a função de virar o personagem para o lado que o botão está sendo pressionado.
        Flip(horizontal);

        HandleLayers();
    }

    private void HandleMovement(float horizontal)
    {   
        //Se a velocidade do player no eixo y for menor que 0, então ele está no chão ( não está no ar - pulando)
        if(MyRigidbody.velocity.y < 0)
        {
            myAnimator.SetBool("land", true);
        }

        if(!Attack && (OnGround || airControl))
        {
            MyRigidbody.velocity = new Vector2(horizontal * movementSpeed, MyRigidbody.velocity.y);
        }

        if(Jump && MyRigidbody.velocity.y == 0)
        {
            MyRigidbody.AddForce(new Vector2(0, jumpForce));
        }

        myAnimator.SetFloat("speed", Mathf.Abs(horizontal));
    }

    private void HandleInputs()
    {
        //Aperta o espaço para pular
        if(Input.GetKeyDown(KeyCode.Space))
        {
            myAnimator.SetTrigger("jump");
        }


        //Pressiona leftShift para realizar um uppercut
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            myAnimator.SetTrigger("uppercut");
        }

        //Botão de soltar o Hadouken
        if (Input.GetKeyDown(KeyCode.V))
        {
            myAnimator.SetTrigger("throw");
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

    //Função responsável por verificar se o player está no chão
    private bool IsGrounded()
    {
        //Verifica se o player está caindo
        if(MyRigidbody.velocity.y <= 0)
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
        if(!OnGround)
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