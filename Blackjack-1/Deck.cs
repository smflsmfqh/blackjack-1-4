using System;
using System.Numerics;

// --- 덱의 카드를 관리하는 클래스 ---
class Deck
{
    // --- 덱 변수 및 속성들 ---
    public string[] _deck { get; private set; }
    public int count { get; private set; } = 0; // 덱 객체 별로 count를 초기화 -> 전체 카드 배열과 인덱스 분리
    private int _aCount = 0; // 덱에서 "A" 카드의 개수
    public string HiddenCard { get { return _deck[0]; } }  // 숨겨진 카드 속성

    // --- 생성자 --- 
    // 전체 카드 배열의 절반 정도의 길이를 가진 덱 생성
    public Deck()
    {
        _deck = new string[20];
    }

    // --- 덱에 카드를 더하는 메서드 --- 
    public void Add(string s)
    {
        _deck[count] = s;
        count++;
    }

    // --- 덱에 있는 가장 최근 카드 한 장 출력 메서드 ---
    public void PrintCard()
    {
        Console.WriteLine($"[{_deck[count - 1]}]");
    }

    // --- 덱에 있는 모든 카드 출력 메서드 ---
    public void PrintAllCards()
    {
        foreach (string s in _deck)
        {
            if (s == null)
            {
                continue;
            }
            else Console.Write($"[{s}] ");
        }
    }

    // --- 덱에 있는 카드 모두 더하는 메서드 ---
    // 덱에 A가 있고 score가 21 초과 시 A값 스케일링 
    public int SumOfCards()
    {
        int score = 0;
        foreach (string d in _deck)
        {
            if (d == null)
            { continue; }
            string dTrimmed = d.Substring(1);
            switch (dTrimmed)
            {
                case "A":
                    score += 11;
                    _aCount++;
                    break;
                case "J":
                case "K":
                case "Q":
                    score += 10;
                    break;
                case null:
                    score += 0;
                    break;
                default:
                    int.TryParse(dTrimmed, out int value);
                    score += value;
                    break;
            }
        }
        if (score > 21 && _aCount != 0)
        {
            score -= 10;
            _aCount--;
        }
        return score;
    }

}
