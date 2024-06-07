using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ex02.ConsoleUtils;

namespace Ex02
{
    internal class Board<T>
    {
        private Card<T>[,] m_Board;
        public Board(int i_rows, int i_cols, T[] cardValue)
        {
            m_Board = new Card<T>[i_rows, i_cols];
            initializeBoard(cardValue);
        }
        public int Rows => m_Board.GetLength(0);
        public int Cols => m_Board.GetLength(1);
        private void initializeBoard(T[] cardValues)
        {
            Random random = new Random();
            int pairsCount = m_Board.GetLength(0) * m_Board.GetLength(1) / 2;
            List<T> cards = new List<T>();         
            List<T> availableValues = new List<T>(cardValues);
            for (int i = 0; i < pairsCount; i++)
            {
                int index = random.Next(availableValues.Count);
                T selectedValue = availableValues[index];
                cards.Add(selectedValue);
                cards.Add(selectedValue);
                availableValues.RemoveAt(index);
            }
            // Shuffle the cards
            cards = cards.OrderBy(x => random.Next()).ToList();
            // Place the shuffled cards on the board
            int cardIndex = 0;
            for (int row = 0; row < m_Board.GetLength(0); row++)
            {
                for (int col = 0; col < m_Board.GetLength(1); col++)
                {
                    m_Board[row, col] = new Card<T>(cards[cardIndex]);
                    cardIndex++;
                }
            }
        }
        public Card<T> GetCard(int i_row, int i_col)
        {
            return m_Board[i_row, i_col];
        }
        public void Draw()
        {
            int numCols = m_Board.GetLength(1);
            int numRows = m_Board.GetLength(0);
            char letter = 'A';
            StringBuilder lineToPrint = new StringBuilder();
            Screen.Clear();
            lineToPrint.Append("  ");
            for (int i = 0; i < numCols; i++)
            {
                lineToPrint.Append(string.Format("  {0} ", letter));
                letter = (char)(letter + 1);
            }
            Console.WriteLine(lineToPrint.ToString());
            printBorderLine();
            for (int row = 0; row < numRows; row++)
            {
                lineToPrint.Clear();
                lineToPrint.Append(string.Format("{0} ", row + 1));
                for (int column = 0; column < numCols; column++)
                {
                    if (m_Board[row, column].IsRevealed)
                    {
                        lineToPrint.Append(string.Format("| {0} ", m_Board[row, column].ToString()));
                    }
                    else
                    {
                        lineToPrint.Append("|   ");
                    }
                }
                lineToPrint.Append("|");
                Console.WriteLine(lineToPrint.ToString());
                printBorderLine();
            }
            Console.WriteLine();
        }
        private void printBorderLine()
        {
            StringBuilder linePrinter = new StringBuilder();
            int numCols = m_Board.GetLength(1);
            linePrinter.Append("  ");
            for (int lineBorder = 0; lineBorder < numCols; lineBorder++)
            {
                linePrinter.Append("====");
            }
            linePrinter.Append("=");
            Console.WriteLine(linePrinter);
        }
        public bool CheckIfAllRevealed()
        {
            bool allRevealed = true;
            foreach (Card<T> card in m_Board)
            {
                if (!card.IsRevealed)
                {
                    allRevealed = false;
                    break;
                }
            }
            return allRevealed; // All cards are revealed
        }
        public void RevealCell(int row, int col)
        {
            GetCard(row, col).IsRevealed = true;
        }
        public void UnRevealCell(int row, int col)
        {
            GetCard(row, col).IsRevealed = false;
        }
        public bool IsAMatch(int first_row, int first_col, int second_row, int second_col)
        {
            bool isEqual = false;
            if (GetCard(first_row, first_col).Equals(GetCard(second_row, second_col)))
            {
                isEqual = true;
            }
            return isEqual;
        }
    }
}