using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoyagerController : MonoBehaviour
{
    //Gezgin "voyager" objesi "ring"in boşluklarını kapatıp bütün bir halka elde etmeyi hedefler.

    [SerializeField] private GameObject littlePart=null;
    [SerializeField] private GameObject completedPart=null;

    private float startAngle;
    private float currentAngle;
    private const float rotationVelocity = 2.5f;

    private GameController gameController;

    private void Start()
    {
        startAngle = transform.localRotation.eulerAngles.y;
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    private void FixedUpdate()
    {
        transform.Rotate(0, rotationVelocity, 0);
        VoyagerTravel();
    }

  
    private void VoyagerTravel()
    {
        Transform createObj = null;
        currentAngle = transform.localRotation.eulerAngles.y;

        if (currentAngle - startAngle > 10 / rotationVelocity)
        {  //Hızına göre değişen sıklıkta kendi boyutunda littlePart(prefab) create eder. 
            if (gameController.ParentRing.childCount > 2)
            {
                startAngle = transform.localRotation.eulerAngles.y;
                createObj = Instantiate(littlePart, transform.position, Quaternion.Euler(transform.eulerAngles)).transform;
            }
        }
        else if (currentAngle > 355)
        { //Halka tamamlanınca tüm little partlar silinir.Yerine tek bir bütün ring konur.
            createObj = Instantiate(completedPart, transform.position, Quaternion.identity).transform;
            VoyagerDisable();
        }

        if (createObj != null)
        {
            createObj.localScale = transform.parent.localScale;
            createObj.GetComponent<MeshRenderer>().material = transform.GetComponent<MeshRenderer>().material;
            createObj.SetParent(transform.parent.transform);
        }
    }

   
    private void VoyagerDisable()
    {
        transform.GetComponent<VoyagerController>().enabled = false;

        for (int i = 0; i < transform.parent.transform.childCount; i++)
        {
            if (transform.parent.transform.GetChild(i).tag == "ToBeDeleted")
            {
                Destroy(transform.parent.transform.GetChild(i).gameObject);
            }
        }
    }
}
