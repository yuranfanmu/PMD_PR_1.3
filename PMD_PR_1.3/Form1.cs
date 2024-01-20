using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/*
 * ЗАДАНИЕ:
 * Реализуйте игру минер на поле 3×3 из кнопок (Button). Первоначально все кнопки не
 * содержат надписей. При попытке нажатия на кнопку на ней либо показывается количество
 * мин, либо надпись «мина!» и меняется цвет окна. Разместите кнопку «Начать заново»
 * возвращающую начальный вид окна.
 * 
 * Примечание: согласно заданию на клетке показывается не число мин вокруг кнопки, а общее число мин
 */

namespace PMD_PR_1._3
{
    public partial class Form1 : Form
    {
        Button[,] field; // матрица кнопок минного поля
        Button btnNewGame; // кнопка начала новой игры
        int[,] mines; // матрица мин
        int minesCounter; // счетчик общего числа мин

        public Form1()
        {
            InitializeComponent();
            start();
        }

        private void start()
        {
            // Метод инициализирует элементы формы, добавляет их на форму и привязывает методы обработки событий
            this.Size = new Size(600, 300);

            mines = new int[3, 3];

            field = new Button[3, 3];
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                {
                    field[i, j] = new Button();
                    field[i, j].Size = new Size(60, 60);
                    field[i, j].Location = new Point(10 + i * 65, 10 + j * 65);
                    field[i, j].Click += mineClickEvent;
                    this.Controls.Add(field[i, j]);

                }
            btnNewGame = new Button();
            btnNewGame.Text = "Начать заново";
            btnNewGame.Size = new Size(200, 190);
            btnNewGame.Location = new Point(220, 10);
            btnNewGame.Click += newGameClickEvent;
            this.Controls.Add(btnNewGame);

            shuffle();
        }

        private void mineClickEvent(object sender, EventArgs e)
        {
            // Метод обработчика события нажатия на кнопку. 
            Button b = (Button)sender;
            int i = (b.Location.X - 10) / 65; // пока вариантов определения индекса кнопки лучше не нашел, возможно можно сделать изящнее
            int j = (b.Location.Y - 10) / 65; // пока вариантов определения индекса кнопки лучше не нашел, возможно можно сделать изящнее
            if (mines[i, j] == 1)
            {
                b.Text = "Мина!";
                this.BackColor = Color.Red;

                for (i = 0; i < 3; i++)
                    for (j = 0; j < 3; j++)
                        field[i, j].Enabled = false; // кажется, что при попадании на мину игра должна заканчиваться и кнопки должны блокироваться
            }
            else
                b.Text = minesCounter.ToString();
            b.Enabled = false;
                
        }

        private void shuffle()
        {
            // Метод случайно расставляет мины по массиву 3х3. Вероятность появления мины в клетке - 50 %
            minesCounter = 0;
            Random r = new Random();
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                {
                    mines[i, j] = r.Next(1, 100) % 2;
                    minesCounter += mines[i, j]; // считаем число мин
                }
        }

        private void newGameClickEvent(object sender, EventArgs e)
        {
            // Метод начинает новую игру, возвращает заливку поля, разблокирует кнопки, удаляет текст, генерирует новое минное поле
            this.BackColor = Color.White;
            shuffle();
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                {
                    field[i, j].Enabled = true;
                    field[i, j].Text = "";
                }
        }
    }
}
