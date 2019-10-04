using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private Board board;
    [SerializeField]
    private AudioClip stoneDropClip;
    [SerializeField]
    private AudioClip winClip;
    [SerializeField]
    private AudioClip drawClip;

    private const int maxStones = 42;

    private Text playerInfo;
    private Text playerTurn;
    private AudioSource audioSource;
    private bool gameDone;
    private PlayerEnum currentPlayer;
    private int usedStones;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playerInfo = GameObject.Find("PlayerInfoTextBox").GetComponent<Text>();
        playerTurn = GameObject.Find("PlayerTurnTextBox").GetComponent<Text>();
        InitializeGame();
    }

    private void InitializeGame()
    {
        board.Initialize();
        currentPlayer = PlayerEnum.red;
        gameDone = false;
        usedStones = 0;
        playerTurn.text = "Turn: " + currentPlayer;
        playerInfo.text = "";
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            InitializeGame();
            return;
        }
        if (gameDone) return;
        if (usedStones >= maxStones)
        {
            SetWinner(PlayerEnum.NONE);
        }

        int slot = GetSelectedSlot();
        if (slot != -1)
        {
            if (board.HasSlotFreeRows(slot))
            {
                board.DropStone(currentPlayer, slot);
                audioSource.PlayOneShot(stoneDropClip);
                usedStones++;
                if (board.CheckConnectFour(currentPlayer))
                {
                    SetWinner(currentPlayer);
                    return;
                }
                SwitchPlayer();
            }
            else
            {
                Debug.Log("Slot " + slot + " is not free");
                SetPlayerInfo("Slot " + slot + " is not free");
            }

        }
    }

    private int GetSelectedSlot()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            return 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            return 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            return 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            return 3;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            return 4;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            return 5;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            return 6;
        }
        else
        {
            return -1;
        }
    }

    public void SwitchPlayer()
    {
        switch (currentPlayer)
        {
            case PlayerEnum.red:
                currentPlayer = PlayerEnum.yellow;
                break;
            case PlayerEnum.yellow:
                currentPlayer = PlayerEnum.red;
                break;
            default:
                Debug.LogError("Impossible Player");
                break;
        }
        playerTurn.text = "Turn: " + currentPlayer;
    }

    public void SetPlayerInfo(string text)
    {
        playerInfo.text = text;
        StartCoroutine(ClearPlayerInfo());
    }

    IEnumerator ClearPlayerInfo()
    {
        yield return new WaitForSeconds(1.5f);
        playerInfo.text = "";
    }

    private void SetWinner(PlayerEnum player)
    {
        Debug.Log(player + " wins");
        StopAllCoroutines();
        if (player == PlayerEnum.NONE)
        {
            playerInfo.text = "Draw";
            audioSource.PlayOneShot(drawClip);
        }
        else
        {
            playerInfo.text = player + " wins";
            audioSource.PlayOneShot(winClip);
        }
        gameDone = true;
    }
}
