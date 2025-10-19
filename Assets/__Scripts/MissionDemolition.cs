using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public enum GameMode
{
    idle,
    playing,
    levelEnd
}

public class MissionDemolition : MonoBehaviour
{
    static private MissionDemolition S; // a private Singleton

    [Header("Inscribed")]
    public Text uitLevel; // The UIText_Level Text
    public Text uitShots; // The UIText_Shots Text
    public Vector3 castlePos; // The place to put castles
    public GameObject[] castles; // An array of the castles

    [Header("Dynamic")]
    public int level; // The current level
    public int levelMax; // The number of levels
    public int shotsTaken;
    static public int TOTAL_LIVES = 6; // The max amount of shots the player can shoot
    public int lives;    // The Total amount of shots the player can shoot
    public GameObject castle; // The current castle
    public GameMode mode = GameMode.idle;
    public string showing = "Show Slingshot"; //FollowCam mode

    // Start is called before the first frame update
    void Start()
    {
        S = this; // Define the Singleton
        lives = TOTAL_LIVES;
        level = 0;
        //shotsTaken = 0;
        levelMax = castles.Length;
        StartLevel();
    }

    public void StartLevel()
    {
        // Get rid of the old castle if one exists
        if (castle != null)
        {
            Destroy(castle);
        }

        //  Destroy old projectiles if the exist
        Projectile.DESTROY_PROJECTILES();

        //Instantiate the new castle
        castle = Instantiate<GameObject>(castles[level]);
        castle.transform.position = castlePos;

        //Reset the goal
        Goal.goalMet = false;

        //Reset lives
        lives = TOTAL_LIVES;

        UpdateGUI();

        mode = GameMode.playing;

        // Zoom out to show both
        FollowCam.SWITCH_VIEW(FollowCam.eView.both);
    }

    void UpdateGUI()
    {
        //  Show the data in the GUITexts
        uitLevel.text = "Level: " + (level + 1) + " of " + levelMax;
        uitShots.text = lives + " out of " + TOTAL_LIVES + " shots left.";
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGUI();

        //Check for level end
        if ((mode == GameMode.playing) && Goal.goalMet)
        {
            // Change mode to stop checkin for level end
            mode = GameMode.levelEnd;

            // Zoom out to show both
            FollowCam.SWITCH_VIEW(FollowCam.eView.both);

            // Start the next level in 2 seconds
            Invoke("NextLevel", 2f);
        }
        //Check if lives is zero.
        if(lives == 0)
        {
            // Send to GameoverScene
            SceneManager.LoadScene("GameoverScene");
        }
    }

    void NextLevel()
    {
        level++;
        if (level == levelMax)
        {
            level = 0;
            shotsTaken = 0;
            // Send to GameoverScene
            SceneManager.LoadScene("GameoverScene");
        }
        StartLevel();
    }

    //  Static method that allows code anywhere to reduce life
    static public void SHOT_FIRED()
    {
        S.lives--;
        //S.shotsTaken++;
    }

    //  Static method that allows code anywhere to get a reference to S.castle
    static public GameObject GET_CASTLE()
    {
        return S.castle;
    }
    
    static public void RESET_LEVEL()
    {
        // Get rid of the old castle if one exists
        if (S.castle != null)
        {
            Destroy(S.castle);
        }

        //  Destroy old projectiles if the exist
        Projectile.DESTROY_PROJECTILES();

        //Instantiate the new castle
        S.castle = Instantiate<GameObject>(S.castles[S.level]);
        S.castle.transform.position = S.castlePos;

        //Reset the goal
        Goal.goalMet = false;

        //Reset lives
        S.lives = TOTAL_LIVES;
    }
}
