using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Контр
{
    public partial class MainWindow : Window
    {
        private Button[,] buttons;
        private int[,] puzzle;
        private int emptyRow, emptyCol;
        private int size = 4;

        public MainWindow()
        {
            InitializeComponent();
            GeneratePuzzle();
            DrawPuzzle();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            int row = Grid.GetRow(button);
            int col = Grid.GetColumn(button);

            if (puzzle[row, col] == -1)
            {
                return;
            }

            if ((row == emptyRow && Math.Abs(col - emptyCol) == 1) ||
                (col == emptyCol && Math.Abs(row - emptyRow) == 1))
            {
                puzzle[emptyRow, emptyCol] = puzzle[row, col];
                puzzle[row, col] = -1;
                emptyRow = row;
                emptyCol = col;

                DrawPuzzle();

                if (CheckWin())
                {
                    MessageBox.Show("Вы выиграли!");
                }
            }
        }

        private void GeneratePuzzle()
        {
            puzzle = new int[size, size];
            int counter = 1;

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (counter == size * size)
                    {
                        puzzle[i, j] = -1;
                        emptyRow = i;
                        emptyCol = j;
                    }
                    else
                    {
                        puzzle[i, j] = counter;
                    }
                    counter++;
                }
            }
        }

        private void DrawPuzzle()
        {
            puzzleGrid.Children.Clear();
            puzzleGrid.ColumnDefinitions.Clear();
            puzzleGrid.RowDefinitions.Clear();

            buttons = new Button[size, size];

            for (int i = 0; i < size; i++)
            {
                puzzleGrid.ColumnDefinitions.Add(new ColumnDefinition());
                puzzleGrid.RowDefinitions.Add(new RowDefinition()); 
                for (int j = 0; j < size; j++)
                {
                    Button button = new Button();
                    if (puzzle[i, j] == -1)
                    {
                        button.Content = "";
                    }
                    else
                    {
                        button.Content = puzzle[i, j].ToString();
                    }
                    buttons[i, j] = button;

                    button.Click += Button_Click;

                    Grid.SetColumn(button, j);
                    Grid.SetRow(button, i);
                    puzzleGrid.Children.Add(button);
                }
            }
        }

        private bool CheckWin()
        {
            int counter = 1;

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (puzzle[i, j] != -1 && puzzle[i, j] != counter % (size * size))
                    {
                        return false;
                    }
                    counter++;
                }
            }

            return true;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            GeneratePuzzle();
            ShufflePuzzle();
            DrawPuzzle();
        }

        private void ShufflePuzzle()
        {
            Random random = new Random();

            for (int i = size - 1; i >= 0; i--)
            {
                for (int j = size - 1; j >= 0; j--)
                {
                    int randomRow = random.Next(0, i + 1);
                    int randomCol = random.Next(0, j + 1);

                    int temp = puzzle[i, j];
                    puzzle[i, j] = puzzle[randomRow, randomCol];
                    puzzle[randomRow, randomCol] = temp;
                }
            }

            int tempEmptyRow = emptyRow;
            int tempEmptyCol = emptyCol;
            emptyRow = size - 1;
            emptyCol = size - 1;
            puzzle[size - 1, size - 1] = -1;
            puzzle[tempEmptyRow, tempEmptyCol] = size * size - 1;
        }
    }
}