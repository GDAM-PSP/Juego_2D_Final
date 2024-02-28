using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Minotaur : MonoBehaviour
{
    Transform player;
    [Header("Ataque")]
    [SerializeField] private Transform controladorAtaque;
    [SerializeField] private Transform controladorEmbestida;
    [SerializeField] private float radioAtaque;
    [SerializeField] private float danoAtaque;
    [SerializeField] private float radioEmbestida;
    [SerializeField] private float danoEmbestida;
    private float tiempoUltimaExplosion;

    private bool mirandoDerecha = true;
    public GameObject Player;
    SpriteRenderer spriteRenderer;
    private Animator animator;
    public float life = 300f;
    public GameObject explosionPrefabs;
    private AudioSource audioSource;
    public AudioClip sonidoHacha;
    public Rigidbody2D rb; // Referencia al Rigidbody del goblin
    // Start is called before the first frame update
    [SerializeField] private BarraDeVidaMinotauro barraDeVida;
    [SerializeField] private float maximoVida;
    public GameObject fadeOutJuego;

    void Start()
    {
        life = maximoVida;
        animator = GetComponent<Animator>();
        player = GameObject.Find("PlayerRed").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        barraDeVida.InicializarBarraDeVida(life);
    }

    // Update is called once per frame
    void Update()
    {
     if (life < 0)
       {
        StartCoroutine(EnemigoMuere());
       }
       else
        {
           float distanciaJugador = Vector2.Distance(transform.position, player.transform.position);
           animator.SetFloat("distanciaJugador", distanciaJugador);
        }
    }

    void Die()
    {
        // Coloca aquí cualquier lógica adicional que desees realizar cuando el minotauro muera
        Debug.Log("El Minotauro ha muerto.");
        Destroy(gameObject);
        SceneManager.LoadScene("FinalWin");
    }

    IEnumerator EnemigoMuere()
    {
        animator.SetTrigger("death");
        // Desactiva el componente Rigidbody del GameObject
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true; // Esto hace que el Rigidbody no sea afectado por fuerzas
        }
        Instantiate(explosionPrefabs, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
        fadeOutJuego.SetActive(true);
        yield return new WaitForSeconds(2f); // Espera 2 segundos
        fadeOutJuego.SetActive(true);
        Die();
    }

    public void MirarJugador()
    {
        if((Player.transform.position.x > transform.position.x && !mirandoDerecha) || (Player.transform.position.x < transform.position.x && mirandoDerecha))
        {
            mirandoDerecha = !mirandoDerecha;
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
        }
    }

    public void Ataque()
    {
        audioSource.PlayOneShot(sonidoHacha);
        Collider2D[] objetos = Physics2D.OverlapCircleAll(controladorAtaque.position, radioAtaque);
        foreach (Collider2D colision in objetos)
        {
            if (colision.CompareTag("Player"))
            {
                colision.GetComponent<PlayerMovement>().ColisionMinotauro();
            }
        }
    }

    public void Embestida()
    {
        Collider2D[] objetos = Physics2D.OverlapCircleAll(controladorEmbestida.position, radioEmbestida);
        foreach (Collider2D colision in objetos)
        {
            if (colision.gameObject.CompareTag("Player"))
            {
                if (Time.time - tiempoUltimaExplosion >= 1f)
                {
                    audioSource.PlayOneShot(sonidoHacha);
                    tiempoUltimaExplosion = Time.time;
                    colision.GetComponent<PlayerMovement>().ColisionMinotauro();
                }
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(controladorAtaque.position, radioAtaque);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(controladorEmbestida.position, radioEmbestida);
    }
    public void TomarDaño(float daño)
    {
        life -= daño;
        barraDeVida.CambiarVidaActual(life);
    }
}
