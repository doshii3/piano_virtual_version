using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using NAudio.Wave;
using static piano.Form1;

namespace piano
{
    public class SoundManager
    {
        private readonly Dictionary<string, string> _soundFiles;
        private readonly Dictionary<string, WaveOutEvent> _loopingPlayers = new();

        public SoundManager()
        {
            _soundFiles = new Dictionary<string, string>
            {
                { "DO", "do.wav" }, // присвоение звука ноте до
                { "RE", "re.wav" }, // присвоение звука ноте ре
                { "MI", "mi.wav" }, // присвоение звука ноте ми
                { "FA", "fa.wav" }, // присвоение звука ноте фа
                { "SOL", "sol.wav" }, // присвоение звука ноте соль
                { "LA", "la.wav" }, // присвоение звука ноте ля
                { "SI", "si.wav" } // присвоение звука ноте си
            };
        }

        public string[] GetNotes() => _soundFiles.Keys.ToArray();
        private readonly HashSet<Keys> _heldKeys = new HashSet<Keys>(); //отслеживание нажатых нот
        public class LoopStream : WaveStream //класс для зацикливания звука - воспроизведения звука так долго, сколько необходимо
        {
            private readonly WaveStream _sourceStream;// исходный поток, который будет зацикливаться
            public LoopStream(WaveStream sourceStream) // конструктор принимает исходный поток в качестве аргумента
            {
                _sourceStream = sourceStream; // сохраняется ссылка на исходный поток
            }
            public override WaveFormat WaveFormat => _sourceStream.WaveFormat; // формат аудио совпадает с форматом исходного потока
            public override long Length => _sourceStream.Length; // также и длина потока совпадает с длиной исходного потока
            public override long Position // текущее положение в потоке
            {
                get => _sourceStream.Position;
                set => _sourceStream.Position = value;
            }
            public override int Read(byte[] buffer, int offset, int count) // метод чтения данных из потока
            {
                int totalBytesRead = 0;

                while (totalBytesRead < count)
                {
                    int bytesRead = _sourceStream.Read(buffer, offset + totalBytesRead, count - totalBytesRead);  // чтение из исходного потока
                    if (bytesRead == 0) // если достигнут конец исходного потока
                    {
                        _sourceStream.Position = 0; // перезапуск
                    }
                    totalBytesRead += bytesRead; // увеличивается общий счетчик прочитанных байт
                }
                return totalBytesRead;
            }
        }
        public void PlaySound(string note) //метод для проигрывания звука
        {
            try
            {
                if (!_soundFiles.ContainsKey(note)) // если нота не прописана
                    throw new ArgumentException($"Нота '{note}' отсутствует."); //выводится окно с исключением

                if (_loopingPlayers.ContainsKey(note)) // проверяется, воспроизводится ли эта нота уже в зацикленном режиме
                    return; // если да, то повторное воспроизведение не начинается

                var reader = new AudioFileReader(_soundFiles[note]); //новый объект для чтения аудиофайла
                var loop = new LoopStream(reader); // новой зацикленный поток
                var player = new WaveOutEvent(); // новый объект для воспроизведения звука
                player.Init(loop); //создается плеер с использованием зацикленного звука
                player.Play(); // воспроизводится звук

                _loopingPlayers[note] = player; // добавление плеера в словарь для управления зацикленным воспроизведением
            }
            catch (Exception ex) // лог исключения
            {
            Log.LogException(ex);
            }
        }

        public void StopSound(string note) // метод для остановки воспроизведения звука
        {
            try
            {
                if (_loopingPlayers.TryGetValue(note, out var player)) // проверяется, воспроизводится ли данная нота
                {
                    player.Stop(); //прекращение воспроизведения
                    player.Dispose(); // освобождаются ресурсы плеера
                    _loopingPlayers.Remove(note); // удаляется запись о плеере из словаря
                }
            }
            catch (Exception ex) //лог исключения
            {
                Log.LogException(ex);
            }
        }

        public Form1 Form1
        {
            get => default;
            set
            {
            }
        }
    }
}
