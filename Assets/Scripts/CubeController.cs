using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class CubeController : MonoBehaviour
{
    [HideInInspector] public bool jumped = false;
    [HideInInspector] public int ringCounter = 0;

    private bool animTimeControl = false;
    private float animTime = 0;

    private Animator anim;
    private GameController gameController;


    private void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        anim = GetComponent<Animator>();
    }
    private void FixedUpdate()
    {
        if (gameController.GameStatusEnum == GameStatus.JUMPSTAY)
        {
            anim.SetBool("Jump", true);
            animTimeControl = true;
        }

        if (animTimeControl)
        {
            animTime += Time.fixedDeltaTime;
            if (animTime > 0.25f)
            {
                anim.SetBool("Jump", false);

                if (Physics.Raycast(transform.GetChild(0).position, Vector3.down, out RaycastHit hit))
                {
                    ringCounter++;
                    gameController.GameStatusEnum = GameStatus.EMPTY;
                }
                else
                {
                    if (gameController.GameStatusEnum != GameStatus.FINISH)
                    {
                        gameController.GameStatusEnum = GameStatus.FINISH;
                        DOTween.KillAll();
                        StartCoroutine(Finish());
                    }
                }

                animTime = 0;
                animTimeControl = false;
            }
        }
    }
    private IEnumerator Finish()
    {
        transform.GetComponent<Rigidbody>().isKinematic = false;
        transform.GetComponent<Animator>().enabled = false;
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}