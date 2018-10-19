using UnityEngine;
using UnityEngine.UI;

public class VideoController : MonoBehaviour
{
    public static VideoController ME;

    public RawImage rawImage;
    public AudioSource audioSource;
    private MovieTexture mMovieTexture;

    private void Awake()
    {
        ME = this;
        rawImage.gameObject.SetActive(false);
    }

    public void PlayMovie(MovieTexture movieTexture)
    {
        rawImage.gameObject.SetActive(true);

        mMovieTexture = movieTexture;

        rawImage.material.mainTexture = movieTexture;
        audioSource.clip = movieTexture.audioClip;

        movieTexture.Play();
        audioSource.Play();
    }

    public void StopMovie()
    {
        mMovieTexture.Play();
        audioSource.Play();
    }
    
    private void OnDestroy()
    {
        ME = null;
    }
}
