using System;
using System.Linq;

namespace pokerhands.functionality{
    public class PokerHand{
        public string[] thisHand;
        private HandRanking _ranking;
        public PokerHand(string cards){
            this.thisHand = cards.Split(' ');
            this.SortHand();
        }
        private int ValueToNumFaceCard(string value, int faceValue){
            switch(value){
                        case "T":
                            return (int) FaceCards.T;
                        case "J":
                            return (int) FaceCards.J;
                        case "Q":
                            return (int) FaceCards.Q;
                        case "K":
                            return (int) FaceCards.K;
                        case "A":
                            return (int) FaceCards.A;
                    }  
            return faceValue;
        }
        private void SortHand(){
            //first, make the hands sortable
            for (int i=0 ; i < this.thisHand.Length; i++){
                string oldVal = this.thisHand[i][0].ToString();
                //if value is 10 through Ace, replace with associated number
                if (!int.TryParse(oldVal, out int newVal)){
                    newVal = ValueToNumFaceCard(oldVal, newVal);

                    this.thisHand[i] = this.thisHand[i].Remove(0,1);
                    this.thisHand[i] = this.thisHand[i].Insert(0,newVal.ToString());
                }
            }
            //then we sort based on card values
            this.thisHand = this.thisHand.OrderBy(s => 
                //example substrings: 2H will return 2,  KC will have become 13C and will return 13
                int.Parse(s.Substring(0,s.Length - 1))).ToArray();
        }
        public Result CompareWith(PokerHand otherHand){

            //then determine rankings
            this._ranking = this.DetermineHandRanking();
            otherHand._ranking = otherHand.DetermineHandRanking();

            //decide result based on hand rankings
            if(this._ranking > otherHand._ranking) return Result.Win;
            if(this._ranking < otherHand._ranking) return Result.Loss;
            return DetermineTieBreaker(otherHand);
        }
        //edge case of exact same hands
        private Result DetermineSameHand(PokerHand otherHand){
            //5 because hand size
            for (int i = 5; i >= 0; i--){
                int myValue = int.Parse(this.thisHand[i].Substring(0,this.thisHand.Length - 1));
                int otherValue = int.Parse(otherHand.thisHand[i].Substring(0,this.thisHand.Length - 1));
                if(myValue > otherValue) return Result.Win;
                if (myValue < otherValue) return Result.Loss;
            }
            return Result.Tie;
        }
        // edge case where same ranking found.  Check who has highest card.  If same highest card, tie.
        private Result DetermineTieBreaker(PokerHand otherHand){
            if (this.DetemrineHighestCard() > otherHand.DetemrineHighestCard()) return Result.Win;
            if (this.DetemrineHighestCard() < otherHand.DetemrineHighestCard()) return Result.Loss;
            return Result.Tie;
        }

        private int DetemrineHighestCard(){
            //get final card's value from sorted list
            string highestCard = this.thisHand.ElementAt(this.thisHand.Length - 1);
            return int.Parse(highestCard.Substring(0,highestCard.Length - 1));
        }
        private HandRanking DetermineHandRanking(){
            if (checkRoyalFlush()) return HandRanking.RoyalFlush;
            if (checkStraightFlush()) return HandRanking.StraightFlush;
            if (checkKind(4)) return HandRanking.FourOfAKind;
            if (checkFullHouse()) return HandRanking.FullHouse;
            if (checkFlush()) return HandRanking.Flush;
            if (checkStraight()) return HandRanking.Straight;
            if (checkKind(3)) return HandRanking.ThreeOfAKind;
            if (checkTwoPair()) return HandRanking.TwoPairs;
            if (checkKind(2)) return HandRanking.Pair;
            return HandRanking.HighCard;
        }
        private bool checkRoyalFlush(){
            char initialSuit = this.thisHand[0][this.thisHand[0].Length - 1];
            for (int i = 0; i < this.thisHand.Length; i++){
                string currentCard = this.thisHand[i];
                string sub = currentCard.Substring(0,currentCard.Length - 1);
                int currentValue = int.Parse(currentCard.Substring(0,currentCard.Length - 1));
                char currentSuit = currentCard[currentCard.Length - 1];

                //if hand doesn't contain any value between 10 and 14(Ace), not a royal flush, return false
                if (!(currentValue >= 10)){
                        return false;
                }
                //if suit doesn't match next card's suit, not a royal flush, return false
                if (!currentSuit.Equals(initialSuit)) return false;
            }
            //card is royal flush if previous conditions met, return true
            return true;
        }
        private bool checkFlush(){
            char suit = this.thisHand[0][1];
            //any suit change means not a flush
            foreach(string card in this.thisHand){
                if (!card[1].Equals(suit)){
                    return false;
                }
            }
            return true;
        }

        private bool checkStraight(){
            int firstValue = int.Parse(this.thisHand[0].Substring(0,this.thisHand[0].Length - 1));
            for (int i = 1; i < this.thisHand.Length; i++){
                int currentValue = int.Parse(this.thisHand[i].Substring(0,this.thisHand[0].Length - 1));
                //if number falls out of +1 sequence
                //NOTE: in the notes of the task, it says Low Aces (e.g. 1) are not valid.  
                // because of this, they have not been implemented, and thus, if a hand has Ace, 2, 3, 4, 5,
                // it will not count as being a valid straight due to this constraint.
                if (!(currentValue - i == firstValue)){
                    return false;
                }
            }
            return true;
        }
        private bool checkStraightFlush(){
            if (checkStraight() && checkFlush()){
                return true;
            }
            return false;
        } 

        private bool checkKind(int numCardsNeeded){
            string[] valueArray = this.thisHand;
            for (int value = 2; value <= 14; value++){
                int numFound = Array.FindAll(this.thisHand, card => card.Substring(0,card.Length - 1).Equals(value.ToString())).Count();
                if (numFound == numCardsNeeded) return true;
            }
            
            return false;
        }
        private bool checkTwoPair(){
            bool firstPairFound = false;
            bool secondPairFound = false;

            for (int value = 2; value <= 14; value++){
                int numFound = Array.FindAll(this.thisHand, card => card.Substring(0,card.Length - 1).Equals(value.ToString())).Count();
                if (numFound == 2 && !firstPairFound) firstPairFound = true;
                else if (numFound == 2 && firstPairFound) secondPairFound = true; 
            }

            return firstPairFound && secondPairFound;
        }
        private bool checkFullHouse(){
            bool threeKindFound = false;
            bool pairFound = false;

            for (int value = 2; value <= 14; value++){
                int numFound = Array.FindAll(this.thisHand, card => card.Substring(0,card.Length - 1).Equals(value.ToString())).Count();
                if (numFound == 3) threeKindFound = true;
                if (numFound == 2) pairFound = true;
            }
            return (threeKindFound && pairFound);
        }
    }
}