# Fractal Genrator #

Хотел уточнить пару моментов.

Минимальные системные требования: 2 ГБ оперативки

___
## Макс. Глубина рекурсии ##
___
- Дерево: 15
- Снежинка: 10
- Множество Кантора: 10
- Ковёр Серпинского: 7
- Треугольник: 10

Если вы передаёте слишком большое значение рекурсии, то значение автоматически поменяется на максимальное для соответствующего фрактала. Прошу учесть этот момент.

___
## Кривая Коха АКА Снежинка
Лично мне понравилась снежинка, чем просто кривая. Снежинка состоит из кривых Коха, так что это тоже своего рода Кривая Коха!

Ещё у меня немного не получилось наложить градиент... Так что она просто окрашивается каждую итерацию.
___
## Множество Кантора ##

Тут только вопрос с градиентом. Начало - это конец. Конец - это начало. Это такое авторское решение.
___
## Масштабирование ОКНА ##

Условие мне не очень понравилось, что нужно каждый раз рисовать, поэтому я сделал на выбор лишь 3 размера. Они работают странно, но что уж поделать. Условие есть условие.

___
## Сохранение Canvas в PNG ##

Для того, чтобы получить PNG картинку фрактала, нужно задать минимальный размер окну. Иначе, фратал не полностью поместится в рамки. 
