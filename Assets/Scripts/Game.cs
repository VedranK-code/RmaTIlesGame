using System.IO;
using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public float rotationSpeed = 220f;
    public float rotationAngle = 90f;
    public float resetDelay=4f;
    public float restartDelay = 3f;
//-------------------------------------------
    public float timeLim = 15f; 
    private float currentTime; 
    public TMP_Text timerText;
    public static int highScore;

    //-------------------------------------------

    public Transform[] cubesToRotate;
    //-------------------------------------------
    public Transform RulePlane;
        public Material OnlyBlueM;
        public Material OnlyGreenM;
        public Material OnlyRedM;
        public Material OnlyOrangeM;
        //-------------------------------------------
        public Material AvoidOrangeM;
        //-------------------------------------------
        public Material AvoidRedAndBlueM;
        public Material AvoidOrangeAndGreenM;
        public Material AvoidOrangeAndPurpleM;
        public Material AvoidGreenAndBlueM;

        //-------------------------------------------
        public Material OnlyGreenAndPurpleM;
        public Material OnlyBlueAndRedM;
        public Material OnlyOrangeAndRedM;
        public Material OnlyBlueAndOrangeM;
        //-------------------------------------------
        public Material OnlyBGPM;
        public Material OnlyBROM;
        public Material OnlyGORM;
        public Material OnlyROPM;
        //-------------------------------------------
        public Material AvoidGOPM;
        public Material AvoidOPBM;
        public Material AvoidRGBM;
        public Material AvoidRGOM;
        //-------------------------------------------
        private Renderer objectRenderer;
    // -------------------------------------------
    public Material flaggedCubeMaterial;
    public Material defaultCubeMaterial;

    public TextMeshProUGUI highScoreText; 

    private int rotatedCubesCount = 0;
    private int flaggedCubesCount = 0;
    private bool hasWon = false;
    private bool canInteract = false;

    private Quaternion[] originalRotations;

    private Quaternion[] sideRotations;

    private int score = 870;
    int scorePerRound= 10;
    private int Streak = 0;
    private int cubeCount = 4;

    public TextMeshProUGUI textToHide;
    private string highScoreKey = "HighScore";
    private bool timerPaused = false;

    private void Start()
    {
        highScore = PlayerPrefs.GetInt(highScoreKey);
       
        objectRenderer = RulePlane.GetComponent<Renderer>();
        timerPaused=true;
        sideRotations = new Quaternion[6];
        sideRotations[0] = Quaternion.Euler(0f, 0f, 0f); // Grey
        sideRotations[1] = Quaternion.Euler(90f, 0f, 0f); // Blue
        sideRotations[2] = Quaternion.Euler(180f, 0f, 0f); // Red
        sideRotations[3] = Quaternion.Euler(0f, 90f, 0f); // Purple
        sideRotations[4] = Quaternion.Euler(0f, -90f, 0f); // Green        
        sideRotations[5] = Quaternion.Euler(-90f, 0f, 0f); // Orange

        highScoreText.text = "Score: " + score.ToString();
        foreach (Transform cube in cubesToRotate)
        {
            cube.gameObject.tag = "Cube";
            cube.GetComponent<Renderer>().material = defaultCubeMaterial; 
        }

        StoreOriginalRotations();
        
        RandomOnlyBlue(cubeCount);
       
            currentTime = timeLim;
        
        
        
    }

    public void UpdateHighScore(int newScore)
{
    if (newScore > highScore)
    {
        highScore = newScore;
        PlayerPrefs.SetInt("HighScore", highScore);
    }
}

private System.Collections.IEnumerator HideTextForDuration(float duration)
    {
        textToHide.enabled = false;
        yield return new WaitForSeconds(duration);
        textToHide.enabled = true;
    }

    private void StoreOriginalRotations()
    {
        originalRotations = new Quaternion[cubesToRotate.Length];
        for (int i = 0; i < cubesToRotate.Length; i++)
        {
            originalRotations[i] = cubesToRotate[i].rotation;
        }
    }

//----------------------------------------------------------------------------------------------
    private void RandomOnlyBlue(int count)
    {
        objectRenderer.material = OnlyBlueM;
        ShuffleCubesArray();

        for (int i = 0; i < count; i++)
        {
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[1].eulerAngles, rotationSpeed); // Rotate to Blue
            cube.gameObject.tag = "FlaggedBlue";
            rotatedCubesCount++;
        }
        Invoke("RotateCubesToPlay", resetDelay);
        StartCoroutine(WaitForCubesToRotateBack());
    }
    private void RandomOnlyGreen(int count)
    {
        objectRenderer.material = OnlyGreenM;
        ShuffleCubesArray();

        for (int i = 0; i < count; i++)
        {
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[4].eulerAngles, rotationSpeed); // Rotate to Green
            cube.gameObject.tag = "FlaggedGreen";
            rotatedCubesCount++;
        }
        Invoke("RotateCubesToPlay", resetDelay);
        StartCoroutine(WaitForCubesToRotateBack());
    }
    private void RandomOnlyRed(int count)
    {
        objectRenderer.material = OnlyRedM;
        ShuffleCubesArray();

        for (int i = 0; i < count; i++)
        {
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[2].eulerAngles, rotationSpeed); // Rotate to Red
            cube.gameObject.tag = "FlaggedRed";
            rotatedCubesCount++;
        }
        Invoke("RotateCubesToPlay", resetDelay);
        StartCoroutine(WaitForCubesToRotateBack());
    }
    private void RandomOnlyOrange(int count)
    {
        objectRenderer.material = OnlyOrangeM;
        ShuffleCubesArray();

        for (int i = 0; i < count; i++)
        {
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[5].eulerAngles, rotationSpeed); // Rotate to Blue
            cube.gameObject.tag = "FlaggedOrange";
            rotatedCubesCount++;
        }
        Invoke("RotateCubesToPlay", resetDelay);
        StartCoroutine(WaitForCubesToRotateBack());
    }

//-------------------------------------------------------------------------------------------------
    private void RandomAvoidOrange(int cubeNumber)
    {
        objectRenderer.material = AvoidOrangeM;
        ShuffleCubesArray();
        int fix=0;
         System.Random random = new System.Random();
        for (int i = 0; i < cubeNumber; i++)
        {
            int randomNumber;
            
            if(fix<2){randomNumber = random.Next(1, 50); fix++;}else{randomNumber = random.Next(1, 101);}
            if(randomNumber<50){
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[1].eulerAngles, rotationSpeed); // Rotate to Blue
            cube.gameObject.tag = "FlaggedBlue";
            rotatedCubesCount++;
            }
            if(randomNumber>50){
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[5].eulerAngles, rotationSpeed); // Rotate to Orange
            cube.gameObject.tag = "Cube";
            }
        }
        Invoke("RotateCubesToPlay", resetDelay);
        StartCoroutine(WaitForCubesToRotateBack());
        
    }
//-------------------------------------------------------------------------------------------------
    private void RandomAvoidRedAndBlue(int cubeNumber)
    {
        objectRenderer.material = AvoidRedAndBlueM;
        ShuffleCubesArray();
         System.Random random = new System.Random();
         int fix=0;
        for (int i = 0; i < cubeNumber; i++)
        {
           
            int randomNumber;
            
            if(fix<2){randomNumber = random.Next(50, 101); fix++;}else{randomNumber = random.Next(1, 101);}
            if(randomNumber>=1 && randomNumber<25){
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[1].eulerAngles, rotationSpeed); // Rotate to Blue
            cube.gameObject.tag = "Cube";
            }

            if(randomNumber>=25 && randomNumber<50){
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[2].eulerAngles, rotationSpeed); // Rotate to Red
            cube.gameObject.tag = "Cube";
            }

             if(randomNumber>=50 && randomNumber<75){
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[4].eulerAngles, rotationSpeed); // Rotate to Green
            cube.gameObject.tag = "FlaggedGreen";
            rotatedCubesCount++;
            }

            if(randomNumber>=75 && randomNumber<100){
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[5].eulerAngles, rotationSpeed); // Rotate to Orange
            cube.gameObject.tag = "FlaggedOrange";
            rotatedCubesCount++;
            }
        }
        Invoke("RotateCubesToPlay", resetDelay);
        StartCoroutine(WaitForCubesToRotateBack());
        
    }
    private void RandomAvoidOrangeAndGreen(int cubeNumber)
    {
        objectRenderer.material = AvoidOrangeAndGreenM;
        ShuffleCubesArray();
         System.Random random = new System.Random();
         int fix=0;
        for (int i = 0; i < cubeNumber; i++)
        {
           
            int randomNumber;
            
            if(fix<2){randomNumber = random.Next(50, 101); fix++;}else{randomNumber = random.Next(1, 101);}
            if(randomNumber>=1 && randomNumber<25){
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[4].eulerAngles, rotationSpeed); // Rotate to Green
            cube.gameObject.tag = "Cube";
            }

            if(randomNumber>=25 && randomNumber<50){
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[5].eulerAngles, rotationSpeed); // Rotate to Orange
            cube.gameObject.tag = "Cube";
            }

             if(randomNumber>=50 && randomNumber<75){
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[1].eulerAngles, rotationSpeed); // Rotate to Blue
            cube.gameObject.tag = "FlaggedBlue";
            rotatedCubesCount++;
            }

            if(randomNumber>=75 && randomNumber<100){
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[2].eulerAngles, rotationSpeed); // Rotate to Red
            cube.gameObject.tag = "FlaggedRed";
            rotatedCubesCount++;
            }
        }
        Invoke("RotateCubesToPlay", resetDelay);
        StartCoroutine(WaitForCubesToRotateBack());
        
    }
    private void RandomAvoidOrangeAndPurple(int cubeNumber)
    {
        objectRenderer.material = AvoidOrangeAndPurpleM;
        ShuffleCubesArray();
         System.Random random = new System.Random();
         int fix=0;
        for (int i = 0; i < cubeNumber; i++)
        {
           
            int randomNumber;
            
            if(fix<2){randomNumber = random.Next(50, 101); fix++;}else{randomNumber = random.Next(1, 101);}
            if(randomNumber>=1 && randomNumber<25){
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[5].eulerAngles, rotationSpeed); // Rotate to Orange
            cube.gameObject.tag = "Cube";
            }

            if(randomNumber>=25 && randomNumber<50){
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[3].eulerAngles, rotationSpeed); // Rotate to Purple
            cube.gameObject.tag = "Cube";
            }

             if(randomNumber>=50 && randomNumber<75){
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[4].eulerAngles, rotationSpeed); // Rotate to Green
            cube.gameObject.tag = "FlaggedGreen";
            rotatedCubesCount++;
            }

            if(randomNumber>=75 && randomNumber<100){
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[1].eulerAngles, rotationSpeed); // Rotate to Blue
            cube.gameObject.tag = "FlaggedBlue";
            rotatedCubesCount++;
            }
        }
        Invoke("RotateCubesToPlay", resetDelay);
        StartCoroutine(WaitForCubesToRotateBack());
        
    }
    private void RandomAvoidGreenAndBlue(int cubeNumber)
    {
        objectRenderer.material = AvoidGreenAndBlueM;
        ShuffleCubesArray();
         System.Random random = new System.Random();
         int fix=0;
        for (int i = 0; i < cubeNumber; i++)
        {
           
            int randomNumber;
            
            if(fix<2){randomNumber = random.Next(50, 101); fix++;}else{randomNumber = random.Next(1, 101);}
            if(randomNumber>=1 && randomNumber<25){
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[1].eulerAngles, rotationSpeed); // Rotate to Blue
            cube.gameObject.tag = "Cube";
            }

            if(randomNumber>=25 && randomNumber<50){
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[4].eulerAngles, rotationSpeed); // Rotate to Green
            cube.gameObject.tag = "Cube";
            }

             if(randomNumber>=50 && randomNumber<75){
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[2].eulerAngles, rotationSpeed); // Rotate to Red
            cube.gameObject.tag = "FlaggedRed";
            rotatedCubesCount++;
            }

            if(randomNumber>=75 && randomNumber<100){
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[5].eulerAngles, rotationSpeed); // Rotate to Orange
            cube.gameObject.tag = "FlaggedOrange";
            rotatedCubesCount++;
            }
        }
        Invoke("RotateCubesToPlay", resetDelay);
        StartCoroutine(WaitForCubesToRotateBack());
        
    }

//-------------------------------------------------------------------------------------------------

    private void RandomOnlyGreenAndPurple(int cubeNumber)
    {
        objectRenderer.material = OnlyGreenAndPurpleM;
        ShuffleCubesArray();
         System.Random random = new System.Random();
         int fix=0;
        for (int i = 0; i < cubeNumber; i++)
        {
           
            int randomNumber;
            
            if(fix<2){randomNumber = random.Next(40, 80); fix++;}else{randomNumber = random.Next(1, 101);}
            if(randomNumber>=1 && randomNumber<20){
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[1].eulerAngles, rotationSpeed); // Rotate to Blue
            cube.gameObject.tag = "Cube";
            }

            if(randomNumber>=20 && randomNumber<40){
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[2].eulerAngles, rotationSpeed); // Rotate to Red
            cube.gameObject.tag = "Cube";
            }

             if(randomNumber>=40 && randomNumber<60){
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[4].eulerAngles, rotationSpeed); // Rotate to Green
            cube.gameObject.tag = "FlaggedGreen";
            rotatedCubesCount++;
            }

            if(randomNumber>=60 && randomNumber<80){
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[3].eulerAngles, rotationSpeed); // Rotate to Purple
            cube.gameObject.tag = "FlaggedPurple";
            rotatedCubesCount++;
            }

            if(randomNumber>=80 && randomNumber<100){
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[5].eulerAngles, rotationSpeed); // Rotate to Orange
            cube.gameObject.tag = "Cube";
            }
        }
        Invoke("RotateCubesToPlay", resetDelay);
        StartCoroutine(WaitForCubesToRotateBack());
        
    }
    private void RandomOnlyBlueAndOrange(int cubeNumber)
    {
        objectRenderer.material = OnlyBlueAndOrangeM;
        ShuffleCubesArray();
         System.Random random = new System.Random();
         int fix=0;
        for (int i = 0; i < cubeNumber; i++)
        {
           
            int randomNumber;
            
            if(fix<2){randomNumber = random.Next(40, 80); fix++;}else{randomNumber = random.Next(1, 101);}
            if(randomNumber>=1 && randomNumber<20){
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[4].eulerAngles, rotationSpeed); // Rotate to Green
            cube.gameObject.tag = "Cube";
            }

            if(randomNumber>=20 && randomNumber<40){
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[2].eulerAngles, rotationSpeed); // Rotate to Red
            cube.gameObject.tag = "Cube";
            }

             if(randomNumber>=40 && randomNumber<60){
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[1].eulerAngles, rotationSpeed); // Rotate to Blue
            cube.gameObject.tag = "FlaggedBlue";
            rotatedCubesCount++;
            }

            if(randomNumber>=60 && randomNumber<80){
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[5].eulerAngles, rotationSpeed); // Rotate to Orange
            cube.gameObject.tag = "FlaggedOrange";
            rotatedCubesCount++;
            }

            if(randomNumber>=80 && randomNumber<100){
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[3].eulerAngles, rotationSpeed); // Rotate to Purple
            cube.gameObject.tag = "Cube";
            }
        }
        Invoke("RotateCubesToPlay", resetDelay);
        StartCoroutine(WaitForCubesToRotateBack());
        
    }
    private void RandomOnlyBlueAndRed(int cubeNumber)
    {
        
        objectRenderer.material = OnlyBlueAndRedM;
        ShuffleCubesArray();
         System.Random random = new System.Random();
         int fix=0;
        for (int i = 0; i < cubeNumber; i++)
        {
           
            int randomNumber;
            
            if(fix<2){randomNumber = random.Next(40, 80); fix++;}else{randomNumber = random.Next(1, 101);}
            if(randomNumber>=1 && randomNumber<20){
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[4].eulerAngles, rotationSpeed); // Rotate to green
            cube.gameObject.tag = "Cube";
            }

            if(randomNumber>=20 && randomNumber<40){
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[3].eulerAngles, rotationSpeed); // Rotate to purple
            cube.gameObject.tag = "Cube";
            }

             if(randomNumber>=40 && randomNumber<60){
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[1].eulerAngles, rotationSpeed); // Rotate to blue
            cube.gameObject.tag = "FlaggedBlue";
            rotatedCubesCount++;
            }

            if(randomNumber>=60 && randomNumber<80){
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[2].eulerAngles, rotationSpeed); // Rotate to red
            cube.gameObject.tag = "FlaggedRed";
            rotatedCubesCount++;
            }

            if(randomNumber>=80 && randomNumber<100){
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[5].eulerAngles, rotationSpeed); // Rotate to Orange
            cube.gameObject.tag = "Cube";
            }
        }
        Invoke("RotateCubesToPlay", resetDelay);
        StartCoroutine(WaitForCubesToRotateBack());
        
    }
    private void RandomOnlyOrangeAndRed(int cubeNumber)
    {
        objectRenderer.material = OnlyOrangeAndRedM;
        ShuffleCubesArray();
         System.Random random = new System.Random();
         int fix=0;
        for (int i = 0; i < cubeNumber; i++)
        {
           
            int randomNumber;
            
            if(fix<2){randomNumber = random.Next(40, 80); fix++;}else{randomNumber = random.Next(1, 101);}
            if(randomNumber>=1 && randomNumber<20){
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[1].eulerAngles, rotationSpeed); // Rotate to Blue
            cube.gameObject.tag = "Cube";
            }

            if(randomNumber>=20 && randomNumber<40){
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[3].eulerAngles, rotationSpeed); // Rotate to Purple
            cube.gameObject.tag = "Cube";
            }

             if(randomNumber>=40 && randomNumber<60){
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[5].eulerAngles, rotationSpeed); // Rotate to Orange
            cube.gameObject.tag = "FlaggedOrange";
            rotatedCubesCount++;
            }

            if(randomNumber>=60 && randomNumber<80){
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[2].eulerAngles, rotationSpeed); // Rotate to Red
            cube.gameObject.tag = "FlaggedRed";
            rotatedCubesCount++;
            }

            if(randomNumber>=80 && randomNumber<100){
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[4].eulerAngles, rotationSpeed); // Rotate to Green
            cube.gameObject.tag = "Cube";
            }
        }
        Invoke("RotateCubesToPlay", resetDelay);
        StartCoroutine(WaitForCubesToRotateBack());
        
    }
    //-------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------
    private void RandomOnlyBGP(int cubeNumber)
    {
        objectRenderer.material = OnlyBGPM;
        ShuffleCubesArray();
         System.Random random = new System.Random();
         int fix=0;
        for (int i = 0; i < cubeNumber; i++)
        {
           
            int randomNumber;
            
            if(fix<2){randomNumber = random.Next(40, 100); fix++;}else{randomNumber = random.Next(1, 101);}
            if(randomNumber>=1 && randomNumber<20){
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[5].eulerAngles, rotationSpeed); // Rotate to Orange
            cube.gameObject.tag = "Cube";
            }

            if(randomNumber>=20 && randomNumber<40){
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[2].eulerAngles, rotationSpeed); // Rotate to Red
            cube.gameObject.tag = "Cube";
            }

             if(randomNumber>=40 && randomNumber<60){
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[4].eulerAngles, rotationSpeed); // Rotate to Green
            cube.gameObject.tag = "FlaggedGreen";
            rotatedCubesCount++;
            }

            if(randomNumber>=60 && randomNumber<80){
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[3].eulerAngles, rotationSpeed); // Rotate to Purple
            cube.gameObject.tag = "FlaggedPurple";
            rotatedCubesCount++;
            }

            if(randomNumber>=80 && randomNumber<100){
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[1].eulerAngles, rotationSpeed); // Rotate to Blue
            cube.gameObject.tag = "FlaggedBlue";
            rotatedCubesCount++;
            }
        }
        Invoke("RotateCubesToPlay", resetDelay);
        StartCoroutine(WaitForCubesToRotateBack());
        
    }
    private void RandomOnlyBRO(int cubeNumber)
    {
        objectRenderer.material = OnlyBROM;
        ShuffleCubesArray();
         System.Random random = new System.Random();
         int fix=0;
        for (int i = 0; i < cubeNumber; i++)
        {
           
            int randomNumber;
            
            if(fix<2){randomNumber = random.Next(40, 100); fix++;}else{randomNumber = random.Next(1, 101);}
            if(randomNumber>=1 && randomNumber<20){
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[4].eulerAngles, rotationSpeed); // Rotate to gre
            cube.gameObject.tag = "Cube";
            }

            if(randomNumber>=20 && randomNumber<40){
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[3].eulerAngles, rotationSpeed); // Rotate to purp
            cube.gameObject.tag = "Cube";
            }

             if(randomNumber>=40 && randomNumber<60){
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[5].eulerAngles, rotationSpeed); // Rotate to org
            cube.gameObject.tag = "FlaggedOrange";
            rotatedCubesCount++;
            }

            if(randomNumber>=60 && randomNumber<80){
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[2].eulerAngles, rotationSpeed); // Rotate to red
            cube.gameObject.tag = "FlaggedRed";
            rotatedCubesCount++;
            }

            if(randomNumber>=80 && randomNumber<100){
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[1].eulerAngles, rotationSpeed); // Rotate to Blue
            cube.gameObject.tag = "FlaggedBlue";
            rotatedCubesCount++;
            }
        }
        Invoke("RotateCubesToPlay", resetDelay);
        StartCoroutine(WaitForCubesToRotateBack());
        
    }
    private void RandomOnlyROP(int cubeNumber)
    {
        objectRenderer.material = OnlyROPM;
        ShuffleCubesArray();
         System.Random random = new System.Random();
         int fix=0;
        for (int i = 0; i < cubeNumber; i++)
        {
           
            int randomNumber;
            
            if(fix<2){randomNumber = random.Next(40, 100); fix++;}else{randomNumber = random.Next(1, 101);}
            if(randomNumber>=1 && randomNumber<20){
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[1].eulerAngles, rotationSpeed); // Rotate to blu
            cube.gameObject.tag = "Cube";
            }

            if(randomNumber>=20 && randomNumber<40){
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[4].eulerAngles, rotationSpeed); // Rotate to gree
            cube.gameObject.tag = "Cube";
            }

             if(randomNumber>=40 && randomNumber<60){
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[2].eulerAngles, rotationSpeed); // Rotate to red
            cube.gameObject.tag = "FlaggedRed";
            rotatedCubesCount++;
            }

            if(randomNumber>=60 && randomNumber<80){
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[3].eulerAngles, rotationSpeed); // Rotate to Purple
            cube.gameObject.tag = "FlaggedPurple";
            rotatedCubesCount++;
            }

            if(randomNumber>=80 && randomNumber<100){
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[5].eulerAngles, rotationSpeed); // Rotate to org
            cube.gameObject.tag = "FlaggedOrange";
            rotatedCubesCount++;
            }
        }
        Invoke("RotateCubesToPlay", resetDelay);
        StartCoroutine(WaitForCubesToRotateBack());
        
    }
    private void RandomOnlyGOR(int cubeNumber)
    {
        objectRenderer.material = OnlyGORM;
        ShuffleCubesArray();
         System.Random random = new System.Random();
         int fix=0;
        for (int i = 0; i < cubeNumber; i++)
        {
           
            int randomNumber;
            
            if(fix<2){randomNumber = random.Next(40, 100); fix++;}else{randomNumber = random.Next(1, 101);}
            if(randomNumber>=1 && randomNumber<20){
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[1].eulerAngles, rotationSpeed); // Rotate to blu
            cube.gameObject.tag = "Cube";
            }

            if(randomNumber>=20 && randomNumber<40){
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[3].eulerAngles, rotationSpeed); // Rotate to purp
            cube.gameObject.tag = "Cube";
            }

             if(randomNumber>=40 && randomNumber<60){
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[4].eulerAngles, rotationSpeed); // Rotate to Green
            cube.gameObject.tag = "FlaggedGreen";
            rotatedCubesCount++;
            }

            if(randomNumber>=60 && randomNumber<80){
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[2].eulerAngles, rotationSpeed); // Rotate to red
            cube.gameObject.tag = "FlaggedRed";
            rotatedCubesCount++;
            }

            if(randomNumber>=80 && randomNumber<100){
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[5].eulerAngles, rotationSpeed); // Rotate to org
            cube.gameObject.tag = "FlaggedOrange";
            rotatedCubesCount++;
            }
        }
        Invoke("RotateCubesToPlay", resetDelay);
        StartCoroutine(WaitForCubesToRotateBack());
        
    }                                               

    //-------------------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------------------
    private void RandomAvoidGOP(int cubeNumber)
    {
        objectRenderer.material = AvoidGOPM;
        ShuffleCubesArray();
         System.Random random = new System.Random();
         int fix=0;
        for (int i = 0; i < cubeNumber; i++)
        {
           
            int randomNumber;
            
            if(fix<2){randomNumber = random.Next(60, 100); fix++;}else{randomNumber = random.Next(1, 101);}
            if(randomNumber>=1 && randomNumber<20){
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[5].eulerAngles, rotationSpeed); // Rotate to Orange
            cube.gameObject.tag = "Cube";
            }

            if(randomNumber>=20 && randomNumber<40){
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[3].eulerAngles, rotationSpeed); // Rotate to Purple
            cube.gameObject.tag = "Cube";
            }

             if(randomNumber>=40 && randomNumber<60){
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[4].eulerAngles, rotationSpeed); // Rotate to Green
            cube.gameObject.tag = "Cube";
            }

            if(randomNumber>=60 && randomNumber<80){
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[2].eulerAngles, rotationSpeed); // Rotate to Red
            cube.gameObject.tag = "FlaggedRed";
            rotatedCubesCount++;
            }

            if(randomNumber>=80 && randomNumber<100){
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[1].eulerAngles, rotationSpeed); // Rotate to Blue
            cube.gameObject.tag = "FlaggedBlue";
            rotatedCubesCount++;
            }
        }
        Invoke("RotateCubesToPlay", resetDelay);
        StartCoroutine(WaitForCubesToRotateBack());
        
    }
    private void RandomAvoidOPB(int cubeNumber)
    {
        objectRenderer.material = AvoidOPBM;
        ShuffleCubesArray();
         System.Random random = new System.Random();
         int fix=0;
        for (int i = 0; i < cubeNumber; i++)
        {
           
            int randomNumber;
            
            if(fix<2){randomNumber = random.Next(60, 100); fix++;}else{randomNumber = random.Next(1, 101);}
            if(randomNumber>=1 && randomNumber<20){
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[5].eulerAngles, rotationSpeed); // Rotate to Orange
            cube.gameObject.tag = "Cube";
            }

            if(randomNumber>=20 && randomNumber<40){
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[3].eulerAngles, rotationSpeed); // Rotate to Purple
            cube.gameObject.tag = "Cube";
            }

             if(randomNumber>=40 && randomNumber<60){
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[1].eulerAngles, rotationSpeed); // Rotate to blu
            cube.gameObject.tag = "Cube";
            }

            if(randomNumber>=60 && randomNumber<80){
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[2].eulerAngles, rotationSpeed); // Rotate to Red
            cube.gameObject.tag = "FlaggedRed";
            rotatedCubesCount++;
            }

            if(randomNumber>=80 && randomNumber<100){
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[4].eulerAngles, rotationSpeed); // Rotate to green
            cube.gameObject.tag = "FlaggedGreen";
            rotatedCubesCount++;
            }
        }
        Invoke("RotateCubesToPlay", resetDelay);
        StartCoroutine(WaitForCubesToRotateBack());
        
    }
    private void RandomAvoidRGB(int cubeNumber)
    {
        objectRenderer.material = AvoidRGBM;
        ShuffleCubesArray();
         System.Random random = new System.Random();
         int fix=0;
        for (int i = 0; i < cubeNumber; i++)
        {
           
            int randomNumber;
            
            if(fix<2){randomNumber = random.Next(60, 100); fix++;}else{randomNumber = random.Next(1, 101);}
            if(randomNumber>=1 && randomNumber<20){
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[1].eulerAngles, rotationSpeed); // Rotate to Blue
            cube.gameObject.tag = "Cube";
            }

            if(randomNumber>=20 && randomNumber<40){
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[2].eulerAngles, rotationSpeed); // Rotate to red
            cube.gameObject.tag = "Cube";
            }

             if(randomNumber>=40 && randomNumber<60){
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[4].eulerAngles, rotationSpeed); // Rotate to Green
            cube.gameObject.tag = "Cube";
            }

            if(randomNumber>=60 && randomNumber<80){
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[3].eulerAngles, rotationSpeed); // Rotate to purple
            cube.gameObject.tag = "FlaggedPurple";
            rotatedCubesCount++;
            }

            if(randomNumber>=80 && randomNumber<100){
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[5].eulerAngles, rotationSpeed); // Rotate to Orange
            cube.gameObject.tag = "FlaggedOrange";
            rotatedCubesCount++;
            }
        }
        Invoke("RotateCubesToPlay", resetDelay);
        StartCoroutine(WaitForCubesToRotateBack());
        
    }                                           
    private void RandomAvoidRGO(int cubeNumber)
    {
        objectRenderer.material = AvoidRGOM;
        ShuffleCubesArray();
         System.Random random = new System.Random();
         int fix=0;
        for (int i = 0; i < cubeNumber; i++)
        {
           
            int randomNumber;
            
            if(fix<2){randomNumber = random.Next(60, 100); fix++;}else{randomNumber = random.Next(1, 101);}
            if(randomNumber>=1 && randomNumber<20){
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[5].eulerAngles, rotationSpeed); // Rotate to Orange
            cube.gameObject.tag = "Cube";
            }

            if(randomNumber>=20 && randomNumber<40){
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[2].eulerAngles, rotationSpeed); // Rotate to Red
            cube.gameObject.tag = "Cube";
            }

             if(randomNumber>=40 && randomNumber<60){
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[4].eulerAngles, rotationSpeed); // Rotate to Green
            cube.gameObject.tag = "Cube";
            }

            if(randomNumber>=60 && randomNumber<80){
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[3].eulerAngles, rotationSpeed); // Rotate to Purple
            cube.gameObject.tag = "FlaggedPurple";
            rotatedCubesCount++;
            }

            if(randomNumber>=80 && randomNumber<100){
            Transform cube = cubesToRotate[i];
            RotateCube(cube, sideRotations[1].eulerAngles, rotationSpeed); // Rotate to Blue
            cube.gameObject.tag = "FlaggedBlue";
            rotatedCubesCount++;
            }
        }
        Invoke("RotateCubesToPlay", resetDelay);
        StartCoroutine(WaitForCubesToRotateBack());
        
    }                                    
    //-------------------------------------------------------------------------------------------------

    private IEnumerator WaitForCubesToRotateBack()
    {
        yield return new WaitForSeconds(resetDelay);
        canInteract = true;
        timerPaused=false;
    }

    private void ShuffleCubesArray()
    {
        for (int i = cubesToRotate.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            Transform temp = cubesToRotate[i];
            cubesToRotate[i] = cubesToRotate[randomIndex];
            cubesToRotate[randomIndex] = temp;
        }
    }

    private void RotateCube(Transform cube, Vector3 targetEulerAngles, float speed)
    {
        Quaternion startRotation = cube.rotation;
        Quaternion targetRotation = Quaternion.Euler(targetEulerAngles);

        StartCoroutine(RotateCubeCoroutine(cube, startRotation, targetRotation, speed));
    }

    private IEnumerator RotateCubeCoroutine(Transform cube, Quaternion startRotation, Quaternion targetRotation, float speed)
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * (speed / 90f);

            cube.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
            yield return null;
        }

    }

    private void RotateCubeBack(Transform cube)
    {
        int cubeIndex = System.Array.IndexOf(cubesToRotate, cube);
        RotateCube(cube, sideRotations[0].eulerAngles, rotationSpeed); // Rotate back to Grey
    }

    private void RotateCubesToPlay()
    {
        foreach (Transform cube in cubesToRotate)
        {
            RotateCubeBack(cube);
        }

        if (!hasWon)
        {
            canInteract = true;
        }
    }


private void Update()
    {
        if(!timerPaused){
          if (currentTime > 0f)
        {
            currentTime -= Time.deltaTime;
            UpdateTimerText();
        }else
        {
            TimerEnd();
            RotateAllCubes(sideRotations[2].eulerAngles, rotationSpeed); // Rotate all cubes to red

                    PlayerPrefs.SetInt("Score", score);
                    SceneManager.LoadScene("HighScore");
                    UpdateHighScore(score);
        }
     }
        
    
        if (canInteract && Input.GetMouseButtonDown(0))
        {
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            

            if (Physics.Raycast(ray, out hit))
            {
                GameObject clickedCube = hit.collider.gameObject;

                if (clickedCube.CompareTag("FlaggedBlue")||clickedCube.CompareTag("FlaggedRed")||clickedCube.CompareTag("FlaggedOrange")||clickedCube.CompareTag("FlaggedGreen")||clickedCube.CompareTag("FlaggedPurple"))
                {
                    if (!clickedCube.GetComponent<Renderer>().material.Equals(flaggedCubeMaterial))
                    {
                        if (clickedCube.CompareTag("FlaggedBlue")){RotateCube(clickedCube.transform, sideRotations[1].eulerAngles, rotationSpeed);} // Rotate to Blue}
                        if (clickedCube.CompareTag("FlaggedRed")){RotateCube(clickedCube.transform, sideRotations[2].eulerAngles, rotationSpeed);}// Rotate to red}
                        if (clickedCube.CompareTag("FlaggedPurple")){RotateCube(clickedCube.transform, sideRotations[3].eulerAngles, rotationSpeed);} // Rotate to purple}
                        if (clickedCube.CompareTag("FlaggedGreen")){RotateCube(clickedCube.transform, sideRotations[4].eulerAngles, rotationSpeed);} // Rotate to green}
                        if (clickedCube.CompareTag("FlaggedOrange")){RotateCube(clickedCube.transform, sideRotations[5].eulerAngles, rotationSpeed);} // Rotate to orange}
                        
                        clickedCube.GetComponent<Renderer>().material = flaggedCubeMaterial;
                        flaggedCubesCount++;
                        clickedCube.gameObject.tag = "Untagged";


                        
                        if (flaggedCubesCount == rotatedCubesCount)
                        {
                            hasWon = true;
                            timerPaused=true;
                            WinScreen(sideRotations[1].eulerAngles, rotationSpeed); // Rotate all cubes to blue                           
                            if (Streak<5)
                            {
                            score += scorePerRound;
                            Streak++;
                            } else if (Streak>=5)
                            {
                                scorePerRound+=(int)(scorePerRound * 0.25);
                                score =score + scorePerRound;
                                Streak++;
                            }
                            

                            Invoke("RestartGame", restartDelay); // Restart the game after a delay
                        }
                    }
                }
                else if(clickedCube.CompareTag("Cube"))
                {
                    RotateAllCubes(sideRotations[2].eulerAngles, rotationSpeed); // Rotate all cubes to red

                    PlayerPrefs.SetInt("Score", score);
                    SceneManager.LoadScene("HighScore");
                    UpdateHighScore(score);
                    
                }
                else if(clickedCube.CompareTag("Untagged"))
                {
                
                }
                else{return;}
            }
        }
    }
   



private void WinScreen(Vector3 targetEulerAngles, float speed)
    {
        
        foreach (Transform cube in cubesToRotate)
        {
            
            RotateCube(cube, targetEulerAngles, speed);
            cube.gameObject.tag = "Untagged";
        }
    }

 
 private void UpdateTimerText()
    {
        int timeInSeconds = Mathf.RoundToInt(currentTime);
        timerText.text = timeInSeconds.ToString()+"s";
    }

    private void TimerEnd()
    {
        Debug.Log("Timer has ended!");
    }
 

 
    private void RotateAllCubes(Vector3 targetEulerAngles, float speed)
    {
        
        foreach (Transform cube in cubesToRotate)
        {
            
            RotateCube(cube, targetEulerAngles, speed);
        }
    }

   
    private void RestartGame()
    {
        
        hasWon = false;
        canInteract = false;
        timerPaused= true;
        flaggedCubesCount = 0;
        rotatedCubesCount = 0;
        currentTime = timeLim;

        foreach (Transform cube in cubesToRotate)
        {
            cube.gameObject.tag = "Cube";
            cube.GetComponent<Renderer>().material = defaultCubeMaterial; // Reset cube materials to default
        }

        if (score == 0) // Check if the game was lost
    {
        cubeCount = 4; // Set the cubeCount to 4 after losing
        
    }
    

        RotateAllCubes(sideRotations[0].eulerAngles, rotationSpeed); // Rotate all cubes back to Grey

                if(Streak<5){
                resetDelay=3f;
                timeLim=15f;
                
                System.Random level = new System.Random();
                int randomLevel = level.Next(1, 6);
                if(randomLevel==1){
                cubeCount=level.Next(4,9);
                RandomOnlyBlue(cubeCount);
                }
                if(randomLevel==2){
                cubeCount=level.Next(4,9);
                RandomOnlyGreen(cubeCount);
                }
                if(randomLevel==3){
                cubeCount=level.Next(4,9);
                RandomOnlyRed(cubeCount);
                }       
                if(randomLevel==4){
                cubeCount=level.Next(4,9);
                RandomOnlyOrange(cubeCount);
                }
                if(randomLevel==5){
                cubeCount=level.Next(4,9);
                RandomAvoidOrange(cubeCount);
                }
        }
                if(Streak>=5 && Streak<10){
                resetDelay=3f;
                timeLim=12f;
       
                System.Random level = new System.Random();
                int randomLevel = level.Next(1, 14);
                //----------------------------------------------------
                if(randomLevel==1){
                cubeCount=level.Next(5,10);
                RandomOnlyBlue(cubeCount);
                }
                if(randomLevel==2){
                cubeCount=level.Next(5,10);
                RandomOnlyGreen(cubeCount);
                }
                if(randomLevel==3){
                cubeCount=level.Next(5,10);
                RandomOnlyRed(cubeCount);
                }       
                if(randomLevel==4){
                cubeCount=level.Next(5,10);
                RandomOnlyOrange(cubeCount);
                }
                if(randomLevel==5){
                cubeCount=level.Next(5,10);
                RandomAvoidOrange(cubeCount);
                }
                //----------------------------------------------------
                if(randomLevel==6){
                cubeCount=level.Next(8,11);
                RandomAvoidRedAndBlue(cubeCount);
                }
                if(randomLevel==7){
                cubeCount=level.Next(8,11);
                RandomAvoidOrangeAndGreen(cubeCount);
                }
                if(randomLevel==8){
                cubeCount=level.Next(8,11);
                RandomAvoidOrangeAndPurple(cubeCount);
                }       
                if(randomLevel==9){
                cubeCount=level.Next(8,11);
                RandomAvoidGreenAndBlue(cubeCount);
                }
                //---------------------------------------------------
                if(randomLevel==10){
                cubeCount=level.Next(8,11);
                RandomOnlyBlueAndOrange(cubeCount);
                }
                if(randomLevel==11){
                cubeCount=level.Next(8,11);
                RandomOnlyBlueAndRed(cubeCount);
                }
                if(randomLevel==12){
                cubeCount=level.Next(8,11);
                RandomOnlyOrangeAndRed(cubeCount);
                }       
                if(randomLevel==13){
                cubeCount=level.Next(8,11);
                RandomOnlyGreenAndPurple(cubeCount);
                }
                //---------------------------------------------------
        }
                if(Streak>=10 && Streak<15){
                resetDelay=2f;
                timeLim=10f;
              
                System.Random level = new System.Random();
                int randomLevel = level.Next(1, 14);
                //----------------------------------------------------
                if(randomLevel==1){
                cubeCount=level.Next(8,11);
                RandomOnlyBlue(cubeCount);
                }
                if(randomLevel==2){
                cubeCount=level.Next(8,11);
                RandomOnlyGreen(cubeCount);
                }
                if(randomLevel==3){
                cubeCount=level.Next(8,11);
                RandomOnlyRed(cubeCount);
                }       
                if(randomLevel==4){
                cubeCount=level.Next(8,11);
                RandomOnlyOrange(cubeCount);
                }
                if(randomLevel==5){
                cubeCount=level.Next(8,15);
                RandomAvoidOrange(cubeCount);
                }
                //----------------------------------------------------
                if(randomLevel==6){
                cubeCount=level.Next(10,15);
                RandomAvoidRedAndBlue(cubeCount);
                }
                if(randomLevel==7){
                cubeCount=level.Next(10,15);
                RandomAvoidOrangeAndGreen(cubeCount);
                }
                if(randomLevel==8){
                cubeCount=level.Next(10,15);
                RandomAvoidOrangeAndPurple(cubeCount);
                }       
                if(randomLevel==9){
                cubeCount=level.Next(10,15);
                RandomAvoidGreenAndBlue(cubeCount);
                }
                //---------------------------------------------------
                if(randomLevel==10){
                cubeCount=level.Next(10,15);
                RandomOnlyBlueAndOrange(cubeCount);
                }
                if(randomLevel==11){
                cubeCount=level.Next(10,15);
                RandomOnlyBlueAndRed(cubeCount);
                }
                if(randomLevel==12){
                cubeCount=level.Next(10,15);
                RandomOnlyOrangeAndRed(cubeCount);
                }       
                if(randomLevel==13){
                cubeCount=level.Next(10,15);
                RandomOnlyGreenAndPurple(cubeCount);
                }
                //---------------------------------------------------
        }
                if(Streak>=15 && Streak<20){
                resetDelay=3f;
                timeLim=8f;
                
                 System.Random level = new System.Random();
                int randomLevel = level.Next(1, 9);
                //----------------------------------------------------
                if(randomLevel==1){
                    cubeCount=level.Next(10,16);
                RandomAvoidGOP(cubeCount);
                }
                if(randomLevel==2){
                    cubeCount=level.Next(10,16);
                RandomAvoidOPB(cubeCount);
                }
                if(randomLevel==3){
                    cubeCount=level.Next(10,16);
                RandomAvoidRGB(cubeCount);
                }       
                if(randomLevel==4){
                    cubeCount=level.Next(10,16);
                RandomAvoidRGO(cubeCount);
                }
                if(randomLevel==5){
                    cubeCount=level.Next(10,16);
                RandomOnlyBGP(cubeCount);
                }
                if(randomLevel==6){
                    cubeCount=level.Next(10,16);
                RandomOnlyROP(cubeCount);
                }
                if(randomLevel==7){
                    cubeCount=level.Next(10,16);
                RandomOnlyBRO(cubeCount);
                }       
                if(randomLevel==8){
                    cubeCount=level.Next(10,16);
                RandomOnlyGOR(cubeCount);
                }
                
        }
                if(Streak>=20 && Streak<25){
                resetDelay=2f;
                timeLim=5f;
        
                System.Random level = new System.Random();
                int randomLevel = level.Next(1, 22);
                //----------------------------------------------------
                if(randomLevel==1){
                cubeCount=level.Next(8,11);
                RandomOnlyBlue(cubeCount);
                }
                if(randomLevel==2){
                cubeCount=level.Next(8,11);
                RandomOnlyGreen(cubeCount);
                }
                if(randomLevel==3){
                cubeCount=level.Next(8,11);
                RandomOnlyRed(cubeCount);
                }       
                if(randomLevel==4){
                cubeCount=level.Next(8,11);
                RandomOnlyOrange(cubeCount);
                }
                if(randomLevel==5){
                cubeCount=level.Next(8,15);
                RandomAvoidOrange(cubeCount);
                }
                //----------------------------------------------------
                if(randomLevel==6){
                cubeCount=level.Next(10,15);
                RandomAvoidRedAndBlue(cubeCount);
                }
                if(randomLevel==7){
                cubeCount=level.Next(10,15);
                RandomAvoidOrangeAndGreen(cubeCount);
                }
                if(randomLevel==8){
                cubeCount=level.Next(10,15);
                RandomAvoidOrangeAndPurple(cubeCount);
                }       
                if(randomLevel==9){
                cubeCount=level.Next(10,15);
                RandomAvoidGreenAndBlue(cubeCount);
                }
                //---------------------------------------------------
                if(randomLevel==10){
                cubeCount=level.Next(10,15);
                RandomOnlyBlueAndOrange(cubeCount);
                }
                if(randomLevel==11){
                cubeCount=level.Next(10,15);
                RandomOnlyBlueAndRed(cubeCount);
                }
                if(randomLevel==12){
                cubeCount=level.Next(10,15);
                RandomOnlyOrangeAndRed(cubeCount);
                }       
                if(randomLevel==13){
                cubeCount=level.Next(10,15);
                RandomOnlyGreenAndPurple(cubeCount);
                }
                //---------------------------------------------------
                if(randomLevel==14){
                    cubeCount=level.Next(10,16);
                RandomAvoidGOP(cubeCount);
                }
                if(randomLevel==15){
                    cubeCount=level.Next(10,16);
                RandomAvoidOPB(cubeCount);
                }
                if(randomLevel==16){
                    cubeCount=level.Next(10,16);
                RandomAvoidRGB(cubeCount);
                }       
                if(randomLevel==17){
                    cubeCount=level.Next(10,16);
                RandomAvoidRGO(cubeCount);
                }
                if(randomLevel==18){
                    cubeCount=level.Next(10,16);
                RandomOnlyBGP(cubeCount);
                }
                if(randomLevel==19){
                    cubeCount=level.Next(10,16);
                RandomOnlyROP(cubeCount);
                }
                if(randomLevel==20){
                    cubeCount=level.Next(10,16);
                RandomOnlyBRO(cubeCount);
                }       
                if(randomLevel==21){
                    cubeCount=level.Next(10,16);
                RandomOnlyGOR(cubeCount);
                }
        }
                if(Streak>=25){
                resetDelay=1.5f;
                timeLim=5f;
               
                System.Random level = new System.Random();
                int randomLevel = level.Next(1, 22);
                //----------------------------------------------------
                if(randomLevel==1){
                cubeCount=level.Next(8,11);
                RandomOnlyBlue(cubeCount);
                }
                if(randomLevel==2){
                cubeCount=level.Next(8,11);
                RandomOnlyGreen(cubeCount);
                }
                if(randomLevel==3){
                cubeCount=level.Next(8,11);
                RandomOnlyRed(cubeCount);
                }       
                if(randomLevel==4){
                cubeCount=level.Next(8,11);
                RandomOnlyOrange(cubeCount);
                }
                if(randomLevel==5){
                cubeCount=level.Next(8,15);
                RandomAvoidOrange(cubeCount);
                }
                //----------------------------------------------------
                if(randomLevel==6){
                cubeCount=level.Next(10,15);
                RandomAvoidRedAndBlue(cubeCount);
                }
                if(randomLevel==7){
                cubeCount=level.Next(10,15);
                RandomAvoidOrangeAndGreen(cubeCount);
                }
                if(randomLevel==8){
                cubeCount=level.Next(10,15);
                RandomAvoidOrangeAndPurple(cubeCount);
                }       
                if(randomLevel==9){
                cubeCount=level.Next(10,15);
                RandomAvoidGreenAndBlue(cubeCount);
                }
                //---------------------------------------------------
                if(randomLevel==10){
                cubeCount=level.Next(10,15);
                RandomOnlyBlueAndOrange(cubeCount);
                }
                if(randomLevel==11){
                cubeCount=level.Next(10,15);
                RandomOnlyBlueAndRed(cubeCount);
                }
                if(randomLevel==12){
                cubeCount=level.Next(10,15);
                RandomOnlyOrangeAndRed(cubeCount);
                }       
                if(randomLevel==13){
                cubeCount=level.Next(10,15);
                RandomOnlyGreenAndPurple(cubeCount);
                }
                //---------------------------------------------------
                if(randomLevel==14){
                    cubeCount=level.Next(10,16);
                RandomAvoidGOP(cubeCount);
                }
                if(randomLevel==15){
                    cubeCount=level.Next(10,16);
                RandomAvoidOPB(cubeCount);
                }
                if(randomLevel==16){
                    cubeCount=level.Next(10,16);
                RandomAvoidRGB(cubeCount);
                }       
                if(randomLevel==17){
                    cubeCount=level.Next(10,16);
                RandomAvoidRGO(cubeCount);
                }
                if(randomLevel==18){
                    cubeCount=level.Next(10,16);
                RandomOnlyBGP(cubeCount);
                }
                if(randomLevel==19){
                    cubeCount=level.Next(10,16);
                RandomOnlyROP(cubeCount);
                }
                if(randomLevel==20){
                    cubeCount=level.Next(10,16);
                RandomOnlyBRO(cubeCount);
                }       
                if(randomLevel==21){
                    cubeCount=level.Next(10,16);
                RandomOnlyGOR(cubeCount);
                }
                
        }

        
        highScoreText.text = "Score: " + score.ToString();
    }
}