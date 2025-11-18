using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace lab8_2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // Клас зберігає дані про книгу
        public class Book
        {
            public int Id { get; set; }          // Номер книги
            public string Title { get; set; }    // Назва
            public string Author { get; set; }   // Автор
            public int Year { get; set; }        // Рік видання

            // Конструктор для ініціалізації полів
            public Book(int id, string title, string author, int year)
            {
                Id = id;
                Title = title;
                Author = author;
                Year = year;
            }

            // Зручний вивід інформації про книгу
            public override string ToString()
            {
                return $"Книга №{Id}, Назва: {Title}, Автор: {Author}, Рік: {Year}";
            }
        }

        // Клас Library реалізує IEnumerable (для foreach) та IEnumerator (для прямого перебору)
        public class Library : IEnumerable, IEnumerator
        {
            private Book[] books;    // масив книг
            private int position = -1; // поточна позиція для IEnumerator
            private int count = 0;     // скільки реально додано книг

            // Створює масив заданого розміру
            public Library(int size)
            {
                books = new Book[size];
            }

            // Індексатор для доступу до книг як до масиву
            public Book this[int index]
            {
                // повертає книгу
                get => (index >= 0 && index < books.Length) ? books[index] : null;

                // записує книгу, якщо індекс в межах
                set
                {
                    if (index >= 0 && index < books.Length)
                        books[index] = value;
                }
            }

            // Метод додавання нової книги
            // Якщо є місце → додаємо та збільшуємо лічильник
            public void Add(Book b, ref int errorCode)
            {
                if (count < books.Length)
                {
                    books[count] = b;
                    count++;
                    errorCode = 0;   // успішно
                }
                else
                {
                    errorCode = 1;   // масив заповнений, помилка
                }
            }

            // Реалізація IEnumerable: повертає елементи для foreach
            public IEnumerator GetEnumerator()
            {
                foreach (var b in books)
                    yield return b;  // повертаємо по одному елементу
            }

            // Реалізація IEnumerator

            // Поточний елемент
            public object Current =>
                (position >= 0 && position < books.Length) ? books[position] : null;

            // Перехід до наступного елемента
            public bool MoveNext()
            {
                position++;
                // Повертаємо true, якщо ще є елементи
                return (position < books.Length);
            }

            // Повертаємося в початкову позицію (перед перший елемент)
            public void Reset() => position = -1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string output = "Список книг:\n\n";

            // Створюємо бібліотеку на 5 книг
            Library lib = new Library(5);
            int error = 0;

            // Додаємо кілька книг
            lib.Add(new Book(1, "Програмування C#", "Іваненко", 2020), ref error);
            lib.Add(new Book(2, "Алгоритми", "Кнут", 2015), ref error);
            lib.Add(new Book(3, "Бази даних", "Петренко", 2018), ref error);
            // 4-та і 5-та позиції залишаться порожніми (null)

            if (error == 1)
            {
                MessageBox.Show("Масив книг переповнений!");
            }

            // Вивід усіх книг через foreach (IEnumerable)
            foreach (Book b in lib)
                if (b != null)
                    output += b + "\n";

            // Використання IEnumerator
            lib.MoveNext();
            output += "\nПоточна книга:\n" + lib.Current + "\n";

            lib.MoveNext();
            output += "\nНаступна книга:\n" + lib.Current;

            // Виводимо в label
            label1.Text = output;
        }
    }
}

