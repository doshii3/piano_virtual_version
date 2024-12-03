namespace piano
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освобождение ресурсов.
        /// </summary>
        /// <param name="disposing">true, если управляемые ресурсы должны быть удалены.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Метод инициализации компонентов.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // Form1
            // 
            this.Size = new System.Drawing.Size(815, 300); // Устанавливаем размеры окна
            this.Name = "Form1";
            this.Text = "Виртуальное пианино"; // Устанавливаем заголовок окна
            this.ResumeLayout(false);
        }
    }
}
