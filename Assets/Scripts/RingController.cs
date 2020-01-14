using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingController : MonoBehaviour
{
    private float ringRotationVelocity = 2;
    private float randomVelocityTime = 3;
    private float animTime = 0;

    private bool initialize = true;

    //Ringin kendi etrafında dönme hareketini kontrol eder. Random yönlere ve hızlarda hareket sağlanır.
    private void FixedUpdate()
    {
        if (initialize)
        {
            animTime += Time.fixedDeltaTime;
            if (animTime > 0.2f)
            {
                initialize = false;
                transform.GetComponent<Animator>().enabled = false;
            }
        }

        randomVelocityTime += Time.deltaTime;

        if (randomVelocityTime > 3)
        {
            ringRotationVelocity = Random.Range(-3f, 3f);
            randomVelocityTime = 0;
        }
        transform.Rotate(0, ringRotationVelocity, 0);
    }
}
