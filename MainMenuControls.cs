using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.Audio;
using Opsive.UltimateCharacterController.Character.Effects;

public class MainMenuControls : ViewBehaviour
{
    [Header("Screens")]
    [SerializeField] GameObject mainMenuScreen = null;
    [SerializeField] GameObject settingsScreen = null;
    [SerializeField] GameObject resolutionsOptionsScreen = null;
    [SerializeField] GameObject audioSettingsScreen = null;
    [SerializeField] GameObject instructionsAndControlsScreen = null;
    [SerializeField] GameObject creditsScreen = null;

    GameObject[] menuScreensList;

    [Header("Main Menu Screen Buttons")]
    [SerializeField] Button newGameButton = null;
    [SerializeField] Button resetGameButton = null;
    [SerializeField] Button settingsButton = null;
    [SerializeField] Button exitGameButton = null;
    [SerializeField] Button creditsScreenButton = null; 

    [Header("Settings Screen Buttons")]
    [SerializeField] Button resolutionsSettingsButton = null;
    [SerializeField] Button audioSettingsButton = null;
    [SerializeField] Button instructionsAndControlsButton = null;
    [SerializeField] Button exitToMainMenuFromSettingsButton = null;

    [Header("Fullscreen Resolutions Buttons")]
    [SerializeField] Button highest_full_resolution_button = null;
    [SerializeField] Button higher_full_resolution_button = null;
    [SerializeField] Button medium_full_resolution_button = null;
    [SerializeField] Button lower_full_resolution_button = null;
    [SerializeField] Button lowest_full_resolution_button = null;

    [Header("Windowed Screens Resolutions Buttons")]
    [SerializeField] Button highest_windowed_resolution_button = null;
    [SerializeField] Button higher_windowed_resolution_button = null;
    [SerializeField] Button medium_windowed_resolution_button = null;
    [SerializeField] Button lower_windowed_resolution_button = null;
    [SerializeField] Button lowest_windowed_resolution_button = null;

    [SerializeField] Button exitOneScreenFromResolutionsScreenButton = null;
    [SerializeField] Button exitToMainMenuFromResolutionsButton = null;

    [SerializeField] Button[] fullResolutionButtons;
    [SerializeField] Button[] windowedResolutionButtons;

    [Header("Resolutions Settings Texts")]
    [SerializeField] Text highest_text = null;
    [SerializeField] Text higher_text = null;
    [SerializeField] Text medium_text = null;
    [SerializeField] Text lower_text = null;
    [SerializeField] Text lowest_text = null;

    Text[] information_texts;
    int green_text_pos = 0;
    Color green_text_color = new Color32(00, 150, 00, 255);

    Vector2Int[] supportedResolutions;

    [Header("Audio Settings Buttons")]
    [SerializeField] Button backOneScreenFromAudioSetScreenButton = null;
    [SerializeField] Button exitToMainFromAudioSetScreenButton = null;

    [Header("Instructions and Controls Screen Elements")]
    [SerializeField] Button backOneScreenFromInstructAndContScreenButton = null;
    [SerializeField] Button exitToMainFromInstructAndContScreenButton = null;
    [SerializeField] Text instructionsAndControlsBodyText = null;

    string instructionsAndControlsTextDocument;

    [Header("Credits Screen Elements")]
    [SerializeField] Button exitToMainFromCreditsScreenButton = null;
    [SerializeField] Text creditsBodyText = null;

    string creditsTextDocument;

    void Start()
    {
        #region Resolutions Preperations
        //set up for resolutions settings
        information_texts = new Text[5] { highest_text, higher_text, medium_text, lower_text, lowest_text };

        fullResolutionButtons = new Button[5] { highest_full_resolution_button, higher_full_resolution_button,
            medium_full_resolution_button, lower_full_resolution_button, lowest_full_resolution_button };

        windowedResolutionButtons = new Button[5] { highest_windowed_resolution_button, higher_windowed_resolution_button,
            medium_windowed_resolution_button, lower_windowed_resolution_button, lowest_windowed_resolution_button };

        AddSupportedReolutionsToArray();

        //sets game to start in highest available resolution
        var highestRes = supportedResolutions.First();
        Screen.SetResolution(highestRes.x, highestRes.y, true);
        SetGreenText();

        SetResolutionsAndGreenTexts();

        #endregion

        //make sure that all additional menu screens are added to this array
        #region Screens and Menu Text Information Preperations 
        //screens are set so that the main menu screen always starts up first

        //any screens that are in the main menu scene MUST be added to this array
        menuScreensList = new GameObject[] { mainMenuScreen, settingsScreen, resolutionsOptionsScreen, audioSettingsScreen, 
            instructionsAndControlsScreen, creditsScreen };

        foreach (GameObject screen in menuScreensList)
        {
            if(screen.name == "Main Menu Screen")
            {
                screen.SetActive(true);
            }
            else
            {
                screen.SetActive(false);
            }
        }

        //set up for the text reader for the credits screen
        //creditsTextDocument = "Assets/TextFiles/inQuinnsitiveCreditsText.txt";

        TextAsset txtAsset = (TextAsset)Resources.Load("inQuinnsitiveCreditsText", typeof(TextAsset));
        creditsBodyText.GetComponent<Text>().text = txtAsset.text;

        //Read the text from directly from the .txt file
        /*StreamReader reader1 = new StreamReader(creditsTextDocument);
        creditsBodyText.GetComponent<Text>().text = reader1.ReadToEnd().ToString();
        reader1.Close();*/


        //set up for the text reader for the insctructions and controls screen
        //instructionsAndControlsTextDocument = "Assets/TextFiles/InstructionsAndControlsText.txt";

        //Read the text from directly from the .txt file
        /*StreamReader reader2 = new StreamReader(instructionsAndControlsTextDocument);
        instructionsAndControlsBodyText.GetComponent<Text>().text = reader2.ReadToEnd().ToString();
        reader2.Close();*/

        TextAsset txtAsset2 = (TextAsset)Resources.Load("InstructionsAndControlsText", typeof(TextAsset));
        instructionsAndControlsBodyText.GetComponent<Text>().text = txtAsset2.text;

        #endregion

        #region Button Listeners
        //main menu screen button listeners
        newGameButton.onClick.AddListener(StartNewGame);
        resetGameButton.onClick.AddListener(ResetGame);
        settingsButton.onClick.AddListener(OpenSettingsScreen);
        exitGameButton.onClick.AddListener(ExitGameFromMenu);
        creditsScreenButton.onClick.AddListener(OpenCreditsAndSpecialThanks);

        //settings screen buttons listeners
        resolutionsSettingsButton.onClick.AddListener(OpenResolutionsSettingsScreen);
        audioSettingsButton.onClick.AddListener(OpenAudioSettingsScreen);
        instructionsAndControlsButton.onClick.AddListener(OpenInstructionsAndControlsScreen);
        exitToMainMenuFromSettingsButton.onClick.AddListener(ExitToMainMenuFromSettingsMethod);

        //resolutions screen button listeners
        exitOneScreenFromResolutionsScreenButton.onClick.AddListener(BackOneScreenFromResolutionsSettings);
        exitToMainMenuFromResolutionsButton.onClick.AddListener(ExitToMainMenuFromResolutionsOptionsScreen);

        //audio settings screen button listeners
        backOneScreenFromAudioSetScreenButton.onClick.AddListener(BackOneScreenFromAudioSettingsScreen);
        exitToMainFromAudioSetScreenButton.onClick.AddListener(ExitToMainMenuFromAudioSettingsScreen);

        //instructions and controls screen buttons 
        backOneScreenFromInstructAndContScreenButton.onClick.AddListener(BackOneScreenFromInstructionsAndControlsScreen);
        exitToMainFromInstructAndContScreenButton.onClick.AddListener(ExitToMainMenuFromInstructionsAndControlsScreen);

        //credits screen button listeners 
        exitToMainFromCreditsScreenButton.onClick.AddListener(ExitToMainMenuFromCreditsScreen);

        #endregion

    }

    #region Main Menu Screen -- Button Controls 
    public void StartNewGame()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void ResetGame()
    {
        PlayerPrefs.DeleteAll();
    }

    public void OpenSettingsScreen()
    {
        mainMenuScreen.SetActive(false);
        settingsScreen.SetActive(true);
    }

    public void ExitGameFromMenu()
    {
        Application.Quit();
    }

    public void OpenCreditsAndSpecialThanks()
    {
        mainMenuScreen.SetActive(false);
        creditsScreen.SetActive(true);
    }
    #endregion

    #region Settings Screen -- Button Controls
    public void OpenResolutionsSettingsScreen()
    {
        settingsScreen.SetActive(false);
        resolutionsOptionsScreen.SetActive(true);
    }

    public void OpenAudioSettingsScreen()
    {
        settingsScreen.SetActive(false);
        audioSettingsScreen.SetActive(true);
    }

    public void OpenInstructionsAndControlsScreen()
    {
        settingsScreen.SetActive(false);
        instructionsAndControlsScreen.SetActive(true);
    }

    public void ExitToMainMenuFromSettingsMethod()
    {
        settingsScreen.SetActive(false);
        mainMenuScreen.SetActive(true);
    }
    #endregion

    #region Resolutions Options 
    public void BackOneScreenFromResolutionsSettings()
    {
        resolutionsOptionsScreen.SetActive(false);
        settingsScreen.SetActive(true);
    }

    public void ExitToMainMenuFromResolutionsOptionsScreen()
    {
        resolutionsOptionsScreen.SetActive(false);
        mainMenuScreen.SetActive(true);
    }

    //make sure only one copy of resolution in list
    public void AddSupportedReolutionsToArray()
    {
        var distinctResoultionsList = Screen.resolutions
            .Select(res => new Vector2Int(res.width, res.height))
            .Distinct();

        supportedResolutions = distinctResoultionsList.OrderByDescending(size => size.magnitude)
            .Take(5)
            .ToArray();
    }

    public void SetResolutionsAndGreenTexts()
    {
        for (int x = 0; x < 5; x++)
        {
            if (x < supportedResolutions.Length)
            {
                fullResolutionButtons[x].GetComponentInChildren<Text>().text = supportedResolutions[x].x + " x " + supportedResolutions[x].y;
                windowedResolutionButtons[x].GetComponentInChildren<Text>().text = supportedResolutions[x].x + " x " + supportedResolutions[x].y;
                //Debug.Log("TEXT IN BUTTONS SHOULD HAVE CHANGED");
    
                var index = x;

                 Subscribe(fullResolutionButtons[x]
                    .onClick, () =>
                    {
                        Screen.SetResolution(supportedResolutions[index].x, supportedResolutions[index].y, true);
                        green_text_pos = index;

                        SetGreenText();
                        //Debug.Log("FULL RES BUTTON : " + fullResolutionButtons[x] + " CLICKED.");
                    });

                Subscribe(windowedResolutionButtons[x]
                    .onClick, () =>
                     {
                         Screen.SetResolution(supportedResolutions[index].x, supportedResolutions[index].y, false);
                         green_text_pos = index;

                         SetGreenText();

                         //Debug.Log("WINDOWED RES BUTTON : " + windowedResolutionButtons[x] + " CLICKED.");
                     });

            }
            else
            {
                fullResolutionButtons[x].gameObject.SetActive(false);
                windowedResolutionButtons[x].gameObject.SetActive(false);
                information_texts[x].gameObject.SetActive(false);
            }
        }
    }

    //might not work
//    protected void Subscribe(UnityEvent e, UnityAction action)
//{
//    UnityEventOwner.Subscribe(e, action);
//}

public void SetGreenText()
    {
        for (int i = 0; 0 < information_texts.Length; i++)
        {
            if (i != green_text_pos && i < 5)
            {
                information_texts[i].color = Color.white;
            }
            else if (i == green_text_pos && i < 5)
            {
                information_texts[i].color = green_text_color;
            }
            else
            {
                break;
            }
        }
    }
    #endregion

    #region Audio Settings Buttons
    public void BackOneScreenFromAudioSettingsScreen()
    {
        audioSettingsScreen.SetActive(false);
        settingsScreen.SetActive(true);
    }

    public void ExitToMainMenuFromAudioSettingsScreen()
    {
        audioSettingsScreen.SetActive(false);
        mainMenuScreen.SetActive(true);
    }
    #endregion

    #region Insctructions and Controls Screen Button Controls
    public void BackOneScreenFromInstructionsAndControlsScreen()
    {
        instructionsAndControlsScreen.SetActive(false);
        settingsScreen.SetActive(true);
    }

    public void ExitToMainMenuFromInstructionsAndControlsScreen()
    {
        instructionsAndControlsScreen.SetActive(false);
        mainMenuScreen.SetActive(true);
    }
    #endregion

    #region Credits Screen Controls 
    public void ExitToMainMenuFromCreditsScreen()
    {
        creditsScreen.SetActive(false);
        mainMenuScreen.SetActive(true);
    }

    
    #endregion
}
