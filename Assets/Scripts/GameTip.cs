using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTip : MonoBehaviour
{
    private string firstTip = "적을 클릭하고, 터렛을 설치하여 적들을 막으세요!";
    private string secondTip = "터렛을 구매하면 랜덤한 위치에 터렛이 생성됩니다.\n하지만 걱정 마세요.\n터렛의 공격 범위는 무한하답니다!";
    private string thirdTip = "같은 숫자, 같은 문양의 터렛을 두개 합치면\n다음 숫자, 랜덤 문양의 터렛이 나옵니다.";
    private string fourthTip = "빨간 다이아는 범위 공격을 할 수 있습니다.\n파란 클로버는 적을 느리게 합니다.\n노란 스페이드는 적을 일정시간 못 움직이게 합니다.\n검은 하트는 빠르게 적을 공격합니다.";
    private string fifthTip = "10 웨이브 마다 보스가 출현합니다.";
    private string sixthTip = "터렛을 팔면\n현재 터렛 구매 시 소요되는 금액의 반을 받습니다.";
    private string seventhTip = "업그레이드를 하여 터렛과 클릭 시 공격을 높여보세요.";
    private string eighthTip = "퀘스트는 함부로 도전하지 마세요.\n무서운 적이 나타납니다.\n자신 있을때 도전 하세요.";
    private string nineTip = "터렛을 클릭하여 터렛의 정보를 확인할 수 있습니다.";

    public List<string> TipLists = new List<string>();

    public Text tipText;
    public Text indexText;

    private int idx;


    private void Start()
    {
        TipLists.Add(firstTip);
        TipLists.Add(secondTip);
        TipLists.Add(thirdTip);
        TipLists.Add(fourthTip);
        TipLists.Add(fifthTip);
        TipLists.Add(sixthTip);
        TipLists.Add(seventhTip);
        TipLists.Add(eighthTip);
        TipLists.Add(nineTip);

        tipText.text = TipLists[0];

        indexText.text = (idx + 1).ToString() + "/" + TipLists.Count;
    }

    public void Before()
    {
        idx--;

        if (idx < 0)
            idx = 0;

        tipText.text = TipLists[idx];
        indexText.text = (idx + 1).ToString() + "/" + TipLists.Count;
    }

    public void After()
    {
        idx++;

        if (idx > TipLists.Count - 1)
            idx = TipLists.Count - 1;

        tipText.text = TipLists[idx];
        indexText.text = (idx + 1).ToString() + "/" + TipLists.Count;
    }
}
