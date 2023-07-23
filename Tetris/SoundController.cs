using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Media;
using System.Threading.Tasks;

namespace Tetris
{
    /// <summary>
    /// Controla os sons do jogo
    /// </summary>

    public class SoundController
    {
        private readonly Dictionary<GameSounds, string> Sounds = new()
        {
            { GameSounds.MainTheme, $"{AppDomain.CurrentDomain.BaseDirectory}\\Assets\\game_theme.wav"},
            { GameSounds.PointWin,  $"{AppDomain.CurrentDomain.BaseDirectory}\\Assets\\point_win.wav"},
            { GameSounds.GameOver,  $"{AppDomain.CurrentDomain.BaseDirectory}\\Assets\\gameover.wav"}
        };

        /// <summary>
        /// Obtém um som para o jogo
        /// </summary>
        /// <param name="Sound">Enum de identidicação do som</param>
        /// <returns></returns>

        public string this[GameSounds Sound]
        {
            get => Sounds[Sound];
        }

        [AllowNull]
        private SoundPlayer soundPlayer;
        public bool isSoundPlaying = false;

        /// <summary>
        /// Reproduz um som de forma assíncrona
        /// </summary>
        /// <param name="soundFileName">Local do arquivo de recurso de áudio</param>
        /// <param name="Loop">Quando <b>true</b>, o audio ficará em loop</param>
        /// <returns></returns>

        public void PlaySoundAsync(string soundFileName, bool Loop = false)
        {
            if (isSoundPlaying)
                StopSound();

            if (!File.Exists(soundFileName))
                return;

            soundPlayer = new SoundPlayer(soundFileName);
            if (Loop)
                soundPlayer.PlayLooping();
            else
                soundPlayer.Play();

            isSoundPlaying = true;
            Task.Run(() =>
            {
                while (isSoundPlaying)
                {
                    Task.Delay(100).Wait();
                }

                isSoundPlaying = false;
            });
        }

        /// <summary>
        /// Reproduz uma cadeia de sons de forma assíncrona
        /// </summary>
        /// <param name="Sounds">Todos os recursos de áudio em ordem</param>
        /// <param name="LoopAtLast">Quando <b>true</b>, o último recursos entrará em loop</param>

        public async void PlaySoundAsync(string[] Sounds, bool LoopAtLast = false)
        {
            PauseSound();
            for (int i = 0; i < Sounds.Length; i++)
            {
                SoundPlayer player = new(Sounds[i]);
                player.Load();
                player.Play();

                bool soundFinished = true;
                if (soundFinished && i != Sounds.Length - 1)
                {
                    soundFinished = false;
                    await Task.Factory.StartNew(() => { player.PlaySync(); soundFinished = true; });
                }
                else if (soundFinished && i == Sounds.Length - 1)
                {
                    PlaySoundAsync(Sounds[i], LoopAtLast);
                }
            }
        }

        /// <summary>
        /// Pausa a reprodução atual
        /// </summary>

        public void PauseSound()
        {
            if (isSoundPlaying && soundPlayer != null)
            {
                soundPlayer.Stop();
            }
        }

        /// <summary>
        /// Substitui o arquivo de áudio atual
        /// </summary>
        /// <param name="newSoundFileName">Novo recurso de áudio</param>
        /// <param name="loop">Quando <b>true</b>, o novo recurso de áudio ficará em loop</param>

        public void ReplaceSound(string newSoundFileName, bool loop = false)
        {
            PauseSound();
            PlaySoundAsync(newSoundFileName, loop);
        }

        /// <summary>
        /// Interrompe a música atual
        /// </summary>

        public void StopSound()
        {
            if (isSoundPlaying && soundPlayer != null)
            {
                soundPlayer.Stop();
                soundPlayer.Dispose();
                isSoundPlaying = false;
            }
        }
    }

    /// <summary>
    /// Recursos de áudio
    /// </summary>

    [Flags]
    public enum GameSounds
    {
        /// <summary>
        /// Áudio principal do game
        /// </summary>
        MainTheme,
        /// <summary>
        /// Áudio sugerido para ganho de pontos
        /// </summary>
        PointWin,
        /// <summary>
        /// Áudio sugerido para o GameOver
        /// </summary>
        GameOver
    }
}
