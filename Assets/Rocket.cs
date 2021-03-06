﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

    [SerializeField]float rcsThrust = 200f;
    [SerializeField] float MainThrust = 50f;
    Rigidbody rigidBody;
    AudioSource audioSource;
    enum State { Alive, Dying, Transcending }
    State state = State.Alive;

    // Use this for initialization
    void Start ()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (state == State.Alive) { Thrust(); Rotate(); } else { audioSource.Stop(); }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive) { return; } // ignore collisions

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                // do nothing
                break;
            case "Finish":
                state = State.Transcending;
                Invoke("LoadNextLevel", 1f); // parameterise time
                break;
            default:
                print("Hit something deadly");
                state = State.Dying;
                Invoke("LoadFirstLevel", 1f); // parameterise time
                break;
        }
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(1); // todo allow for more than 2 levels
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
