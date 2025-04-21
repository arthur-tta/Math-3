using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

public class UIMainMenu : UICanvas
{
    public GameObject buttons;

    public GameObject playPanel;
    public GameObject testPanel;

    // test panel
    public TMP_Dropdown dropdownA;
    private int selectedIndexA;
    public TMP_Dropdown dropdownB;
    private int selectedIndexB;


    public TMP_InputField targetInput;
    private int target;
    public TMP_InputField boardsInput;
    private int board;

    private void Start()
    {
        dropdownA.onValueChanged.AddListener((index) => {
            selectedIndexA = index;
            OnDropdownValueChanged(dropdownA, index);
        });

        dropdownB.onValueChanged.AddListener((index) => {
            selectedIndexB = index;
            OnDropdownValueChanged(dropdownB, index);
        });

        targetInput.onEndEdit.AddListener((value) => {
            OnInputFieldValueChanged(targetInput, value);

            if (int.TryParse(value, out int result))
            {
                if(result == 0)
                {
                    target = 10000; // default value
                    Debug.Log("Không điền giá trị, cài đặt là giá trị mặc định !");
                    Debug.Log("Mục tiêu cần đạt được là: " + target);
                }
                target = result;
                Debug.Log("Mục tiêu cần đạt được là: " + target);
                
            }
            else
            {
                target = 10000; // default value
                Debug.Log("Giá trị nhập bị lỗi, cài đặt là giá trị mặc định !");
                Debug.Log("Mục tiêu cần đạt được là: " + target);

            }

        });

        boardsInput.onEndEdit.AddListener((value) => {
            OnInputFieldValueChanged(boardsInput, value);

            if (int.TryParse(value, out int result))
            {
                if (result == 0)
                {
                    board = 100; // default value
                    Debug.Log("Không điền giá trị, cài đặt là giá trị mặc định !");
                    Debug.Log("Tổng số ván chơi sẽ là: " + board);
                }
                board = result;
                Debug.Log("Tổng số ván chơi sẽ là: " + board);
            }
            else
            {
                board = 100; // default value
                Debug.Log("Giá trị nhập bị lỗi, cài đặt là giá trị mặc định !");
                Debug.Log("Tổng số ván chơi sẽ là: " + board);
            }

        });

    }


    public void OnClickPlayButton()
    {
        buttons.SetActive(false);
        playPanel.SetActive(true);
    }

    public void OnClickTestButton()
    {
        buttons.SetActive(false);
        testPanel.SetActive(true);
    }

    public void OnClickTrainButton()
    {

    }

    public void OnClickQuitButton()
    {

    }

    public void OnClickExitButon()
    {
        buttons.SetActive(true);
        testPanel.SetActive(false);
        playPanel.SetActive(false);

    }


    // play panel
    public void OnClickStartPlayWithEasyMode()
    {
        UIManager.Instance.CloseAll();
        UIManager.Instance.OpenUI<UIMainGame>();

        //GameManager.Instance.SetDifficutly(Difficultly.Easy);
       // GameManager.Instance.StartGame(GameMode.Playing);

        
    }

    public void OnClickStartPlayWithMediumMode()
    {

    }
    public void OnClickStartPlayWithHardMode()
    {

    }


    // test panel
    public void OnClickStartTest()
    {
        // Lấy mode tương ứng từ selectedIndex
        Mode modeA = GetModeFromIndex(selectedIndexA);
        Mode modeB = GetModeFromIndex(selectedIndexB);

        // Gán mặc định nếu chưa nhập gì
        if (board == 0) board = 100;
        if (target == 0) target = 10000;

        // Chuyển UI và bắt đầu game
        UIManager.Instance.CloseAll();
        UIManager.Instance.OpenUI<UIMainGame>();
        UIManager.Instance.GetUI<UIMainGame>().Initialized(target);

        Board.Instance.StartGame(GameMode.Test);
        GameManager.Instance.StartTestGame(modeA, modeB);
    }

    // Hàm helper để chuyển selected index sang Mode
    private Mode GetModeFromIndex(int index)
    {
        switch (index)
        {
            case 1:
                return Mode.BruteForcePlus;
            case 2:
            case 3:
            default:
                return Mode.BruteForce;
        }
    }
    private void OnDropdownValueChanged(TMP_Dropdown dropdown, int index)
    {
        string selectedText = dropdown.options[index].text;
        Debug.Log($"{dropdown.name} selected: {selectedText}");
    }
    private void OnInputFieldValueChanged(TMP_InputField inputField, string input)
    { 
    }
}
