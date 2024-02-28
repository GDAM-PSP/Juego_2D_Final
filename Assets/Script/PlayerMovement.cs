using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D Rigidbody2D;

    private float Horizontal;
    public float speed = 4f;
    public float jumpForce = 7f;
    private bool grounded;
    private Animator animator;
    private SpriteRenderer render;
    private Vector2 velocidadAnterior;
    private int points = 0;
    private int maxScore;
    public TextMeshProUGUI pointText, maxPointsText;
    // Variables para controlar el tiempo del último cambio en la velocidad
    float tiempoUltimoCambio = 0f;
    float tiempoEspera = 0.3f;
    public float vidas = 3f;
    public Camera cameraPlayer;
    private AudioSource audioSourcePlayer;
    public AudioClip sonidoCogerFruta, sonidoCurar, sonidoVolar, sonidoMuerte;
    public GameObject fadeOutJuego;

    void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        render = GetComponent<SpriteRenderer>();
        audioSourcePlayer = GetComponent<AudioSource>();
        maxScore = PlayerPrefs.GetInt("MaxScore", 0);

        if (maxScore != 0)
        {
            maxPointsText.text = "Max Puntos: " + maxScore.ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (animator.GetBool("death") == false)
        {
            Horizontal = Input.GetAxisRaw("Horizontal");



            if (Input.GetKeyDown(KeyCode.W) && grounded)
            {
                animator.SetTrigger("jump");
                animator.SetBool("down", false);
                Jump();
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                //Ataque
                animator.SetTrigger("attack1");
            }

            if (Horizontal < 0)
            {
                render.flipX = true;
            }
            if (Horizontal > 0)
            {
                render.flipX = false;
            }

            Vector2 velocidadActual = Rigidbody2D.velocity;
            // Verificar si la velocidad ha cambiado
            if (velocidadActual != velocidadAnterior && grounded)
            {
                // La velocidad ha cambiado, aquí puedes realizar acciones
                animator.SetBool("sprint", false);

                // Actualizar la velocidad anterior para la siguiente iteración
                velocidadAnterior = velocidadActual;

                // Actualizar el tiempo del último cambio
                tiempoUltimoCambio = Time.time;
            }
            else
            {
                if (Time.time - tiempoUltimoCambio >= tiempoEspera)
                {
                    animator.SetBool("sprint", true);
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (animator.GetBool("death") == false)
        {
            Rigidbody2D.velocity = new Vector2(Horizontal * speed, Rigidbody2D.velocity.y);

            Vector2 velocidadActual = Rigidbody2D.velocity;
            // Verificar si la velocidad ha cambiado
            if (velocidadActual != velocidadAnterior && grounded)
            {
                // La velocidad ha cambiado, aquí puedes realizar acciones
                animator.SetBool("sprint", false);

                // Actualizar la velocidad anterior para la siguiente iteración
                velocidadAnterior = velocidadActual;
            }
            else
            {
                animator.SetBool("sprint", true);
            }
        }
    }
    private void Jump()
    {
        Rigidbody2D.AddForce(Vector2.up * jumpForce);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            grounded = true;
            animator.SetBool("down", false);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            grounded = false;
            animator.SetBool("down", true);
        }
    }

    public void Destruir()
    {
        cameraPlayer.transform.parent = null;
        animator.SetBool("death", true);
        fadeOutJuego.SetActive(true);
        StartCoroutine(Death());
    }
    IEnumerator Death()
    {
        audioSourcePlayer.PlayOneShot(sonidoMuerte);
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
        SceneManager.LoadScene("FinalGameOver");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "fruta")
        {
            audioSourcePlayer.PlayOneShot(sonidoCogerFruta);
            points++;
            Debug.Log(points);
            pointText.text = "Puntos: " + points.ToString();
            Debug.Log("Puntos" + points);
            Destroy(collision.gameObject);
        }

        if (collision.tag == "manzana")
        {
            //Si nos curamos, solo da 1 punto, si tenemos todas las vidas, nos da 2 puntos
            if (vidas < 3)
            {
                audioSourcePlayer.PlayOneShot(sonidoCurar);
                vidas++;
                points++;
            } else
            {
                audioSourcePlayer.PlayOneShot(sonidoCogerFruta);
                points += 2;
            }

            Debug.Log(points);
            pointText.text = "Puntos: " + points.ToString();
            Debug.Log("Puntos" + points);
            Destroy(collision.gameObject);
        }

        if(collision.tag == "ventilador")
        {
            audioSourcePlayer.PlayOneShot(sonidoVolar);
            animator.SetTrigger("jump");
            animator.SetBool("down", false);
            Rigidbody2D.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
        }

        //Miramos lo último si el maxScore es mayor al anterior
        if (points > maxScore)
        {
            maxScore = points;
            maxPointsText.text = "Max Puntos: " + maxScore.ToString();
            PlayerPrefs.SetInt("MaxScore", maxScore);

        }
    }
    public void ColisionMinotauro()
    {
        vidas--;
        if(vidas <= 0)
        {
            Destruir();
        }
        else
        {
            Animator animator = GetComponent<Animator>();
            animator.SetTrigger("dammage");
        }
    }

    

   
}
