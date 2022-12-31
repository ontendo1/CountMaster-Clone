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
    List<GameObject> soliders = new List<GameObject>();
    List<Transform> startingSoliders = new List<Transform>();
    Rigidbody rb;
    void Start()
    {
        startingSoliders = GetComponentsInChildren<Transform>().ToList();
        foreach (Transform t in startingSoliders)
        {
            if (t.gameObject.transform.parent != null)
                soliders.Add(t.gameObject);
        }
    }
    void Update()
    {
        SwipeDetection();
        transform.Translate(Vector3.forward * zSpeed * Time.deltaTime);
    }
    void ChangeSoliderNumber()
    {
        for (int i = 0; i < count; i++)
        {
            GameObject solider = Instantiate(this.solider, transform, false);
            soliders.Add(solider);
        }
        ReshapeSoliders();
    }
    void ReshapeSoliders()
    {
        int n = 0;
        int line = 0;
        int lineSize = Mathf.CeilToInt(soliders.Count / 10);

        foreach (GameObject solider in soliders)
        {
            solider.transform.localPosition = new Vector3(line * soliderSpacingX - xGap * lineSize, transform.position.y, n * soliderSpacingZ - zGap);
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
            int newCount = soliders.Count + countValue.value;
        }
        if (other.CompareTag("Cross"))
        {
            CountValue countValue = other.GetComponent<CountValue>();
        }
    }
}
