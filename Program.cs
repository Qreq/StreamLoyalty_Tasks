using System;
using pokerhands.functionality;

namespace pokerhands
{
    class Program
    {
        static void Main(string[] args)
        {
            PokerHand royalFlush = new PokerHand("QD TD AD KD JD");
            PokerHand straightFlush = new PokerHand("4D 5D 6D 7D 8D");
            PokerHand Kind4 = new PokerHand("4D 4H 4C 4S 8D");
            PokerHand strongerKind4 = new PokerHand("AC AS AD AH 9D");
            PokerHand fullHouse = new PokerHand("4D 4H 4C 5H 5S");
            PokerHand strongerFullHouse = new PokerHand("6H 6C 6S TH TS");
            PokerHand flush = new PokerHand("4H 7H 5H 8H TH");
            PokerHand straight = new PokerHand("5C 6H 7S 8D 9C");
            PokerHand Kind3 = new PokerHand("4D 4H 4S 8S 9C");
            PokerHand twoPair = new PokerHand("4D 4S 6H 6S 8C");
            PokerHand pair = new PokerHand("4H 2C 8D 9H 8H");
            PokerHand High9 = new PokerHand ("4H 5C 2D 7D 9H");
            PokerHand HighAce = new PokerHand("6H AS QD 9H 2C");
            


            // same values bar suit for real match tie
            PokerHand sameTwoPair = new PokerHand("4H 4C 8S 6D 6C");
            PokerHand sameHighAce = new PokerHand("2D 6H 4S TC AD");

            // win/lose situations
            Console.WriteLine(royalFlush.CompareWith(twoPair));
            Console.WriteLine(Kind3.CompareWith(fullHouse));

            //same ranking, but win/lose situation still occurs due to value ranking
            Console.WriteLine(fullHouse.CompareWith(strongerFullHouse));
            Console.WriteLine(strongerKind4.CompareWith(Kind4));

            //tie situations
            Console.WriteLine(HighAce.CompareWith(sameHighAce));
            Console.WriteLine(twoPair.CompareWith(sameTwoPair));
        }
    }
}
