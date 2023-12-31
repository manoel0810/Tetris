﻿using System.Diagnostics.CodeAnalysis;

namespace Tetris
{
    public class GameState
    {
        public Block CurrentBlock
        {
            get => currentBlock;
            private set
            {
                currentBlock = value;
                currentBlock.Reset();
            }
        }

        [AllowNull]
        public Block HeldBlock { get; private set; }

        [AllowNull]
        private Block currentBlock;

        public GameGrid GameGrid { get; }
        public BlockQueue BlockQueue { get; }
        public SoundController SoundController { get; }
        public bool GameOver { get; private set; }
        public int Score { get; private set; }
        public bool CanHold { get; private set; }

        public GameState()
        {
            GameGrid = new GameGrid(22, 10);
            BlockQueue = new BlockQueue();
            SoundController = new SoundController();
            CurrentBlock = BlockQueue.GetAndUpdate();
        }

        public void HoldBlock()
        {
            if (!CanHold)
                return;

            if (HeldBlock == null)
            {
                HeldBlock = CurrentBlock;
                CurrentBlock = BlockQueue.GetAndUpdate();
            }
            else
                (HeldBlock, CurrentBlock) = (CurrentBlock, HeldBlock);


            CanHold = false;
        }

        private bool BlockFits()
        {
            foreach (Position p in CurrentBlock.TilePositions())
            {
                if (!GameGrid.IsEmptyCell(p.Row, p.Column))
                    return false;
            }

            return true;
        }

        public void RotateBlockCW()
        {
            CurrentBlock.RotateCW();
            if (!BlockFits())
                CurrentBlock.RotateCCW();
        }

        public void RotateBlockCCW()
        {
            CurrentBlock.RotateCCW();
            if (!BlockFits())
                CurrentBlock.RotateCW();
        }

        public void MoveBlockLeft()
        {
            CurrentBlock.Move(0, -1);
            if (!BlockFits())
                CurrentBlock.Move(0, 1);
        }

        public void MoveBlockRight()
        {
            CurrentBlock.Move(0, 1);
            if (!BlockFits())
                CurrentBlock.Move(0, -1);
        }

        private bool IsGameOver()
        {
            return !(GameGrid.IsRowEmpty(0) && GameGrid.IsRowEmpty(1));
        }

        private void PlaceBlock()
        {
            foreach (Position p in currentBlock.TilePositions())
            {
                GameGrid[p.Row, p.Column] = CurrentBlock.Id;
            }

            int CurrentScore = Score;
            Score += GameGrid.ClearFullRows();
            if (Score > CurrentScore)
                SoundController.PlaySoundAsync(new string[] { SoundController[GameSounds.PointWin], SoundController[GameSounds.MainTheme] }, true);

            if (IsGameOver())
                GameOver = true;
            else
                CurrentBlock = BlockQueue.GetAndUpdate();
        }

        /// <summary>
        /// Move o bloco para baixo
        /// </summary>

        public void MoveBlockDown()
        {
            CurrentBlock.Move(1, 0);
            if (!BlockFits())
            {
                CurrentBlock.Move(-1, 0);
                PlaceBlock();
            }
        }

        private int TileDropDistance(Position p)
        {
            int drop = 0;

            while (GameGrid.IsEmptyCell(p.Row + drop + 1, p.Column))
            {
                drop++;
            }

            return drop;
        }

        public int BlockDropDistance()
        {
            int drop = GameGrid.Rows;

            foreach (Position p in CurrentBlock.TilePositions())
            {
                drop = System.Math.Min(drop, TileDropDistance(p));
            }

            return drop;
        }

        public void DropBlock()
        {
            CurrentBlock.Move(BlockDropDistance(), 0);
            PlaceBlock();
        }
    }
}
