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
using System.IO;
using Fractal;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Threading;

namespace FractalGenrator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Создание всех фракталов.
        Line line = new("Line");
        Flake flake = new("Flake", ref pl);
        BinaryTree tree = new("Binary Tree");
        Carpet carpet = new("Carpet");
        Triangle triangle = new("Triangle");

        // Приближение фрактала.
        ScaleTransform transformScale = new();

        // Последний нажатый фрактал.
        LinkedList<int> lastFractal = new();

        // Настройки монитора.
        double widthWindow = System.Windows.SystemParameters.PrimaryScreenWidth;
        double heightWindow = System.Windows.SystemParameters.PrimaryScreenHeight;

        // Начальный и конечный цвет градиента.
        IEnumerable<Color> gradient;
        static SolidColorBrush s_startGrad;
        static SolidColorBrush s_endGrad;

        // Вспомогательные переменные.
        int SelectedZoom;
        static Polyline pl = new();
        static Polygon pol = new();

        // Флажки для проверки состояния рендеринга.
        bool flagTree, flagFlake, flagLine, flagCarpet, flagTriangle, saveCheck, gradientCheck = false;
        public bool angleCheck = false;

        // Левая и правая ветвь дерева.
        private static double s_angleL, s_angleR;
        private double SnowflakeSize;
        private static int s_depth = 10;
        private int cntDepth = 0;
        private int frames = 0;
        private int fps = 30;
        private int lengthBetween = 20;

        // Углы для снежинки (кривой Коха).
        double[] dTheta = new double[4] { 0, Math.PI / 3, -2 * Math.PI / 3, Math.PI / 3 };


        /// <summary>
        /// Инициализация компонентов.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            // Фиксация размеров окна.
            Width = System.Windows.SystemParameters.PrimaryScreenWidth / 2;
            Height = (System.Windows.SystemParameters.PrimaryScreenHeight / 2) + 10;
            MinWidth = widthWindow / 2;
            MinHeight = heightWindow / 2;
            MaxWidth = Width;
            MaxHeight = Height;

            Title = "Fractals";

            // Установка начальных цветов градиента.
            startColor.SelectedBrush = Brushes.Brown;
            endColor.SelectedBrush = Brushes.Green;
            s_startGrad = startColor.SelectedBrush;
            s_endGrad = endColor.SelectedBrush;
            gradient = GetGradients(s_startGrad.Color, s_endGrad.Color, s_depth);

            // Инициализация приближения.
            canvas1.LayoutTransform = transformScale;
            canvas1.Width = canvas1.Height;

            // Инициализация размера снежинки (кривой Коха).
            double ysize = 0.8 * canvas1.Height / (Math.Sqrt(3) * 4 / 3);
            double xsize = 0.8 * canvas1.Width / 2;
            double size = 0;
            if (ysize < xsize)
                size = ysize;
            else
                size = xsize;
            SnowflakeSize = 2 * size;
            pl.Stroke = Brushes.Black;
        }
        /// <summary>
        /// Приближение фрактала.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ZoomSlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            SelectedZoom = (int)sliderZoom.Value;
            transformScale.ScaleX = SelectedZoom;
            transformScale.ScaleY = SelectedZoom;
            zoomLabel.Text = $"Zoom: {(int)sliderZoom.Value}";
        }

        /// <summary>
        /// Получение списка цветов из градиента.
        /// </summary>
        /// <param name="start"> Начальный цвет </param>
        /// <param name="end"> Конечный цвет </param>
        /// <param name="steps"> Число итераций </param>
        /// <returns></returns>
        public static IEnumerable<Color> GetGradients(Color start, Color end, int steps)
        {
            // Т.к. у некоторых методов рекурсия идёт в минус, при глубине 1, массив может выходить за рамки.
            // Минимальный размер массива 3.
            if (steps == 1) { steps += 2; }

            // Цвета на промежуточных итераций.
            int stepA = ((end.A - start.A) / (steps - 1));
            int stepR = ((end.R - start.R) / (steps - 1));
            int stepG = ((end.G - start.G) / (steps - 1));
            int stepB = ((end.B - start.B) / (steps - 1));

            for (int i = 0; i < steps; i++)
            {
                yield return Color.FromArgb((byte)(start.A + (stepA * i)),
                                            (byte)(start.R + (stepR * i)),
                                            (byte)(start.G + (stepG * i)),
                                            (byte)(start.B + (stepB * i)));
            }
        }

        /// <summary>
        /// Начальные значения для отрисовки любого фрактала.
        /// </summary>
        /// <param name="fractal"></param>
        private void ClickSettings(int fractal)
        {
            UnfollowAll();
            saveCheck = true;
            lastFractal.AddLast(fractal);
            gradient = GetGradients(startColor.SelectedBrush.Color, endColor.SelectedBrush.Color, s_depth);
            canvas1.Children.Clear();
            tbLabel.Text = "";
            frames = 0;
            cntDepth = 1;
        }


        /*--------------------------------------------------------------*/
        // ------------------------ Кнопки -----------------------------// 
        /*--------------------------------------------------------------*/

        /// <summary>
        /// Чеккер для градиента.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Gradient_Click(object sender, RoutedEventArgs e)
        {
            if (gradientCheck == false) { gradientCheck = true; }
            else { gradientCheck = false; }
        }

        /// <summary>
        /// Кнопка отрисовки Треугольника. Номер фрактала - 5.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTriangle_Click(object sender, RoutedEventArgs e)
        {
            ClickSettings(5);

            // Максимальная глубина для треугольника - 10.
            if (s_depth > 10) { s_depth = 10; }
            flagTriangle = false;
            Title = triangle.Name;

            // Подписка анимации треугольна на рендеринг.
            CompositionTarget.Rendering += StartAnimationTriangle;
        }
        
        /// <summary>
        /// Кнопка отрисовки Ковра Серпинского. Номер фрактала - 4.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCarpet_Click(object sender, RoutedEventArgs e)
        {
            ClickSettings(4);

            // Максимальная глубина для ковра - 7.
            if (s_depth > 7) { s_depth = 7; }
            flagCarpet = true;
            Title = carpet.Name;
            
            // Подписка анимации коврика на рендеринг.
            CompositionTarget.Rendering += StartAnimationCarpet;
        }

        /// <summary>
        /// Кнопка отрисовки линии. Номер фрактала - 3.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLine_Click(object sender, RoutedEventArgs e)
        {
            ClickSettings(3);

            // Максимальная глубина для линии - 10.
            if (s_depth > 10) { s_depth = 10; }
            flagLine = true; 
            Title = line.Name;

            // Подписка анимации линии на рендеринг.
            CompositionTarget.Rendering += StartAnimationLine;
        }

        /// <summary>
        /// Кнопка отрисовки снежинки. Номер фрактала - 2.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFlake_Click(object sender, RoutedEventArgs e)
        {
            ClickSettings(2);

            // Максимальная глубина для снежинки (кривая Коха).
            if (s_depth > 10) { s_depth = 10; }
            flagFlake = true;
            Title = flake.Name;
            canvas1.Children.Add(pl);

            // Подписка анимации снежинки Коха на рендеринг.
            CompositionTarget.Rendering += StartAnimationFlake;
        }

        /// <summary>
        /// Кнопка отрисовки бинарного дерева. Номер фрактала - 1.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTree_Click(object sender, RoutedEventArgs e)
        {
            ClickSettings(1);

            // Максимальная глубина для бинарного дерева.
            if (s_depth > 15) { s_depth = 15; }
            gradient = GetGradients(endColor.SelectedBrush.Color, startColor.SelectedBrush.Color, s_depth);
            flagTree = true;
            Title = tree.Name;

            // Подписка анимации бинарного дерева на рендеринг.
            CompositionTarget.Rendering += StartAnimationTree;
        }

        /// <summary>
        /// Кнопка сохранения картинки.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            // Ожидание окончания отрисовки.
            if (saveCheck)
            {
                // Всё еще окночание отрисовки.
                if (flagTree || flagFlake || flagLine || flagCarpet || flagTriangle)
                {
                    MessageBox.Show("Please, wait the end of rendering.");
                }
                else
                {
                    // Диалоговое окно выбора директории.
                    CommonOpenFileDialog dialog = new CommonOpenFileDialog();
                    string path;
                    dialog.IsFolderPicker = true;
                    if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                    {
                        path = dialog.FileName + @"\fractal.png";
                        SaveCanvasToFile(canvas1, 759, path);
                    }
                }
            }
            else MessageBox.Show("There is nothing to save!");
        }
        
        /// <summary>
        /// Кнопка выбора длины между линиями (только для фрактала линии).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Length_Click(object sender, RoutedEventArgs e)
        {
            // Создание окна ввода длины.
            LengthWindow lenWindow = new();
            if (lenWindow.ShowDialog() == true)
            {
                // Проверка корректности данных на допустимые значения. Для длины это от 1 до 50.
                if (int.TryParse(lenWindow.LengthText, out int lengthBet) || (lengthBetween > 0) || (lengthBetween < 51))
                {
                    lengthBetween = lengthBet;
                }
                else
                {
                    MessageBox.Show("Incorrect input for length! (From 1 to 50)");
                    lenWindow.Close();
                }
            }
            else
            {
                // Установка изначального значения в 20 пикселей.
                lengthBetween = 20;
            }
        }

        /// <summary>
        /// Кнопка выбора кадров.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Frame_Click(object sender, RoutedEventArgs e)
        {
            // Создание окна выбора кадров.
            FrameWindow frameWindow = new();

            if (frameWindow.ShowDialog() == true)
            {
                // Проверка корректности вводимых значений. (от 1 до 99 кадров).
                if (int.TryParse(frameWindow.frameText, out int FPS) || (lengthBetween > 0) || (lengthBetween < 100))
                {
                    fps = FPS;
                }
                else
                {
                    MessageBox.Show("Incorrect input for framse! (From 1 to 100)");
                    frameWindow.Close();
                }
            }
        }

        /// <summary>
        /// Кнопка выбора углов для дерева.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Angle_Click(object sender, RoutedEventArgs e)
        {
            if (flagTree == false)
            {
                // Создание нового окна выбора двух углов.
                AngleWindow angleWindow = new();

                if (angleWindow.ShowDialog() == true)
                {
                    // Задаем значение для левой и правой ветви дерева.
                    s_angleL = angleWindow.slider1.Value;
                    s_angleR = angleWindow.slider2.Value;

                    angleCheck = true;
                    MessageBox.Show("Angle of Binary Tree was changed!");
                    angleWindow.Close();
                }
                else
                {
                    angleCheck = false;
                    MessageBox.Show("Angle of Binary tree is default now.");
                    angleWindow.Close();
                }
            }
            else
            {
                MessageBox.Show("Wait the end of rendering.");
            }
        }

        /// <summary>
        /// Кнопка выбора глубины рекурсии.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Depth_Click(object sender, RoutedEventArgs e)
        {
            // Создание окна для выбора глубины рекурсии.
            DepthWindow depthWindow = new(s_depth);

            if (flagFlake || flagTree || flagLine || flagCarpet || flagTriangle)
            {
                MessageBox.Show("Wait the end of rendering!");
                depthWindow.Close();
            }
            else
            {
                if (depthWindow.ShowDialog() == true)
                {
                    if (int.TryParse(depthWindow.DepthText, out int depthFromWindow) && (depthFromWindow >= 1) && (depthFromWindow <= 16))
                    {
                        s_depth = depthFromWindow;
                        cntDepth = 1;


                        MessageBox.Show("Depth was succesfully changed");

                        // Проверка связанного списка на последний элемент. 
                        if (lastFractal.Count != 0)
                        {
                            // Очистка канваса для последующей отрисовки последнего выбраного фрактала.
                            canvas1.Children.Clear();
                            // Если последний элемент - дерево. Меняем местами начальный цвет и конечный.
                            // Так как у дерева, начальный элемент - это ветки, а нужно наоборот.
                            if (lastFractal.Last.Value == 1)
                            {
                                gradient = GetGradients(endColor.SelectedBrush.Color, startColor.SelectedBrush.Color,
                                    depthFromWindow);
                            }
                            else
                            {
                                gradient = GetGradients(startColor.SelectedBrush.Color, endColor.SelectedBrush.Color,
                                    depthFromWindow);
                            }
                            switch (lastFractal.Last.Value)
                            {
                                // Дерево.
                                case 1:
                                    CompositionTarget.Rendering += StartAnimationTree;
                                    break;
                                // Снежинка Коха.
                                case 2:
                                    CompositionTarget.Rendering += StartAnimationFlake;
                                    break;
                                // Линия.
                                case 3:
                                    CompositionTarget.Rendering += StartAnimationLine;
                                    break;
                                // Ковёр Серпинского.
                                case 4:
                                    CompositionTarget.Rendering += StartAnimationCarpet;
                                    break;
                               // Треугольник.
                                case 5:
                                    CompositionTarget.Rendering += StartAnimationTriangle;
                                    break;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Incorrect input for depth! (From 1 to 15)");
                    }
                }
                else
                {
                    MessageBox.Show("Depth was not selected!");

                }
            }
        }

        /// <summary>
        /// Максимальный размер окна.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Maximum_Click(object sender, RoutedEventArgs e)
        {
            canvas1.Children.Clear();
            MinWidth = widthWindow;
            MinHeight = heightWindow - 20;
            canvas1.Width = heightWindow;
            WindowChanging();
        }

        /// <summary>
        /// Средний размер окна.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Medium_Click(object sender, RoutedEventArgs e)
        {
            canvas1.Children.Clear();
            MinWidth = widthWindow / 2 + heightWindow / 4;
            MinHeight = heightWindow / 2 + heightWindow / 4;
            canvas1.Width = heightWindow / 2 + heightWindow / 4;
            WindowChanging();
        }

        /// <summary>
        /// Минимальный рзамер окна.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Minimum_Click(object sender, RoutedEventArgs e)
        {
            canvas1.Children.Clear();
            MinWidth = widthWindow / 2;
            MinHeight = heightWindow / 2;
            canvas1.Width = heightWindow / 2;
            WindowChanging();
        }

        /*--------------------------------------------------------------------------------*/
        // ------------------------ Начало Анимации Фрактала -----------------------------// 
        /*--------------------------------------------------------------------------------*/


        /// <summary>
        /// Анимация треугольника.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartAnimationTriangle(object sender, EventArgs e)
        {
            // Кадры. Начальное кол-во кадров = 20. То есть каждый 20й кадр срабатывает отрисовка.
            frames += 1;
            if (frames % fps == 0)
            {
                // Отрисовка треугольника.
                triangle.DrawTriangle(canvas1, cntDepth, pol, 0, gradient, gradientCheck);
                
                string str = $"{triangle.Name}. Depth = {cntDepth}.";
                tbLabel.Text = str;
                cntDepth += 1;

                if (cntDepth > s_depth)
                {
                    tbLabel.Text = $"{triangle.Name}. Depth = {s_depth}. Finished";
                    // Отписываем все флаги, чтобы можно было менять глубину рекурсии.
                    // Также отписываем от рендеринга.
                    Permission();
                    CompositionTarget.Rendering -= StartAnimationTriangle;
                }
            }
        }
        
        /// <summary>
        /// Анимация ковра.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartAnimationCarpet(object sender, EventArgs e)
        {
            // Кадры. Начальное кол-во кадров = 20. То есть каждый 20й кадр срабатывает отрисовка.
            frames += 1;
            if (frames % fps == 0)
            {
                // Отрисовка ковра Серпинского.
                carpet.DrawCarpet(canvas1, cntDepth, pol, 0, gradient, gradientCheck);

                string str = $"{carpet.Name}. Depth = {cntDepth}";
                tbLabel.Text = str;
                cntDepth += 1;

                if (cntDepth > s_depth)
                {
                    tbLabel.Text = $"{carpet.Name}. Depth = {s_depth}. Finished";
                    // Отписка от всего.
                    Permission();
                    CompositionTarget.Rendering -= StartAnimationCarpet;
                }
            }
        }

        /// <summary>
        /// Анимация линии.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartAnimationLine(object sender, EventArgs e)
        {
            frames += 1;
            if (frames % fps == 0)
            {
                line.DrawLine(canvas1, cntDepth, new Point(0, 0), canvas1.Width, lengthBetween, gradient, gradientCheck);

                string str = $"{line.Name}. Depth = {cntDepth}";
                tbLabel.Text = str;
                cntDepth += 1;
                if (cntDepth > s_depth)
                {
                    tbLabel.Text = $"{line.Name}. Depth = {s_depth}. Finished";
                    Permission();
                    CompositionTarget.Rendering -= StartAnimationLine;
                }
            }
        }

        /// <summary>
        /// Анимация дерева.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartAnimationTree(object sender, EventArgs e)
        {
            frames += 1;
            if (frames % fps == 0)
            {
                tree.DrawBinaryTree(canvas1, cntDepth, new Point(canvas1.Width / 2, canvas1.Height * 0.77),
                    0.2 * canvas1.Width, 3 * Math.PI / 2, angleCheck, gradient, gradientCheck, 2, s_angleL, s_angleR);
                string str = $"{tree.Name}. Depth = {cntDepth}";
                tbLabel.Text = str;
                cntDepth += 1;
                if (cntDepth > s_depth)
                {
                    tbLabel.Text = $"{tree.Name}. Depth = {s_depth}. Finished";
                    Permission();
                    CompositionTarget.Rendering -= StartAnimationTree;
                }
            }
        }

        /// <summary>
        /// Анимация кривой Кохи (снежинки).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartAnimationFlake(object sender, EventArgs e)
        {
            frames += 1;
            if (frames % fps == 0)
            {
                pl.Points.Clear();
                flake.DrawSnowFlake(canvas1, SnowflakeSize, cntDepth, gradient, gradientCheck);
                string str = "Snow Flake - Depth = " + cntDepth.ToString();
                tbLabel.Text = str;
                cntDepth += 1;

                if (cntDepth > s_depth)
                {
                    tbLabel.Text = $"{flake.Name} - Depth = {s_depth}. Finished";
                    Permission();
                    CompositionTarget.Rendering -= StartAnimationFlake;
                }
            }
        }

        /*--------------------------------------------------------------------------------*/
        // ------------------------ Сохранение Canvas в PNG ------------------------------// 
        /*--------------------------------------------------------------------------------*/

        /// <summary>
        /// Создание BitMap и последующая передача для сохранения.
        /// </summary>
        /// <param name="canvas">Канвас</param>
        /// <param name="dpi">DPI</param>
        /// <param name="filename">Путь.</param>
        public static void SaveCanvasToFile(Canvas canvas, int dpi, string filename)
        {
            // Сохранение картинки размером 5000x5000
            var renderTarget = new RenderTargetBitmap(5000, 5000, dpi, dpi, PixelFormats.Pbgra32);
            renderTarget.Render(canvas);

            // Передача полученных данных в метод обработки для создания картинки.
            SaveRTBAsPNGBMP(renderTarget, filename);
        }

        /// <summary>
        /// Обработка данных в картинку формата .png
        /// </summary>
        /// <param name="bmp">Данные канваса</param>
        /// <param name="filename">Путь</param>
        private static void SaveRTBAsPNGBMP(RenderTargetBitmap bmp, string filename)
        {
            try
            {
                // Сохраение канваса в .png
                var enc = new PngBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bmp));
                using (var stm = System.IO.File.Create(filename))
                {
                    enc.Save(stm);
                    MessageBox.Show("Picture was saved!");
                }
            }
            catch
            {
                MessageBox.Show("Error creating image.");
            }
        }


        /*--------------------------------------------------------------------------------*/
        // ------------------------ Вспомогательные Методы -------------------------------// 
        /*--------------------------------------------------------------------------------*/
        
        /// <summary>
        /// Отписка всех флагов для смены глубины рекурсии и возможности сохранять канвас в картинку.
        /// </summary>
        private void Permission()
        {
            flagTree = false;
            flagFlake = false;
            flagLine = false;
            flagTriangle = false;
            flagCarpet = false;
        }

        /// <summary>
        /// Отписка от всех анимаций.
        /// Если не использовать это, то при клике на фрактал,
        /// флаг не успевает сработать, поэтому функция смена глубины фрактала
        /// становится  невозможной.
        /// </summary>
        private void UnfollowAll()
        {
            CompositionTarget.Rendering -= StartAnimationTriangle;
            CompositionTarget.Rendering -= StartAnimationCarpet;
            CompositionTarget.Rendering -= StartAnimationLine;
            CompositionTarget.Rendering -= StartAnimationTree;
            CompositionTarget.Rendering -= StartAnimationFlake;
        }

        /// <summary>
        /// Отрисовка последнего фрактала.
        /// Отрабатывает при изменении размера окна.
        /// </summary>
        private void DrawTheLastFractal()
        {
            if (lastFractal.Count != 0)
            {
                canvas1.Children.Clear();
                switch (lastFractal.Last.Value)
                {
                    case 1:
                        tree.DrawBinaryTree(canvas1, s_depth, new Point(canvas1.Width / 2, canvas1.Height * 0.77), 0.2 * canvas1.Width, 3 * Math.PI / 2, angleCheck, gradient, gradientCheck, 2, s_angleL, s_angleR);
                        break;
                    case 2:
                        flake.DrawSnowFlake(canvas1, SnowflakeSize, s_depth, gradient, gradientCheck);
                        break;
                    case 3:
                        line.DrawLine(canvas1, s_depth, new Point(0, 0), canvas1.Width, lengthBetween, gradient, gradientCheck);
                        break;
                    case 4:
                        carpet.DrawCarpet(canvas1, s_depth, pol, 0, gradient, gradientCheck);
                        break;
                    case 5:
                        triangle.DrawTriangle(canvas1, s_depth, pol, 0, gradient, gradientCheck);
                        break;
                }
            }
        }

        /// <summary>
        /// Метод для смены размера окна.
        /// </summary>
        private void WindowChanging()
        {
            Width = MinWidth;
            Height = MinHeight;
            MaxWidth = Width;
            MaxHeight = Height;
            canvas1.Height = canvas1.Width;
            // Если не рендерится фрактал, тогда отрисовать его при новых значениях канваса.
            if (!(flagFlake || flagTree || flagLine || flagCarpet || flagTriangle))
            {
                DrawTheLastFractal();
            }
        }
    }
}

