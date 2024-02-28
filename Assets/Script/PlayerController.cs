using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private float moveHorizontal;
    private Rigidbody2D rigidPlayer;
    private float speed = 5;
    private SpriteRenderer render;
    private float jumpForce = 7.8f;
    private bool isOnGround = true;
    private int points = 0;
    public TextMeshProUGUI pointText;
    private AudioSource audioSourcePlayer;
    public AudioClip jumpAudioClip, DiamondAudioClip, attackAudioClip, explosionAudioClip;
    public Camera cameraPlayer;
    public GameObject explosionPrefab;
    private Animator animator;
    public SpawnManager[] spawnManagerControllers;
    public GameObject panelGameOver;

    // Start is called before the first frame update
    void Start()
    {
        rigidPlayer = GetComponent<Rigidbody2D>();
        render = GetComponent<SpriteRenderer>();
        audioSourcePlayer = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //atacar
            animator.SetTrigger("attack");
            audioSourcePlayer.PlayOneShot(attackAudioClip);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) && isOnGround)
        {
            //Saltar
            audioSourcePlayer.PlayOneShot(jumpAudioClip);
            rigidPlayer.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
        moveHorizontal = Input.GetAxis("Horizontal");
        if(moveHorizontal < 0)
        {
            render.flipX = true;
        }
        if (moveHorizontal > 0)
        {
            render.flipX = false;
        }
        //rigidPlayer.position = rigidPlayer.position + new Vector3(moveHorizontal,0,0);
        transform.position += new Vector3(moveHorizontal,0,0) * Time.deltaTime * speed;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            isOnGround = true;
        } 
        
        if(collision.gameObject.tag == "Enemy")
        {
            cameraPlayer.transform.parent = null;
            Instantiate(explosionPrefab,transform.position + new Vector3(0,0.5f,0), Quaternion.identity);
            for (int i = 0; i < spawnManagerControllers.Length; i++)
            {
                spawnManagerControllers[i].CancelarEnemigos();
            }
            panelGameOver.SetActive(true);
            Destroy(gameObject);
        }        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isOnGround= false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "fruta")
        {
            //audioSourcePlayer.PlayOneShot(DiamondAudioClip);
            points++;
            Debug.Log(points);
            pointText.text = "Puntos: " + points.ToString();
            Debug.Log("Puntos" + points);
            Destroy(collision.gameObject);
        }
    }
}
