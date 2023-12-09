using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    // 유저 아이디 변수
    public InputField id;

    // 유저 패스워드 변수
    public InputField password;

    // 검사 텍스트 변수
    public Text notify;

    // Start is called before the first frame update
    void Start()
    {
        notify.text = "";
    }

    // 아이디와 패스워드 저장 함수
    public void SaveUserData()
    {
        if (CheckInput(id.text, password.text) == false)
        {
            return;
        }

        // 만약 시스템에 저장돼 있는 아이디가 존재하지 않는다면
        if (!PlayerPrefs.HasKey(id.text))
        {
            // 사용자의 아이디는 Key, 패스워드는 Value으로 설정해 저장
            PlayerPrefs.SetString(id.text, password.text);
            notify.text = "계정이 생성되었습니다.";
        }
        // 그렇지 않으, 이미 아이디가 존재한다는 메세지를 출력
        else
        {
            notify.text = "이미 존재하는 아이디입니다.";
        }
    }

    // 로그인 함수
    public void CheckUserData()
    {
        if (CheckInput(id.text, password.text) == false)
        {
            return;
        }

        if (!PlayerPrefs.HasKey(id.text))
        {
            notify.text = "입력하신 아이디는 존재하지않는 아이디입니다.";
            return;
        }

        // 사용자가 입력한 아이디를 키로 사용해서 시스템에 저장된 값을 불러온다
        string pass = PlayerPrefs.GetString(id.text);

        // 만약 사용자가 입력한 패스워드와 시스템에서 불러온 값을 비교해서 동일하다면
        if (password.text == pass)
        {
            // 다음 씬을 로드
            SceneManager.LoadScene(1);
        }
        else
        {
            notify.text = "입력하신 패스워드가 일치하지 않습니다.";
        }
    }

    // 입력 확인 함수
    bool CheckInput(string id, string password)
    {
        // 만약 입력란이 하나라도 비어 있으면 유저 정보 입력을 요구
        if (id == "" || password == "")
        {
            notify.text = "아이디 또는 패스워드를 입력하세요.";
            return false;
        }
        else
        {
            return true;
        }
    }
}
