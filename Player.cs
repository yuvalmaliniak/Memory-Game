using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex02
{
    internal class Player
    {
        private string m_PlayerName;
        private readonly bool m_IsHuman;
        private int m_UserPoints;
        public Player(string i_playerName, bool i_IsHuman)
        {
            m_PlayerName = i_playerName;
            m_IsHuman = i_IsHuman;
            m_UserPoints = 0;
        }
        public int UserPoints
        {
            get
            {
                return m_UserPoints;
            }
            set
            {
                m_UserPoints = value;
            }
        }
        public string PlayerName
        {
            get { return m_PlayerName; }
        }
        public void AddPoints()
        {
            m_UserPoints += 1;
        }
        public bool IsHuman
        {
            get { return m_IsHuman; }
        }       
    }
}