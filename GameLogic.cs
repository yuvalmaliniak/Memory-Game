using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Ex02
{
    internal class GameLogic<T>
    {
        private Board<T> m_Board;
        private Player[] m_Players;
        private bool m_IsGameOver;
        private int m_PlayingPlayer; // Index - 0 or 1
        private List<(int,int)> m_SeenMovesForAI;
        public GameLogic(Board<T> i_Board, Player[] i_players)
        {
            m_Players = i_players;
            m_Board = i_Board;
            m_IsGameOver = false;
            m_PlayingPlayer = 0;
            m_SeenMovesForAI = new List<(int, int)>();
        }
        public bool isGameOver
        {
            get { return m_IsGameOver; }
            set { m_IsGameOver = value; }
        }
        public Player[] GetPlayers
        {
            get { return m_Players; }
        }
        public int PlayingPlayer
        {
            get { return m_PlayingPlayer; }
            set { m_PlayingPlayer = value; }
        }
        public void DrawBoard() => m_Board.Draw();
        public Player GetCurrentPlayer()
        {
            return m_Players[PlayingPlayer];
        }
        public Board<T> Board
        {
            get { return m_Board; }
            set { m_Board = value; }
        }
        public bool IsCardRevealed(int i_row, int i_column)
        {
            return m_Board.GetCard(i_row, i_column).IsRevealed;
        }

        public (int x, int y) GetRandomMove()
        {
            Random random = new Random();
            int rowInt = random.Next(1, m_Board.Rows + 1);
            char rowChar = (char)('A' + rowInt - 1);  // Convert random integer to character

            int col = random.Next(1, m_Board.Cols + 1);

            while (IsCardRevealed(rowInt, col))
            {
                rowInt = random.Next(1, m_Board.Rows + 1);
                rowChar = (char)('A' + rowInt - 1);
                col = random.Next(1, m_Board.Cols + 1);
            }
            return (rowChar, col - 1);
        }
        public string GetAsStringRandomMove()
        {
            Random generateIndex = new Random();
            int rowSize = m_Board.Rows;
            int columnSize = m_Board.Cols;
            int chosenRow = generateIndex.Next(rowSize);
            int chosenColumn = generateIndex.Next(columnSize);

            while (IsCardRevealed(chosenRow, chosenColumn))
            {
                chosenRow = generateIndex.Next(rowSize);
                chosenColumn = generateIndex.Next(columnSize);
            }
            StringBuilder randomMove = new StringBuilder();
            char selectedColumn = (char)('A' + chosenColumn);
            char selectedRow = (char)(chosenRow + '1');
            randomMove.Append(selectedColumn);
            randomMove.Append(selectedRow);
            return randomMove.ToString();
        }
        public bool CompleteTurn(int i_firstRow, int i_firstCol, int i_secondRow, int i_secondCol)
        {
            bool isEqual = m_Board.IsAMatch(i_firstRow, i_firstCol, i_secondRow, i_secondCol);
            if (isEqual)
            {
                m_Players[m_PlayingPlayer].AddPoints();
            }
            return isEqual;
        }
        public void FlipNotEqual(int i_firstRow, int i_firstCol, int i_secondRow, int i_secondCol)
        {
            m_Board.UnRevealCell(i_firstRow, i_firstCol);
            m_Board.UnRevealCell(i_secondRow, i_secondCol);
        }
    }
}