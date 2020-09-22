using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameInstance : MonoBehaviour {

    private static GameInstance instance;

   private bool isPlayerAlive = true;
    [SerializeField] private float DistanceWalked = 0f;
    [SerializeField] private int BulletsShot = 0;
    [SerializeField] private int EnemiesKilled = 0;
    [SerializeField] private int AbsorbedBullets = 0;
    [SerializeField] private int MoneyCollected = 0;
    [SerializeField] private int ForceFieldsCollected = 0;
    [SerializeField] private float ForceFieldSecondsCollected = 0f;
    private float totalScore = 0;
    public Text UI_ScoreValue;

    public Canvas UI_DeadScreen;

    [Header("Invoker System")]
    private Action[] actions;
    [SerializeField] private float[] actionTimers;
    [SerializeField] private bool[] dirtyIndex;
    [SerializeField] private int count = 0;
    [SerializeField] private int totalCount = 0;

    private void Start() {
        if (instance == null) {
            instance = this;
            UI_DeadScreen.enabled = false;
            Time.timeScale = 1f;
            isPlayerAlive = true;
            totalScore = 0f;

            //Quick solution. Deadline soon.
            Weapon.BulletsShot = 0;
        }
    }

    #region My own callback in # seconds implementation
    private void Update() {
        if (count > 0) {
            InvokeTimedFunctions();
        }
    }

    private void InvokeTimedFunctions() {
        float deltaTime = Time.deltaTime;
        bool isDirty = InvokeMethod(deltaTime);
        if (isDirty == true) {
            CleanUpDirtyFunctions();
        }
    }

    private bool InvokeMethod(float deltaTime) {
        bool isDirty = false;
        for (int i = 0; i < count; i++) {
            actionTimers[i] -= deltaTime;
            if (actionTimers[i] <= 0f) {
                if (actions[i] != null) {
                    actions[i]();
                }
                dirtyIndex[i] = true;
                isDirty = true;
            }
        }
        return isDirty;
    }

    private void CleanUpDirtyFunctions() {
        Action[] actions = this.actions;
        float[] timers = this.actionTimers;

        int newArrayLength = 0;
        for (int i = 0; i < this.count; i++) {
            if (this.dirtyIndex[i] == false) {
                newArrayLength++;
            }
        }

        this.actions = new Action[newArrayLength == 0 ? 3 : newArrayLength];
        this.actionTimers = new float[newArrayLength == 0 ? 3 : newArrayLength];
        int tempCount = 0;
        for (int i = 0; i < this.count; i++) {
            if (this.dirtyIndex[i] == false) {
                this.actions[tempCount] = actions[i];
                this.actionTimers[tempCount] = timers[i];
                tempCount++;
            }
        }
        this.dirtyIndex = new bool[newArrayLength == 0 ? 3 : newArrayLength];
        this.count = tempCount;
        this.totalCount = this.actions.Length;
    }

    public static void InvokeFunction(Action callbackFunc, float seconds) {
        if (instance.actions == null) {
            instance.actions = new Action[3];
            instance.actionTimers = new float[3];
            instance.dirtyIndex = new bool[3];
            instance.totalCount = instance.actions.Length;
        }
        else if (instance.count >= instance.totalCount - 1) {
            Action[] actions = instance.actions;
            float[] timers = instance.actionTimers;
            bool[] dirty = instance.dirtyIndex;

            instance.dirtyIndex = new bool[2 + instance.totalCount];
            instance.actions = new Action[2 + instance.totalCount];
            instance.actionTimers = new float[2 + instance.totalCount];

            actions.CopyTo(instance.actions, 0);
            timers.CopyTo(instance.actionTimers, 0);
            dirty.CopyTo(instance.dirtyIndex, 0);

            instance.totalCount = instance.actions.Length;
        }
        instance.actions[instance.count] = callbackFunc;
        instance.actionTimers[instance.count] = seconds;
        instance.count++;
    }

    #endregion

    void LateUpdate() {
        DistanceWalked = PlayerController.DistanceWalked;
        BulletsShot = Weapon.BulletsShot;
        EnemiesKilled = EnemyCoordinator.EnemiesKilled;
        AbsorbedBullets = PlayerForceField.AbsorbedBullets;
        MoneyCollected = PlayerInventory.CollectedMoney;
        ForceFieldSecondsCollected = PlayerInventory.ForceFieldSecondsCollected;
        ForceFieldsCollected = PlayerInventory.forceFieldsCollected;
        if (isPlayerAlive == true) {
            totalScore = CalculateScore();
            UI_ScoreValue.text = totalScore.ToString("0");
        }
    }

    public static float CalculateScore() {
        
        float totalScore = 0;
        totalScore += Mathf.RoundToInt(instance.DistanceWalked / 10);
        totalScore += instance.BulletsShot * 10;
        totalScore += instance.EnemiesKilled * 100;
        totalScore += instance.AbsorbedBullets * 50;
        totalScore += instance.MoneyCollected;
        return totalScore;
    }

    public static void GameOver() {
        Time.timeScale = 0.1f;
        RectTransform rectTransform = instance.UI_ScoreValue.transform.parent.GetComponent<RectTransform>();
        Vector2 scorePosition = rectTransform.position;
        scorePosition.y = Screen.height / 2 + 100f;
        rectTransform.position = scorePosition;
        foreach (var item in PlayerController.GameObject.GetComponentsInChildren<SkinnedMeshRenderer>()) {
            item.enabled = false;
        }
        instance.UI_DeadScreen.enabled = true;
        PlayerPrefs.SetInt("HighScore", (int)instance.totalScore);
    }

    private void OnDisable() {
        instance = null;
    }

}

