using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorAudio : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip clip1, clip2, grunido;
    public float distanciaMovimiento = 10f;
    private GameObject minotaur;
    private GameObject player;
    private GameObject vidaMinotauro;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("PlayerRed");
        minotaur = GameObject.Find("Minotaur");
        vidaMinotauro = GameObject.Find("VidaBoss");
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(minotaur.transform.position, player.transform.position);

        if (distanceToPlayer >= distanciaMovimiento)
        {
            ReproducirClip1();
            vidaMinotauro.SetActive(false);
        }
        else
        {
            ReproducirClip2();
            vidaMinotauro.SetActive(true);
        }
    }

    // Método para reproducir el primer clip
    public void ReproducirClip1()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Stop(); // Detiene la reproducción actual si la hay
            audioSource.clip = clip1; // Establece el clip que deseas reproducir
            audioSource.Play(); // Inicia la reproducción del clip
        }
    }

    // Método para reproducir el segundo clip
    public void ReproducirClip2()
    {
        if (!audioSource.isPlaying || audioSource.clip != clip2)
        {
            audioSource.Stop(); // Detiene la reproducción actual si la hay
            audioSource.PlayOneShot(grunido);
            CinemachineMovimientoCamara.Instance.MoverCamara(4f, 4f, 4f);
            audioSource.clip = clip2; // Establece el clip que deseas reproducir
            audioSource.Play(); // Inicia la reproducción del clip
        }
    }
}
