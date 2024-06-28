/*using UnityEngine;
using System.Collections;

public class CubeRotator : MonoBehaviour
{
    public Quaternion[] sideRotations;
    private int currentSideIndex = 0;
    private bool isAnimating = false;

    void Start()
    {
        // Initialize the array of side rotations
        sideRotations = new Quaternion[6];

        // Assign the desired rotations to each index in the array
        sideRotations[0] = Quaternion.Euler(0f, 0f, 0f); // Grey
        sideRotations[1] = Quaternion.Euler(90f, 0f, 0f); // Blue
        sideRotations[2] = Quaternion.Euler(180f, 0f, 0f); // Red
        sideRotations[3] = Quaternion.Euler(0f, 90f, 0f); // Purple
        sideRotations[4] = Quaternion.Euler(0f, -90f, 0f); // Green        
        sideRotations[5] = Quaternion.Euler(-90f, 0f, 0f); // Orange
    }

    void Update()
    {
        if (!isAnimating && Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    StartCoroutine(RotateToNextSide());
                }
            }
        }
    }

    IEnumerator RotateToNextSide()
    {
        isAnimating = true;

        // Determine the target rotation based on the current side index
        Quaternion targetRotation = sideRotations[currentSideIndex];

        // Perform the rotation animation
        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.01f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 220f * Time.deltaTime);
            yield return null;
        }

        // Increment the current side index
        currentSideIndex++;
        if (currentSideIndex >= sideRotations.Length)
        {
            currentSideIndex = 0;
        }

        isAnimating = false;
    }
}*/
using UnityEngine;
using System.Collections;

public class CubeRotator : MonoBehaviour
{
    public Quaternion[] sideRotations;
    private int currentSideIndex = 0;
    private bool isAnimating = false;

    void Start()
    {
        // Initialize the array of side rotations
        sideRotations = new Quaternion[6];

        // Assign the desired rotations to each index in the array
        sideRotations[0] = Quaternion.Euler(0f, 0f, 0f); // Grey
        sideRotations[1] = Quaternion.Euler(90f, 0f, 0f); // Blue
        sideRotations[2] = Quaternion.Euler(180f, 0f, 0f); // Red
        sideRotations[3] = Quaternion.Euler(0f, 90f, 0f); // Purple
        sideRotations[4] = Quaternion.Euler(0f, -90f, 0f); // Green        
        sideRotations[5] = Quaternion.Euler(-90f, 0f, 0f); // Orange
    }

    void Update()
    {
        // Code for other functionality, if any
    }

    public void StartRotation()
    {
        if (!isAnimating)
        {
            StartCoroutine(RotateToNextSide());
        }
    }

    public void RotateToGray()
    {
        if (!isAnimating)
        {
            StartCoroutine(RotateToSide(sideRotations[0]));
        }
    }

    public void RotateToBlue()
    {
        if (!isAnimating)
        {
            StartCoroutine(RotateToSide(sideRotations[1]));
        }
    }

    public void RotateToRed()
    {
        if (!isAnimating)
        {
            StartCoroutine(RotateToSide(sideRotations[2]));
        }
    }

    public bool IsRotated()
    {
        return currentSideIndex != 0;
    }

    public bool IsRotatedToBlue()
    {
        return currentSideIndex == 1;
    }

    IEnumerator RotateToNextSide()
    {
        isAnimating = true;

        // Determine the target rotation based on the current side index
        Quaternion targetRotation = sideRotations[currentSideIndex];

        // Perform the rotation animation
        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.01f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 220f * Time.deltaTime);
            yield return null;
        }

        // Increment the current side index
        currentSideIndex++;
        if (currentSideIndex >= sideRotations.Length)
        {
            currentSideIndex = 0;
        }

        isAnimating = false;
    }

    IEnumerator RotateToSide(Quaternion targetRotation)
    {
        isAnimating = true;

        // Perform the rotation animation
        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.01f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 220f * Time.deltaTime);
            yield return null;
        }

        isAnimating = false;
    }
}

