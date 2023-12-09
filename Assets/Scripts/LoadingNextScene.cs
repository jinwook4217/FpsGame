using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingNextScene : MonoBehaviour
{
    // 진행할 씬 번호
    public int sceneNumber = 2;

    // 로딩 슬라이더
    public Slider loadingBar;

    // 로딩 진행 텍스트
    public Text loadingText;


    void Start()
    {
        StartCoroutine(TrasitionNextScene(sceneNumber));
    }


    // 비동기 씬 로드 코루틴
    IEnumerator TrasitionNextScene(int index)
    {
        // 지정된 씬을 비동기 방식으로 로드
        AsyncOperation ao = SceneManager.LoadSceneAsync(index);

        // 로드되는 씬의 모습이 화면에 보이지 않도록
        ao.allowSceneActivation = false;

        // 로딩이 완료될 때까지 반복해서 씬의 요소들을 로드하고 진행 과정을 화면에 표시
        while(!ao.isDone)
        {
            // 로딩 진행률을 슬라이더 바와 텍스트로 표시
            loadingBar.value = ao.progress;
            loadingText.text = (ao.progress * 100f).ToString() + "%";

            // 만약 씬 로드 진행률이 90%를 넘어가면
            if (ao.progress >= 0.9f)
            {
                // 지정된 씬을 화면에 보이게 합니다
                ao.allowSceneActivation = true;
            }

            // 다음 프레임이 될 때까지 기다린다
            yield return null;
        }
    }
}
