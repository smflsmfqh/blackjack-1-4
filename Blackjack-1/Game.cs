using System;
using System.Dynamic;
using System.IO.Pipelines;
class Game
{
    // --- 게임 변수 및 속성들 ---
    private string[] deck = new string[52]; 

    // class Game has a class Participant
    private Participant player = new Participant("플레이어");
    private Participant dealer = new Participant("딜러");

    private int Count { get; set; } = 0; // 전체 카드 배열의 인덱스
    private bool IsBlackJack { get; set; } = false;
    private bool IsSeveentennMore { get; set; } = false;
    private bool IsBust { get; set; } = false;
    private bool IsStand { get; set; } = false;
    private string[] playerWinStates { get; } = { "플레이어의 승리!", "딜러의 승리!", "무승부!" }; // 플레이어를 기준으로 게임 결과 상태를 담은 문자열 배열

    // --- 카드 배열 생성자 ---
    // 전체 카드 배열 생성하고 카드 셔플
    public Game()
    {
        string[] suits = { "♠", "♥", "♣", "◆" };
        string[] ranks = { "A", "J", "K", "Q", "2", "3", "4", "5", "6", "7", "8", "9", "10" };
        int index = 0;
        foreach (string s in suits)
        {
            foreach (string rank in ranks)
            {
                deck[index++] += s + rank;
            }
        }
    }

    // --- 카드 셔플 메서드 ---
    public void Shuffle()
    {
        Random random = new Random();
        for (int i = deck.Length - 1; i > 0; i--)
        {
            int j = random.Next(0, i + 1);
            string temp = deck[i];
            deck[i] = deck[j];
            deck[j] = temp;
        }

        Console.WriteLine("카드를 섞는 중...\n");
    }

    // --- 블랙잭 게임 플레이 메서드 ---
    // 1. 초기턴 2. 플레이어 턴 3. 딜러 턴 4. 게임 종료 - 승자 및 결과 출력
    public void Play()
    {
        FirstTurn(); // 1. 초기턴
        PlayerTurn(); // 2. 플레이어 턴

        Console.WriteLine();
        Console.Write($"{dealer.Name}의 숨겨진 카드: [{dealer.HiddenCard}]");
        Console.WriteLine();
        Console.Write($"{dealer.Name}의 패: ");
        dealer.PrintAllCards();
        Console.WriteLine();
        dealer.PrintSum();
        Console.WriteLine();

        DealerTurn(); // 3. 딜러 턴

        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine();

        // 4. 게임 종료 - 승자 및 결과 출력
        WhoisWinner(); 
        ShowFinalResults();
    }

    // --- 첫 번째 턴 메서드 ---
    public void FirstTurn()
    {
        Console.WriteLine("=== 블랙잭 게임 ===\n");
        Console.WriteLine();
        Shuffle();
        player.ReceiveCard(deck[Count++]);
        player.ReceiveCard(deck[Count++]);
        dealer.ReceiveCard(deck[Count++]);
        dealer.ReceiveCard(deck[Count++]);
        Console.WriteLine();
        Console.WriteLine("=== 초기 패 ===");
        Console.Write($"{dealer.Name}의 패: [??] ");
        dealer.PrintCard();
        Console.WriteLine($"{dealer.Name} 점수: ??");
        Console.WriteLine();
        Console.Write($"{player.Name}의 패: ");
        player.PrintAllCards();
        Console.WriteLine();
        player.PrintSum();
        Console.WriteLine();
    }

    // --- 플레이어 턴 메서드 ---
    private void PlayerTurn()
    {
        while (!IsBlackJack && !IsBust && !IsStand)
        {
            CheckBlackJack();
            if (IsBlackJack) { break; }
            Console.Write("H(Hit) 또는 S(Stand)를 선택하세요: ");
            CheckStand();
            if (IsStand) { break; }
            Console.WriteLine();
            player.ReceiveCard(deck[Count++]);
            Console.Write($"{player.Name}가 카드를 받았습니다: ");
            player.PrintCard();
            Console.Write($"{player.Name}의 패: ");
            player.PrintAllCards();
            Console.WriteLine();
            player.PrintSum();
            CheckBust();
            if (IsBust) { break; }
            Console.WriteLine();
        }
    }

    // --- 딜러 턴 메서드 ---
    private void DealerTurn()
    {
        while (!IsBust && !IsSeveentennMore)
        {
            CheckDealerCard();
            if (IsSeveentennMore) { break; }
            Console.Write($"{dealer.Name}가 카드를 받습니다: ");
            dealer.PrintCard();
            Console.Write($"{dealer.Name}의 패: ");
            dealer.PrintAllCards();
            Console.WriteLine();
            dealer.PrintSum();
            Console.WriteLine();
            CheckBust();
            if (IsBust) { break; }
        }
    }

    // --- 블랙잭 검사 메서드 ---
    private bool CheckBlackJack()
    {
        if (player.Score == 21 || dealer.Score == 21)
        {
            IsBlackJack = true;
        }
        else
        {
            IsBlackJack = false;
        }
        return IsBlackJack;
    }

    // --- 딜러 카드가 17 이상인지 검사 메서드 ---
    private bool CheckDealerCard()
    {
        if (dealer.Score < 17)
        {
            IsSeveentennMore = false;
            dealer.ReceiveCard(deck[Count++]);
        }
        else
        {
            IsSeveentennMore = true;
        }
        return IsSeveentennMore;
    }

    // --- 버스트 검사 메서드 ---
    private bool CheckBust()
    {
        if (player.Score > 21 || dealer.Score > 21)
        {
            IsBust = true;
        }
        else
        {
            IsBust = false;
        }
        return IsBust;
    }

    // --- 플레이어가 Stand를 선택했는지 검사 메서드 ---
    private bool CheckStand()
    {
        bool IsValidate = false;
        string input = string.Empty;
        while (!IsValidate)
        {
            input = Console.ReadLine().Trim().Replace(" ", "").ToUpper();
            if (input == "S" || input == "H")
            {
                IsValidate = true; 
            }
            else
            {
                Console.WriteLine("다시 입력하세요(H or S): ");
            }
        }

        if (input == "S")
        {
            Console.WriteLine($"{player.Name}가 Stand를 선택했습니다");
            Console.WriteLine();
            IsStand = true;
        }
        else { IsStand = false; }

        return IsStand;
    }

    // --- 승자 판별 메서드 ---
    // 플레이어를 기준으로한 게임 결과 상태 배열에서, 알맞은 결과 반환
    public string WhoisWinner()
    {
        if (IsBust)
        {
            if (player.Score > 21 &&  dealer.Score > 21)
            {
                return playerWinStates[2];
            }
            else if (player.Score > 21) 
            {
                return playerWinStates[1]; 
            }
            else
            {
                return playerWinStates[0];
            }

        }
        else if (IsBlackJack)
        {
            if (player.Score == 21 && dealer.Score == 21)
            {
                return playerWinStates[2];
            }
            else if (player.Score == 21)
            {
                return playerWinStates[0];
            }
            else { return playerWinStates[1]; }
        }
        else
        {
            if (player.Score == dealer.Score)
            {
                return playerWinStates[2];
            }
            else if (player.Score > dealer.Score)
            {
                return playerWinStates[0];
            }
            else { return playerWinStates[1]; }
        }

    }

    // --- 게임 결과 출력 메서드 ---
    public void ShowFinalResults()
    {
        string result = WhoisWinner();
        Console.WriteLine("=== 게임 결과 ===");
        Console.WriteLine($"{player.Name}: {player.Score}점");
        Console.WriteLine($"{dealer.Name}: {dealer.Score}점");
        Console.WriteLine();
        if (IsBlackJack == true) { Console.WriteLine("블랙잭!"); }
        else if (IsBust == true) { Console.WriteLine("버스트!"); }
        Console.WriteLine();
        if (result == playerWinStates[2]) { Console.WriteLine(playerWinStates[2]); }
        else if (result == playerWinStates[0]) { Console.WriteLine(playerWinStates[0]); }
        else if (result == playerWinStates[1]) { Console.WriteLine(playerWinStates[1]); }
    }

    public void Reset()
    {
        IsBlackJack = false;
        IsBust = false;
        IsSeveentennMore = false;
        IsStand = false;
        Count = 0;
    }

}