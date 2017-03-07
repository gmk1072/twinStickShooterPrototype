using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// UI State available in the game. Set them using SetState()
/// </summary>
public enum UIState
{
    GAME,
    DEAD,
    WIN   
}

/// <summary>
/// Basic UI script that will handle relevant UI information for our prototype
/// </summary>
public class UIScript : MonoBehaviour {

    // UI Panels
    private UIState uiState;
    private Transform[] panels;

    // Check if input is ready, should we reload the scene yet
    private bool canReload;
    private float time;
    private Text timeText;

    // number of enemies to count for
    private int numEnemies;

    /// <summary>
    /// Initializes current state to NONE and ensures all panels are set to
    /// inactive.
    /// </summary>
    private void Awake()
    {
        // Set start state to none
        canReload = false;
        time = 0.0f;

        // Get all panels and disable them
        panels = new Transform[transform.childCount];
        for(int i = transform.childCount; i-- != 0;)
        {
            panels[i] = transform.GetChild(i);
            panels[i].gameObject.SetActive(false);
        }

        // Figure out number of enemies
        numEnemies = FindObjectsOfType<CustomEnemy>().Length;

        // Grab time text from game panel
        timeText = panels[(int)UIState.GAME].FindChild("TimeText").GetComponent<Text>();

        // Init to starting state
        SetState(UIState.GAME);
    }

    private void Update()
    {
        switch(uiState)
        {
            case UIState.GAME:
                // increment timer
                time += Time.deltaTime;
                UpdateTime();
                break;
            case UIState.DEAD:
            case UIState.WIN:
                if (canReload && Input.GetKeyDown(KeyCode.R))
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                break;
        }
    }

    /// <summary>
    /// Sets the current UI state.
    /// </summary>
    /// <param name="state">The new state to switch to.</param>
    public void SetState(UIState state)
    {
        // Hide current state if valid
        panels[(int)uiState].gameObject.SetActive(false);

        // Set active new ui state
        panels[(int)state].gameObject.SetActive(true);

        // Transition to new state
        switch (state)
        {
            case UIState.GAME:
                break;
            case UIState.DEAD:
                StartCoroutine(DeathTextAppear());
                break;
            case UIState.WIN:
                StartCoroutine(WinTextAppear());
                break;
        }

        // Set new state
        uiState = state;
    }

    /// <summary>
    /// Decrement number of enemies to prepare for a win.
    /// </summary>
    public void DecrementEnemies()
    {
        // Only do this if the current state is WIN
        if (uiState != UIState.WIN)
        {
            numEnemies--;
            if (numEnemies <= 0)
                SetState(UIState.WIN);
        }
    }

    /// <summary>
    /// Update time on the game canvas UI
    /// </summary>
    private void UpdateTime()
    {
        // Calculate relevent time info
        int miliseconds = (int)((time - (int)time) * 100.0f);
        int seconds = (int)time % 60;
        int minutes = (int)time / 60;

        // Set text
        timeText.text = minutes.ToString("00") + ":" +
            seconds.ToString("00") + "." +
            miliseconds.ToString("00");
    }

    /// <summary>
    /// Coroutine for some dramatic pausing
    /// </summary>
    /// <returns></returns>
    private IEnumerator DeathTextAppear()
    {
        // Grab refs to text
        Transform deadText = panels[(int)UIState.DEAD].FindChild("DeadText");
        Transform restartText = panels[(int)UIState.DEAD].FindChild("RestartText");
        restartText.gameObject.SetActive(false);
        canReload = true;

        // Have RIP appear
        //deadText.gameObject.SetActive(true);

        // Wait 1 seconds
        yield return new WaitForSeconds(1.0f);

        // Have restart option appear
        restartText.gameObject.SetActive(true);

        // Exit
        yield break;
    }

    /// <summary>
    /// Have winning information appear slowly and timed as well
    /// </summary>
    /// <returns></returns>
    private IEnumerator WinTextAppear()
    {
        // Grab refs to text
        Transform winText = panels[(int)UIState.WIN].FindChild("WinText");
        Transform timeText = panels[(int)UIState.WIN].FindChild("TimeText");
        Transform restartText = panels[(int)UIState.WIN].FindChild("RestartText");
        restartText.gameObject.SetActive(false);
        timeText.gameObject.SetActive(false);

        // Have win appear
        //winText.gameObject.SetActive(true);

        // Wait 1 seconds
        yield return new WaitForSeconds(1.0f);

        // Update time text
        timeText.GetComponent<Text>().text = this.timeText.text;

        // Have time appear
        timeText.gameObject.SetActive(true);

        // Wait 1 second
        yield return new WaitForSeconds(2.0f);

        // Allow restart
        canReload = true;
        restartText.gameObject.SetActive(true);

        // Exit
        yield break;
    }
}
