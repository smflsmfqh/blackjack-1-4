using System;
using System.ComponentModel;
using System.Reflection.Metadata;

// --- 플레이어와 딜러 객체를 생성할 클래스 --- 
// class Participant has a class Deck
class Participant
{
    public string Name { get; set; }
    public Deck hand = new Deck();
    public int Score { get { return hand.SumOfCards(); } }

    public string HiddenCard { get { return hand.HiddenCard; } }
    public Participant(string name)
    {
        Name = name;
    }
    public void ReceiveCard(string card)
    {
        hand.Add(card);
    }

    public void PrintCard()
    {
        hand.PrintCard();
    }
    public void PrintAllCards()
    {
        hand.PrintAllCards();
    }
    public void PrintSum()
    {
        Console.WriteLine($"{Name} 점수: {Score}");
    }

}