using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] float _rcsThrust = 100f;
    [SerializeField] float _mainThrust = 100f;

    Rigidbody _rb;
    AudioSource _as;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _as = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Thrust();
        Rotate();
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                print("All good!");
                break;
            default:
                print("DIEEE");
                break;
        }
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            _rb.AddRelativeForce(Vector3.up * _mainThrust);
            if (!_as.isPlaying)
            {
                _as.Play();
            }
        }
        else
        {
            _as.Stop();
        }
    }

    private void Rotate()
    {
        _rb.freezeRotation = true; // manually control rotation

        var rotationInThisFrame = _rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationInThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationInThisFrame);
        }

        _rb.freezeRotation = false; // resume physics control of rotation
    }

}
