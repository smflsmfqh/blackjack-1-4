using System;

string retry = string.Empty;

while (retry != "n")
{
    Console.Clear();
    Game BlackJack = new Game();
    BlackJack.Play();

    Console.WriteLine();
    Console.Write("새 게임을 하시겠습니까? (Y/N): ");

    bool isValidate = false;
    while (!isValidate)
    {
        retry = Console.ReadLine().Trim().Replace(" ", "").ToLower();
        if (retry == "y" || retry == "n")
        {
            isValidate = true;
        }
        else
        {
            Console.Write("다시 입력하세요(Y or N): ");
        }
    }
    BlackJack.Reset();
}

    
