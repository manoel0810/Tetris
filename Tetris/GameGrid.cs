using System;

namespace Tetris
{
    /// <summary>
    /// Grade base do jogo
    /// </summary>
    public class GameGrid
    {
        private readonly int[,] Grid;

        /// <summary>
        /// Número de linhas para a matriz
        /// </summary>
        public int Rows { get; }
        /// <summary>
        /// Número de colunas para a matriz
        /// </summary>
        public int Columns { get; }

        public int this[int r, int c]
        {
            get => Grid[r, c];
            set => Grid[r, c] = value;
        }

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="rows">Número de linhas</param>
        /// <param name="columns">Número de colunas</param>

        public GameGrid(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            Grid = new int[rows, columns];
        }

        #region Methodes

        /// <summary>
        /// Verifica se uma posição (x, y) está dentro dos limites da matriz
        /// </summary>
        /// <param name="r">Número da linha</param>
        /// <param name="c">Número da coluna</param>
        /// <returns><b>true</b> caso esteja dentro dos limites da matriz</returns>

        public bool IsInsideGrid(int r, int c)
        {
            return r >= 0 && r < Rows && c >= 0 && c < Columns;
        }

        /// <summary>
        /// Verifica se uma posição (x, y) dentro da matriz está vazia 
        /// </summary>
        /// <param name="r">Número da linha</param>
        /// <param name="c">Número da coluna</param>
        /// <returns><b>true</b> caso esteja vazia</returns>

        public bool IsEmptyCell(int r, int c)
        {
            return IsInsideGrid(r, c) && Grid[r, c] == 0;
        }

        /// <summary>
        /// Verifica se uma linha dentro da matriz está totalmente ocupada
        /// </summary>
        /// <param name="r">Número da linha</param>
        /// <returns><b>true</b> caso esteja totalmente preenchida</returns>

        public bool IsFullRow(int r)
        {
            if (r > (Rows - 1) || r < 0)
                throw new ArgumentOutOfRangeException(nameof(r), "Index fora dos limites da matriz");

            for (int c = 0; c < Columns; c++)
                if (Grid[r, c] == 0)
                    return false;

            return true;
        }

        /// <summary>
        /// Verifica se uma linha está completamente vazia
        /// </summary>
        /// <param name="r">Número da linha</param>
        /// <returns><b>true</b> caso esteja totalmente vazia</returns>

        public bool IsRowEmpty(int r)
        {
            if (r > (Rows - 1) || r < 0)
                throw new ArgumentOutOfRangeException(nameof(r), "Index fora dos limites da matriz");

            for (int c = 0; c < Columns; c++)
                if (Grid[r, c] != 0)
                    return false;

            return true;
        }

        private void ClearRow(int r)
        {
            for (int c = 0; c < Columns; c++)
                Grid[r, c] = 0;
        }

        private void MoveRowDown(int r, int numRows)
        {
            for (int c = 0; c < Columns; c++)
            {
                Grid[r + numRows, c] = Grid[r, c];
                Grid[r, c] = 0;
            }
        }

        /// <summary>
        /// Limpa todas as linhas totalmente preenchidas e move as adjacentes para baixo (caso existam)
        /// </summary>
        /// <returns>Número de linhas apagadas</returns>

        public int ClearFullRows()
        {
            int count = 0;
            for (int r = Rows - 1; r >= 0; r--)
            {
                if (IsFullRow(r))
                {
                    ClearRow(r);
                    count++;
                }
                else if (count > 0)
                    MoveRowDown(r, count);
            }

            return count;
        }

        #endregion
    }
}
