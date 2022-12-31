using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] GameObject solider; 
    [SerializeField] int count;
    [SerializeField] float soliderSpacingX, soliderSpacingZ, xGap, zGap;
    [SerializeField] float hSpeed, zSpeed, hBound; 
    [HideInInspector] public List<GameObject> soliders = new List<GameObject>();
    List<Transform> startingSoliders = new List<Transform>();
    List<Transform> temporarySoliders = new List<Transform>();
    float defaultSoliderYValue;
    void Start()
    {
        //Oyunun başındaki oyuncu (asker) miktarını getirmek için.
        startingSoliders = GetComponentsInChildren<Transform>().ToList(); 
        
        //Oyuncularımızın sahnede ayarlanmış y pozisyonunu alır
        defaultSoliderYValue = startingSoliders.ElementAt(1).transform.localPosition.y;

        //Başlangıçtaki oyuncuları genel olarak kullanacağımız listeye atama:
        foreach (Transform t in startingSoliders)
        {
            if (t.gameObject.transform.parent != null) //Parent object'i de listeye eklenmesin diye.
                soliders.Add(t.gameObject);
        }
    }
    void Update()
    {
        //Ekran kaydırma kontrolleri
        SwipeDetection(); 

        //Sürekli olarak ileri gitmek için
        transform.Translate(Vector3.forward * zSpeed * Time.deltaTime);
    }
    private void FixedUpdate()
    {
        //Eğer listemizdeki elemanlar sıfır veya sıfırdan küçükse oyun oyun bitsin.
        if (soliders.Count <= 0) 
        {
            print("GAME OVER!!");   //Oyun bitince olacak olaylar buraya yazılacak.
        }
    }

    void ChangeSoliderNumber()
    {
        //İşlem yapılması kolaylaşsın diye şu anki askerlerimizi geçici bir listede tutuyorum.
        temporarySoliders = GetComponentsInChildren<Transform>().ToList();

        //Yeni asker sayısına eşitlemek için farkı kadar asker ekliyoruz veya çıkarıyoruz.
        for (int i = 0; i < (count - temporarySoliders.Count + 1); i++)
        {
            GameObject solider = Instantiate(this.solider, transform, false);
            soliders.Add(solider);
        }

        //Askerlerin şeklini tekrar ayarlıyoruz.
        ReshapeSoliders();
    }

    public void ReshapeSoliders()
    {
        int n = 0;
        int line = 0;
        int lineSize = Mathf.CeilToInt(soliders.Count / 9);

        //Askerleri ana oyuncu üzerinde ortalamak için.
        xGap = lineSize * 1.15f / 2;

        //Askerlerin bir şekil halinde dizilmesi için döngü kullanıyoruz.
        //Kare şeklinde dizmeyi tercih ettim.
        foreach (GameObject solider in soliders)
        {
            solider.transform.localPosition = new Vector3(line * soliderSpacingX - xGap, defaultSoliderYValue, n * soliderSpacingZ - zGap);
            if (n < lineSize)
            {
                n++;
            }
            else
            {
                n = 0;
                line++;
            }
        }
    }

    //Kontrol kodları.
    Vector2 firstTouchPosition;
    float swipeDir;
    Vector3 moveSpeed;
    void SwipeDetection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            firstTouchPosition.x = Input.mousePosition.x;
        }
        else if (Input.GetMouseButton(0))
        {
            swipeDir = Input.mousePosition.x - firstTouchPosition.x;
            firstTouchPosition.x = Input.mousePosition.x;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            swipeDir = 0f;
        }
        moveSpeed = Vector3.right * swipeDir * hSpeed * Time.deltaTime;
        transform.Translate(moveSpeed);
        transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Clamp(transform.position.z, -hBound, hBound));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Add"))
        {
            CountValue countValue = other.GetComponent<CountValue>();

            //Asker sayısını tutan değişkene; listedeki asker sayısına kazandığımız değeri ekliyoruz.
            count = soliders.Count + countValue.value;

            ChangeSoliderNumber();
            other.transform.parent.gameObject.SetActive(false);
        }
        
        if (other.CompareTag("Cross"))
        {
            CountValue countValue = other.GetComponent<CountValue>();
            count = soliders.Count * countValue.value;
            ChangeSoliderNumber();
            other.transform.parent.gameObject.SetActive(false);
        }
    }
}
