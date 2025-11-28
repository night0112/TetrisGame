using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;//シーン遷移のライブラリ
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
 //変数作成
 //スポナー
 //生成されたブロック格納

    Spawner spawner;//スポナー
    Block activeBlock;//生成されたブロック格納
    
    public Text Score_text;
    public int score_num = 0;
    public GameObject score_object = null;

    //変数の作成

    [SerializeField]
    private float dropInterval = 0.25f; //次にブロックが落ちるまでのインターバル時間
    float nextdropTimer;//次にブロックが落ちるまでの時間
    //変数作成
    //ボードのスクリプトを格納
    Board board;


    //次回はここから　動画１：１９：２８

    //変数作成
    //入力受付タイマー（３種類）
    float nextKeyDownTimer, nextKeyLeftRightTimer, nextKeyRotateTimer;

    //入力インターバル（３種類）
    [SerializeField]
    private float nextKeyDownInterval, nextKeyLeftRightInterval, nextKeyRotateInterval;

    //変数作成
    //パネルの格納
    [SerializeField]
    private GameObject gameOverPanel;

    //ゲームオーバー判定
    bool gameOver;

   

    //スポナーオブジェクトをスポナー変数に核のするコードの記述

    private void Start()//動画４２分から
    {
        Score_text = GetComponent<Text>();

        spawner = GameObject.FindObjectOfType<Spawner>();

        //ボードを変数に格納する
        board = GameObject.FindObjectOfType<Board>();

        spawner.transform.position = Raunding.Round(spawner.transform.position);

        //タイマーの初期設定
        nextKeyDownTimer = Time.time + nextKeyDownInterval;
        nextKeyLeftRightTimer = Time.time + nextKeyLeftRightInterval;
        nextKeyRotateTimer = Time.time + nextKeyRotateInterval;


        //スポナークラスからブロック生成関数を呼んで変数に格納する
        if (!activeBlock)
        {
            activeBlock = spawner.SpawnBlock();
        }

        //ゲームオーバーパネルの非表示
        if (gameOverPanel.activeInHierarchy)
        {
            gameOverPanel.SetActive(false);
        }
    }

    private void Update()
    {
        Text score_text = score_object.GetComponent<Text>();
        score_text.text = "Score:" + score_num;

        if (gameOver)
        {
            return;
        }

       

        PlayerInput();

       /* //Updateで時間の判定をして判定0第で落下関数を呼ぶ
        if (Time.time > nextdropTimer)
        {
            nextdropTimer = Time.time + dropInterval;

            if (activeBlock)
            {
                activeBlock.MoveDown();


                //UpdateでBoardクラスの関数を呼び出してボードから出ていないか確認
                if (!board.CheckPosition(activeBlock))
                {
                    activeBlock.MoveUp();

                    board.SaveBlockInGrid(activeBlock);

                    activeBlock = spawner.SpawnBlock();
                }
            }
        }*/
    }


    //関数の作成
    //キーの入力を検知してブロックを動かす関数
    //ボードの底に着いたときに次のブロックを生成する関数

    void PlayerInput()
    {
        if(Input.GetKey(KeyCode.D) && (Time.time > nextKeyLeftRightTimer) || Input.GetKeyDown(KeyCode.D))
        {
            activeBlock.MoveRight();//右移動

            nextKeyLeftRightTimer = Time.time + nextKeyLeftRightInterval;

            if (!board.CheckPosition(activeBlock))
            {
                activeBlock.MoveLeft();
            }
        }
        else if(Input.GetKey(KeyCode.A) && (Time.time > nextKeyLeftRightTimer) || Input.GetKeyDown(KeyCode.A))
        {

            activeBlock.MoveLeft();//左移動

            nextKeyLeftRightTimer = Time.time + nextKeyLeftRightInterval;

            if (!board.CheckPosition(activeBlock))
            {
                activeBlock.MoveRight();
            }
        }
        else if (Input.GetKeyDown(KeyCode.E) && (Time.time > nextKeyRotateTimer))
        {
            activeBlock.RotateRight();
            nextdropTimer = Time.time + nextKeyRotateInterval;

            if (!board.CheckPosition(activeBlock))
            {
                activeBlock.RotateLeft();
            }
        }
        else if (Input.GetKey(KeyCode.S) && (Time.time > nextKeyDownTimer) || (Time.time > nextdropTimer))
        {

            activeBlock.MoveDown();//下移動

            nextKeyDownTimer = Time.time + nextKeyDownInterval;
            nextdropTimer = Time.time + dropInterval;

            if (!board.CheckPosition(activeBlock))
            {
                if(board.OverLimit(activeBlock))
                {
                    GameOver();
                }
                else
                {
                    //底についた時の処理
                    BottomBoard();
                }
               
            }
        }
    }


    void BottomBoard()
    {
        activeBlock.MoveUp();
        board.SaveBlockInGrid(activeBlock);

        activeBlock = spawner.SpawnBlock();

        nextKeyDownTimer = Time.time;
        nextKeyLeftRightTimer = Time.time;
        nextKeyRotateTimer = Time.time;

        board.ClearAllRows();   //埋まっていれば消える

        
        
        score_num += 10;
    }

    public void ScorePlus()//ブロックが壊れた時のスコア加算
    {
        score_num += 50;
    }

    //関数作成
    //ゲームオーバーになったらパネルを表示する
    public void GameOver()
    {
        activeBlock.MoveUp();

        if (!gameOverPanel.activeInHierarchy)
        {
            gameOverPanel.SetActive(true);
        }

        gameOver = true;
    }

    //シーンを再読み込みする
    public void Restrat()
    {
        SceneManager.LoadScene(0);
    }
}
