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
 * Примечание 20.01.24: согласно заданию на клетке показывается не число мин вокруг кнопки, а общее число мин
 * Примечание 23.01.24: переработан алгоритм. При нажатии на кнопку без мины показывается число мин вокруг 
 */

namespace PMD_PR_1._3
{
    public partial class Form1 : Form
    {
        Button[,] field; // матрица кнопок минного поля
        Button btnNewGame; // кнопка начала новой игры
        int fieldSize = 2; // размерность поля (для масштабируемости игры), РЕКОМЕНДОВАННЫЕ ЗНАЧЕНИЯ от 2 до 6
        int[,] mines; // матрица мин
        int[,] minesExpanded; // расширеннная матрица мин (для упрощения подсчета мин вокруг клетки)

        public Form1()
        {
            InitializeComponent();
            start();
        }

        private void start()
        {
            // Метод инициализирует элементы формы, добавляет их на форму и привязывает методы обработки событий
            this.Size = new Size(600, 320);

            mines = new int[fieldSize, fieldSize];
            minesExpanded = new int[fieldSize + 2, fieldSize + 2];

            field = new Button[fieldSize, fieldSize];
            for (int i = 0; i < fieldSize; i++)
                for (int j = 0; j < fieldSize; j++)
                {
                    field[i, j] = new Button();
                    field[i, j].Size = new Size(240 / fieldSize, 240 / fieldSize);
                    field[i, j].Location = new Point(10 + i * (field[i,j].Size.Width + 5), 10 + j * (field[i, j].Size.Height + 5));
                    field[i, j].Click += mineClickEvent;
                    this.Controls.Add(field[i, j]);

                }
            btnNewGame = new Button();
            btnNewGame.Text = "Начать заново";
            btnNewGame.Size = new Size(200, 250);
            btnNewGame.Location = new Point(280, 10);
            btnNewGame.Click += newGameClickEvent;
            this.Controls.Add(btnNewGame);

            shuffle();
        }

        private int getMinesAround(int x, int y)
        {
            // Метод возвращает -1, в матрице под заданными индексами находится мина, в остальных случаях возвращает число мин вокруг данного элемента матрицы
            if (mines[x, y] == -1)
                return -1;
            int counter = 0;
            for (int i = x; i < x + 3; i++)
                for (int j = y; j < y + 3; j++)
                    if (minesExpanded[i, j] == -1)
                        counter++;
            return counter;
        }

        private void mineClickEvent(object sender, EventArgs e)
        {
            // Метод обработчика события нажатия на кнопку. 
            Button b = (Button)sender;
            int i = (b.Location.X - 10) / (field[0, 0].Size.Width + 5); // пока вариантов определения индекса кнопки лучше не нашел, возможно можно сделать изящнее
            int j = (b.Location.Y - 10) / (field[0, 0].Size.Height + 5); // пока вариантов определения индекса кнопки лучше не нашел, возможно можно сделать изящнее
            if (mines[i, j] == -1)
            {
                b.Text = "Мина!";
                this.BackColor = Color.Red;

                for (i = 0; i < fieldSize; i++)
                    for (j = 0; j < fieldSize; j++)
                        field[i, j].Enabled = false; // кажется, что при попадании на мину игра должна заканчиваться и кнопки должны блокироваться
            }
            else
                b.Text = getMinesAround(i, j).ToString();
            b.Enabled = false;
                
        }

        private void shuffle()
        {
            // Метод случайно расставляет мины по массиву 3х3. Вероятность появления мины в клетке - 50 %
            //minesCounter = 0;
            Random r = new Random();
            for (int i = 0; i < fieldSize; i++)
                for (int j = 0; j < fieldSize; j++)
                {
                    mines[i, j] = - r.Next(1, 100) % 2;
                    minesExpanded[i + 1, j + 1] = mines[i, j];
                    //minesCounter += mines[i, j]; // считаем число мин
                }
        }

        private void newGameClickEvent(object sender, EventArgs e)
        {
            // Метод начинает новую игру, возвращает заливку поля, разблокирует кнопки, удаляет текст, генерирует новое минное поле
            this.BackColor = Color.White;
            shuffle();
            for (int i = 0; i < fieldSize; i++)
                for (int j = 0; j < fieldSize; j++)
                {
                    field[i, j].Enabled = true;
                    field[i, j].Text = "";
                }
        }
    }
}
