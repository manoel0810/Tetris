namespace Tetris
{
    /// <summary>
    /// Representa uma posição (x, y) == (r, c) dentro de uma matriz de ordem (R, C) | R >= (x - 1), C >= (y - 1)
    /// </summary>
    public class Position
    {
        /// <summary>
        /// Identifica a linha da posição
        /// </summary>
        public int Row { get; set; }
        /// <summary>
        /// Identifica a coluna da posição
        /// </summary>
        public int Column { get; set; }

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="row">Receba a linha para a posição</param>
        /// <param name="column">Recebe a coluna para a posição</param>

        public Position(int row, int column)
        {
            Row = row;
            Column = column;
        }
    }
}
