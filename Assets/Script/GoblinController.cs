using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinController : MonoBehaviour
{
    Transform player;
    public GameObject Player;
    SpriteRenderer spriteRenderer;
    float speed = 3f;
    private Animator animator;
    public float live;
    public GameObject bombPrefab; // Prefab de la bomba
    public float bombForce = 10f; // Fuerza con la que se lanzar� la bomba
    private Rigidbody2D rb; // Referencia al Rigidbody del goblin
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.Find("PlayerRed").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
        live = 100f;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Player != null && Player.activeSelf)
        {
            if (live <= 0)
            {
                StartCoroutine(EnemigoMuere());
            }

            float distance = Mathf.Abs(Player.transform.position.x - transform.position.x);
            if (distance < 10.0f && distance > 5.0f)
            {
                animator.SetBool("bomb", false);
                animator.SetBool("run", true);
                transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
                if (transform.position.x < player.position.x)
                {
                    spriteRenderer.flipX = false;
                }
                else
                {
                    spriteRenderer.flipX = true;
                }
            }
            if (distance < 5.0f)
            {
                animator.SetBool("bomb", true);
                animator.SetBool("run", false);
            }
        }

    }
    void Die()
    {
        // Coloca aqu� cualquier l�gica adicional que desees realizar cuando el goblin muera
        Debug.Log("El goblin ha muerto.");
        Destroy(gameObject); // Destruye el GameObject del goblin
    }
    IEnumerator EnemigoMuere()
    {
        animator.SetBool("death", true);
        yield return new WaitForSeconds(1f); // Espera 2 segundos
        Die();
    }
    public void LanzarBomba()
    {
        if (Player != null && Player.activeSelf)
        {
            // Calcula la posici�n de instanciaci�n de la bomba en el centro del duende
            Vector3 posicionInstanciacion = transform.position + new Vector3(0f, 1f, 0f); // Ajusta el valor Y seg�n sea necesario

            // Instancia el prefab de la bomba en la posici�n del duende
            GameObject bomba = Instantiate(bombPrefab, posicionInstanciacion, Quaternion.identity);

            // Calcula la direcci�n hacia la que se lanzar� la bomba
            Vector2 direccionLanzamiento = (player.position - posicionInstanciacion).normalized;

            // Obt�n el componente Rigidbody2D de la bomba
            Rigidbody2D rbBomba = bomba.GetComponent<Rigidbody2D>();

            // Aplica una fuerza a la bomba en la direcci�n calculada
            rbBomba.AddForce(direccionLanzamiento * bombForce, ForceMode2D.Impulse);

            // Inicia la corutina para destruir la bomba despu�s de un tiempo
            StartCoroutine(DestruirBomba(bomba));
        }
    }

    IEnumerator DestruirBomba(GameObject bomba)
    {
        StartCoroutine(movimientoCamaraPorExplosion());
        yield return new WaitForSeconds(1.7f); // Espera 2 segundos
        Destroy(bomba); 
    }
    IEnumerator movimientoCamaraPorExplosion()
    {
        yield return new WaitForSeconds(1); // Esperar 2 segundos
        //Aplicamos movimiento a la camara con cada explosion de la bomba
        CinemachineMovimientoCamara.Instance.MoverCamara(5, 5, 0.2f);
    }
}
