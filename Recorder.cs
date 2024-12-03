using System;
using System.IO;
using System.Windows.Forms;
using NAudio.Wave;

namespace piano
{
    public class Recorder
    {
        private WaveFileWriter _waveFileWriter;
        private WasapiLoopbackCapture _loopbackCapture;
        public bool IsRecording { get; private set; }

        public void StartRecording() //метод для начала записи
        {
            try
            {
                IsRecording = true; //статус записи - активный
                _loopbackCapture = new WasapiLoopbackCapture(); // инициализация захвата звука с системного выхода
                string filePath = $"recording_{DateTime.Now:yyyyMMdd_HHmmss}.wav"; //создается новый wav файл с названием нынешней даты и времени
                _waveFileWriter = new WaveFileWriter(filePath, _loopbackCapture.WaveFormat); // инициализация записи звука в wav файл с использованием формата захвата

                _loopbackCapture.DataAvailable += (s, args) => // обработчик данных, доступных для записи
                {
                    _waveFileWriter.Write(args.Buffer, 0, args.BytesRecorded); //записываются буферные данные в wav файл
                };

                _loopbackCapture.RecordingStopped += (s, args) => // обработчик завершения записи
                {
                    _waveFileWriter?.Dispose(); // закрытие файлового потока
                    _loopbackCapture?.Dispose();
                };

                _loopbackCapture.StartRecording(); //начало записи
                MessageBox.Show("Запись начата!");
            }
            catch (Exception ex)
            {
                Log.LogException(ex); //лог исключения
                MessageBox.Show("Произошла ошибка. Проверьте файл логов.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void StopRecording() //метод для завершения записи
        {
            IsRecording = false; //статус записи - не активный
            _loopbackCapture?.StopRecording(); //конец записи
            MessageBox.Show("Запись завершена!");
        }

        public void PlayRecording() //метод для воспроизведения записи
        {
            try
            {
                var openFileDialog = new OpenFileDialog //открытие диалогового окна для выбора wav файла
                {
                    Filter = "Wave Files (*.wav)|*.wav",
                    Title = "Выберите записанный файл"
                };

                if (openFileDialog.ShowDialog() == DialogResult.OK) // если запись выбрана
                {
                    using var reader = new AudioFileReader(openFileDialog.FileName); // создается объект для чтения выбранного аудиофайла
                    using var player = new WaveOutEvent(); // и для воспроизведения звука
                    player.Init(reader); // инициализируется проигрыватель с использованием прочитанного файла
                    player.Play(); // и начинается воспроизведение
                    while (player.PlaybackState == PlaybackState.Playing)
                    {
                        System.Threading.Thread.Sleep(100); // временная пауза для предотвращения чрезмерной загрузки процессора
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogException(ex); // лог исключения
                MessageBox.Show("Произошла ошибка во время воспроизведения записи. Проверьте файл логов для подробностей.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
