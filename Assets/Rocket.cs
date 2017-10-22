using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

    // TODO Fix Lighting Bug
    [SerializeField]float rcsThrust = 200f;
    [SerializeField] float MainThrust = 50f;

    Rigidbody rigidBody;
    AudioSource audioSource;

	// Use this for initialization
	void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        Thrust();
        Rotate();
    }

    void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                print("Hit finish"); //todo remove
                SceneManager.LoadScene(1);
                break;
            default:
                print("Dead"); // Kill player | Reload level
                SceneManager.LoadScene(0);
                break;
        }
    }

    private void Thrust()
    {

        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * MainThrust);
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Stop();
        }
    }

    private void Rotate()
    {
        rigidBody.freezeRotation = true; // take manual control of rotation

        float rotationThisFrame = rcsThrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }
        rigidBody.freezeRotation = false; // resume physics rotation
    }
}
