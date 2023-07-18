using System;
using Tetris.Blocks;

namespace Tetris
{
    /// <summary>
    /// Controla os blocos para as chamadas
    /// </summary>

    public class BlockQueue
    {
        private readonly Block[] Blocks = new Block[]
        {
            new IBlock(),
            new JBlock(),
            new LBlock(),
            new OBlock(),
            new SBlock(),
            new TBlock(),
            new ZBlock()
        };

        private readonly Random random = new();

        /// <summary>
        /// Obtém o próximo bloco da chamada
        /// </summary>
        public Block NextBlock { get; private set; }

        /// <summary>
        /// Construtor
        /// </summary>

        public BlockQueue()
        {
            NextBlock = RandomBlock();
        }

        #region Methodes

        private Block RandomBlock()
        {
            return Blocks[random.Next(Blocks.Length)];
        }

        /// <summary>
        /// Retorna um novo bloco para o jogo e atualiza a variável <b>NextBlock</b> para um outro bloco
        /// </summary>
        /// <returns></returns>

        public Block GetAndUpdate()
        {
            Block block = NextBlock;

            do
            {
                NextBlock = RandomBlock();
            } while (block.Id == NextBlock.Id);

            return block;
        }

        #endregion
    }
}
