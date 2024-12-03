using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using NAudio.Wave;

namespace piano
{
    public partial class Form1 : Form
    {
        private SoundManager _soundManager;
        private Recorder _recorder;

        public Form1()
        {
            InitializeComponent();
            this.KeyPreview = true;

            _soundManager = new SoundManager();
            _recorder = new Recorder();

            CreateMenu();
            CreatePianoKeys();
        }

        private void CreateMenu() // создание формы
        {
            var menu = new MenuStrip();
            var fileMenu = new ToolStripMenuItem("File");
            fileMenu.DropDownItems.Add(new ToolStripMenuItem("Запись", null, RecordMenu_Click));
            fileMenu.DropDownItems.Add(new ToolStripMenuItem("Сыграть мелодию", null, PlayMenu_Click));
            fileMenu.DropDownItems.Add(new ToolStripMenuItem("Выйти", null, (s, e) => Close()));

            menu.Items.Add(fileMenu);
            MainMenuStrip = menu;
            Controls.Add(menu);
        }

        private void CreatePianoKeys() // создание клавиш пианино
        {
            var notes = _soundManager.GetNotes();
            int buttonWidth = 80, buttonHeight = 200;

            for (int i = 0; i < notes.Length; i++)
            {
                var button = new Button
                {
                    Text = notes[i],
                    Width = buttonWidth,
                    Height = buttonHeight,
                    Location = new System.Drawing.Point(10 + i * (buttonWidth + 10), 50),
                    Tag = notes[i]
                };
                button.MouseDown += (s, e) => _soundManager.PlaySound((string)button.Tag); //при нажатии мышью на клавиши пианино - проигрывается звук
                button.MouseUp += (s, e) => _soundManager.StopSound((string)button.Tag); // при отпускании мыши с клавиши пианино - звук прекращает воспроизводиться
                Controls.Add(button);
            }
        }

        protected override void OnKeyDown(KeyEventArgs e) // метод для обработки нажатия клавиш на клавиатуре
        {
            try
            {
                base.OnKeyDown(e);
                string note = Utilities.GetNoteFromKey(e.KeyCode);
                if (!string.IsNullOrEmpty(note))
                {
                    _soundManager.PlaySound(note); // когда клавиша нажата - воспроизводится звук
                }
            }
            catch (Exception ex) 
            {
                Log.LogException(ex); // лог исключения
                MessageBox.Show("Произошла ошибка. Проверьте файл логов.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void OnKeyUp(KeyEventArgs e) // метод для обработки отпускания клавиши на клавиатуре
        {
            try
            {
                base.OnKeyUp(e);
                string note = Utilities.GetNoteFromKey(e.KeyCode);
                if (!string.IsNullOrEmpty(note))
                {
                    _soundManager.StopSound(note); // прекратить воспроизведение звук при отпускании клавиши
                }
            }
            catch (Exception ex)  // лог исключения
            {
                Log.LogException(ex);
                MessageBox.Show("Произошла ошибка. Проверьте файл логов.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }   
        }
        
        private void RecordMenu_Click(object sender, EventArgs e) //событие для нажатия на "запись"
        {
            try
            {
                if (_recorder.IsRecording)
                {
                    _recorder.StopRecording(); // если уже идёт запись, то закончить её
                }
                else
                {
                    _recorder.StartRecording(); // если запись не идёт, то начать её
                }
            }
            catch (Exception ex)
            {
                Log.LogException(ex);
            }
        }

        private void PlayMenu_Click(object sender, EventArgs e) //событие для нажатия на "сыграть мелодию"
        {
            try 
            {
                _recorder.PlayRecording(); //вызов метода PlayRecording
            }
            catch (Exception ex)
            {
                Log.LogException(ex); // лог исключения, если оно выйдет
                MessageBox.Show("Произошла ошибка. Проверьте файл логов.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
    }
}
