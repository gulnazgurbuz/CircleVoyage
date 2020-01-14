using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;


public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject[] rings;
    [SerializeField] private GameObject firstRing;
    [SerializeField] private Material[] materials;

    [HideInInspector] public GameStatus GameStatusEnum;

    [HideInInspector] public Transform ParentRing;

    [HideInInspector] private int ringCounter;

    private Text textForUI;

    private CubeController cube;

    private void Awake()
    {
        DOTween.KillAll();
    }

    private void Start()
    {
        GameStatusEnum = GameStatus.START;

        ParentRing = GameObject.Find("ParentRing").transform;
        cube = GameObject.Find("Cube").GetComponent<CubeController>();
        textForUI = GameObject.Find("Canvas").transform.GetChild(0).GetComponent<Text>();
    }

    private void Update()
    {
#if UNITY_EDITOR//Test için.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene
                (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
#endif
        if (Input.GetMouseButtonDown(0) && GameStatusEnum == GameStatus.EMPTY)
            GameStatusEnum = GameStatus.JUMP;

    }

    private void FixedUpdate()
    {
        DisplayRingCount();

        ringCounter = cube.ringCounter;
        switch (GameStatusEnum)
        {
            case GameStatus.START:
                CreateRing(true);
                CreateRing();
                CreateRing();
                GameStatusEnum = GameStatus.EMPTY;
                break;
            case GameStatus.JUMP:
                CreateRing();
                GameStatusEnum = GameStatus.JUMPSTAY;
                break;
            case GameStatus.JUMPSTAY:
                JumpStay();
                break;
            case GameStatus.EMPTY:
                break;
        }
    }

    private void CreateRing(bool firstRingControl = false)
    {
        ReScale();
        Material newRingMat = materials[Random.Range(0, materials.Length)];
        GameObject newRing = null;

        if (firstRingControl)
        {
            newRing = firstRing;
            Transform obj = Instantiate(newRing, Vector3.zero, Quaternion.identity).transform;
            obj.GetComponent<MeshRenderer>().material = newRingMat;
            obj.SetParent(ParentRing);

        }
        else
        {
            newRing = rings[Random.Range(0, rings.Length)];
            Transform obj = Instantiate(newRing, Vector3.zero, Quaternion.identity).transform;
            for (int i = 0; i < obj.childCount; i++)
            {
                obj.GetChild(i).GetComponent<MeshRenderer>().material = newRingMat;
            }
            obj.SetParent(ParentRing);
            if (ParentRing.childCount > 3)
            {
                obj.GetComponent<Animator>().enabled = true;
            }

        }

    }
    private void ReScale()
    {
        int childCount = ParentRing.childCount;
        if (childCount > 2)
        {//DoScale ile scale işlemini zamana bağladım. 
            for (int i = 0; i < childCount; i++)
            {
                Transform trf = ParentRing.GetChild(i);
                float scaleSize = 1.22f;
                trf.DOScale(new Vector3(trf.localScale.x * scaleSize, trf.localScale.y, trf.localScale.z * scaleSize), 0.1f);
            }
        }
        else if ((childCount > 0))
        {//Initialize aşamasında ilk oluşan 3 ring için bu scale kısmını kullanır.
            for (int i = 0; i < childCount; i++)
            {
                Transform trf = ParentRing.GetChild(i);
                float scaleSize = 1.22f;
                trf.localScale = new Vector3(trf.localScale.x * scaleSize, trf.localScale.y, trf.localScale.z * scaleSize);
            }
        }

        if (childCount == 10)
        {//10 "ring" olunca en dıştaki "ring" silinir.
            ParentRing.GetChild(0).DOKill();
            Destroy(ParentRing.GetChild(0).gameObject);
        }
    }

    private void JumpStay() 
    {//Üzerine atladığımız "ring"in(sondan 3. create edilen) hareketi kapatılır ve tamamlayıcı obje "voyager" harekete geçer.
        int childCount = ParentRing.childCount;
        if (childCount > 3)
        {
            Transform obj = ParentRing.GetChild(childCount - 3);
            obj.GetChild(0).GetComponent<VoyagerController>().enabled = true;
            obj.GetComponent<RingController>().enabled = false;
        }
    }

    private void DisplayRingCount()
    {
        textForUI.text = ringCounter.ToString();
    }

}
