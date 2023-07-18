using System.Collections.Generic;

namespace Tetris
{
    public abstract class Block
    {
        /// <summary>
        /// Todas as posições rotacionadas possíveis para o bloco
        /// </summary>
        protected abstract Position[][] Tiles { get; }
        /// <summary>
        /// Offset de início (posição inicial)
        /// </summary>
        protected abstract Position StartOffset { get; }
        /// <summary>
        /// Identifica o Id único para distinção do bloco
        /// </summary>
        public abstract int Id { get; }

        private int rotationState;
        private readonly Position offset;

        /// <summary>
        /// Construtor
        /// </summary>

        public Block()
        {
            offset = new Position(StartOffset.Row, StartOffset.Column);
        }

        #region Methodes

        /// <summary>
        /// Retorna a posição atual do objeto
        /// </summary>
        /// <returns></returns>

        public IEnumerable<Position> TilePositions()
        {
            foreach (Position p in Tiles[rotationState])
            {
                yield return new Position(p.Row + offset.Row, p.Column + offset.Column);
            }
        }

        /// <summary>
        /// Rotaciona o bloco no sentido horário
        /// </summary>

        public void RotateCW()
        {
            rotationState = (rotationState + 1) % Tiles.Length;
        }

        /// <summary>
        /// Rotaciona o bloco no sentido anti-horário
        /// </summary>

        public void RotateCCW()
        {
            if (rotationState == 0)
                rotationState = Tiles.Length - 1;
            else
                rotationState--;
        }

        /// <summary>
        /// Move o bloco para um novo offset
        /// </summary>
        /// <param name="r">Incremento em Row</param>
        /// <param name="c">Incremento em Column</param>

        public void Move(int r, int c)
        {
            offset.Row += r;
            offset.Column += c;
        }

        /// <summary>
        /// Define o offset inicial como a posição atual do block e zera sua rotação
        /// </summary>

        public void Reset()
        {
            rotationState = 0;
            offset.Row = StartOffset.Row;
            offset.Column = StartOffset.Column;
        }

        #endregion
    }
}
