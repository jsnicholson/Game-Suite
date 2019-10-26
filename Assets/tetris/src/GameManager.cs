using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// the primary game manager, handles initialising this instance
/// </summary>
public class GameManager : MonoBehaviour {

// ===== PRIVATE MEMBERS =====
    private GameVariables m_variables;
    private TetrisGrid m_grid;
    private Spawner m_spawner;

    private AudioSource m_audioSource;
    // audio clips
    public AudioClip audio_lineClear;
// ~ PRIVATE MEMBERS

    void Start() {
        Initialise();
    }

// ===== CONSTRUCTOR =====
    public void Initialise() {
        m_variables = GetComponent<GameVariables>();
        m_grid = GameObject.Find("Grid").GetComponent<TetrisGrid>();
        m_grid.Initialise(this);
        m_spawner = GetComponent<Spawner>();
        m_spawner.Initialise(this);

        m_audioSource = GetComponent<AudioSource>();

        TetrisEvents.e_PieceLanded.AddListener(PieceLanded);
        TetrisEvents.e_GameOver.AddListener(GameOver);
    }

// ===== PUBLIC =====
    public GameVariables GetVariables() {
        return m_variables;
    }

    public TetrisGrid GetGrid() {
        return m_grid;
    }

    public Spawner GetSpawner() {
        return m_spawner;
    }

    /// <summary>
    /// listens for PieceLanded event
    /// </summary>
    private void PieceLanded() {
        // tell the grid to attempt to clear lines
        bool bAnyCleared = m_grid.ClearLines();

        if (bAnyCleared)
            Debug.Log("line cleared!");

        // spawn a new tetromino
        m_spawner.SpawnTetromino(); 
    }

    private void GameOver() {
        // disable spawner
        m_spawner.enabled = false;

        // get all tetrominos still in level
        Tetromino[] arr_remainingMinos = FindObjectsOfType<Tetromino>();
        
        // start adding rigidbodies
        StartCoroutine(AddRigidbodies(arr_remainingMinos));
        StopCoroutine("AddRigidbodies");
    }

    /// <summary>
    /// adds a Rigidbody2D to each remaining mino and fires upward with random angle
    /// </summary>
    /// <param name="arr_t"></param>
    /// <returns></returns>
    IEnumerator AddRigidbodies(Tetromino[] arr_t) {
        foreach(Tetromino t in arr_t) {
            t.enabled = false;

            foreach(Transform o in t.gameObject.transform) {
                Rigidbody2D rb = o.gameObject.AddComponent<Rigidbody2D>();
                rb.AddForce(Vector2.up * Random.Range(100, 200));
                rb.AddForce(Vector2.right * Random.Range(-100, 100));
            }
           
            yield return new WaitForSeconds(0.05f);
        }
    }

} // ~GameManager