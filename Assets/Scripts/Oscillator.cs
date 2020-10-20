using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);
    [SerializeField] float period = 5f;

    //[SerializeField] [Range(0, 1)] float movementFactor;
    float movementFactor;
    Vector3 _startingPos;

    void Start()
    {
        _startingPos = transform.position;
    }

    void Update()
    {
        if (Mathf.Epsilon < period)
        {
            float cycles = Time.time / period;

            const float tau = Mathf.PI * 2;
            float rawSineWave = Mathf.Sin(cycles * tau);

            movementFactor = rawSineWave / 2f + 0.5f;
            Vector3 offset = movementVector * movementFactor;
            transform.position = _startingPos + offset;
        }
    }
}
