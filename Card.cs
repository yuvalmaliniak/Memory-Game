using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex02
{
    internal class Card<T>
    {
        private T m_CardValue;
        private bool m_IsRevealed;
        public Card(T i_cardValue)
        {
            m_CardValue = i_cardValue;
            m_IsRevealed = false;
        }
        public bool IsRevealed
        {
            get { return m_IsRevealed; }
            set { m_IsRevealed = value; }
        }
        public T CardValue
        {
            get { return m_CardValue; }
        }
        public bool Equals(Card<T> otherCard)
        {
            return otherCard != null && EqualityComparer<T>.Default.Equals(m_CardValue, otherCard.m_CardValue);
        }
        public override string ToString()
        {
            return m_CardValue?.ToString();
        }
    }
}
