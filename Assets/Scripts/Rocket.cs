using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum State
{
    Alive, Transcending, Dying
}

public class Rocket : MonoBehaviour
{
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;
    [SerializeField] float levelLoadDelay = 3f;

    [SerializeField] AudioClip mainEngineAudio;
    [SerializeField] AudioClip transcendingAudio;
    [SerializeField] AudioClip deathAudio;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem transcendingParticles;
    [SerializeField] ParticleSystem deathParticles;

    Rigidbody _rb;
    AudioSource _as;
    State _state;
    bool _isCollisionDisable;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _as = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Debug.isDebugBuild)
        {
            ResponseToDebugKeys();
        }

        if (_state == State.Alive)
        {
            RespondToThrustInput();
            RespondToRotateInput();
        }
    }

    private void ResponseToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextScene();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            _isCollisionDisable = !_isCollisionDisable;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isCollisionDisable) return;
        if (_state != State.Alive) return;

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartDyingSequence();
                break;
        }
    }

    private void StartSuccessSequence()
    {
        _state = State.Transcending;
        _as.Stop();
        _as.PlayOneShot(transcendingAudio);
        mainEngineParticles.Stop();
        transcendingParticles.Play();
        Invoke(nameof(LoadNextScene), levelLoadDelay);
    }

    private void StartDyingSequence()
    {
        _state = State.Dying;
        _as.Stop();
        _as.PlayOneShot(deathAudio);
        mainEngineParticles.Stop();
        deathParticles.Play();
        Invoke(nameof(LoadFirstScene), levelLoadDelay);
    }

    private void LoadFirstScene()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextScene()
    {
        var activeSceneIndex = SceneManager.GetActiveScene().buildIndex;
        var nextSceneIndex = activeSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else
        {
            _as.Stop();
            mainEngineParticles.Stop();
        }
    }

    private void ApplyThrust()
    {
        _rb.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        if (!_as.isPlaying)
        {
            _as.PlayOneShot(mainEngineAudio);
        }
        mainEngineParticles.Play();
    }

    private void RespondToRotateInput()
    {
        _rb.angularVelocity = Vector3.zero;

        var rotationInThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationInThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationInThisFrame);
        }
    }

}
