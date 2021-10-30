using System;


namespace ConsoleClient.CardGame.Cards.Primitives
{
    public class HpChangeEventArgs : EventArgs
    {
        public HpChangeEventArgs(int value)
        {
            Value = value;
        }


        public HpChangeEventArgs(object sender, int value)
        {
            Sender = sender;
            Value = value;
        }


        public object Sender { get; set; }

        public int Value { get; set; }
    }
}
