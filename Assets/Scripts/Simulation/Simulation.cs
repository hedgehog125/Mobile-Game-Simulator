using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Simulation {
    public const string introCutscene = "Intro";
    public const string phoneScene = "PhoneMenu";
    public const string mainMenuScene = "MainMenu";
    public const string winScene = "Outro";

    public const int gameCount = 2;

    private static List<string> gotAllMessage;
    private static List<string> timeUpMessage;
    private static List<string> winMessage;
    private const string firstUnlockMessage = "Press backspace to swipe up and go to the homescreen";

    public static CutsceneTextController textBox;
    public static FactPageController factBox;

    static Simulation() {
        gotAllMessage = new List<string>();
        gotAllMessage.Add("Ted: Nice, you got them all. I've installed the next game now");

        timeUpMessage = new List<string>();
        timeUpMessage.Add("Ted: And time's up. You can continue playing if you like but I'll only research things in the next game");

        winMessage = new List<string>();
        winMessage.Add("Ted: Alright. That's all of them. You can look away from the phone now");
    }

    public static Difficulty difficulty;
    public class Difficulty {
        public int gameTimeLimit;

        public Difficulty(int level) {
            if (level == 0) {
                gameTimeLimit = (10 * 60) * 50;
            }
            else if (level == 1) {
                gameTimeLimit = (5 * 60) * 50;
			}
		}
    }

    public static bool inGame;
    public static int gameID;

    public static bool menuPopupActive;
    public static bool stayOnLastActive;
    public static bool revisitingGame;
    public static bool firstGamePlay;
    public static bool gotAllInGame;

    public static bool preventClose;

    public static void Spend(int amount) {
        currentSave.spent += amount;
	}

    public static void IncreaseTime(int amount) {
        currentSave.timeLeft -= amount;
        if (currentSave.timeLeft < 0) {
            TimesUp();
        }
    }
    public static void GotAllPoints() {
        Progress(false, gotAllMessage);
    }


    private static void TimesUp() {
        Progress(false, timeUpMessage);
    }

    private static void Progress(bool changeScene, List<string> message) {
        if (gameID == gameCount) { // Last game
            textBox.stayOnLast = false;
            textBox.nextScene = winScene;

            textBox.Display(winMessage);
        }
        else {
            currentSave.timeLeft = difficulty.gameTimeLimit;

            bool firstNewUnlock = false;
            if (gameID <= currentSave.gamesUnlocked) { // Unlock the next game if that doesn't decrease the progress
                currentSave.gamesUnlocked = gameID + 1;
                firstNewUnlock = gameID == 1;
            }

            if (changeScene) {
                ChangeScene(phoneScene);
                return;
            }

            if (message != null) {
                List<string> newMessage = new List<string>(message);

                textBox.stayOnLast = false;
                textBox.nextScene = "";
                if (firstNewUnlock) {
                    newMessage.Add(firstUnlockMessage);
                    textBox.stayOnLast = true;
                }

                textBox.Display(newMessage);
            }
        }
    }

    public static void UpdateVars(List<int> knowledgePoints) {
        bool textBoxShowing = textBox == null? false : textBox.gameObject.activeSelf;
        bool factBoxShowing = factBox == null? false : factBox.gameObject.activeSelf;

        menuPopupActive = textBoxShowing || factBoxShowing;
        stayOnLastActive = textBoxShowing? (textBox.stayOnLast && textBox.onLast) : false;
        revisitingGame = (gameID == 0 || gameID == -1)? false : currentSave.gamesUnlocked != gameID;

        bool gotAll = true;
        if (knowledgePoints.Count != 0) {
            foreach (int id in knowledgePoints) {
                if (! currentSave.knowledgePointsGot[id]) {
                    gotAll = false;
                    break;
                }
            }
        }
        gotAllInGame = gotAll;
    }

    public static void StartPlaying() {
        if (currentSave.watched.intro) {
            ChangeScene(phoneScene);
		}
        else {
            ChangeScene(introCutscene);
        }
    }

    public static void BackToMainMenu() {
        ChangeScene(mainMenuScene);
    }

    public static void ChangeScene(string sceneName, bool keepAudio=false) {
        if (! keepAudio) {
            AudioSource[] audios = GameObject.FindObjectsOfType<AudioSource>();

            foreach (AudioSource audio in audios) {
                audio.Stop();
            }
        }

        SceneManager.LoadScene(sceneName);
	}

    public static void Init(bool initSave) {
        if (initSave) InitSave();
    }
    public static void Init() {
        Init(true);
    }

    private static void InitSave() {
        if (currentSave == null) {
            currentSave = NewSave();
            Debug.LogWarning("Had to generate save, this should only happen if you're running from a different scene than the main menu.");

            // Set some values for easier development
            currentSave.watched.intro = true;

            currentSave.DNGSave.opens = 1;

            currentSave.DNGSave.plays = 1;
            currentSave.NFTMatchSave.plays = 0;

            currentSave.gamesUnlocked = 2;
		}
	}

    public static List<Save> saves = new List<Save>();
    public static Save currentSave;
    public static Save NewSave() {
        Save save = new Save();
        saves.Add(save);
        difficulty = new Difficulty(save.difficultyLevel);

        return save;
	}
}
